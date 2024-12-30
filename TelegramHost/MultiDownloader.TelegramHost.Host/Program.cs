using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using MultiDownloader.TelegramHost.Processor;
using MultiDownloader.TelegramHost.Host;

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

            services.AddGrpcConfiguration(context.Configuration["GrpcServerAddress"] ?? "");
            services.AddRepositories();
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
