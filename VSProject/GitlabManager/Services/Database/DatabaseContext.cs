using System;
using GitlabManager.Services.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace GitlabManager.Services.Database
{
    /// <summary>
    /// Configures the sqlite database and registers custom converters
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<DbAccount> Accounts { get; set; }
        public DbSet<DbProject> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=gitlabmanager.db"
            );
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbProject>()
                // Array-To-String Converter for TagList
                .Property(e => e.TagList)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}