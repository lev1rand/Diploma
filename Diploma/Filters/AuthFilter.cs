using DiplomaServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;

namespace DiplomaAPI.Filters
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var service = context.HttpContext.RequestServices.GetService<IMemoryCache>();
            CacheUserAuthDataModel cacheData = null;

            context.HttpContext.Request.Body.Position = 0;
            StreamReader sr = new StreamReader(context.HttpContext.Request.Body);
            string bodyStringified = sr.ReadToEndAsync().Result;
            context.HttpContext.Request.Body.Position = 0;

            var bodyData = JsonConvert.DeserializeObject<AuthTemplateModel>(bodyStringified);

            service.TryGetValue(bodyData.SessionId, out cacheData);

            if(cacheData == null)
            {
                context.Result = new UnauthorizedResult();
            }

            var accessTokenIncoming = context.HttpContext.Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", "");

            var accessTokenFromCache = cacheData.AccessToken;

            if (accessTokenFromCache != accessTokenIncoming)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        public void OnAuthorizationExecuted(AuthorizationFilterContext context)
        {

        }
    }
}
