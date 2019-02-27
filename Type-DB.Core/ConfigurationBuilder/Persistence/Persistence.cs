using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using TypeDB.Exceptions;
using TypeDB.Extensions;

namespace TypeDB
{
    public class Persistence
    {
        public PersistenceType Type { get; set; } = PersistenceType.None;

        /// <summary>
        /// Define the number of objects pr. chunk file stored on disk. If your structures are large, consider making this smaller.
        /// This variable cannot be changed after database has been created.
        /// </summary>
        public int? ChunkSize { get; set; } = null;

        /// <summary>
        /// Define the local path where to stored all databases created in this app instance.
        /// </summary>
        public string Location { get; set; } = TemporaryLocation;

        /// <summary>
        /// Define the Persistence interval to perform the data writing, by default every 10 seconds
        /// </summary>
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(10);

        public static string TemporaryLocation { get => Path.Combine(Path.GetTempPath(), "TypeDB"); }
        public static string ApplicationDataLocation { get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TypeDB"); }
        public static string CurrentLocation { get => Directory.GetCurrentDirectory(); }

        /// <summary>
        /// Fetch the datas from the Location property and add it 
        /// </summary>
        /// <param name="instance">Define the parent instance</param>
        internal void Fetch(Instance instance)
        {
            if (!Directory.Exists(Location))
            {
                return;
            }
            
            var persistenceFiles = Directory.EnumerateFiles(Location, "*.*", SearchOption.TopDirectoryOnly).Where(x => x.EndsWith(".tdb") || x.EndsWith(".db")).ToArray();
            foreach (var file in persistenceFiles)
            {
                var persistenceFileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                var firstDotForSplit = persistenceFileNameWithoutExtension.IndexOf(".");
                if(firstDotForSplit != -1)
                {
                    var database = persistenceFileNameWithoutExtension.Substring(0, firstDotForSplit);
                    var collection = persistenceFileNameWithoutExtension.Substring(firstDotForSplit + 1);
                    using (var db = instance.OpenDatabase(database, true))
                    {
                        foreach (var entityItem in File.ReadAllLines(file).ToList())
                        {
                            var entity = JsonConvert.DeserializeObject<Entity>(entityItem);
                            if(entity.Meta.Expiration.HasValue)
                            {
                                instance.TimersHoster.Add(new Timer(x => db.Drop(collection, entity.Key), null, entity.Meta.Expiration.Value, TimeSpan.Zero));
                            }
                            db.AddEntity(collection, entity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Invoke the Persistence of all the objects, this method is generally called from a Persistence Timer
        /// </summary>
        /// <param name="instance">Define the parent instance</param>
        public void Invoke(Instance instance)
        {
            if(!Directory.Exists(Location))
            {
                try
                {
                    Directory.CreateDirectory(Location);
                }
                catch
                {
                    throw new TypeDBGeneralException($"Unable to use the '{Location}' path for persistence");
                }
            }

            if(this.ChunkSize.HasValue)
            {
                // TODO: implement chunked persistence
            }
            else
            {
                foreach(var database in instance.Databases.Where(x => !string.IsNullOrWhiteSpace(x.Name)))
                {
                    foreach(var collection in database.Entities)
                    {
                        var databaseFilename = Path.Combine(Location, database.Name + "." + collection.Key + ".db");
                        File.WriteAllLines(databaseFilename, collection.Value.Select(x => JsonConvert.SerializeObject(x.Value)).ToArray());
                    }
                }
            }
        }
    }
}
