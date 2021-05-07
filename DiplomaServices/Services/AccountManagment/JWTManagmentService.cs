using DataAccess;
using DataAccess.Authentification;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DiplomaServices.Models;
using DiplomaServices.Services.Interfaces;

namespace DiplomaServices.Services.AccountManagment
{
    public class JWTManagmentService : IJWTManagmentService
    {
        #region private members

        private readonly IUnitOfWork uow;

        #endregion

        public JWTManagmentService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public AuthResponseModel CreateToken(CreateTokenModel createTokenModel)
        {
            var identity = GetIdentity(createTokenModel.Login, createTokenModel.Password);

            if (identity == null)
            {
                throw new Exception("Invalid username or password.");
            }

            var encodedJwt = GenerateAccessToken();

            var user = uow.Users.GetByPredicate(u => u.Login == identity.Name)[0];

            AuthResponseModel response = new AuthResponseModel
            {
                AccessToken = encodedJwt,
                RefreshToken = GenerateRefreshToken(),
                UserLogin = user.Login,
                UserName = user.Name,
                UserId = user.Id.ToString()
            };

            user.AccessToken = response.AccessToken;
            uow.Users.Update(user);
            uow.Save();
            
            return response;
        }

        public string RefreshAccessToken()
        {
            return GenerateAccessToken();
        }

        public void RemoveToken(RemoveTokenModel removeTokenModel)
        {
            var user = uow.Users.Get(u=>u.Id == removeTokenModel.UserId);
            user.AccessToken = null;
            uow.Users.Update(user);
            uow.Save();
        }

        private ClaimsIdentity GetIdentity(string userName, string password)
        {
            var users = uow.Users.GetByPredicate(x => x.Login == userName && x.Password == password);
            if (users != null && users.Count < 2)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, users[0].Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, users[0].Password)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            else
            {
                throw new KeyNotFoundException("User wasn't found!");
            }
        }
        private string GenerateAccessToken()
        {
            var jwt = new JwtSecurityToken(
                   issuer: AuthOptions.ISSUER,
                   audience: AuthOptions.AUDIENCE,
                   notBefore: DateTime.UtcNow,
                   expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                   signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
