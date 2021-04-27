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

        #endregion

        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
            createValidator = new CreateUserValidator();
            updateValidator = new UpdateUserValidator();
        }
        public void CreateUser(CreateAccountModel model)
        {
           /* if (!createValidator.Validate(model).IsValid)
            {
                throw new ArgumentException(createValidator
                    .Validate(model)
                    .Errors
                    .First()
                    .ErrorMessage);
            }*/

            var user = mapper.Map<CreateAccountModel, User>(model);
            uow.Users.Create(user);
            uow.Save();
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


