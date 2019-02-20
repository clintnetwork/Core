using System;
using System.Linq;
using Newtonsoft.Json;

namespace TypeDB
{
    public static class PrintingExtension
    {
        // public static void Print(this Database currentDatabase, string key) => Console.WriteLine(JsonConvert.SerializeObject(currentDatabase.Entities.Where(x => x.Key.Equals(key)).FirstOrDefault(), Formatting.Indented));

        public static void PrintAll(this Database currentDatabase) => Console.WriteLine(JsonConvert.SerializeObject(currentDatabase.Entities, Formatting.Indented));
    }
}