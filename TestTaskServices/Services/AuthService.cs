using TestTaskServices.Models;
using TestTaskServices.Services.Interfaces;

namespace TestTaskServices.Services
{
    public class AuthService : IAuthService
    {
        IJWTManagmentService jwtService;

        MapperService mapper;
        public AuthService(IJWTManagmentService jwtService)
        {
            this.jwtService = jwtService;
            mapper = new MapperService();
        }
        public string SignIn(SignInModel model)
        {
            return jwtService.CreateToken(mapper.Map<SignInModel, CreateTokenModel>(model));
        }

        public void SignOut(SignOutModel model)
        {
            jwtService.RemoveToken(mapper.Map<SignOutModel, RemoveTokenModel>(model));
        }
    }
}
