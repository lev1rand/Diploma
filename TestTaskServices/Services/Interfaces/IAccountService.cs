using TestTaskServices.Models;

namespace TestTaskServices.Services.Interfaces
{
    public interface IAccountService
    {
        public int CreateAccount(CreateAccountModel account);
    }
}
