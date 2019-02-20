using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using TypeDB.Extensions;

namespace TypeDB.Interception
{
    internal static class InterceptorExtensions
    {
        /// <summary>
        /// Interceptor implemented in each Setter Methods, this interceptor break the execution of the caller method when it's needed
        /// </summary>
        internal static bool Interceptor(this Database currentDatabase, Instance instance, MethodBase callerMethod, DTO dto)
        {
            if (callerMethod.GetCustomAttributes(typeof(InterceptionAttribute), true).Any())
            {
                InterceptionAttribute interceptionAttribute = callerMethod.GetCustomAttributes(typeof(InterceptionAttribute), true).FirstOrDefault() as InterceptionAttribute;

                // Break the execution of the Method if Entity is Locked
                if (currentDatabase.Exist(dto.Collection, dto.Key))
                {
                    var entity = currentDatabase.GetEntity(dto.Collection, dto.Key);
                    if(entity.Meta.IsLocked)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Execute the Persistence if the Persistence Type is Iteration
        /// </summary>
        internal static void PersistenceInterceptor(this Database currentDatabase, Instance instance)
        {
            if (instance.Configuration.Persistence.Type == PersistenceType.Iteration)
            {
                instance.Configuration.Persistence.Invoke(instance);
            }
        }
    }
}