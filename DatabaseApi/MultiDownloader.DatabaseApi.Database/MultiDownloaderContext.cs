using Microsoft.EntityFrameworkCore;
using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.Database
{
    public class MultiDownloaderContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Job> Jobs { get; set; }


        public MultiDownloaderContext(DbContextOptions<MultiDownloaderContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.ChatId)
                .ValueGeneratedNever();

            modelBuilder.Entity<Job>()
                .HasOne(j => j.User)
                .WithMany(u => u.Jobs)
                .HasForeignKey(j => j.ChatId)
                .HasPrincipalKey(u => u.ChatId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
