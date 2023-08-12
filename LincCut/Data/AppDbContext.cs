using LincCut.Models;
using Microsoft.EntityFrameworkCore;

namespace LincCut.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}
        public DbSet<UrlInfo> urls { get; set; }
        public DbSet<Click> clicks { get; set; }
        public DbSet<Users> users { get; set; }
    }
}
