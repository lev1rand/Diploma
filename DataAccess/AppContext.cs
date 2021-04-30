using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity
                .ToTable("User")
                .HasKey(k => k.Id);

                entity
                .Property(i => i.Id).HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

                entity
                .Property(p => p.Name).HasColumnName("Name")
                .IsRequired();

                entity
                .Property(p => p.Login).HasColumnName("Login")
                .IsRequired();

                entity
                .Property(p => p.Password)
                .HasColumnName("PasswordHash")
                .IsRequired();

                entity
               .Property(p => p.IsEmailVerified)
               .HasColumnName("IsEmailVerified")
               .IsRequired();

                entity
               .Property(p => p.Salt)
               .HasColumnName("HashingSalt")
               .IsRequired();

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
