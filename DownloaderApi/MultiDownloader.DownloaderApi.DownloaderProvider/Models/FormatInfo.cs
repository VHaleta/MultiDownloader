namespace MultiDownloader.DownloaderApi.DownloaderProvider.Models
{
    public class FormatInfo
    {
        public string? Id { get; set; }
        public string? Extension { get; set; }
        public string? Resolution { get; set; }
        public string? Proto {  get; set; }
        public string? Size { get; set; } //TODO: size in mb
    }
}
