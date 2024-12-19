namespace MultiDownloader.TelegramHost.Models
{
    public class User
    {
        public long ChatId { get; set; }
        public string Username { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public DateTime LastActivityDate { get; set; }

        //EFCore
        public List<Job> Jobs { get; set; } = new List<Job>();
    }
}
