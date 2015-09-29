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
            HasName(x => x.Name);
            HasDescription(x => x.Description);
            IsDocumentType(DocumentType.Program);
        }
    }

    public class OtherTestDocumentConfiguration : DocumentConfiguration<OtherTestDocument, int>
    {
        public OtherTestDocumentConfiguration()
        {
            HasKey(x => x.Id);
            HasName(x => x.Name);
            HasDescription(x => x.Description);
            IsDocumentType(DocumentType.Project);
        }
    }
}
