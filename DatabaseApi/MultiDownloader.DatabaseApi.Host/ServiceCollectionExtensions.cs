using Microsoft.EntityFrameworkCore;
using MultiDownloader.DatabaseApi.Business.Repositories;
using MultiDownloader.DatabaseApi.Database;
using MultiDownloader.DatabaseApi.Database.Repositories;
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
                .AddQueryType<GraphQlQueries>()
//                .AddMutationType<GraphQlMutations>()
                .AddProjections()
                .AddFiltering();

            return services;
        }

        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
