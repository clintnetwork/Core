using System;
using Xunit;
using TypeDB;
using System.ComponentModel.DataAnnotations;
using TypeDB.Tests.Models;

namespace TypeDB.Tests
{
    public class BasicDatabaseTests
    {
        [Fact]
        public void OpenLocalDBAndPopulate()
        {
            using (var tdb = new TypeDB.Core(TypeDB.Mode.Standalone).Connect())
            {
                using (var testdb = tdb.OpenDatabase("test", true))
                {
                    testdb.Set<Player>("elena", new Player()
                    {
                        Name = "Elena",
                        Age = 22
                    });

                    testdb.Set<Player>(new Gamer()
                    {
                        Id = Entity.NewId(),
                        Name = "Elena",
                        Age = 22
                    });
                }

                //TODO: implement
                //tdb.RenameDatabase("test", "ok");
            }
        }

        [Fact]
        public void UseIdNamesAndVerify()
        {
            using (var tdb = new TypeDB.Core(TypeDB.Mode.Standalone).Connect())
            {
                using (var testdb = tdb.OpenDatabase("test", true))
                {
                    // Add the Identifier as a custom Id name.
                    testdb.IdNames.Add("Identifier");

                    testdb.Set(new Viewer()
                    {
                        Identifier = Guid.NewGuid(),
                        Name = "Elena",
                        Age = 22
                    });
                }
            }
        }

        [Fact]
        public void ReadCollectionNameFromBaseType()
        {
            using (var tdb = new TypeDB.Core(TypeDB.Mode.Standalone).Connect())
            {
                using (var testdb = tdb.OpenDatabase("test", true))
                {
                    // Add the Identifier as a custom Id name.
                    testdb.IdNames.Add("Identifier");

                    testdb.Set(new Viewer()
                    {
                        Identifier = Guid.NewGuid(),
                        Name = "Elena",
                        Age = 22
                    });

                    testdb.Set(Entity.NewId(), new Player()
                    {
                        Name = "Elena",
                        Age = 22
                    });

                    Assert.Equal(1, testdb.CollectionCount);

                    testdb.Set(new Gamer { Id = Entity.NewId(), Age = 30, Name = "Sarah" });

                    Assert.Equal(2, testdb.CollectionCount);
                }
            }
        }
    }
}
