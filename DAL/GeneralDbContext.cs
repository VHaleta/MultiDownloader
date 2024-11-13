using Microsoft.EntityFrameworkCore;
using MultiDownloader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDownloader.DAL
{
    public class GeneralDbContext : DbContext
    {
        public GeneralDbContext(DbContextOptions<GeneralDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Jobs)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.JobId);
        }
    }
}
