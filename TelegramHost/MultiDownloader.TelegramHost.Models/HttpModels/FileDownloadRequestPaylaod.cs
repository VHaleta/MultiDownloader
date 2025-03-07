using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDownloader.TelegramHost.Models.HttpModels
{
    public class FileDownloadRequestPaylaod
    {
        public string URL { get; set; }

        public string Format { get; set; }

        public string Resolution { get; set; }
    }
}
