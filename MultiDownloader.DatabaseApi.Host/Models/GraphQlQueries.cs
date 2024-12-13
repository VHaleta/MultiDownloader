using MultiDownloader.DatabaseApi.Database;
using MultiDownloader.DatabaseApi.Database.Models;

namespace MultiDownloader.DatabaseApi.Host.Models
{
    public class GraphQlQueries
    {
        public IQueryable<User> Read([Service] MultiDownloaderContext contex) =>
            contex.Users;
    }
}
