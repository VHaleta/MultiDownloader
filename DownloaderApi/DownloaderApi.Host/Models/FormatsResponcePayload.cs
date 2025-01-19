using MultiDownloader.DownloaderApi.DownloaderProvider.Models;

namespace MultiDownloader.DownloaderApi.Host.Models
{
    public class FormatsResponcePayload
    {
        public IEnumerable<FormatInfo> Formats { get; set; }
    }
}
