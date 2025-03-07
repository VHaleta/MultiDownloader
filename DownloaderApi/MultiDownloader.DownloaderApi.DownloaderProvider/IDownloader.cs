﻿using MultiDownloader.DownloaderApi.DownloaderProvider.Models;

namespace MultiDownloader.DownloaderApi.DownloaderProvider
{
    public interface IDownloader
    {
        Task<IEnumerable<FormatInfo>> GetAvailableFormatsAsync(string url);

        Task<FileData> DownloadFileAsync(string url, string format, string resolution);
    }
}
