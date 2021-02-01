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
            string[] dbDirs = Directory.GetDirectories(DatabaseServer.ROOT_PATH + "/data/");
            foreach (var dbDir in dbDirs)
            {
                List<Collection> collections = new List<Collection>();
                string[] collectionDirs = Directory.GetDirectories(dbDir);
                foreach (var collectionDir in collectionDirs)
                {
                    string[] documentFiles = Directory.GetFiles(collectionDir);
                    List<Document> documents = documentFiles.Select(documentFile => new Document(documentFile) { ID = Path.GetFileNameWithoutExtension(documentFile) }).ToList();
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
