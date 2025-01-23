using MultiDownloader.TelegramHost.Models;

namespace MultiDownloader.TelegramHost.Database
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
                        ChatId = 1,
                        Username = "User1",
                        FirstName = "FirstName1",
                        LastName = "LastName1",
                        LastActivityDate = DateTime.Now
                    },
                    new User()
                    {
                        ChatId = 2,
                        Username = "User2",
                        FirstName = "FirstName2",
                        LastName = "LastName2",
                        LastActivityDate = DateTime.Now
                    },
                    new User()
                    {
                        ChatId = 3,
                        Username = "User3",
                        FirstName = "FirstName3",
                        LastName = "LastName3",
                        LastActivityDate = DateTime.Now
                    },
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
