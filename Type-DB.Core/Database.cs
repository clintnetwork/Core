using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TypeDB.Annotations;
using TypeDB.Exceptions;
using TypeDB.Interception;
using TypeDB.Utils;

namespace TypeDB
{
    /// <summary>
    /// TypeDB Database Object
    /// </summary>
    public class Database : IDisposable
    {
        private const string DefaultCollectionName = "default";

        /// <summary>
        /// The parent Instance
        /// </summary>
        internal Instance Instance { get; set; }

        /// <summary>
        /// The Databae Name
        /// </summary>
        public string Name { get; set; }

        //TODO: to reformat
        public List<string> IdNames { get; set; }
        public long CollectionCount => Entities.LongCount();

        /// <summary>
        /// The TypeDB Entities list in the current Database
        /// </summary>
        /// TODO: implement pipeline
        internal Dictionary<string, Dictionary<string, Entity>> Entities { get; set; }

        /// <summary>
        /// Initialize the Database with parent Instance
        /// </summary>
        internal Database(Instance instance)
        {
            Entities = new Dictionary<string, Dictionary<string, Entity>>();
            IdNames = new List<string> { "Id" };
            Instance = instance;
        }

        /// <summary>
        /// Set a text Entity
        /// </summary>
        /// <param name="key">Entity key</param>
        /// <param name="value">Entity value (string only)</param>
        /// <param name="createIfNotExist">Define if the entity will be created if does not exists</param>
        public void SetText(string key, string value, bool createIfNotExist = true)
        {
            // TODO: maybe use System.String for collection name ?
            Set(DefaultCollectionName, key, value, createIfNotExist);
        }

        /// <summary>
        /// Get the text value stored. Use the Text variation of Get and Set if you want to perform your own serialization.
        /// </summary>
        /// <param name="key">Entity key</param>
        public string GetText(string key)
        {
            // TODO: maybe use System.String for collection name ?
            return (string)Entities[DefaultCollectionName][key].Value;
        }

        /// <summary>
        /// Return if the specified Entity exist
        /// </summary>
        /// <param name="key">Entity key</param>
        public bool Exist(string key)
        {
            var collection = GetCollectionByEntity(key);
            return Exist(collection, key);
        }

        /// <summary>
        /// Return if the specified Entity exist
        /// </summary>
        /// <param name="collection">Entity collection</param>
        /// <param name="key">Entity key</param>
        public bool Exist(string collection, string key)
        {
            try
            {
                return Entities[collection].Any(x => x.Key.Equals(key));
            }
            catch (KeyNotFoundException)
            {
                return false;
                // throw new TypeDBNotFoundException($"The '{collection}' collection does not exist.");
            }
        }

        /// <summary>
        /// Set an Entity
        /// </summary>
        /// <param name="value">Entity Value</param>
        /// <param name="createIfNotExist">Define if the entity will be created if does not exists</param>
        public void Set<T>(T value, bool createIfNotExist = true)
        {
            string objectKey = GetObjectIdentifier(value);

            if (string.IsNullOrWhiteSpace(objectKey))
            {
                throw new TypeDBIdentifierNotFoundException("Unable to find object identifier. Please verify that the property marked with [Key] or specified with the IdNames contains a value.");
            }

            string collectionKey = GetCollectionIdentifier(value);

            Set(collectionKey, objectKey, value, createIfNotExist);
        }

        /// <summary>
        /// Set an Entity by his key
        /// </summary>
        /// <param name="key">Entity key</param>
        /// <param name="value">Entity value</param>
        /// <param name="createIfNotExist">Define if the entity will be created if does not exists</param>
        public void Set<T>(string key, T value, bool createIfNotExist = true)
        {
            string collectionKey = GetCollectionIdentifier(value);

            Set(collectionKey, key, value, createIfNotExist);
        }

        /// <summary>
        /// Set an Entity by his key and collection
        /// </summary>
        /// <param name="collection">Entity collection</param>
        /// <param name="key">Entity key</param>
        /// <param name="value">Entity Value</param>
        /// <param name="createIfNotExist">Define if the entity will be created if does not exists</param>
        [Interception(MethodName = nameof(Set), Type = InterceptionType.Setter)]
        public void Set<T>(string collection, string key, T value, bool createIfNotExist = true)
        {
            #region Setter Method Interceptor (SMI)
            if (this.Interceptor(Instance, MethodBase.GetCurrentMethod(), new DTO { Collection = collection, Key = key, Value = value })) return;
            #endregion
            Dictionary<string, Entity> coll;

            // Add the collection if it does not exists. In the future we should consider allowing
            // parameters/configuration to be supplied or manual configuration of collections.
            if (!Entities.ContainsKey(collection))
            {
                coll = Entities[collection] = new Dictionary<string, Entity>();
            }
            else
            {
                coll = Entities[collection];
            }

            if (coll.ContainsKey(key))
            {
                Entity entity = coll[key];
                entity.Value = value;
                entity.Meta.OnEdited = DateTime.UtcNow;
                entity.Meta.IsUpdated = true;
                entity.Meta.Version++;
                MarkDirty(entity);
            }
            else if (createIfNotExist)
            {
                Entity entity = new Entity
                {
                    Key = key,
                    Value = value,
                    Meta = new Meta
                    {
                        OnCreated = DateTime.UtcNow,
                        OnEdited = DateTime.UtcNow,
                        Type = value.GetType().Name
                    }
                };

                coll.Add(key, entity);
                MarkDirty(entity);
            }
            else
            {
                throw new TypeDBNotFoundException($"The entity '{key}' does not exists");
            }

            // Execute the Persistence when needed
            this.PersistenceInterceptor(Instance);
        }

        /// <summary>
        /// Get an Entity by his key and collection
        /// </summary>
        /// <param name="key">Entity key</param>
        public T Get<T>(string key)
        {
            return Get<T>(typeof(T).FullName, key);
        }

        /// <summary>
        /// Get an Entity by his key and collection
        /// </summary>
        /// <param name="collection">Entity collection</param>
        /// <param name="key">Entity key</param>
        public T Get<T>(string collection, string key)
        {
            if (Exist(collection, key))
            {
                return (T)Entities[collection].FirstOrDefault(x => x.Key.Equals(key)).Value.Value;
            }
            else
            {
                throw new TypeDBValueException($"Unable to get the specified entity '{key}'.");
            }
        }

        // TODO: Figure out if we should have a singular Parallel implementation that always returns Entity and then
        // translate that into a dictionary of explicit generic type, or if we should optimize and avoid additional dictionary.
        // For now we'll leave it with explicit dictionary, and then we must perform performance test validation.
        public Dictionary<string, T> Filter<T>(string collection, Func<T, bool> predicate, bool parallel = false)
        {
            // #region Setter Method Interceptor (SMI)
            // if (this.Interceptor(Instance, MethodBase.GetCurrentMethod(), new DTO { Collection = collection })) return;
            // #endregion

            Dictionary<string, Entity> coll = Entities[collection];

            Dictionary<string, T> filteredList = new Dictionary<string, T>();

            if (parallel)
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                // Use ParallelOptions instance to store the CancellationToken
                ParallelOptions po = new ParallelOptions
                {
                    CancellationToken = cts.Token
                };

                Parallel.ForEach(coll, po, (entity) =>
                {
                    T ent = (T)entity.Value.Value;
                    bool include = predicate.Invoke(ent);

                    if (include)
                    {
                        lock (filteredList)
                        {
                            filteredList.Add(entity.Key, (T)entity.Value.Value);
                        }
                    }
                });
            }
            else
            {
                foreach (KeyValuePair<string, Entity> entity in coll)
                {
                    T ent = (T)entity.Value.Value;
                    bool include = predicate.Invoke(ent);

                    if (include)
                    {
                        filteredList.Add(entity.Key, (T)entity.Value.Value);
                    }
                }
            }

            return filteredList;
        }

        // See TODO comment on the other Filter method.
        //public Dictionary<string, Entity> FilterEntities<T>(string collection, Func<T, bool> predicate)
        //{
        //    var coll = this.Entities[collection];

        //    Dictionary<string, Entity> filteredList = new Dictionary<string, Entity>();

        //    CancellationTokenSource cts = new CancellationTokenSource();

        //    // Use ParallelOptions instance to store the CancellationToken
        //    ParallelOptions po = new ParallelOptions();
        //    po.CancellationToken = cts.Token;

        //    Parallel.ForEach(coll, po, (entity) =>
        //    {
        //        var ent = (T)entity.Value.Value;
        //        var include = predicate.Invoke(ent);

        //        if (include)
        //        {
        //            filteredList.Add(entity.Key, entity.Value);
        //        }
        //    });

        //    return filteredList;
        //}

        //public IEnumerable<T> Filter<T>(string collection, Func<KeyValuePair<string, Entity>, bool> predicate)
        //{
        //    var coll = this.Entities[collection];
        //    var result = coll.Where(predicate).Select(item => (T)item.Value.Value);
        //    return result;

        //    Expression<Func<Car, bool>>
        //}

        /// <summary>
        /// Define an expiration DateTime of an Entity by his key
        /// </summary>
        /// <param name="key">Entity key</param>
        /// <param name="ttl">Time to Live</param>
        public void Expire(string key, TimeSpan ttl)
        {
            var collection = GetCollectionByEntity(key);
            Expire(collection, key, ttl);
        }

        /// <summary>
        /// Define an expiration DateTime of an Entity by his key and collection
        /// </summary>
        /// <param name="collection">Entity collection</param>
        /// <param name="key">Entity key</param>
        /// <param name="ttl">Time to Live</param>
        [Interception(MethodName = nameof(Expire), Type = InterceptionType.Setter)]
        public void Expire(string collection, string key, TimeSpan ttl)
        {
            #region Setter Method Interceptor (SMI)
            if (this.Interceptor(Instance, MethodBase.GetCurrentMethod(), new DTO { Collection = collection, Key = key })) return;
            #endregion

            if (Exist(collection, key))
            {
                Meta metas = Entities[collection][key].Meta;
                metas.Expiration = ttl;
                metas.IsUpdated = true;
                Entities[collection][key].Meta = metas;

                Instance.TimersHoster.Add(new Timer(x => Drop(collection, key), null, ttl, TimeSpan.Zero));
            }

            // Execute the Persistence when needed
            this.PersistenceInterceptor(Instance);
        }

        /// <summary>
        /// Lock an Entity by his key
        /// </summary>
        /// <param name="key">Entity key</param>
        public void Lock(string key)
        {
            var collection = GetCollectionByEntity(key);
            Lock(collection, key);
        }

        /// <summary>
        /// Lock an Entity by his key and collection
        /// </summary>
        /// <param name="collection">Entity collection</param>
        /// <param name="key">Entity key</param>
        // [Interception(MethodName = nameof("Lock"), Type = InterceptionType.Setter)]
        public void Lock(string collection, string key)
        {
            #region Setter Method Interceptor (SMI)
            if (this.Interceptor(Instance, MethodBase.GetCurrentMethod(), new DTO { Collection = collection, Key = key })) return;
            #endregion

            if (Exist(collection, key))
            {
                Meta metas = Entities[collection][key].Meta;
                metas.IsLocked = true;
                metas.IsUpdated = true;
                Entities[collection][key].Meta = metas;
            }

            // Execute the Persistence when needed
            this.PersistenceInterceptor(Instance);
        }

        /// <summary>
        /// Unlock an Entity by his key
        /// </summary>
        /// <param name="key">Entity key</param>
        public void Unlock(string key)
        {
            var collection = GetCollectionByEntity(key);
            Unlock(collection, key);
        }

        /// <summary>
        /// Unlock an Entity by his key and collection
        /// </summary>
        /// <param name="collection">Entity collection</param>
        /// <param name="key">Entity key</param>
        public void Unlock(string collection, string key)
        {
            #region Setter Method Interceptor (SMI)
            if (this.Interceptor(Instance, MethodBase.GetCurrentMethod(), new DTO { Collection = collection, Key = key })) return;
            #endregion

            if (Exist(collection, key))
            {
                Meta metas = Entities[collection][key].Meta;
                metas.IsLocked = false;
                metas.IsUpdated = true;
                Entities[collection][key].Meta = metas;
            }

            // Execute the Persistence when needed
            this.PersistenceInterceptor(Instance);
        }

        public void Decrement<T>(string key, T value) where T : struct
        {
            // Increment(key, value * -1);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Increment the value of an Entity
        /// </summary>
        public void Increment<T>(string key, T value) where T : struct
        {
            // #region Setter Method Interceptor (SMI)
            // Interceptor(Instance, MethodBase.GetCurrentMethod(), new DTO { Collection = collection, Key = key, Value = value });
            // #endregion

            // TODO: use Exist()
            // TODO: found a solution to have two types incrementation
            if (value.IsNumericType())
            {
                try
                {
                    var previousValue = Get<T>(key);
                    if(previousValue.IsNumericType())
                    {                        
                        var newValue = Convert.ToInt32(previousValue) + Convert.ToInt32(value);
                        Set(previousValue.GetType().FullName, key, newValue);
                    }
                    else
                    {
                        throw new TypeDBValueException("Unable to Increment the value, it's not a numeric type.");
                    }
                }
                catch (TypeDBNotFoundException)
                {
                    throw new TypeDBNotFoundException("Unable to Increment two different entities types");
                }
                catch (TypeDBValueException)
                {
                    throw new TypeDBValueException("Unable to Increment the value, it's not a numeric type or the entity does not exist.");
                }
            }
            else
            {
                throw new TypeDBValueException("Unable to Increment the value, it's not a numeric type.");
            }

            // Execute the Persistence when needed
            this.PersistenceInterceptor(Instance);
        }

        /// <summary>
        /// Drop an Entity by selecting his key
        /// </summary>
        /// <param name="key">Entity key</param>
        public void Drop(string key)
        {
            var collection = GetCollectionByEntity(key);
            Drop(collection, key);
        }

        /// <summary>
        /// Drop an Entity by selecting his key and collection
        /// </summary>
        /// <param name="collection">Entity collection</param>
        /// <param name="key">Entity key</param>
        [Interception(MethodName = nameof(Drop), Type = InterceptionType.Setter)]
        public void Drop(string collection, string key)
        {
            #region Setter Method Interceptor (SMI)
            if (this.Interceptor(Instance, MethodBase.GetCurrentMethod(), new DTO { Collection = collection, Key = key })) return;
            #endregion

            Entities[collection].Remove(key);

            // Execute the Persistence when needed
            this.PersistenceInterceptor(Instance);
        }

        /// <summary>
        /// Clear Database Entities of a specified collection
        /// </summary>
        /// <param name="collection">Collection to clear</param>

        [Interception(MethodName = nameof(Clear), Type = InterceptionType.Setter)]
        public void Clear(string collection)
        {
            #region Setter Method Interceptor (SMI)
            if (this.Interceptor(Instance, MethodBase.GetCurrentMethod(), new DTO { Collection = collection })) return;
            #endregion

            Entities[collection].Clear();

            // Execute the Persistence when needed
            this.PersistenceInterceptor(Instance);
        }

        /// <summary>
        /// Clear all Databases Entities
        /// </summary>
        [Interception(MethodName = nameof(Clear), Type = InterceptionType.Setter)]
        public void Clear()
        {
            #region Setter Method Interceptor (SMI)
            if (this.Interceptor(Instance, MethodBase.GetCurrentMethod(), new DTO { })) return;
            #endregion

            Entities.Clear();

            // Execute the Persistence when needed
            this.PersistenceInterceptor(Instance);
        }

        public void Dispose() { }

        private void MarkDirty(Entity entity)
        {
            // TODO: Implement logic to perform persistence when object becomes dirty.
            // TODO: added by clint: if the persistence is configured as Iteration Mode
            entity.Meta.IsDirty = true;
        }

        /// <summary>
        /// Get the first collection name founded for a specified Entity
        /// </summary>
        internal string GetCollectionByEntity(string key)
        {
            if (Entities.Any(x => x.Value.Keys.Contains(key)))
            {
                return Entities.Where(x => x.Value.Keys.Contains(key)).FirstOrDefault().Key;
            }
            else
            {
                throw new TypeDBGeneralException($"Unable to find '{key}' entity in any collection.");
            }
        }

        private object GetObjectIdentifierValue(object value)
        {
            string key = string.Empty;

            PropertyInfo[] props = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            // First scan for IdAttribute.
            foreach (PropertyInfo prop in props)
            {
                object[] attributes = prop.GetCustomAttributes(typeof(IdAttribute), false);

                if (attributes.Any())
                {
                    return prop.GetValue(value);
                }
            }

            // Scan for properties with the supported identifier names.
            foreach (PropertyInfo prop in props)
            {
                if (IdNames.Contains(prop.Name))
                {
                    return prop.GetValue(value);
                }
            }

            return key;
        }

        private string GetObjectIdentifier(object value)
        {
            object identifierValue = GetObjectIdentifierValue(value);
            string key;

            if (identifierValue == null)
            {
                key = string.Empty;
            }
            else
            {
                key = identifierValue.ToString();
            }

            return key;
        }

        private string GetCollectionIdentifier(object value)
        {
            Type type = value.GetType();
            CollectionAttribute attribute = type.GetCustomAttribute<CollectionAttribute>(true);

            if (attribute != null)
            {
                return attribute.Name;
            }

            return type.FullName;
        }
    }
}