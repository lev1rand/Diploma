using TestTaskServices.Models;

namespace TestTaskServices.Services.Interfaces
{
    public interface IJWTManagmentService
    {
        public AuthResponseModel CreateToken(CreateTokenModel createTokenModel);

        public void RemoveToken(RemoveTokenModel removeTokenModel);

        public string RefreshAccessToken();
    }
}
