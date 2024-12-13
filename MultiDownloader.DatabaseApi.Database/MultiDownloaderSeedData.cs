using MultiDownloader.DatabaseApi.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDownloader.DatabaseApi.Database
{
    public static class MultiDownloaderSeedData
    {
        public static void EnsureSeedData(this MultiDownloaderContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>()
                {
                    new User()
                    {
                        ChatId = 0,
                        Username = "TestUser",
                        FirstName = "FirstName",
                        LastName = "LastName",
                        LastActivityDate = DateTime.Now
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
