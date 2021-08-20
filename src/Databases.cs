using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lucraft.Database
{
    public static class Databases
    {
        private static readonly List<Database> databases = new();

        public static void Load()
        {
            if (!Directory.Exists(DatabaseServer.DATA_PATH))
            {
                Directory.CreateDirectory(DatabaseServer.DATA_PATH);
            }
            string[] dbDirs = Directory.GetDirectories(DatabaseServer.DATA_PATH);
            foreach (var dbDir in dbDirs)
            {
                var collections = new List<Collection>();
                string[] collectionDirs = Directory.GetDirectories(dbDir);
                foreach (var collectionDir in collectionDirs)
                {
                    string[] documentFiles = Directory.GetFiles(collectionDir);
                    var documents = documentFiles.Select(documentFile => new Document(documentFile) { ID = Path.GetFileNameWithoutExtension(documentFile) }).ToList();
                    collections.Add(new Collection
                    {
                        ID = Path.GetFileName(collectionDir),
                        Path = collectionDir,
                        Documents = documents,
                    });
                }
                databases.Add(new Database
                {
                    Id = Path.GetFileName(dbDir),
                    Collections = collections
                });
            }
        }

        public static Database GetDatabase(string id)
        {
            return databases.FirstOrDefault(database => database.Id == id);
        }
    }
}
