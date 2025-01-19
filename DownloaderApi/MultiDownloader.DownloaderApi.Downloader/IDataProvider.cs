using MultiDownloader.DownloaderApi.DownloaderProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDownloader.DownloaderApi.Downloader
{
    public interface IDataProvider
    {
        public IEnumerable<FormatInfo> GetAvailableFormats(string url);
    }
}
