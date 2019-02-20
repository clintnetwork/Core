using System;
using System.Collections.Generic;
using System.Linq;
using TypeDB.Exceptions;
using System.Reflection;

namespace TypeDB
{
    /// <summary>
    /// TypeDB Instance Object
    /// </summary>
    public class Instance : IDisposable
    {
        private readonly Core Root;

        [Obsolete]
        internal string Uid = Guid.NewGuid().ToString();

        /// <summary>
        /// TypeDB Instance Configuration
        /// </summary>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// TypeDB Database List
        /// </summary>
        /// TODO: implement pipeline
        internal List<Database> Databases { get => this.Root.Pipeline.GetDatabases(); set => this.Root.Pipeline.SetDatabases(this.Databases, value); }

        /// <summary>
        /// Initialize a TypeDB Instance and the Database List
        /// </summary>
        public Instance(Core core)
        {
            this.Root = core;
        }

        /// <summary>
        /// Create a new TypeDB database
        /// </summary>
        /// <param name="databaseName">The database name, 'default' is the default database</param>
        /// <exception cref="TypeDBGeneralException">TypeDBGeneralException</exception>
        public void CreateDatabase(string databaseName = "default")
        {
            if (this.Databases.Any(x => x.Name.Equals(databaseName)))
            {
                throw new TypeDBGeneralException($"The database '{databaseName}' already exists.");
            }
            else
            {
                SetAndEmitDatabase(new Database(this)
                {
                    Name = databaseName
                });
            }
        }

        /// <summary>
        /// Open a TypeDB database
        /// </summary>
        /// <param name="databaseName">'default' is the default database name</param>
        /// <param name="createIfNotExist">If the database does not exist, it will created automatically</param>
        /// <exception cref="TypeDBNotFoundException">TypeDBNotFoundException</exception>
        public Database OpenDatabase(string databaseName = "default", bool createIfNotExist = false)
        {
            // if (configuration != null)
            // {
            //     this.Configuration = configuration;

            //     // TODO: Create code that will auto-find an proper AppData location.
            //     if (!string.IsNullOrWhiteSpace(this.Configuration.AppDataPath))
            //     {
            //         // Create the directory for app data.
            //         System.IO.Directory.CreateDirectory(this.Configuration.AppDataPath);

            //         // Create the directory for the database.
            //         System.IO.Directory.CreateDirectory(System.IO.Path.Combine(this.Configuration.AppDataPath, databaseName));
            //     }
            // }

            if (this.Databases.Any(x => x.Name.Equals(databaseName)))
            {
                return this.Databases.FirstOrDefault(x => x.Name.Equals(databaseName));
            }
            else if(createIfNotExist)
            {
                SetAndEmitDatabase(new Database(this)
                {
                    Name = databaseName
                });

                return this.OpenDatabase(databaseName);
            }
            else
            {
                throw new TypeDBNotFoundException($"The database '{databaseName}' does not exist.");
            }
        }

        /// <summary>
        /// Flush all the TypeDB database
        /// </summary>
        /// <param name="databaseName">The name of the database</param>
        public void FlushDatabase(string databaseName = "default")
        {
            if (this.Databases.Any(x => x.Name.Equals(databaseName)))
            {
                this.Databases.FirstOrDefault(x => x.Name.Equals(databaseName)).Clear();
            }
            else
            {
                throw new TypeDBNotFoundException($"The database '{databaseName}' does not exist.");
            }
        }

        /// <summary>
        /// Drop a TypeDB database
        /// </summary>
        /// <param name="databaseName">The name of the database</param>
        /// <exception cref="TypeDBNotFoundException">TypeDBNotFoundException</exception>
        public void DropDatabase(string databaseName = "default")
        {
            if (this.Databases.Any(x => x.Name.Equals(databaseName)))
            {
                this.Databases.RemoveAll(x => x.Name.Equals(databaseName));
            }
            else
            {
                throw new TypeDBNotFoundException($"The database '{databaseName}' does not exist.");
            }
        }

        /// <summary>
        /// List the TypeDB Databases in the current Instance
        /// </summary>
        public List<Database> GetDatabases()
        {
            return this.Databases;
        }

        /// <summary>
        /// Rename a Database
        /// </summary>
        /// <param name="databaseName">The name of the Database</param>
        /// <param name="newDatabaseName">The new name of the Database</param>
        /// <exception cref="TypeDBNotFoundException">TypeDBNotFoundException</exception>
        public void RenameDatabase(string databaseName, string newDatabaseName)
        {
            if (this.Databases.Any(x => x.Name.Equals(databaseName)))
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new TypeDBNotFoundException($"The database '{databaseName}' does not exist.");
            }
        }

        // TODO: change this
        private void SetAndEmitDatabase(Database database)
        {
            var tempDatabases = this.Databases;
            tempDatabases.Add(database);
            this.Databases = tempDatabases;
        }

        public override string ToString()
        {
            return "lool";
        }

        /// <summary>
        /// Dispose the TypeDB Instance
        /// </summary>
        public void Dispose()
        {
        }
    }
}