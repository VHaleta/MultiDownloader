using Microsoft.EntityFrameworkCore;
using MultiDownloader.TelegramHost.Models;

namespace MultiDownloader.TelegramHost.Database
{
    public class MultiDownloaderContext : DbContext
    {
        public DbSet<User> Users { get; set; }


        public MultiDownloaderContext(DbContextOptions<MultiDownloaderContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.ChatId)
                .ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);
        }
    }
}
