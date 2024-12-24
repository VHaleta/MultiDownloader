using MultiDownloader.DatabaseApi.Business.Repositories;
using MultiDownloader.DatabaseApi.Database;
using MultiDownloader.DatabaseApi.Host;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build())
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbConfiguration(builder.Configuration["ConnectionStrings:MultiDownloaderDb"]);
builder.Services.AddGraphQlConfiguration();
builder.Services.AddBusinessLayer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MultiDownloaderContext>();
    dbContext.EnsureSeedData();
    var testRepository = scope.ServiceProvider.GetService<IUserRepository>();
    if (testRepository == null)
    {
        throw new Exception("IUserRepository is not registered correctly.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapGraphQL();
//app.UseAuthorization();

app.Run();
