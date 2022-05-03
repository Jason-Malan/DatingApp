using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options): base(options) {}

        public DbSet<PlatformUser>? PlatformUsers { get; set; }
        public DbSet<Photo>? Photos { get; set; }
    }
}  
