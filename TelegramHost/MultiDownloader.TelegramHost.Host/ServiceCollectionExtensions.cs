using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using MultiDownloader.DatabaseApi.GrpcHost.Jobs;
using MultiDownloader.DatabaseApi.GrpcHost.Users;
using MultiDownloader.TelegramHost.Host.Repositories;
using MultiDownloader.TelegramHost.TgBotProcessor.Repositories;

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

        public static IServiceCollection AddGrpcConfiguration(this IServiceCollection services, string grpcServerAddress)
        {
            services.AddSingleton(GrpcChannel.ForAddress(grpcServerAddress));

            services.AddScoped<JobService.JobServiceClient>(provider =>
            {
                var channel = provider.GetRequiredService<GrpcChannel>();
                return new JobService.JobServiceClient(channel);
            });

            services.AddScoped<UserService.UserServiceClient>(provider =>
            {
                var channel = provider.GetRequiredService<GrpcChannel>();
                return new UserService.UserServiceClient(channel);
            });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
