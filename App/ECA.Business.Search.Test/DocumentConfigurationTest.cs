using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;

namespace ECA.Business.Search.Test
{
    public class DocumentConfigurationTestConfigurationClass : DocumentConfiguration<DocumentConfigurationTestClass, int>
    {
        public DocumentConfigurationTestConfigurationClass()
        {
            HasKey(x => x.Id);
            HasTitle(x => x.Title);
            HasSubtitle(x => x.Subtitle);
            HasDescription(x => x.Description);
            HasAdditionalField(x => x.AdditionalField1);
            HasAdditionalField(x => x.AdditionalField2);

        }
    }

    public class DocumentConfigurationTestClass
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string AdditionalField1 { get; set; }

        public string AdditionalField2 { get; set; }


        public string Description { get; set; }

        public DocumentType DocumentType { get; set; }
    }

    [TestClass]
    public class DocumentConfigurationTest
    {
        [TestMethod]
        public void TestGetTitle()
        {
        }
    }
}
