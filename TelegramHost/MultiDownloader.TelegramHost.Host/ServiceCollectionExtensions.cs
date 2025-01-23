using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiDownloader.TelegramHost.Database;
using MultiDownloader.TelegramHost.Database.Repositories;
using MultiDownloader.TelegramHost.TgBotProcessor.Repositories;

namespace MultiDownloader.TelegramHost.Host
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbConfiguration(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<MultiDownloaderContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
