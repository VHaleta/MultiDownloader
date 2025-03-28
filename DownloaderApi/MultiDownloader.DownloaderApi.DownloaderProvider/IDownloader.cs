using MultiDownloader.DownloaderApi.DownloaderProvider.Models;

namespace MultiDownloader.DownloaderApi.DownloaderProvider
{
    public interface IDownloader
    {
        Task<IEnumerable<FormatInfo>> GetAvailableFormatsAsync(string url);

        Task<FileData> DownloadVideoFileAsync(string url, string resolution);
    }
}
