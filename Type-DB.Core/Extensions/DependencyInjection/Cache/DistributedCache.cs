using System;
using Microsoft.Extensions.DependencyInjection;

namespace TypeDB.Extensions.DependencyInjection.Cache
{
    public static class DistributedCache
    {
        /// <summary>
        /// Implement TypeDB Distributed Cache as dependency injection
        /// </summary>
        public static IServiceCollection AddTypeDBCache(this IServiceCollection services, Instance instance)
        {
            return null;
        }

        // public static IServiceCollection AddTypeDBCache(this IServiceCollection services, Action<TypeDBOptions> setupAction)
        // {
        //     return null;
        // }
    }
}