using System.ComponentModel.DataAnnotations;

namespace TestTaskServices.Models
{
    public class CodeModel
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Number { get; set; }

    }
}
