﻿using System.ComponentModel.DataAnnotations;

namespace MultiDownloader.TelegramHost.Models
{
    public class User
    {
        [Key]
        public long ChatId { get; set; }
        public string Username { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string? LastName { get; set; } = String.Empty;
        public DateTime LastActivityDate { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
