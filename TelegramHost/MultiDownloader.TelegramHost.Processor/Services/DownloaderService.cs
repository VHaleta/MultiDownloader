using MultiDownloader.TelegramHost.Models.HttpModels;
using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace MultiDownloader.TelegramHost.TgBotProcessor.Services
{
    public class DownloaderService
    {
        private readonly ILogger _logger;
        private static readonly HttpClient client = new HttpClient();

        public DownloaderService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<List<FormatInfo>> GetAvailableFormats(Uri uri)
        {
            var responce = await SendGetRequest<FormatsResponcePayload>(
                String.Format("http://localhost:5091/api/downloader/formats?url={0}", uri.ToString()));
            if(responce.Error != null)
            {
                throw new Exception("Http responce error: " + responce.Error);
            }
            else
            {
                _logger.Information(String.Join(" | ", responce.Formats.Select(x => String.Format($"{x.Id} {x.Extension} {x.Resolution}"))));
                return responce.Formats.ToList();
            }
        }

        public async Task<string> DownloadAudioFile(Uri uri)
        {
            var responce = await SendPostRequest<FileDownloadResponcePayload, FileDownloadRequestPaylaod>(
                String.Format("http://localhost:5091/api/downloader/download"),
                new FileDownloadRequestPaylaod()
                {
                    URL = uri.ToString(),
                    Format = "mp3",
                    Resolution = "audio"
                });
            if (responce.Error != null)
            {
                throw new Exception("Http responce error: " + responce.Error);
            }
            else
            {
                return responce.Path;
            }
        }

        public async Task<string> DownloadVideoFile(Uri uri, string resolution)
        {
            var responce = await SendPostRequest<FileDownloadResponcePayload, FileDownloadRequestPaylaod>(
                String.Format("http://localhost:5091/api/downloader/download"),
                new FileDownloadRequestPaylaod()
                {
                    URL = uri.ToString(),
                    Format = "mp4",
                    Resolution = resolution
                });
            if (responce.Error != null)
            {
                throw new Exception("Http responce error: " + responce.Error);
            }
            else
            {
                _logger.Information("Received path: " +  responce.Path ?? "is null");
                return responce.Path;
            }
        }

        private async Task<ResponseModel> SendGetRequest<ResponseModel>(string url)
        {
            _logger.Information("Sending Http get request : " + url);
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseModel>(responseBody)
                ?? throw new Exception("Json deserialization error for: " + responseBody);
        }

        private async Task<ResponseModel> SendPostRequest<ResponseModel, RequestPayload>(string url, RequestPayload requestPayload)
        {
            var json = JsonConvert.SerializeObject(requestPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            _logger.Information($"{response.StatusCode}: {responseBody}");
            return JsonConvert.DeserializeObject<ResponseModel>(responseBody)
                ?? throw new Exception("Json deserialization error for: " + responseBody);
        }
    }
}
