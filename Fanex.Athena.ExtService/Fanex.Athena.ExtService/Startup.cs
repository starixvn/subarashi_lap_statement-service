using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Services;
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
using Microsoft.IdentityModel.Tokens;
using RestSharp;

namespace Fanex.Athena.ExtService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            services.Configure<ApplicationOptions>(Configuration.GetSection("ApplicationOptions"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<List<User>>(Configuration.GetSection("Users"));

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            LogManager
            .SetDefaultLogCategory(appSettings.LogCategory)
            .Use(new List<ILogEngine>
             {
                new RabbitMQLogging(appSettings.RabbitMQ)
             });


            var conns = GetConnectionString();

            DbSettingProviderManager
              .StartNewSession()
              .WithSqlServer(resourcePath: "~/SpConfig")
              .Use(conns)
              .Run();

            services.AddHealthChecks();
            services.AddMemoryCache();

            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
            });


            /*DI*/
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IPFilterAttribute>();
            services.AddSingleton<ICommonService, CommonService>();
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IBetListApiClient, BetListApiClient>();
            services.AddTransient<ISummaryStatementService, SummaryStatementService>();
            services.AddTransient<IStatementByGroupService, StatementByGroupService>();
            services.AddTransient<IEncryptService, IEncryptService>();
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
                    },
                    OnMessageReceived = async context =>
                    {
                        if (!string.IsNullOrEmpty(context.Token))
                        {
                            var dataProtector = context.HttpContext.RequestServices.GetService<IEncryptService>();
                            var encryptedToken = context.Token;
                            var plainToken = dataProtector.Decrypt(encryptedToken);
                            context.Token = plainToken;
                            await Task.CompletedTask;
                        }
                    }
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                options.FormFieldName = "__RequestVerificationToken";
                options.HeaderName = "X-CSRF-TOKEN-LAP-EXT";
                options.SuppressXFrameOptionsHeader = false;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
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

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseJWTokenOnPage();
            app.UseAuthentication();
            app.UseResponseCaching();
            app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    ).MapRoute(name: "api", template: "api/{controller}/{action}/{id?}");
            });
        }
    }
}

