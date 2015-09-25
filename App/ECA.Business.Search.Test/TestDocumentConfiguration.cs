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
            HasAdditionalField(x => x.AdditionalField);
            IsDocumentType(DocumentType.Program);
        }
    }

    public class OtherTestDocumentConfiguration : DocumentConfiguration<OtherTestDocument, int>
    {
        public OtherTestDocumentConfiguration()
        {
            HasKey(x => x.Id);
            HasTitle(x => x.Name);
            HasAdditionalField(x => x.AdditionalField);
            IsDocumentType(DocumentType.Project);
        }
    }
}
