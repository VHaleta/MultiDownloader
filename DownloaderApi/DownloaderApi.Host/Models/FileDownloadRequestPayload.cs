namespace MultiDownloader.DownloaderApi.Host.Models
{
    public class FileDownloadRequestPayload
    {
        public string URL { get; set; }

        public string Format { get; set; }

        public string Resolution { get; set; }
    }
}
