using LincCut.Models;
using Microsoft.EntityFrameworkCore;

namespace LincCut.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}
        public DbSet<UrlInfo> urls { get; set; }
        public DbSet<Click> clicks { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=shortener;Username=postgres;Password= ");
        //}
    }
}
