using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TypeDB.Exceptions;

namespace TypeDB.Tunneling
{
    internal class Pipeline
    {
        private readonly Core Root;

        private List<Database> LocalDatabases = new List<Database>();

        public Pipeline(Core core)
        {
            this.Root = core;
        }

        public List<Database> GetDatabases()
        {
            //TODO: remove
            // Console.WriteLine("> Databases GET");

            var isPersistent = this.Root.CurrentInstance.Configuration.IsPersistent;
            var isBinded = this.Root.CurrentInstance.Configuration.IsBinded;

            if(!isBinded)
            {
                switch (this.Root.CurrentInstance.Configuration.Mode)
                {
                    default:
                    case Mode.Standalone:
                        //TODO: implement
                        return this.LocalDatabases;

                    case Mode.Remote:
                        var remoteDatabases = new List<Database>();
                        JsonConvert.DeserializeObject<string[]>(new RestSharp.RestClient(this.Root.CurrentInstance.Configuration.Endpoint.BuildApi("/api/databases")).Execute(new RestSharp.RestRequest(RestSharp.Method.GET)).Content).ToList().ForEach(databaseName => remoteDatabases.Add(new Database(this.Root.CurrentInstance)
                        {
                            Name = databaseName
                        }));
                        return remoteDatabases;
                }
            }
            else
            {
                return this.LocalDatabases;
                // TODO: configure the Server side
                throw new TypeDBGeneralException("Configure the Server side");
            }
        }

        public void SetDatabases(List<Database> previousValue, List<Database> value)
        {
            //TODO: remove
            // Console.WriteLine("> Databases SET");
            this.LocalDatabases = value;

            var isPersistent = this.Root.CurrentInstance.Configuration.IsPersistent;
            var isBinded = this.Root.CurrentInstance.Configuration.IsBinded;

            var databaseName = value.Select(x => x.Name).Except(previousValue.Select(x => x.Name)).FirstOrDefault();

            if (!isBinded)
            {
                switch (this.Root.CurrentInstance.Configuration.Mode)
                {
                    default:
                    case Mode.Standalone:
                        this.LocalDatabases.Add(new Database(this.Root.CurrentInstance)
                        {
                            Name = databaseName
                        });
                        break;

                    case Mode.Remote:
                        //TODO: implement renaming
                        //TODO: maybe change API endpoint
                        new RestSharp.RestClient(this.Root.CurrentInstance.Configuration.Endpoint.BuildApi("/api/databases", "database=" + databaseName)).Execute(new RestSharp.RestRequest(RestSharp.Method.POST));
                        break;
                }
            }
            else
            {
                // TODO: configure the Server side
                throw new TypeDBGeneralException("Configure the Server side");
            }
        }
    }
}