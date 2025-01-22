
using MultiDownloader.DownloaderApi.Host;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting up the service");

    var builder = WebApplication.CreateBuilder(args);

    // Add logging
    builder.Host.UseSerilog((context, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration));

    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Add services
    builder.Services.AddDownloaderProcessor();

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5091); // For HTTP
                                   // If you need HTTPS, you'll need to supply a certificate
                                   // options.ListenAnyIP(7063, listenOptions => listenOptions.UseHttps("path/to/cert.pfx", "password"));
    });

    var app = builder.Build();

    Log.Information("Application environment: {Environment}", app.Environment.EnvironmentName);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    //app.UseAuthorization();

    app.MapControllers();

    app.UseSerilogRequestLogging();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}