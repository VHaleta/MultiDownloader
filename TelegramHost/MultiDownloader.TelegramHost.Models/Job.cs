using MultiDownloader.Models.Enums;

namespace MultiDownloader.TelegramHost.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public JobSourse Sourse { get; set; }
        public FileType FileType { get; set; }
        public string URL { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public ResultStatus ResultStatus { get; set; } = ResultStatus.InProgress;

        // EFCore
        public long ChatId { get; set; }
        public User User { get; set; }
    }
}
