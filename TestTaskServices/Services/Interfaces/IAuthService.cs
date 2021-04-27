using TestTaskServices.Models;

namespace TestTaskServices.Services.Interfaces
{
    public interface IAuthService
    {
        public string SignIn(SignInModel model);
        public void SignOut(SignOutModel model);
    }
}
