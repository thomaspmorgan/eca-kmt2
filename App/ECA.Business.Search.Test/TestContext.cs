using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search.Test
{
    public class SimpleEntity : IIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid DocumentTypeId
        {
            get
            {
                return SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID;
            }
        }

        public int GetId()
        {
            return this.Id;
        }
    }

    public class SimpleEntityConfiguration : DocumentConfiguration<SimpleEntity, int>
    {
        public static Guid SIMPLE_ENTITY_DOCUMENT_TYPE_ID = Guid.Parse("76545b33-f560-41ae-837f-80e8793b4b2a");

        public const string SIMPLE_ENTITY_DOCUMENT_NAME = "document name";

        public SimpleEntityConfiguration()
        {
            HasKey(x => x.Id);
            HasName(x => x.Name);
            HasDescription(x => x.Description);
            IsDocumentType(SIMPLE_ENTITY_DOCUMENT_TYPE_ID, SIMPLE_ENTITY_DOCUMENT_NAME);
        }
    }

    public class TestContext : DbContext
    {
        public TestContext()
        {
            this.SimpleEntities = new TestDbSet<SimpleEntity>();
        }

        public TestDbSet<SimpleEntity> SimpleEntities { get; set; }
    }
}
