using System;
using Newtonsoft.Json;
using TypeDB;

namespace TypeDB.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new TypeDB Instance (Standalone = without server)
            using (var tdb = new TypeDB.Core(Mode.Standalone)
            .Configure(new Configuration                                // Configure some points if needed
            {
            })
            .UsePersistence(new Persistence                             // Configure Persistence
            {
                Type = PersistenceType.OnlyInvoke,                        
                Location = TypeDB.Persistence.TemporaryLocation,
                // ChunkSize = 10,
                Interval = TimeSpan.FromMinutes(1)                      // Persistence is executed each 1 minute (Snapshot mode)
            })
            // .UseReplication()                                        // Configure various things like Replication or Encryption
            // .UseTriggers()
            // .UseAuthentication()
            // .UseEncryption()
            // .UseQuotas()
            .Connect())
            {
                using (var testdb = tdb.OpenDatabase("test", true))     // Open a Database named "test"
                {
                    testdb.Set<int>("clint", 29);                       // Set some stuffs
                    testdb.Set<int>("quentin", 22);
                    testdb.Set<int>("bob", 48);

                    testdb.Set("players", "elena", "anything you want");
                    
                    testdb.Increment("clint", 1);                       // Increment the value of "clint" Entity

                    testdb.Lock("bob");                                 // Lock an Entity
                    // testdb.Unlock("bob");

                    testdb.Drop("bob");                                 // Try to Remove an Entity (will fail because this entity is locked)                            

                    var newAge = testdb.Get<int>("clint");
                    Console.WriteLine(newAge);

                    // testdb.PrintAll();                               // Display all Entities

                    // var backup = testdb.Save();                      // Backup the Database in a JSON string
                    // Console.WriteLine(backup);
                }
            }
        }
    }
}
