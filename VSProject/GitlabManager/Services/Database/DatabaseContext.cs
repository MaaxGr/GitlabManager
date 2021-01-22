using System;
using GitlabManager.Services.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace GitlabManager.Services.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=gitlabmanager.db"
            );
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                // Array-To-String Converter for TagList
                .Property(e => e.TagList)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}