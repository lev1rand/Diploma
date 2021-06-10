using DiplomaServices.Models;

namespace DiplomaServices.Interfaces
{
    public interface IAuthService
    {
        public AuthResponseModel SignIn(SignInModel model);
        public void SignOut(SignOutModel model);
        public string RefreshAccessToken(RefreshModel refreshModel);
    }
}
