using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace JackSite.Authentication.PermissionServer
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CzPermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _resources;
        private static readonly IMemoryCache MemoryCache = new MemoryCache(new MemoryCacheOptions());
        private readonly TimeSpan _cacheDuration;

        public CzPermissionAttribute(params string[] resources)
        {
            _resources = resources ?? Array.Empty<string>();
            _cacheDuration = TimeSpan.FromMinutes(30); // 默认30分钟
        }

        public CzPermissionAttribute(string[] resources = null, int cacheMinutes = 30)
        {
            _resources = resources ?? Array.Empty<string>();
            _cacheDuration = TimeSpan.FromMinutes(cacheMinutes); // 默认30分钟
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}