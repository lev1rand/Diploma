using System.ComponentModel.DataAnnotations;
using DataAccess.Entities.TestEntities;

namespace DataAccess.Entities.ManyToManyEntities
{
    public class UsersTests
    {
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}
