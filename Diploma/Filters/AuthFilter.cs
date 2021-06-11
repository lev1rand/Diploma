using DiplomaServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
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

            var sessionId = GetSessionId(context);

            if(sessionId == null)
            {
                throw new Exception("Будь ласка, вкажіть id вашої сесії (параметр sessionId).");
            }

            service.TryGetValue(sessionId, out cacheData);

            if (cacheData == null)
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

        private string GetSessionId(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.Body.Position = 0;
            StreamReader sr = new StreamReader(context.HttpContext.Request.Body);
            string bodyStringified = sr.ReadToEndAsync().Result;
            context.HttpContext.Request.Body.Position = 0;

            if (bodyStringified == "")
            {
                StringValues queryData = "";
                context.HttpContext.Request.Query.TryGetValue("sessionId", out queryData);

                if (queryData.Count == 0)
                {
                    throw new Exception("Вам потрібно вказати id сесії в заголовку або тілі запиту (sessionId).");
                }
                else
                {
                    return queryData[0];
                }
            }
            else
            {
                var bodyData = JsonConvert.DeserializeObject<AuthTemplateModel>(bodyStringified);
                if (bodyData != null)
                {
                    return bodyData.SessionId;
                }
                else
                {
                    throw new Exception("Будь ласка, вкажіть id сесії в тілі або заголовку запиту (sessionId).");
                }
            }
        }
    }
}
