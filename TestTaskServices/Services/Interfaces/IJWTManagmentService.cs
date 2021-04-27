using TestTaskServices.Models;

namespace TestTaskServices.Services.Interfaces
{
    public interface IJWTManagmentService
    {
        public string CreateToken(CreateTokenModel createTokenModel);

        public void RemoveToken(RemoveTokenModel removeTokenModel);
        public string RefreshToken();
    }
}
