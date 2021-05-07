using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text;

namespace DiplomaAPI.Filters
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var accessToken = context.HttpContext.Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", "");
            var accessTokenFromSession = new string(Encoding.ASCII.GetChars(context.HttpContext.Session.Get("accessToken")));

            if (context.HttpContext.Session.Get("userId") == null ||
                accessTokenFromSession != accessToken)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        public void OnAuthorizationExecuted(AuthorizationFilterContext context)
        {

        }
    }
}
