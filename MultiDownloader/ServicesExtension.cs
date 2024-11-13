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
    public static class ServicesExtension
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
        {
            //TODO: AddConnectionString
            services.AddDbContext<GeneralDbContext>(options =>
                options.UseSqlServer(""));
            return services;
        }
    }
}
