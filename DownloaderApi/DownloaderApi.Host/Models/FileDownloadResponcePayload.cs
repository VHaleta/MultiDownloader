﻿using MultiDownloader.DownloaderApi.DownloaderProvider.Models;

namespace MultiDownloader.DownloaderApi.Host.Models
{
    public class FileDownloadResponcePayload
    {
        public FileData FileData { get; set; }

        public string? Error { get; set; }
    }
}
