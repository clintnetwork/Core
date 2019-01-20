using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeDB
{
    public class Pipeline
    {
        private readonly Core Root;

        private List<Database> LocalDatabases = new List<Database>();

        public Pipeline(Core core)
        {
            this.Root = core;
        }

        public List<Database> GetDatabases()
        {
            Console.WriteLine("> Databases GET");
            Console.WriteLine(JsonConvert.SerializeObject(this.LocalDatabases.Select(x => x.Name)) + Environment.NewLine);

            var isPersistent = this.Root.CurrentInstance.Configuration.IsPersistent;
            var isBinded = this.Root.CurrentInstance.Configuration.IsBinded;

            if(!isBinded)
            {
                switch (this.Root.CurrentInstance.Configuration.Mode)
                {
                    default:
                    case Mode.Standalone:
                        /*return new List<Database>
                        {
                            new Database(this.Root.CurrentInstance)
                            {
                                Name = "lol"
                            }
                        };*/
                        return this.LocalDatabases;

                    case Mode.Remote:
                        return this.LocalDatabases;
                }
            }
            else
            {
                // TODO: configure the Server side
                throw new TypeDBGeneralException("Configure the Server side");
            }
        }

        public void SetDatabases(List<Database> value)
        {
            Console.WriteLine("> Databases SET");
            Console.WriteLine(JsonConvert.SerializeObject(value.Select(x => x.Name)) + Environment.NewLine);
            this.LocalDatabases = value;
        }
    }
}