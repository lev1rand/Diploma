using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities.ManyToManyEntities
{
    public class UsersCourses
    {
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
