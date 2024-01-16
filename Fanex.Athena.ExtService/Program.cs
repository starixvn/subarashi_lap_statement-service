using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fanex.Athena.ExtService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Statement Service";

            CreateHost(args).Run();
        }

        public static IHost CreateHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                var env = builderContext.HostingEnvironment;
                config.SetBasePath(env.ContentRootPath);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: false);
                var envFromSetting = config.Build().GetValue<string>("AppSettings:Environment");
                config.AddJsonFile($"App_Data/Users/Users.{envFromSetting}.json", optional: false, reloadOnChange: false);
                config.AddJsonFile($"App_Data/Whitelist/Whitelist.{envFromSetting}.json", optional: false, reloadOnChange: false);
                config.AddEnvironmentVariables();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseSentry();
            })
            .Build();
    }
}