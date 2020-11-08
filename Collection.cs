using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lucraft.Database
{
    public class Collection
    {
        public string ID { get; set; }
        public string Path { get; set; }
        public List<Document> Documents { get; set; }

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
                    error = null;
                    Documents.Remove(document);
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
