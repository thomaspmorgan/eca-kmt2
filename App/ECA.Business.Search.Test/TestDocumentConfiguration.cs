using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search.Test
{
    public class TestDocumentConfiguration : DocumentConfiguration<TestDocument, int>
    {
        public TestDocumentConfiguration()
        {
            HasKey(x => x.Id);
            HasTitle(x => x.Name);
            IsDocumentType(DocumentType.Program);
        }
    }
}
