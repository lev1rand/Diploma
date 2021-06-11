using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Fathername { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsEmailVerified { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public int Role { get; set; }

        [DefaultValue(null)]
        public string AccessToken { get; set; }

        //[DefaultValue(0)]
        //public int NotificationsCount { get; set; }
    }
}
