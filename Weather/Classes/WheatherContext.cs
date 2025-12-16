using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Weather.Classes
{
    public class WheatherContext: DbContext
    {
        public DbSet<CacheWheather> Cache { get; set; }
        public DbSet<UseAPI> ApiUsage { get; set; }

        static WheatherContext()
        {
            using (var context = new WheatherContext())
            {
                context.Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=127.0.0.1;port=3306;database=weatherdb;user=root;password=;",
                new MySqlServerVersion(new Version(8, 0, 11)));
        }
    }
}
