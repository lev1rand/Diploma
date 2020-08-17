using System.ComponentModel.DataAnnotations;

namespace TestTaskServices.Models
{
    public class UpdateCodeModel: CodeModel
    {
        [Required]
        public override int Id { get; set; }
    }
}
