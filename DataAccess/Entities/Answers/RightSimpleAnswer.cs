
using DataAccess.Entities.TestEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Answers
{
    //Right answers for questions from 1 to infinity possible answers
    public class RightSimpleAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public float Grade { get; set; }

        [Required]
        public int ResponseOptionId { get; set; }
        public ResponseOption ResponseOption { get; set; }
    }
}
