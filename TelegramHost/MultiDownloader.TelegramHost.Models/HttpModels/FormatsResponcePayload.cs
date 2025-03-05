namespace MultiDownloader.TelegramHost.Models.HttpModels
{
    public class FormatsResponcePayload
    {
        public IEnumerable<FormatInfo> Formats { get; set; }

        public string? Error { get; set; }
    }
}
