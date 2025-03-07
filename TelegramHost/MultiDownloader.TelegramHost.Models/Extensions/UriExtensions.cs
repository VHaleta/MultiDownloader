using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MultiDownloader.TelegramHost.Models.Extensions
{
    public static class UriExtensions
    {
        public static Uri ClearUri(this Uri uri)
        {
            switch (uri.Host)
            {
                case "www.youtube.com":
                    var queryParams = HttpUtility.ParseQueryString(uri.Query);
                    string videoId = queryParams["v"];
                    string newUrl = "https://www.youtube.com/watch?v=" + videoId;
                    return new Uri(newUrl);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
