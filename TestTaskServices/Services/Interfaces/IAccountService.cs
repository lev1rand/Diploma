using TestTaskServices.Models;

namespace TestTaskServices.Services.Interfaces
{
    public interface IAccountService
    {
        public void CreateAccount(CreateAccountModel account);
    }
}
