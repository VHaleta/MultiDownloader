using Microsoft.EntityFrameworkCore;
using MultiDownloader.DatabaseApi.Business.Repositories;
using MultiDownloader.DatabaseApi.Database;
using MultiDownloader.DatabaseApi.Database.Repositories;
using MultiDownloader.DatabaseApi.GrpcHost.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services
    .AddScoped<IJobRepository, JobRepository>()
    .AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<MultiDownloaderContext>(options =>
                options.UseSqlServer(builder.Configuration["ConnectionStrings:MultiDownloaderDb"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<UserService>();
app.MapGrpcService<JobService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
