using System;
using System.Collections.Generic;
using System.Linq;
using TypeDB.Interfaces;

namespace TypeDB
{
    public class Instance : IDisposable
    {
        /// <summary>
        /// TypeDB Instance Configuration
        /// </summary>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// TypeDB Database List
        /// </summary>
        /// TODO: implement pipeline
        private List<Database> Databases { get; set; }

        //private Authentication Authentication { get; set; }
        //private List<Right> Rights { get; set; }
        //private List<Trigger> Triggers { get; set; }
        //private List<Script> Scripts { get; set; }

        /// <summary>
        /// Initialize a TypeDB Instance and the Database List
        /// </summary>
        public Instance()
        {
            this.Databases = new List<Database>();
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
                this.Databases.Add(new Database(this)
                {
                    Name = databaseName
                });
            }
        }

        /// <summary>
        /// Open a TypeDB database
        /// </summary>
        /// <param name="databaseName">'default' is the default database name</param>
        /// <param name="createIfNotExist">If the database does not exist, it will automatically create</param>
        /// <returns></returns>
        public IDatabase OpenDatabase(string databaseName = "default", bool createIfNotExist = false)
        {
            if (this.Databases.Any(x => x.Name.Equals(databaseName)))
            {
                return this.Databases.FirstOrDefault(x => x.Name.Equals(databaseName));
            }
            else if(createIfNotExist)
            {
                this.Databases.Add(new Database(this)
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
        public void DropDatabase(string databaseName)
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