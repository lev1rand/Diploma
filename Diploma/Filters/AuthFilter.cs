using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DiplomaAPI.Filters
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(context.HttpContext.Session.Get("userId") == null)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        public void OnAuthorizationExecuted(AuthorizationFilterContext context)
        {
            
        }
    }
}
