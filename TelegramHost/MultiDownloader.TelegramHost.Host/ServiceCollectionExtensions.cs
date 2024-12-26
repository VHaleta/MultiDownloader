using Microsoft.Extensions.DependencyInjection;
//using MultiDownloader.DatabaseApi.GrpcHost.Jobs;
//using MultiDownloader.DatabaseApi.GrpcHost.Users;

namespace MultiDownloader.TelegramHost.Host
{
    public static class ServiceCollectionExtensions
    {
        //public static IServiceCollection AddDbConfiguration(this IServiceCollection services, string? connectionString)
        //{
        //    services.AddDbContext<MultiDownloaderContext>(options =>
        //        options.UseSqlServer(connectionString));
        //    return services;
        //}

        public static IServiceCollection AddGrpcConfiguration(this IServiceCollection services)
        {
            //services.AddScoped<JobService.JobServiceClient>();
            //services.AddScoped<UserService.UserServiceClient>();

            return services;
        }
    }
}
