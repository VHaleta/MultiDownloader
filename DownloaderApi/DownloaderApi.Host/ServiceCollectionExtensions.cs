using MultiDownloader.DownloaderApi.Host.Interfaces;

namespace MultiDownloader.DownloaderApi.Host
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDownloaderProcessor(this IServiceCollection services)
        {
            //services.AddScoped<IDownloaderProcessor, >
            return services;
        }
    }
}
