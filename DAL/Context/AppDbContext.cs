using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductEntity>()
               .HasMany(p => p.Users)
               .WithMany(u => u.Products)
               .UsingEntity(j => j.ToTable("UserProducts"));

            modelBuilder.Entity<ProductEntity>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
