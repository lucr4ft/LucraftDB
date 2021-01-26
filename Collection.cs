using Lucraft.Database.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lucraft.Database
{
    public class Collection
    {
        public string ID { get; set; }
        public string Path { get; set; }
        public List<Document> Documents { get; set; }

        public CollectionResponseModel GetModel()
        {
            List<DocumentResponseModel> documentModels = new List<DocumentResponseModel>();
            Documents.ForEach((Document doc) => documentModels.Add(doc.GetModel()));
            return new CollectionResponseModel { Id = this.ID, Documents = documentModels };
        }

        public Document GetDocument(string id)
        {
            foreach (var document in Documents)
            {
                if (document.ID == id)
                    return document;
            }
            return null;
        }

        public Document CreateDocument(string id)
        {
            if (id.Equals("*"))
            {
                SimpleLogger.Log(Level.Debug, "generating random id");
                id = Utilities.GetRandomId();
                // i dont know if the id will ever repeat itself
                // this is just a safety meassure
                // to make sure no data is overwritten,
                // because the id aready existed
                while (GetDocument(id) != null)
                {
                    SimpleLogger.Log(Level.Debug, "random id already in use");
                    SimpleLogger.Log(Level.Debug, "generating new random id");
                    id = Utilities.GetRandomId();
                }
            }
            Document document = new Document(Path + "/" + id + ".db")
            {
                ID = id
            };
            Documents.Add(document);
            return document;
        }

        public bool DeleteDocument(string id, out string error)
        {
            Document document = GetDocument(id);
            if (document != null)
            {
                try
                {
                    File.Delete(Path + "/" + id + ".db");
                    Documents.Remove(document);
                    error = null;
                    return true;
                }
                catch (Exception e)
                {
                    error = e.Message;
                }
            } 
            else
            {
                error = "Document with ID " + id + " does not exist";
            }
            return false;
        }
    }
}
