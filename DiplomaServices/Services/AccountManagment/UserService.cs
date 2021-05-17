using DataAccess;
using DataAccess.Entities;
using DiplomaServices.Interfaces;
using DiplomaServices.Mapping;
using DiplomaServices.Models;
using DiplomaServices.Pagination;
using System.Collections.Generic;
using System.Linq;

namespace DiplomaServices.Services.AccountManagment
{
    public class UserService : IUserService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        private readonly PasswordHasher passwordHasher;

        #endregion

        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
            passwordHasher = new PasswordHasher();
        }
        public int CreateUser(CreateAccountModel model)
        {
            var user = mapper.Map<CreateAccountModel, User>(model);

            var hashInfo = passwordHasher.GetEncodedInfoWithSaltGenerated(user.Password);

            user.Password = hashInfo.PasswordHash;
            user.Salt = hashInfo.Salt;

            uow.Users.Create(user);
            uow.Save();

            return user.Id;
        }
        public CreateAccountModel GetUserById(int id)
        {
            return mapper.Map<User, CreateAccountModel>(uow.Users.Get(u => u.Id == id));
        }
        public IEnumerable<CreateAccountModel> GetAll()
        {
            return mapper.Map<IQueryable<User>, IEnumerable<CreateAccountModel>>(uow.Users.GetAll()).ToList();
        }

        public CheckIfUserExistsResponseModel CheckIfUserExists(string login)
        {
            var user = uow.Users.Get(u => u.Login == login);
            var response = new CheckIfUserExistsResponseModel();
            response.Login = login;

            if (user == null)
            {
                response.ErrorMessage = "User was not found!";
                response.IsFound = false;

                return response;
            }

            if (!user.IsEmailVerified)
            {
                response.ErrorMessage = "User email is not verified!";
                response.IsFound = false;

                return response;
            }

            response.IsFound = true;

            return response;
        }

        public PagedResponse<List<UserModel>> GetUsersPaginated(PaginationFilter filter)
        {
            var totalCount = uow.Users.GetAll().Count();

            var pagedDataFromDb = uow.Users.GetPaginated(filter.PageNumber, filter.PageSize);
            var pagedDataForResponse = new List<UserModel>();
            foreach (var user in pagedDataFromDb)
            {
                pagedDataForResponse.Add(mapper.Map<User, UserModel>(user));
            }

            return new PagedResponse<List<UserModel>>(pagedDataForResponse, filter.PageNumber, filter.PageSize, totalCount);
        }
    }
}


