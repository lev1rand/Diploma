using System.Collections.Generic;
using DiplomaServices.Models;
using DiplomaServices.Pagination;

namespace DiplomaServices.Interfaces
{
    public interface IUserService
    {
        public int CreateUser(CreateAccountModel model);
        public CreateAccountModel GetUserById(int id);
        public IEnumerable<CreateAccountModel> GetAll();
        public CheckIfUserExistsResponseModel CheckIfUserExists(string login);
        public PagedResponse<List<UserModel>> GetUsersPaginated(PaginationFilter filter);
    }
}
