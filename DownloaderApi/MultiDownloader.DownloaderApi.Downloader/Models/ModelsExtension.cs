using MultiDownloader.DownloaderApi.DownloaderProvider.Models;

namespace MultiDownloader.DownloaderApi.Downloader.Models
{
    public static class ModelsExtension
    {
        public static FormatInfo MapToFormatInfo(this YoutubeFormatInfo youtubeFormatInfo) =>
            new FormatInfo()
            {
                Id = youtubeFormatInfo.Id,
                Resolution = youtubeFormatInfo.Resolution,
                Extension = youtubeFormatInfo.Extension
            };
    }
}
