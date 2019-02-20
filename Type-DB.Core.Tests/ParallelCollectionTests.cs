using System;
using TypeDB.Tests.Models;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace TypeDB.Tests
{
    public class ParallelCollectionTests
    {
        [Fact]
        public void RunQueryInParallel()
        {
            using (Instance tdb = new TypeDB.Core(TypeDB.Mode.Standalone).Connect())
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

                    var childPlayers = testdb.Filter<Player>("Player", filter => filter.Age < 18, true);
                    Assert.Equal(18, childPlayers.LongCount());
                }
            }
        }

        [Fact]
        public void RunQuery()
        {
            using (Instance tdb = new TypeDB.Core(TypeDB.Mode.Standalone).Connect())
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

                    var childPlayers = testdb.Filter<Player>("Player", filter => filter.Age < 18, false);
                    Assert.Equal(18, childPlayers.LongCount());
                }
            }
        }
    }
}
