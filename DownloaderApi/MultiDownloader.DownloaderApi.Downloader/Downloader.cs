using MultiDownloader.DownloaderApi.Downloader.Models;
using MultiDownloader.DownloaderApi.Downloader.Providers;
using MultiDownloader.DownloaderApi.DownloaderProvider;
using MultiDownloader.DownloaderApi.DownloaderProvider.Models;
using Serilog;
using System.Text.RegularExpressions;

namespace MultiDownloader.DownloaderApi.Downloader
{
    public class Downloader : IDownloader
    {
        private readonly YoutubeDataProviderYTDLP _youtubeDataProvider;
        private readonly TiktokDataProvider _tiktokDataProvider;
        private readonly ILogger _logger;

        public Downloader(ILogger logger)
        {
            _youtubeDataProvider = new YoutubeDataProviderYTDLP(logger);
            _tiktokDataProvider = new TiktokDataProvider(logger);
            _logger = logger;
        }

        public async Task<FileData> DownloadVideoFileAsync(string url, string resolution)
        {
            string hostName = new Uri(url).Host;
            var dataProvider = GetDataProvider(hostName);

            return await Task.Run(() =>
            {
                return dataProvider.DownloadVideoFile(url, resolution);
            });
        }

        public async Task<IEnumerable<FormatInfo>> GetAvailableFormatsAsync(string url)
        {
            string hostName = new Uri(url).Host;
            var dataProvider = GetDataProvider(hostName);


            return await Task.Run(() =>
            {
                return dataProvider.GetAvailableFormats(url);
            });
        }

        private IDataProvider? GetDataProvider(string hostName)
        {
            switch (hostName)
            {
                case "www.youtube.com":
                    return _youtubeDataProvider;
                case "vm.tiktok.com":
                    return _tiktokDataProvider;
                default:
                    return null;
            }
        }
    }
}
