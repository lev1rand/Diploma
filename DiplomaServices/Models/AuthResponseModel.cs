﻿namespace DiplomaServices.Models
{
    public class AuthResponseModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserLogin { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}