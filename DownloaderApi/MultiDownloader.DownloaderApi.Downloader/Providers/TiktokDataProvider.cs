using MultiDownloader.DownloaderApi.Downloader.Models;
using MultiDownloader.DownloaderApi.DownloaderProvider.Models;
using Serilog;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MultiDownloader.DownloaderApi.Downloader.Providers
{
    public class TiktokDataProvider : IDataProvider
    {
        private readonly string _pythonScriptPath;
        private readonly string _pythonExe = "python3";
        private readonly ILogger _logger;

        public TiktokDataProvider(ILogger logger)
        {
            _logger = logger;
            _pythonScriptPath = Path.Combine(AppContext.BaseDirectory, "python", "download_tiktok.py");
            _logger.Information(_pythonScriptPath);
        }

        public IEnumerable<FormatInfo> GetAvailableFormats(string url)
        {
            string cmd = $"{_pythonScriptPath} \"{url}\" --formats";
            string output = ExecutePythonCmd(cmd);

            var ttFormats = JsonSerializer.Deserialize<List<TiktokFormatInfo>>(output.Trim())
                ?? throw new JsonException();
            List<FormatInfo> formats = ttFormats.Select(x => x.MapToFormatInfo())
                .Where(x => x.Resolution is not null)
                .DistinctBy(x => x.Resolution).ToList();

            return formats;
        }

        public FileData DownloadVideoFile(string url, string resolution)
        {
            string[] resolutionSptit = resolution.Split('X', 'x');
            string cmd = $"\"{_pythonScriptPath}\" \"{url}\" --download {resolutionSptit[0]} {resolutionSptit[1]}";

            string output = ExecutePythonCmd(cmd);
            string path = Regex.Matches(output, @"(/[^\r\n]+\.mp4)")
                        .Cast<Match>()
                        .LastOrDefault()?.Value;

            return new FileData() { Path = path, Name = "-" };
        }

        /// <returns>Output from cli</returns>
        private string ExecutePythonCmd(string cmd)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = _pythonExe,
                Arguments = cmd,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInfo);
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            process.WaitForExit();

            _logger.Information(output);

            if(String.IsNullOrEmpty(errors))
            {
                _logger.Error(errors);
            }
            return output;
        }
    }
}
