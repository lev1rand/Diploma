using DataAccess.Entities.TestEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Answers
{
    public class UserAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int? ResponseOptionId { get; set; }
        public ResponseOption ResponseOption { get; set; }
    }
}
