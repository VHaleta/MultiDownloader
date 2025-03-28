namespace MultiDownloader.TelegramHost.Models.HttpModels
{
    public class FileDownloadResponcePayload
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string? Error { get; set; }
    }
}
