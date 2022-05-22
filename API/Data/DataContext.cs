using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext: DbContext
    {
        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options): base(options) {}

        public DbSet<PlatformUser>? PlatformUsers { get; set; }
        public DbSet<Photo>? Photos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=DatingDB;Integrated Security=True");
            }
        }
    }
}  
