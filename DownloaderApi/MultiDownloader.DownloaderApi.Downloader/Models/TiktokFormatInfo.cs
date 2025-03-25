using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDownloader.DownloaderApi.Downloader.Models
{
    public class TiktokFormatInfo
    {
        public string? Extension { get; set; }
        public string? Resolution { get; set; }
        public string? Protocol { get; set; }
        public int? FileSize { get; set; }
    }
}
