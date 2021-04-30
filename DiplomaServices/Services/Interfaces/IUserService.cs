using System.Collections.Generic;
using DiplomaServices.Models;

namespace DiplomaServices.Services.Interfaces
{
    public interface IUserService
    {
        public int CreateUser(CreateAccountModel model);
        public CreateAccountModel GetUserById(int id);
        public IEnumerable<CreateAccountModel> GetAll();
    }
}
