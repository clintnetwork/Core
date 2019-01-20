using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeDB.Dumping;
using TypeDB.Interfaces;
using TypeDB.Utils;

namespace TypeDB
{
    /// <summary>
    /// TypeDB Database Object
    /// </summary>
    public class Database : IDatabase
    {

        /// <summary>
        /// The parent Instance
        /// </summary>
        public Instance Instance { get; set; }

        /// <summary>
        /// The Databae Name
        /// </summary>
        public string Name { get; set; }

        private List<Object> _objects;

        /// <summary>
        /// The TypeDB Objects list in the current Database
        /// </summary>
        /// TODO: implement pipeline
        public List<Object> Objects
        {
            get
            {
                //Console.WriteLine($"> object readed");
                return this._objects;
            }
            set
            {
                //Console.WriteLine($"> object writed");
                this._objects = value;
            }
        }

        /// <summary>
        /// Initialize the Database with parent Instance
        /// </summary>
        public Database(Instance instance)
        {
            this.Objects = new List<Object>();
            this.Instance = instance;
        }

        /// <summary>
        /// Get the specified object type
        /// </summary>
        /// <param key="key">The object key</param>
        /// <returns>The object type</returns>
        public Type GetType(string key) => this.Objects.Where(x => x.Key.Equals(key)).FirstOrDefault().Meta.Type;

        /// <summary>
        /// Get the specified object metas
        /// </summary>
        /// <param name="key">The object key</param>
        /// <returns></returns>
        public Meta GetMeta(string key) => this.Objects.Where(x => x.Key.Equals(key)).FirstOrDefault().Meta;

        /// <summary>
        /// Return if the specified object exist
        /// </summary>
        /// <param key="key">The key of the object</param>
        /// <returns>True or False</returns>
        public bool Exist(string key) => this.Objects.Any(x => x.Key.Equals(key));

        public string Print(string key) => JsonConvert.SerializeObject(this.Objects.Where(x => x.Key.Equals(key)).FirstOrDefault(), Formatting.Indented);
        public string PrintAll() => JsonConvert.SerializeObject(this.Objects, Formatting.Indented);

        /// <summary>
        /// Return all the database objects
        /// </summary>
        /// <returns>An Object list</returns>;
        public List<Object> GetAll() => this.Objects;

        /// <summary>
        /// Set a new Object or modify it if it already exists
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="key">Object Key</param>
        /// <param name="value">Object Value</param>
        /// <param name="createIfNotExist">Create the object if it does not already exist, true by default</param>
        
        public void Set<T>(string key, T value, bool createIfNotExist = true)
        {
            if (!this.Objects.Any(x => x.Key.Equals(key)) && createIfNotExist)
            {
                this.Objects.Add(new Object
                {
                    Key = key,
                    Value = value,
                    Meta = new Meta
                    {
                        OnCreated = DateTime.Now,
                        Type = value.GetType()
                    }
                });
            }
            else if (this.Objects.Any(x => x.Key.Equals(key)))
            {
                var getObject = this.Objects.Where(x => x.Key.Equals(key)).FirstOrDefault();
                getObject.Value = value;
                getObject.Meta = new Meta()
                {
                    OnEdited = DateTime.Now,
                    IsUpdated = true
                };
            }
            else
            {
                throw new TypeDBNotFoundException($"The object '{key}' does not exists");
            }
        }

        public int Update<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get an Object by Guid
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="guid">Object Guid to search</param>
        /*public T Get<T>(Guid guid)
        {
            throw new NotImplementedException();
        }*/

        /// <summary>
        /// Get an Object by Key
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="key">Object Key to search</param>
        public T Get<T>(string key)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(Objects.Where(x => x.Key.Equals(key)).FirstOrDefault().Value.ToString());
            }
            catch (InvalidCastException)
            {
                throw new Exception("The specified type is incorrect.");
            }
            catch (NullReferenceException)
            {
                throw new Exception("The specified object does not exist.");
            }
            catch (JsonReaderException)
            {
                throw new Exception("Unknown error.");
            }
        }

        public void Expire(string key, TimeSpan ttl)
        {
            if (this.Objects.Any(x => x.Key.Equals(key)))
            {
                var getObjectMeta = this.Objects.Where(x => x.Key.Equals(key)).FirstOrDefault().Meta;
                getObjectMeta.Expiration = DateTime.Now.AddMinutes(ttl.TotalMinutes);
                getObjectMeta.IsUpdated = true;
                this.Objects.Where(x => x.Key.Equals(key)).FirstOrDefault().Meta = getObjectMeta;
            }
        }

        public void Increment<T>(string key, T value)
        {
            if(value.IsNumericType())
            {
                if(this.Exist(key))
                {
                    this.Set(key, this.Get<double>(key) + (value as double?));
                }
                else
                {
                    this.Set(key, value);
                }
            }
            else
            {
                throw new TypeDBValueException("It's not a numeric type.");
            }
        }

        public void Decrement<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void Drop(string key)
        {
            throw new NotImplementedException();
        }

        public void Drop(Predicate<Object> keys)
        {
            throw new NotImplementedException();
        }

        public void Lock(string key)
        {
            throw new NotImplementedException();
        }

        public void Unlock(string key)
        {
            throw new NotImplementedException();
        }

        #region Development Stage
        public dynamic Merge(string key1, string key2)
        {
            var object1 = JObject.Parse(this.Get<object>(key1).ToString());
            object1.Merge(JObject.Parse(this.Get<object>(key2).ToString()), new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });
            return (dynamic)object1;
        }

        public void Cast<T1, T2>(string key)
        {
            var getObject = this.Objects.Where(x => x.Key.Equals(key)).FirstOrDefault();
            getObject.Meta = new Meta()
            {
                Type = typeof(T2).GetType()
            };
        }
        #endregion

        #region Dumping
        public string Dump()
        {
            throw new NotImplementedException();
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }

        public void DumpToFile(string filename)
        {
            throw new NotImplementedException();
        }

        public void RestoreFromFile(string filename)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}   