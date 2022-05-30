using API.Entities;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace API.Data
{
    public class DataContext: DbContext
    {
        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options): base(options) {}

        public DbSet<PlatformUser> PlatformUsers { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserLike> UserLike { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=DatingDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                .HasKey(x => new { x.SourceUserId, x.LikedUserId });
            //builder.Entity<UserLike>()
            //    .HasNoKey();
        }

    }
}  
