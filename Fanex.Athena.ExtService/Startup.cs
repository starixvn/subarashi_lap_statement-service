using Fanex.Athena.ExtService.BetList.Services;
using Fanex.Athena.ExtService.Common.Attributes;
using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Athena.ExtService.Core.Attributes;
using Fanex.Athena.ExtService.Core.Extensions;
using Fanex.Athena.ExtService.Core.Middleware;
using Fanex.Athena.ExtService.Core.Models;
using Fanex.Athena.ExtService.Core.Models.DTO;
using Fanex.Athena.ExtService.Core.Services;
using Fanex.Data;
using Fanex.Data.SqlServer;
using Fanex.RemotingRef;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService
{
    public class Startup
    {
        private const string ApplicationOptionSettingKey = "ApplicationOptions";
        private const string AppSettingKey = "AppSettings";
        private const int AliveIn180Seconds = 180;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddControllersWithViews();

#if DEBUG
            if (HostingEnvironment.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }
#endif

            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            services.Configure<ApplicationOptions>(Configuration.GetSection(ApplicationOptionSettingKey));

            var appSettings = Configuration.GetSection(AppSettingKey);
            services.Configure<AppSettings>(appSettings);
            services.Configure<List<User>>(Configuration.GetSection("Users"));

            var appOption = Configuration.GetSection(ApplicationOptionSettingKey).Get<ApplicationOptions>();

            ConfigFanexData();

            ConfigDistributedCache(services, appOption);

            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
            });

            ConfigAuthentication(services);

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddHealthChecks()
                .AddCheck<WhoAmIHealthCheck>("WhoAmIHealthCheck");

            RefDataTypeContainer
                .ConnectTo(new Uri(appSettings["Fanex.RemotingRef.Service.Url"]))
                .FromApplication("Fanex.Athena.ExtService");

            services.AddHttpContextAccessor();
            services.AddHttpsRedirection(options =>
            {
                const int httpsPort = 443;
                options.HttpsPort = httpsPort;
            });

            DIRegister(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UsePingCheck();
            app.UseForwardedHeaders();

            if (string.Equals(Configuration["UseErrorHandler"], "1"))
            {
                app.UseErrorHandler();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWhoAmICheck();

            if (!HostingEnvironment.IsDevelopment())
            {
                app.UseHsts();
                app.UseWhen(
                    context => !context.Request.FromInternal(),
                    builder => builder.UseHttpsRedirection()
                    );
            }

            app.UseStaticResource(HostingEnvironment.IsDevelopment());
            app.UseUnderMaintenance();
            app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
            app.UseRouting();
            app.UseJWTokenOnPage();
            app.UseAuthorization();
            app.UseResponseCaching();
            UseRequestLocalization(app);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private static void UseRequestLocalization(IApplicationBuilder app)
        {
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "MM-dd-yyyy",
                LongDatePattern = "MM-dd-yyyy hh:mm:ss tt"
            };
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"){ DateTimeFormat= dateformat },
            };
            var supportedUICultures = new[]
            {
                new CultureInfo("en-US"){ DateTimeFormat= dateformat },
                new CultureInfo("zh-TW"){ DateTimeFormat= dateformat },
                new CultureInfo("zh-Hans"){ DateTimeFormat= dateformat },
                new CultureInfo("zh-CN"){ DateTimeFormat= dateformat },
                new CultureInfo("vi-VN"){ DateTimeFormat= dateformat },
                new CultureInfo("id-ID"){ DateTimeFormat= dateformat },
                new CultureInfo("th-TH"){ DateTimeFormat= dateformat },
            };
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedUICultures,
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

        private static void ConfigDistributedCache(IServiceCollection services, ApplicationOptions appOption)
        {
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

            services.AddSingleton<ICustomDistributedCache, CustomDistributedCache>();
        }

        private void DIRegister(IServiceCollection services)
        {
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IPFilterAttribute>();
            services.AddScoped<CheckCustomerAttribute>();
            services.AddScoped<IBrowserService, BrowserService>();
            services.AddSingleton<ICommonService, CommonService>();

            ConfigBetListApiClient(services);

            services.AddTransient<ISummaryStatementService, SummaryStatementService>();
            services.AddTransient<IStatementByGroupService, StatementByGroupService>();

            services.AddUnderMaintenance(Configuration);
        }

        private static void ConfigBetListApiClient(IServiceCollection services)
        {
            const string BetListApiClient = "BetListApiClient";
            const string BetListTokenKey = "BETLIST_TOKEN_KEY";

            services.AddTransient<IBetListApiClient, BetListApiClient>();

            services.AddHttpClient(BetListApiClient, (serviceProvider, client) =>
            {
                var options = serviceProvider.GetService<IOptions<AppSettings>>();
                client.BaseAddress = new Uri(options.Value.BetListApiHostUrl);
                client.DefaultRequestHeaders.Add(BetListTokenKey, options.Value.BetListApiTokenValue);
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });
        }

        private void ConfigAuthentication(IServiceCollection services)
        {
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
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
        }

        private void ConfigFanexData()
        {
            var listConnectionName = Configuration.GetSection("ConnectionStrings").GetChildren().Select(_ => _.Key);
            var connectionStrings = new Dictionary<string, ConnectionConfiguration>(listConnectionName
                .Select(connectionName => new KeyValuePair<string, ConnectionConfiguration>(connectionName,
                    new ConnectionConfiguration(connectionName, Configuration.GetConnectionString(connectionName)))
                ));
            DbSettingProviderManager
              .StartNewSession()
              .WithSqlServer(resourcePath: "~/SpConfig/FanexSpName.xml")
              .Use(connectionStrings)
              .Run();
        }
    }
}