using Microsoft.EntityFrameworkCore;
using MultiDownloader.DatabaseApi.Database;
using MultiDownloader.DatabaseApi.Host.Models;

namespace MultiDownloader.DatabaseApi.Host
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbConfiguration(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<MultiDownloaderContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddGraphQlConfiguration(this IServiceCollection services)
        {
            services
                .AddGraphQLServer()
                .AddQueryType<GraphQlQueries>();

            return services;
        }
    }
}
