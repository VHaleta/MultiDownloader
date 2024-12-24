using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.Database
{
    public static class MultiDownloaderSeedData
    {
        public static void EnsureSeedData(this MultiDownloaderContext context)
        {
            if (!context.Users.Any() || !context.Jobs.Any())
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

                var jobs = new List<Job>()
                {
                    new Job()
                    {
                        Sourse = "YouTube",
                        FileType = "Opus",
                        URL = "URL",
                        Title = "Title1",
                        ResultStatus = "Success",
                        ChatId = 1,
                    },
                    new Job()
                    {
                        Sourse = "YouTube",
                        FileType = "Opus",
                        URL = "URL",
                        Title = "Title2",
                        ResultStatus = "Success",
                        ChatId = 1,
                    },
                    new Job()
                    {
                        Sourse = "YouTube",
                        FileType = "Opus",
                        URL = "URL",
                        Title = "Title3",
                        ResultStatus = "Success",
                        ChatId = 1,
                    },
                    new Job()
                    {
                        Sourse = "YouTube",
                        FileType = "Opus",
                        URL = "URL",
                        Title = "Title4",
                        ResultStatus = "Success",
                        ChatId = 2,
                    },
                    new Job()
                    {
                        Sourse = "YouTube",
                        FileType = "Opus",
                        URL = "URL",
                        Title = "Title5",
                        ResultStatus = "Success",
                        ChatId = 2,
                    },
                    new Job()
                    {
                        Sourse = "YouTube",
                        FileType = "Opus",
                        URL = "URL",
                        Title = "Title6",
                        ResultStatus = "Success",
                        ChatId = 2,
                    },
                    new Job()
                    {
                        Sourse = "YouTube",
                        FileType = "Opus",
                        URL = "URL",
                        Title = "Title7",
                        ResultStatus = "Success",
                        ChatId = 3,
                    },
                    new Job()
                    {
                        Sourse = "YouTube",
                        FileType = "Opus",
                        URL = "URL",
                        Title = "Title8",
                        ResultStatus = "Success",
                        ChatId = 3,
                    },
                    new Job()
                    {
                        Sourse = "YouTube",
                        FileType = "Opus",
                        URL = "URL",
                        Title = "Title9",
                        ResultStatus = "Success",
                        ChatId = 3,
                    }
                };
                context.Jobs.AddRange(jobs);

                context.SaveChanges();
            }
        }
    }
}
