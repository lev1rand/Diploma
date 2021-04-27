using System.Collections.Generic;
using TestTaskServices.Models;

namespace TestTaskServices.Services.Interfaces
{
    public interface IUserService
    {
        public void CreateUser(CreateAccountModel model);
        public CreateAccountModel GetUserById(int id);
        public IEnumerable<CreateAccountModel> GetAll();
    }
}
