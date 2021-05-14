using DataAccess.Entities;
using DataAccess.Entities.Answers;
using DataAccess.Entities.ManyToManyEntities;
using DataAccess.Entities.TestEntities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppContext : DbContext
    {
        #region Db Sets

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<RightSimpleAnswer> RightSimpleAnswers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<ResponseOption> ResponseOptions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<UsersCourses> UsersCourses { get; set; }
        public DbSet<UsersTests> UsersTests { get; set; }

        #endregion

        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersCourses>()
            .HasKey(uc => new { uc.CourseId, uc.UserId });

            modelBuilder.Entity<UsersTests>()
            .HasKey(ut => new { ut.TestId, ut.UserId });
        }
    }
}
