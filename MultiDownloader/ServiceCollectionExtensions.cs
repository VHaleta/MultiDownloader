using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiDownloader.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDownloader.Host
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbConfiguration(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<GeneralDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}
