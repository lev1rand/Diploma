namespace DiplomaServices.Models
{
    public class CheckIfUserExistsResponseModel
    {
        public string ErrorMessage { get; set; }
        public string Login { get; set; }
        public bool IsFound { get; set; }
    }
}
