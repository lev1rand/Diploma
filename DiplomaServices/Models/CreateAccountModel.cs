namespace DiplomaServices.Models
{
    public class CreateAccountModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fathername { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public bool IsEmailVerified { get; set; }

    }
}
