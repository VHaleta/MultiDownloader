using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.AspNetCore.Hosting;

namespace MultiDownloader.Host
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build())
                .CreateLogger();

            try
            {
                Log.Information("Starting up the service");

                var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        config.AddUserSecrets<Program>();
                    })
                    .ConfigureServices((context, services) =>
                    {
                        services.AddSingleton<ITelegramBotClient>(provider =>
                            new TelegramBotClient(context.Configuration["TelegramBot:Token"] ?? ""));

                        services.AddHostedService<TelegramBotHostedService>();
                    })
                    .UseSerilog((context, services, configuration) =>
                    {
                        configuration.ReadFrom.Configuration(context.Configuration);
                    })
                    .Build();

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddUserSecrets<Program>();
            });
    }
}
