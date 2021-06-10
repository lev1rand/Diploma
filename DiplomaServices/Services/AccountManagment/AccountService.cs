using DataAccess;
using System;
using DiplomaServices.Models;
using DiplomaServices.Interfaces;

namespace DiplomaServices.Services.AccountManagment
{
    public class AccountService : IAccountService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly IUserService userService;

        #endregion

        public AccountService(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }
        public int CreateAccount(CreateAccountModel account)
        {
                if (uow.Users.GetSeveral(u => u.Login == account.Login).Count > 0)
                {
                    throw new Exception(string.Format("Користувач з електронною поштою {0} вже існує!", account.Login));
                }
                else
                {
                    return userService.CreateUser(account);
                }
            }
        }
    }

