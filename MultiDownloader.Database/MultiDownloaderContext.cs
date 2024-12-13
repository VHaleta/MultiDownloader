using Microsoft.EntityFrameworkCore;
using MultiDownloader.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDownloader.Database
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
            modelBuilder.Entity<Job>()
                .HasOne(j => j.User)
                .WithMany(u => u.Jobs)
                .HasForeignKey(j => j.ChatId)
                .HasPrincipalKey(u => u.ChatId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
