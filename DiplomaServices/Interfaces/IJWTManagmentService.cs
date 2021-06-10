using DiplomaServices.Models;

namespace DiplomaServices.Interfaces
{
    public interface IJWTManagmentService
    {
        public AuthResponseModel CreateToken(CreateTokenModel createTokenModel);
        public string RefreshAccessToken(string login);
        public void RemoveToken(RemoveTokenModel removeTokenModel);
    }
}
