using Microsoft.EntityFrameworkCore;
using System;

namespace Weather.Classes
{
    public class WheatherContext : DbContext
    {
        public DbSet<CacheWheather> Cache { get; set; }
        public DbSet<UseAPI> ApiUsage { get; set; }

        public WheatherContext()
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    "server=127.0.0.1;port=3306;user=root;password=;database=weather;",
                    new MySqlServerVersion(new Version(8, 0, 11)));
            }
        }
    }
}