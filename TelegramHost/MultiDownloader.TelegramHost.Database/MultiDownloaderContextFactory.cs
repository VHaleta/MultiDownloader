using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MultiDownloader.TelegramHost.Database
{
    internal class MultiDownloaderContextFactory : IDesignTimeDbContextFactory<MultiDownloaderContext>
    {
        public MultiDownloaderContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.tghost.json")
                .Build();

            var builder = new DbContextOptionsBuilder<MultiDownloaderContext>();
            string connectionString = configuration.GetConnectionString("MultiDownloaderDb") ?? "";
            builder.UseSqlServer(connectionString);

            return new MultiDownloaderContext(builder.Options);
        }
    }
}
