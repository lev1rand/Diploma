using TestTaskServices.Models;

namespace TestTaskServices.Services.Interfaces
{
    public interface IAuthService
    {
        public AuthResponseModel SignIn(SignInModel model);
        public void SignOut(SignOutModel model);
        public string RefreshAccessToken();
    }
}
