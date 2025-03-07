namespace MultiDownloader.DownloaderApi.DownloaderProvider.Models
{
    public class FileData
    {
        public byte[] Data { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Format { get; set; }
    }
}
