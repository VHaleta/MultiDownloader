using MultiDownloader.DownloaderApi.Downloader.Executors;
using MultiDownloader.DownloaderApi.Downloader.Models;
using MultiDownloader.DownloaderApi.DownloaderProvider.Models;
using Serilog;
using System.Text.RegularExpressions;

namespace MultiDownloader.DownloaderApi.Downloader.Providers
{
    public class YoutubeDataProviderYTDLP : IDataProvider
    {
        private readonly ILogger _logger;
        private readonly string[] supportedFormats = new string[]
        {

        };

        private readonly string _getFormatsCommand =
            "yt-dlp --list-formats --flat-playlist --skip-download " +
            "--extractor-args \"youtube:player-client=-ios\" " +
            "\"{0}\"";
        // --cache-dir ~/.cache/yt-dlp

        public YoutubeDataProviderYTDLP(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<FormatInfo> GetAvailableFormats(string url)
        {
            string cmdResult = LinuxCmdExecutor.RunCommand(String.Format(_getFormatsCommand, url));
            //Log.Information(cmdResult);
            List<YoutubeFormatInfo> ytFormats = ParseCmdOutput(cmdResult);
            List<FormatInfo> formats = ytFormats.Select(x => x.MapToFormatInfo())
                .Where(x => x.Resolution != "unsupported").ToList();

            return formats;
        }

        public static List<YoutubeFormatInfo> ParseCmdOutput(string input)
        {
            var formats = new List<YoutubeFormatInfo>();

            // Regex to match each format line (excluding headers and separators)
            string pattern = @"^(?<Id>[^\s]+)\s+(?<Ext>[^\s]+)\s+(?<Resolution>[^\s]+)\s+(?<Fps>[^\s]*)\s+(?<Channel>[^\s]*)\s*\|\s*(?<FileSize>[^\s]*)\s+(?<Tbr>[^\s]*)\s+(?<Protocol>[^\s]*)\s+\|\s+(?<VCodec>[^\s]+)\s+(?<Vbr>[^\s]*)\s+(?<ACodec>[^\s]*)\s+(?<Abr>[^\s]*)\s+(?<Asr>[^\s]*)\s+(?<MoreInfo>.*)";

            // Process the input line by line
            foreach (var line in input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                // Skip header and separator lines
                if (line.StartsWith("[info]") || line.StartsWith("ID") || line.StartsWith("-"))
                    continue;

                var match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    formats.Add(new YoutubeFormatInfo
                    {
                        Id = match.Groups["Id"].Value,
                        Extension = match.Groups["Ext"].Value,
                        Resolution = match.Groups["Resolution"].Value,
                        Fps = match.Groups["Fps"].Value,
                        Channel = match.Groups["Channel"].Value,
                        FileSize = match.Groups["FileSize"].Value,
                        Tbr = match.Groups["Tbr"].Value,
                        Protocol = match.Groups["Protocol"].Value,
                        VCodec = match.Groups["VCodec"].Value,
                        Vbr = match.Groups["Vbr"].Value,
                        ACodec = match.Groups["ACodec"].Value,
                        Abr = match.Groups["Abr"].Value,
                        Asr = match.Groups["Asr"].Value,
                        MoreInfo = match.Groups["MoreInfo"].Value
                    });
                }
            }

            return formats;
        }
    }
}
