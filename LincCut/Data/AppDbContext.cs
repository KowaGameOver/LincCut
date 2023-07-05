using LincCut.Models;
using Microsoft.EntityFrameworkCore;

namespace LincCut.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UrlInfo> UrlInfos { get; set; }
        public DbSet<Click> Clicks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=CutUrlDB;Username=postgres;Password= ");
        }
    }
}
