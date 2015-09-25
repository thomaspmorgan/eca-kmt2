using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search.Test
{
    public class TestDocument : IDocumentable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Subtitle { get; set; }

        public DocumentType GetDocumentType()
        {
            return DocumentType.Program;
        }

        public List<string> GetDocumentFields()
        {
            return new List<string> {
                "Name",
                "Description"
            };

        }

        public string GetValue(string field)
        {
            if (field == "Name")
            {
                return this.Name;
            }
            else if (field == "Description")
            {
                return this.Description;
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        public string GetTitle()
        {
            return this.Name;
        }

        public string GetDescription()
        {
            return this.Description;
        }

        public string GetSubtitle()
        {
            return this.Subtitle;
        }

        public object GetId()
        {
            return this.Id;
        }
    }
}
