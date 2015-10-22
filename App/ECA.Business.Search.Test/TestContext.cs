using ECA.Core.Data;
using ECA.Core.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace ECA.Business.Search.Test
{
    public class SimpleEntityDocumentSaveAction : DocumentSaveAction<SimpleEntity>
    {
        public SimpleEntityDocumentSaveAction(AppSettings settings) : base(settings)
        {
            DocumentKeys = new Dictionary<SimpleEntity, DocumentKey>();
        }

        public bool IsCreatedEntityActuallyExcluded { get; set; }
        public bool IsModifiedEntityActuallyExcluded { get; set; }
        public bool IsDeletedEntityActuallyExcluded { get; set; }

        public Dictionary<SimpleEntity, DocumentKey> DocumentKeys { get; set; }



        public override bool IsCreatedEntityExcluded(SimpleEntity createdEntity)
        {
            return IsCreatedEntityActuallyExcluded;
        }

        public override bool IsDeletedEntityExcluded(SimpleEntity deletedEntity)
        {
            return IsDeletedEntityActuallyExcluded;
        }

        public override bool IsModifiedEntityExcluded(SimpleEntity modifiedEntity)
        {
            return IsModifiedEntityActuallyExcluded;
        }

        public override DocumentKey GetDocumentKey(SimpleEntity entity, DbEntityEntry<SimpleEntity> entityEntry)
        {
            return DocumentKeys[entity];
        }
    }

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

        public const string SIMPLE_ENTITY_DOCUMENT_NAME = "documentname";

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
