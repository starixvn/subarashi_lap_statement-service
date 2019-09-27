using Fanex.Athena.ExtService.BetList.Services;
using Fanex.Athena.ExtService.Common.Attributes;
using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Athena.ExtService.Core.Attributes;
using Fanex.Athena.ExtService.Core.Middleware;
using Fanex.Athena.ExtService.Core.Models;
using Fanex.Athena.ExtService.Core.Models.DTO;
using Fanex.Athena.ExtService.Core.Services;
using Fanex.Data;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.RabbitMQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService
{
    public class Startup
    {
        private const string ApplicationOptionSettingKey = "ApplicationOptions";
        private const string AppSettingKey = "AppSettings";
        private const int SixtyDays = 60;
        private const int AliveIn180Seconds = 180;
        private const int UMREQUEST_TIMEOUT = 10;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            services.Configure<ApplicationOptions>(Configuration.GetSection(ApplicationOptionSettingKey));
            services.Configure<AppSettings>(Configuration.GetSection(AppSettingKey));
            services.Configure<List<User>>(Configuration.GetSection("Users"));

            var appOption = Configuration.GetSection(ApplicationOptionSettingKey).Get<ApplicationOptions>();

            LogManager
                .SetDefaultLogCategory(appOption.LogCategory)
                .Use(new List<ILogEngine>
                 {
                    new RabbitMQLogging(appOption.RabbitMQ)
                 });

            ConfigDataProtection(services, appOption);

            ConfigFanexData();

            services.AddMemoryCache();
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new ConfigurationOptions
                {
                    KeepAlive = AliveIn180Seconds,
                    Password = appOption.Redis.Pass,
                    EndPoints =
                    {
                        { appOption.Redis.Server, appOption.Redis.Port }
                    },
                };
                options.InstanceName = appOption.Redis.AppName;
            });
            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
            });

            ConfigAuthentication(services);

            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                options.FormFieldName = "__RequestVerificationToken";
                options.HeaderName = "X-CSRF-TOKEN-LAP-EXT";
                options.SuppressXFrameOptionsHeader = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            DIRegister(services);
        }

        private void DIRegister(IServiceCollection services)
        {
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IPFilterAttribute>();
            services.AddScoped<CheckCustomerAttribute>();
            services.AddSingleton<ICommonService, CommonService>();
            services.AddSingleton<ILogger>(x => LogService.LoggerInstance);

            ConfigUMRestClient(services);
            ConfigBetListApiClient(services);

            services.AddTransient<ISummaryStatementService, SummaryStatementService>();
            services.AddTransient<IStatementByGroupService, StatementByGroupService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpContextHelper, UMHttpContextHelper>();
            services.AddSingleton<IMaintenanceService, MaintenanceService>();
        }

        private void ConfigUMRestClient(IServiceCollection services)
        {
            const string UMClient = "UMClient";

            services.AddSingleton(Configuration.GetSection("ONEbook.UM").Get<UMClientConfiguration>());
            services.AddSingleton<IPHelper, IPHelperImpl>();
            services.AddSingleton<IUMCacheService, UMCacheService>();
            services.AddSingleton<IUMRestClient>(s => new UMRestClient(
                UMClient, s.GetService<UMClientConfiguration>(), s.GetService<IPHelper>(), s.GetService<IHttpClientFactory>()));
            services.AddHttpClient(UMClient, (serviceProvider, client) =>
            {
                var configs = serviceProvider.GetService<UMClientConfiguration>();
                client.BaseAddress = new Uri(configs.ServiceUri);
                client.Timeout = TimeSpan.FromSeconds(UMREQUEST_TIMEOUT);
            });
        }

        private static void ConfigBetListApiClient(IServiceCollection services)
        {
            const string BetListApiClient = "BetListApiClient";
            const string BetListTokenKey = "BETLIST_TOKEN_KEY";

            services.AddSingleton<IBetListApiClient>(s => new BetListApiClient(
                BetListApiClient, s.GetService<IHttpClientFactory>()));
            services.AddHttpClient(BetListApiClient, (serviceProvider, client) =>
            {
                var options = serviceProvider.GetService<IOptions<AppSettings>>();
                client.BaseAddress = new Uri(options.Value.BetListApiHostUrl);
                client.DefaultRequestHeaders.Add(BetListTokenKey, options.Value.BetListApiTokenValue);
            });
        }

        private void ConfigAuthentication(IServiceCollection services)
        {
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    TokenDecryptionKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RoleClaimType = ClaimsIdentity.DefaultNameClaimType,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });
        }

        private void ConfigFanexData()
        {
            var conns = GetConnectionString();

            DbSettingProviderManager
              .StartNewSession()
              .WithSqlServer(resourcePath: "~/SpConfig")
              .Use(conns)
              .Run();
        }

        private static void ConfigDataProtection(IServiceCollection services, ApplicationOptions appOption)
        {
            var configRedix = new ConfigurationOptions
            {
                EndPoints =
                {
                    { appOption.Redis.Server, appOption.Redis.Port }
                },
                KeepAlive = AliveIn180Seconds,
                Password = appOption.Redis.Pass
            };
            var redis = ConnectionMultiplexer.Connect(configRedix);
            services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, appOption.Redis.Pass)
                .SetDefaultKeyLifetime(TimeSpan.FromDays(SixtyDays))
                .SetApplicationName(appOption.Redis.AppName);
        }

        private Dictionary<string, ConnectionConfiguration> GetConnectionString()
        {
            var connectionStrings = new Dictionary<string, ConnectionConfiguration>
            {
                {
                    "DBEXT.bodb02.bodbLicenseeNet",
                    new ConnectionConfiguration("DBEXT.bodb02.bodbLicenseeNet",
                Configuration.GetConnectionString("DBEXT.bodb02.bodbLicenseeNet"))
                },
                {
                    "DBACC.bodb02.bodbLicenseeNet",
                    new ConnectionConfiguration("DBACC.bodb02.bodbLicenseeNet",
                Configuration.GetConnectionString("DBACC.bodb02.bodbLicenseeNet"))
                }
            };
            return connectionStrings;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseErrorHandler();
                app.UseHsts();
            }
            CustomizeCultureSupport(app);
            ConfigCacheForStaticResource(app, env);
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseJWTokenOnPage();
            app.UseAuthentication();
            app.UseResponseCaching();
            app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
            app.UseUMHandler();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    ).MapRoute(name: "api", template: "api/{controller}/{action}/{id?}");
            });
        }

        private static void ConfigCacheForStaticResource(IApplicationBuilder app, IHostingEnvironment env)
        {
            const int OneDayInSecond = 86400;
            const int TenMinInSecond = 600;
            var cachePeriod = env.IsDevelopment() ? TenMinInSecond : OneDayInSecond;
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });
        }

        private static void CustomizeCultureSupport(IApplicationBuilder app)
        {
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "MM-dd-yyyy",
                LongDatePattern = "MM-dd-yyyy hh:mm:ss tt"
            };
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"){ DateTimeFormat= dateformat },
                new CultureInfo("zh-TW"){ DateTimeFormat= dateformat },
                new CultureInfo("zh-Hans"){ DateTimeFormat= dateformat },
                new CultureInfo("zh-CN"){ DateTimeFormat= dateformat },
            };
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider
                    {
                        UIQueryStringKey = "lang"
                    }
                },
            };

            app.UseRequestLocalization(requestLocalizationOptions);
        }
    }
}