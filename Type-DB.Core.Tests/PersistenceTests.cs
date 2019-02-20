using System;
using System.Collections.Generic;
using System.Text;
// using TypeDB.Persistence;
using TypeDB.Tests.Models;
using Xunit;

namespace TypeDB.Tests
{
    public class PersistenceTests
    {
        [Fact]
        public void PerformBasicDiskPersistance()
        {
            using (Instance tdb = new TypeDB.Core(TypeDB.Mode.Standalone)
                .Configure(new Configuration()
                {
                })
                //TODO: implement persistence
                .Connect())
            {
                using (Database testdb = tdb.OpenDatabase("test", true))
                {
                    for (int i = 0; i < 500; i++)
                    {
                        testdb.Set<Player>(TypeDB.Entity.NewId(), new Player()
                        {
                            Name = "Elena" + i,
                            Age = i
                        });
                    }

                    // testdb.Save();

                    // DiskPersistence disk = new DiskPersistence();
                    // disk.Save(testdb);
                }
            }
        }
    }
}
