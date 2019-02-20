using System;
using System.IO;
using Newtonsoft.Json;
using TypeDB.Exceptions;

namespace TypeDB
{
    /// <summary>
    /// Backup Logic
    /// </summary>
    public static class BackupExtensions
    {
        /// <summary>
        /// Export only the Database entities
        /// </summary>
        /// <param name="formatting">Specify if you want a JSON formatting</param>
        public static string Save(this Database currentDatabase, bool formatting = true) => JsonConvert.SerializeObject(currentDatabase.Entities.Values, formatting ? Formatting.Indented : Formatting.None);

        /// <summary>
        /// Export all the Database entities in a JSON file
        /// </summary>
        /// <param name="location">Specify the location of the backup file</param>
        /// <param name="formatting">Specify if you want a JSON formatting</param>
        public static void SaveAs(this Database currentDatabase, string location, bool formatting = true)
        {
            try
            {
                File.WriteAllText(location, JsonConvert.SerializeObject(currentDatabase.Entities.Values, formatting ? Formatting.Indented : Formatting.None));
            }
            catch
            {
                throw new TypeDBGeneralException("Unable to export the selected database.");
            }
        }

        public static void Restore(this Database currentDatabase, string source)
        {
            throw new NotImplementedException();
        }

        public static void RestoreFromFile(this Database currentDatabase, string filename)
        {
            throw new NotImplementedException();
        }
    }
}