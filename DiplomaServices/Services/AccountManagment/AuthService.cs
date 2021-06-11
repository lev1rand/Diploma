using DataAccess;
using System;
using DiplomaServices.Models;
using DiplomaServices.Interfaces;
using DiplomaServices.Mapping;
using Microsoft.Extensions.Caching.Memory;

namespace DiplomaServices.Services.AccountManagment
{
    public class AuthService : IAuthService
    {
        #region Fields

        private readonly IJWTManagmentService jwtService;

        private readonly IUnitOfWork uow;

        private readonly CachingService cachingService;

        private readonly MapperService mapper;

        private readonly PasswordHasher passwordHasher;

        #endregion

        #region Public Methods

        public AuthService(IJWTManagmentService jwtService, IUnitOfWork uow, IMemoryCache cache)
        {
            this.jwtService = jwtService;
            this.uow = uow;

            mapper = new MapperService();
            passwordHasher = new PasswordHasher();
            cachingService = new CachingService(cache);
        }
        public AuthResponseModel SignIn(SignInModel model)
        {
            var user = uow.Users.Get(u => u.Login == model.Login);
            if (user == null)
            {
                throw new Exception("Користувач не знайдений!");
            }
            else
            {
                if (!user.IsEmailVerified)
                {
                    throw new Exception("Електронна пошта не підтверджена!");
                }
                else
                {
                    var hashedTrial = passwordHasher.GetEncodedInfoWithSaltExisting(model.Password, user.Salt);

                    if (hashedTrial != user.Password)
                    {
                        throw new Exception("Невірний пароль! Будь ласка, перевірте введені дані.");
                    }
                    else
                    {
                        model.Password = user.Password;

                        var createTokenResponse = jwtService.CreateToken(mapper.Map<SignInModel, CreateTokenModel>(model));

                        var sessionKey = cachingService.CacheUserAuthData(createTokenResponse.RefreshToken,
                            createTokenResponse.AccessToken,
                            new UserModel
                            {
                                Id = createTokenResponse.UserId,
                                Login = createTokenResponse.UserLogin,
                                Name = createTokenResponse.UserName
                            });

                        return new AuthResponseModel
                        {
                            AccessToken = createTokenResponse.AccessToken,
                            RefreshToken = createTokenResponse.RefreshToken,
                            IsEmailVerified = createTokenResponse.IsEmailVerified,
                            UserName = createTokenResponse.UserName,
                            UserId = createTokenResponse.UserId,
                            UserLogin = createTokenResponse.UserLogin,
                            Role = createTokenResponse.Role,
                            SessionId = sessionKey
                        };
                    }
                }
            }
        }
        public void SignOut(SignOutModel model)
        {
            jwtService.RemoveToken(mapper.Map<SignOutModel, RemoveTokenModel>(model));
            cachingService.RemoveData(model.SessionId);
        }
        public string RefreshAccessToken(RefreshModel refreshModel)
        {
            var userConfig = cachingService.GetUserAuthDataFromCache(refreshModel.SessionId);

            if (userConfig is null)
            {
                throw new Exception(string.Format("Неможливо знайти сессію з id {0} !", refreshModel.SessionId));
            }

            if (userConfig.RefreshToken == refreshModel.RefreshToken)
            {
                var accessToken = jwtService.RefreshAccessToken(userConfig.UserLogin);

                cachingService.RemoveData(refreshModel.SessionId);
                cachingService.CacheUserAuthData(userConfig.RefreshToken,
                    accessToken,
                    new UserModel
                    {
                        Id = userConfig.UserId,
                        Name = userConfig.UserName,
                        Login = userConfig.UserLogin
                    },
                    refreshModel.SessionId);

                return accessToken;
            }
            else
            {
                throw new Exception("Невірний refresh-токен :с");
            }
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
