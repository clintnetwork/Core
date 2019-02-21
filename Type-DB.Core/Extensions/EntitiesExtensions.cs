using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeDB.Extensions
{
    public static class EntitiesExtensions
    {
        public static void AddEntity(this Database currentDatabase, string collection, Entity entity)
        {
            if(currentDatabase.Entities.Any(x => x.Key.Equals(collection)))
            {
                currentDatabase.Entities[collection][entity.Key] = entity;
            }
            else
            {
                currentDatabase.Entities.Add(collection, new Dictionary<string, Entity>());
                currentDatabase.Entities[collection][entity.Key] = entity;
            }
        }

        /// <summary>
        /// Get the specified entity type
        /// </summary>
        /// <param key="key">The entity key</param>
        public static string GetType(this Database currentDatabase, string collection, string key)
        {
            return currentDatabase.Entities[collection][key].Meta.Type;
        }

        /// <summary>
        /// Get the specified entity
        /// </summary>
        /// <param name="key">The entity key</param>
        public static T GetObject<T>(this Database currentDatabase, string collection, string key)
        {
            return (T)currentDatabase.Entities[collection][key].Value;
        }

        /// <summary>
        /// Get the specified entity
        /// </summary>
        /// <param name="key">The entity key</param>
        public static Entity GetEntity(this Database currentDatabase, string collection, string key)
        {
            return currentDatabase.Entities[collection][key];
        }

        /// <summary>
        /// Get the specified entity metas
        /// </summary>
        /// <param name="key">The entity key</param>
        public static Meta GetMeta(this Database currentDatabase, string collection, string key)
        {
            return currentDatabase.Entities[collection][key].Meta;
        }

        /// <summary>
        /// Return all the database Entities
        /// </summary>
        public static IEnumerable<T> GetAll<T>(this Database currentDatabase, string collection)
        {
            Dictionary<string, Entity> list = currentDatabase.Entities[collection];
            return list.Select(item => (T)item.Value.Value);
        }

        /// <summary>
        /// Return all databases Entities
        /// </summary>
        public static Dictionary<string, Dictionary<string, Entity>> GetAll(this Database currentDatabase) => currentDatabase.Entities;

        public static bool EntityExist(this Database currentDatabase, string collection, string key)
        {
            if(currentDatabase.Entities.Any(x => x.Key.Equals(collection)))
            {
                // if(currentDatabase.Entities.)
            }
            return false;
        }
    }
}