using DataAccess;
using DataAccess.Authentification;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestTaskServices.Models;
using TestTaskServices.Services.Interfaces;

namespace TestTaskServices.Services
{
    public class JWTManagmentService : IJWTManagmentService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        #endregion

        public JWTManagmentService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
        }

        public string CreateToken(CreateTokenModel createTokenModel)
        {
            var identity = GetIdentity(createTokenModel.Login, createTokenModel.Password);

            if (identity == null)
            {
                throw new Exception("Invalid username or password.");
            }

            var now = DateTime.UtcNow;

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return JsonConvert.SerializeObject(response);
        }

        public void RemoveToken(RemoveTokenModel removeTokenModel)
        {

        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var users = uow.Users.GetByPredicate(x => x.Login == username && x.Password == password);
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

            // если пользователь не найден
            return null;
        }

        public string RefreshToken()
        {
            return null;
        }
    }
}
