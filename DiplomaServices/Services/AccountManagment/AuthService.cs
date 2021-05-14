using DataAccess;
using System;
using DiplomaServices.Models;
using DiplomaServices.Interfaces;
using DiplomaServices.Mapping;

namespace DiplomaServices.Services.AccountManagment
{
    public class AuthService : IAuthService
    {
        #region

        private readonly IJWTManagmentService jwtService;

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        private readonly PasswordHasher passwordHasher;

        #endregion
        public AuthService(IJWTManagmentService jwtService, IUnitOfWork uow)
        {
            this.jwtService = jwtService;
            this.uow = uow;

            mapper = new MapperService();
            passwordHasher = new PasswordHasher();
        }
        public AuthResponseModel SignIn(SignInModel model)
        {
            var user = uow.Users.Get(u => u.Login == model.Login);
            if (user == null)
            {
                throw new Exception("User was not found!");
            }
            else
            {
                if (!user.IsEmailVerified)
                {
                    throw new Exception("User email is not verified!");
                }
                else
                {
                    var hashedTrial = passwordHasher.GetEncodedInfoWithSaltExisting(model.Password, user.Salt);

                    if (hashedTrial != user.Password)
                    {
                        throw new Exception("Password isn't valid! Please, check your input!");
                    }
                    else
                    {
                        model.Password = user.Password;

                        return jwtService.CreateToken(mapper.Map<SignInModel, CreateTokenModel>(model));
                    }
                }
            }
        }

        public void SignOut(SignOutModel model)
        {
            jwtService.RemoveToken(mapper.Map<SignOutModel, RemoveTokenModel>(model));
        }

        public string RefreshAccessToken()
        {
            return jwtService.RefreshAccessToken();
        }
    }
}
