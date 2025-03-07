namespace MultiDownloader.TelegramHost.Models.HttpModels
{
    public class FileDownloadResponcePayload
    {
        public FileData FileData { get; set; }

        public string? Error { get; set; }
    }
}
