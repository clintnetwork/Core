using System;
using System.Collections.Generic;
using System.Linq;
using TypeDB.Interfaces;

namespace TypeDB
{
    /// <summary>
    /// TypeDB Instance Object
    /// </summary>
    public class Instance : IDisposable
    {
        private readonly Core Root;

        public string Uid = Guid.NewGuid().ToString();

        /// <summary>
        /// TypeDB Instance Configuration
        /// </summary>
        internal Configuration Configuration { get; set; }

        /// <summary>
        /// Define the Private Configuration
        /// </summary>
        //private readonly PrivateConfiguration privateConfiguration;

        /// <summary>
        /// TypeDB Database List
        /// </summary>
        /// TODO: implement pipeline
        internal List<Database> Databases { get => this.Root.Pipeline.GetDatabases(); set => this.Root.Pipeline.SetDatabases(value); }

        //private Authentication Authentication { get; set; }
        //private List<Right> Rights { get; set; }
        //private List<Trigger> Triggers { get; set; }
        //private List<Script> Scripts { get; set; }

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
        /// <returns></returns>
        public IDatabase OpenDatabase(string databaseName = "default", bool createIfNotExist = false)
        {
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

        private void SetAndEmitDatabase(Database database)
        {
            var tempDatabases = this.Databases;
            tempDatabases.Add(database);
            this.Databases = tempDatabases;
        }

        /// <summary>
        /// Flush all the TypeDB database
        /// </summary>
        /// <param name="databaseName">The name of the database</param>
        public void FlushDatabase(string databaseName = "default")
        {
            if (this.Databases.Any(x => x.Name.Equals(databaseName)))
            {
                this.Databases.FirstOrDefault(x => x.Name.Equals(databaseName)).Objects.RemoveAll(x => true);
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
        /// Dispose the TypeDB Instance
        /// </summary>
        public void Dispose()
        {
        }
    }
}