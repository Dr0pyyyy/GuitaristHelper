using Guitarist_Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace Guitarist_Helper
{
    partial class Program
    {
        static void Main(string[] args)
        {
            //Setting up program from appsettings.json
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            //Settings up logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            //Setting up services
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IMusicManager, SpotifyManager>();
                })
                .UseSerilog()
                .Build();

            Console.Write("Insert playlist ID: ");
            string playlistID = Console.ReadLine();
            if (playlistID != null)
            {
                var service = ActivatorUtilities.CreateInstance<SpotifyManager>(host.Services);
                var playlist = service.GetPlaylist(playlistID).Result;
            }
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}