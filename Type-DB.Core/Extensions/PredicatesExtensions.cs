using System;
using System.Linq;

namespace TypeDB
{
    public static class PredicatesExtensions
    {
        public static void Drop(this Database currentDatabase, string collection, Predicate<Entity> keys)
        {
            // currentDatabase.GetCollectionByEntity()
            // currentDatabase.Entities[collection].

            throw new NotImplementedException();
        }
    }
}