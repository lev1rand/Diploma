namespace DiplomaServices.Models
{
    public class CacheUserAuthDataModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLogin { get; set; }
    }
}
