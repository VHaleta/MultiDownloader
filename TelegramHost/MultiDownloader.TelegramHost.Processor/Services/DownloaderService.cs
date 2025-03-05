using MultiDownloader.TelegramHost.Models.HttpModels;
using Serilog;
using System.Text;
using System.Text.Json;

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

        public async Task<List<string>> GetAvailableFormats(string url)
        {
            var responce = await SendGetRequest<FormatsResponcePayload>(url);
            if(responce.Error != null)
            {
                throw new Exception("Http responce error: " + responce.Error);
            }
            else
            {
                _logger.Information(String.Join(" | ", responce.Formats.Select(x => x.Id)));
                return responce.Formats.Select(x => x.Id).ToList();
            }
        }

        private async Task<ResponseModel> SendGetRequest<ResponseModel>(string url)
        {
            _logger.Information("Sending Http get request : " + url);
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ResponseModel>(responseBody)
                ?? throw new Exception("Json deserialization error for: " + responseBody);
        }

        private async Task SendPostRequest(string url)
        {
            var postData = new
            {
                title = "foo",
                body = "bar",
                userId = 1
            };

            var json = System.Text.Json.JsonSerializer.Serialize(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("POST Response:\n" + responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"POST Request error: {e.Message}");
            }
        }
    }
}
