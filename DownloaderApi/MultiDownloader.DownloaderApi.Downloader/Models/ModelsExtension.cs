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
                Extension = youtubeFormatInfo.Extension,
                Proto = youtubeFormatInfo.Protocol
            };

        private static string MapResolution(this string str) => str switch
        {
            "192x144" => "144p",
            "256x144" => "144p",
            "426x240" => "240p",
            "640x360" => "360p",
            "854x480" => "480p",
            "1280x720" => "720p",
            "1920x1080" => "1080p",
            "audio" => "audio",
            _ => "unsupported"
        };
    }
}
