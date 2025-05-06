namespace Fanex.Athena.ExtService
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using BetList.Services;
    using Common.Attributes;
    using Common.Models.DTO;
    using Common.Services;
    using Core.Attributes;
    using Core.Extensions;
    using Fanex.Athena.ExtService.Core.Configurations;
    using Core.Middleware;
    using Core.Models;
    using Core.Models.DTO;
    using Core.Services;
    using Data;
    using Data.SqlServer;
    using Fanex.Data.Vault;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using RemotingRef;
    using Report.Services;
    using StackExchange.Redis;

    public class Startup
    {
        private const int AliveIn180Seconds = 180;
        private const string ApplicationOptionSettingKey = "ApplicationOptions";
        private const string AppSettingKey = "AppSettings";

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

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            services.Configure<ApplicationOptions>(Configuration.GetSection(ApplicationOptionSettingKey));

            var appSettings = Configuration.GetSection(AppSettingKey);
            services.Configure<AppSettings>(appSettings);
            services.Configure<List<User>>(Configuration.GetSection("Users"));
            services.Configure<List<ApiKey>>(Configuration.GetSection("ApiKeys"));

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
                options.ForwardLimit = null;
            });

            var appOption = Configuration.GetSection(ApplicationOptionSettingKey).Get<ApplicationOptions>();

            ConfigFanexData(services);

            ConfigDistributedCache(services, appOption);

            services.AddResponseCaching(options => { options.UseCaseSensitivePaths = true; });

            ConfigAuthentication(services);

            services.Configure<IdentitySetting>(Configuration.GetSection("IdentityServerSetting"));
            services.AddUserAccessTokenManagement();
            services.AddScoped<IContextService, ContextService>();

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddHealthChecks()
                .AddCheck<WhoAmIHealthCheck>("WhoAmIHealthCheck")
                .AddCheck<WhoAmIHeaderHealthCheck>("WhoAmIHeaderHealthCheck");

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
            app.UseHealthChecks("/whoamiheader", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                AllowCachingResponses = false,
                Predicate = registration => registration.Name == "WhoAmIHeaderHealthCheck",
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var result = JsonConvert.SerializeObject(report, Formatting.Indented);
                    await context.Response.WriteAsync(result);
                }
            });
            app.UseForwardedHeaders();
            app.UseWhoAmICheck();

            if (string.Equals(Configuration["UseErrorHandler"], "1"))
            {
                app.UseErrorHandler();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

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
            app.UseRateLimit();
            UseRequestLocalization(app);
            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
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
                new CultureInfo("en-US") { DateTimeFormat = dateformat }
            };

            var supportedUICultures = new[]
            {
                new CultureInfo("en-US") { DateTimeFormat = dateformat },
                new CultureInfo("zh-TW") { DateTimeFormat = dateformat },
                new CultureInfo("zh-Hans") { DateTimeFormat = dateformat },
                new CultureInfo("zh-CN") { DateTimeFormat = dateformat },
                new CultureInfo("vi-VN") { DateTimeFormat = dateformat },
                new CultureInfo("id-ID") { DateTimeFormat = dateformat },
                new CultureInfo("th-TH") { DateTimeFormat = dateformat }
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
                }
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
                    }
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
            services.AddScoped<ApiKeyAuthAttribute>();
            services.AddScoped<CheckCustomerAttribute>();
            services.AddScoped<IBrowserService, BrowserService>();
            services.AddSingleton<ICommonService, CommonService>();
            services.AddSingleton<IOAuthService, OAuthService>();

            ConfigBetListApiClient(services);

            services.AddTransient<ISummaryStatementService, SummaryStatementService>();
            services.AddTransient<IStatementByGroupService, StatementByGroupService>();
            services.AddTransient<IPlayerReportService, PlayerReportService>();

            services.AddUnderMaintenance(Configuration);
        }

        private static void ConfigBetListApiClient(IServiceCollection services)
        {
            const string BetListApiClient = "BetListApiClient";

            services.AddTransient<IBetListApiClient, BetListApiClient>();

            services.AddHttpClient(BetListApiClient, (serviceProvider, client) =>
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopeServiceProvider = scope.ServiceProvider;
                    var options = scopeServiceProvider.GetService<IOptions<AppSettings>>();
                    var oAuthService = scopeServiceProvider.GetService<IOAuthService>();
                    var contextService = scopeServiceProvider.GetService<IContextService>();

                    var setting = options.Value;
                    client.BaseAddress = new Uri(options.Value.BetListApiHostUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, contextService.OAuthAccessToken);
                }
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });
        }

        private void ConfigAuthentication(IServiceCollection services)
        {
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            //Add config for JwtBearerOptions to get session from IdentityServer access token
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, CustomJwtBearerPostConfigureOptions>());
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
                        ValidateLifetime = true
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

        private void ConfigFanexData(IServiceCollection services)
        {
            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer(Configuration)
                .UseDefaultDbSettingProvider("~/SpConfig/FanexSpName.xml", true)
                .Run();

            services.TryAddSingleton<VaultDbAliasService, VaultDbAliasServiceImp>();
        }
    }
}