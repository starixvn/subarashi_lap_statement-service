using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;

using RestSharp;

using StackExchange.Redis;
namespace Fanex.Athena.ExtService
{
    public class Startup
    {
        const string ApplicationOptionSettingKey = "ApplicationOptions";
        const string AppSettingKey = "AppSettings";
        const int SixtyDays = 60;
        const int AliveIn180Seconds = 180;

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

            ConfigHealthCheckService(services, appOption);
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
                options.SuppressXFrameOptionsHeader = false;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            DIRegister(services);

        }

        private void ConfigHealthCheckService(IServiceCollection services, ApplicationOptions appOption)
        {
            const long MaxMemory = 8000000000;
            const long MaxDisk = 8000;
            const long MaxWorkingSet = 2000000000;
            services.AddHealthChecksUI()
                .AddHealthChecks()
                .AddAsyncCheck("Server", async () =>
                {

                    var ip = await System.Net.Dns.GetHostAddressesAsync(
                        System.Net.Dns.GetHostName());
                    var result = new StringBuilder();
                    foreach (var item in ip)
                    {
                        if (!item.Equals("::1"))
                        {
                            result.AppendLine(item + ",");
                        }
                    }
                    return HealthCheckResult.Healthy(result.ToString());

                })
                .AddPrivateMemoryHealthCheck(MaxMemory, "Memmory")
                .AddDiskStorageHealthCheck(o =>
                {
                    o.AddDrive("C:\\", MaxDisk);
                    o.AddDrive("E:\\", MaxDisk);
                }, "Disk")
                .AddWorkingSetHealthCheck(MaxWorkingSet, "WorkingSet")
                .AddPingHealthCheck(x =>
                {
                    var dbServers = Configuration.GetSection("DBServer").Get<IList<string>>();
                    foreach (var item in dbServers)
                    {
                        x.AddHost(item, 5);
                    }
                }, "Database")
                .AddRedis($"{appOption.Redis.Server}:{appOption.Redis.Port},allowAdmin=true,password={appOption.Redis.Pass}");
        }

        private static void DIRegister(IServiceCollection services)
        {
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IPFilterAttribute>();
            services.AddScoped<CheckCustomerAttribute>();
            services.AddSingleton<ICommonService, CommonService>();
            services.AddSingleton<ILogger>(x => LogService.LoggerInstance);
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IBetListApiClient, BetListApiClient>();
            services.AddTransient<ISummaryStatementService, SummaryStatementService>();
            services.AddTransient<IStatementByGroupService, StatementByGroupService>();
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

            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            })
            .UseHealthChecksUI(x => x.UIPath = "/healthzui");
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
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("zh-TW"),
                new CultureInfo("zh-Hans"),
                new CultureInfo("zh-CN"),

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
                }
            };

            app.UseRequestLocalization(requestLocalizationOptions);
        }
    }
}

