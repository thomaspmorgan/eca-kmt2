using System;
namespace ECA.WebApi.Security
{
    public interface IUserCacheService
    {
        System.Runtime.Caching.CacheItem Add(UserCache userCache);
        UserCache GetUserCache(IWebApiUser user);
        System.Threading.Tasks.Task<UserCache> GetUserCacheAsync(IWebApiUser user);
    }
}
