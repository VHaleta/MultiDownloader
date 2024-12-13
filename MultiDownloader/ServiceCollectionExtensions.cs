using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiDownloader.Database;

namespace MultiDownloader.Host
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbConfiguration(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<MultiDownloaderContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}
