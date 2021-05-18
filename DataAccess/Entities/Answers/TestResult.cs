using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Answers
{
    public class TestResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public decimal SummaryGrade { get; set; }

        [Required]
        public int UserAnswerId { get; set; }
        public UserAnswer UserAnswer { get; set; }
    }
}
