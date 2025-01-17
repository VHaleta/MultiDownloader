using MultiDownloader.DownloaderApi.Host.Models;

namespace MultiDownloader.DownloaderApi.Host.Interfaces
{
    public interface IDownloaderProcessor
    {
        Task<IEnumerable<string>> GetAvailableFormatsAsync(FormatsRequestPayload requestPayload);
    }
}
