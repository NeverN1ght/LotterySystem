using LotterySystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LotterySystem.Data.Contexts
{
    public class SqlDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public SqlDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureDeleted();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Albums)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Album>()
                .HasMany(a => a.Photos)
                .WithOne(p => p.Album)
                .HasForeignKey(a => a.AlbumId);
        }
    }
}
