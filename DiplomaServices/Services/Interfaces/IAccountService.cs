using DiplomaServices.Models;

namespace DiplomaServices.Services.Interfaces
{
    public interface IAccountService
    {
        public int CreateAccount(CreateAccountModel account);
    }
}
