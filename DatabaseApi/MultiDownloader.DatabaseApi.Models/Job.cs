﻿using System.ComponentModel.DataAnnotations;

namespace MultiDownloader.DatabaseApi.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        public string Sourse { get; set; } = String.Empty;
        public string FileType { get; set; } = String.Empty;
        public string URL { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public string ResultStatus { get; set; } = String.Empty;

        // EFCore
        public long ChatId { get; set; }
        public User User { get; set; }
    }
}
