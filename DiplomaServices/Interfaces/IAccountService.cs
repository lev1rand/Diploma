using DiplomaServices.Models;

namespace DiplomaServices.Interfaces
{
    public interface IAccountService
    {
        public int CreateAccount(CreateAccountModel account);
    }
}
