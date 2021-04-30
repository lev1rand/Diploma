using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TestTaskServices.Models;
using TestTaskServices.Services.Interfaces;
using TestTaskServices.Validation;

namespace TestTaskServices.Services
{
    public class UserService: IUserService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        private readonly CreateUserValidator createValidator;

        private readonly UpdateUserValidator updateValidator;

        private readonly PasswordHasher passwordHasher;

        #endregion

        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
            createValidator = new CreateUserValidator();
            updateValidator = new UpdateUserValidator();
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
            return mapper.Map<User, CreateAccountModel>(uow.Users.Get(id));
        }
        public IEnumerable<CreateAccountModel> GetAll()
        {
            return mapper.Map<IQueryable<User>, IEnumerable<CreateAccountModel>>(uow.Users.GetAll()).ToList();
        }

        /*public IEnumerable<UserModel> GetWithPagination()
        {
            return mapper.Map<IQueryable<User>, IEnumerable<UserModel>>(uow.Users.GetAll()).ToList();
        }*/

        public CodeModel Get(int id)
        {
            return mapper.Map<Code, CodeModel>(uow.Codes.Get(id));
        }

    }
}


