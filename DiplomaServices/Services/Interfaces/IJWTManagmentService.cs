using DiplomaServices.Models;

namespace DiplomaServices.Services.Interfaces
{
    public interface IJWTManagmentService
    {
        public AuthResponseModel CreateToken(CreateTokenModel createTokenModel);
        public string RefreshAccessToken();
        public void RemoveToken(RemoveTokenModel removeTokenModel);
    }
}
