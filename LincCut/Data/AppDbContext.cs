using LincCut.Models;
using LincCut.SnakeCaseConvertner;
using Microsoft.EntityFrameworkCore;

namespace LincCut.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConvertToSnakeCase();
        }
        public DbSet<UrlInfo> urls { get; set; }
        public DbSet<Click> clicks { get; set; }
        public DbSet<User> users { get; set; }
    }
}
