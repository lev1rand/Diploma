namespace DiplomaServices.Models
{
    public class AuthResponseModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserLogin { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public bool IsEmailVerified { get; set; }
        public int Role { get; set; }
        public string SessionId { get; set; }
    }
}
