using DataAccess.Entities.Test;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities.Answers
{
    //Right answers for questions from 1 to infinity possible answers
    public class RightSimpleAnswer
    {
        public float Grade { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
