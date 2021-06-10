using DiplomaServices.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace DiplomaServices.Services
{
    public class CachingService
    {
        private readonly IMemoryCache cache;

        public CachingService(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public string CacheUserAuthData(string refreshToken, string accessToken, UserModel user)
        {
            var key = Guid.NewGuid().ToString();

            cache.Set(
               key,
                new CacheUserAuthDataModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserId = user.Id,
                    UserLogin = user.Login,
                    UserName = user.Name
                },
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
                });

            return key;
        }
        public CacheUserAuthDataModel GetUserAuthDataFromCache(string key)
        {
            CacheUserAuthDataModel cachedUserInfo = null;
            cache.TryGetValue(key, out cachedUserInfo);

            if (cachedUserInfo != null)
            {
                return cachedUserInfo;
            }
            else
            {
                throw new Exception(string.Format("Сессію з id {0} не знайдено.", key));
            }
        }
        public void RemoveData(string key)
        {
            cache.Remove(key);
        }
    }
}
