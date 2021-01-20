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
    }
}