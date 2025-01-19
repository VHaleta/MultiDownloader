using MultiDownloader.DownloaderApi.Downloader;
using MultiDownloader.DownloaderApi.DownloaderProvider;

namespace MultiDownloader.DownloaderApi.Host
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDownloaderProcessor(this IServiceCollection services)
        {
            services.AddScoped<IDownloader, Downloader.Downloader>();
            return services;
        }
    }
}
