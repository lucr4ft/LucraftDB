using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Lucraft.Database
{
    public class Databases
    {
        private static readonly List<Database> databases = new List<Database>();

        private Databases() { }

        public static void Load()
        {
            string[] dbDirs = Directory.GetDirectories(DatabaseServer.ROOT_PATH + "\\data\\");
            foreach (var dbDir in dbDirs)
            {
                List<Collection> collections = new List<Collection>();
                string[] collectionDirs = Directory.GetDirectories(dbDir);
                foreach (var collectionDir in collectionDirs)
                {
                    List<Document> documents = new List<Document>();
                    string[] documentFiles = Directory.GetFiles(collectionDir);
                    foreach (var documentFile in documentFiles)
                    {
                        documents.Add(new Document(documentFile)
                        {
                            ID = Path.GetFileNameWithoutExtension(documentFile)
                        });
                    }
                    collections.Add(new Collection
                    {
                        ID = Path.GetFileName(collectionDir),
                        Path = collectionDir,
                        Documents = documents,
                    });
                }
                databases.Add(new Database
                {
                    ID = Path.GetFileName(dbDir),
                    Collections = collections
                });
            }
        }

        public static Database GetDatabase(string id)
        {
            foreach (var database in databases)
            {
                if (database.ID == id)
                    return database;
            }
            return null;
        }
    }
}
