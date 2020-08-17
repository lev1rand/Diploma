using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class TestContext : DbContext
    {
        public DbSet<Code> Codes { get; set; }
        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Code>(entity =>
            {
                entity
                .ToTable("Codes")
                .HasKey(k => k.Id);

                entity
                .Property(i => i.Id).HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

                entity
                .Property(p => p.Name).HasColumnName("Name")
                .IsRequired();

                entity
                .Property(p => p.Number).HasColumnName("Number")
                .HasMaxLength(3)
                .IsRequired();

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
