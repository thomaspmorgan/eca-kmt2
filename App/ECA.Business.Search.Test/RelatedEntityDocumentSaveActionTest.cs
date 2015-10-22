using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Configuration;
using ECA.Core.Settings;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using System.Data.Entity;
using ECA.Core.DynamicLinq;

namespace ECA.Business.Search.Test
{
    public class RelatedEntityDocumentSaveActionTestClass : RelatedEntityDocumentSaveAction<SimpleEntity>
    {
        public RelatedEntityDocumentSaveActionTestClass(AppSettings settings) : base(settings)
        {
        }

        public DocumentKey GetDocumentKey(int id)
        {
            return new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, id);
        }

        public override List<DocumentKey> GetRelatedEntityDocumentKeysOfAddedEntity(SimpleEntity addedEntity)
        {
            var list = new List<DocumentKey>();
            list.Add(GetDocumentKey(addedEntity.Id));
            return list;
        }

        public override Task<List<DocumentKey>> GetRelatedEntityDocumentKeysOfAddedEntityAsync(SimpleEntity addedEntity)
        {
            return Task.FromResult<List<DocumentKey>>(GetRelatedEntityDocumentKeysOfAddedEntity(addedEntity));
        }

        public override List<DocumentKey> GetRelatedEntityDocumentKeysOfModifiedEntity(SimpleEntity updatedOrDeletedEntity, DbPropertyValues originalValues)
        {
            var id = originalValues.GetValue<int>(PropertyHelper.GetPropertyName<SimpleEntity>(x => x.Id));
            var list = new List<DocumentKey>();
            list.Add(GetDocumentKey(id));
            return list;

        }

        public override Task<List<DocumentKey>> GetRelatedEntityDocumentKeysOfModifiedEntityAsync(SimpleEntity updatedOrDeletedEntity, DbPropertyValues originalValues)
        {
            return Task.FromResult<List<DocumentKey>>(GetRelatedEntityDocumentKeysOfModifiedEntity(updatedOrDeletedEntity, originalValues));
        }

        public override bool IsCreatedEntityExcluded(SimpleEntity createdEntity)
        {
            return false;
        }

        public override bool IsDeletedEntityExcluded(SimpleEntity deletedEntity)
        {
            return false;
        }

        public override bool IsModifiedEntityExcluded(SimpleEntity modifiedEntity)
        {
            return false;
        }
    }

    [TestClass]
    public class RelatedEntityDocumentSaveActionTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private RelatedEntityDocumentSaveActionTestClass saveAction;
        private TestContext context;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            context = new TestContext();
            saveAction = new RelatedEntityDocumentSaveActionTestClass(settings);
            saveAction.Context = context;
        }

        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsNotNull(saveAction.RelatedEntityDocumentKeys);
        }

        [TestMethod]
        public void TestGetDocumentKey()
        {
            Action a = () => saveAction.GetDocumentKey(new SimpleEntity(), null);
            a.ShouldThrow<NotSupportedException>().WithMessage("This method intentionally not implemented.");
        }

        [TestMethod]
        public void TestGetBatchMessage_NoRelatedEntityKeocumentKeys()
        {
            Assert.AreEqual(0, saveAction.RelatedEntityDocumentKeys.Count);
            var message = saveAction.GetBatchMessage();
            Assert.AreEqual(0, message.CreatedDocuments.Count());
            Assert.AreEqual(0, message.DeletedDocuments.Count());
            Assert.AreEqual(0, message.ModifiedDocuments.Count());
        }

        [TestMethod]
        public void TestGetBatchMessage_HasKeys()
        {
            var key1 = new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, 1);
            var key2 = new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, 2);
            saveAction.RelatedEntityDocumentKeys.Add(key1);
            saveAction.RelatedEntityDocumentKeys.Add(key2);

            Assert.AreEqual(2, saveAction.RelatedEntityDocumentKeys.Count);
            var message = saveAction.GetBatchMessage();
            Assert.AreEqual(0, message.CreatedDocuments.Count());
            Assert.AreEqual(0, message.DeletedDocuments.Count());
            Assert.AreEqual(2, message.ModifiedDocuments.Count());

            Assert.AreEqual(key1.ToString(), message.ModifiedDocuments.First());
            Assert.AreEqual(key2.ToString(), message.ModifiedDocuments.Last());
        }

        [TestMethod]
        public async Task TestBeforeSaveChanges_AddedEntity()
        {
            using (ShimsContext.Create())
            {
                var created = new SimpleEntity
                {
                    Id = 1
                };
                saveAction.CreatedEntities.Add(created);
                var typedEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<SimpleEntity>
                {
                    EntityGet = () => created,
                    StateGet = () => EntityState.Added
                };
                var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                {
                    EntityGet = () => created,
                    StateGet = () => EntityState.Added
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(entityEntry);
                    return entries;
                };
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<SimpleEntity>((ctx, e) =>
                {
                    return typedEntityEntry;
                });

                Action testAndReset = () =>
                {
                    Assert.AreEqual(1, saveAction.RelatedEntityDocumentKeys.Count);
                    var expectedDocumentKey = new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, created.Id);
                    Assert.AreEqual(expectedDocumentKey, saveAction.RelatedEntityDocumentKeys.First());
                    saveAction.RelatedEntityDocumentKeys.Clear();
                };

                saveAction.BeforeSaveChanges(context);
                testAndReset();

                await saveAction.BeforeSaveChangesAsync(context);
                testAndReset();
            }
        }

        [TestMethod]
        public async Task TestBeforeSaveChanges_NonAddedEntity()
        {
            using (ShimsContext.Create())
            {
                var created = new SimpleEntity
                {
                    Id = 1
                };
                saveAction.CreatedEntities.Add(created);
                var dbPropertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                dbPropertyValues.GetValueOf1String<int>((propertyName) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<SimpleEntity>(x => x.Id), propertyName);
                    return created.Id;
                });
                var typedEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<SimpleEntity>
                {
                    EntityGet = () => created,
                    StateGet = () => EntityState.Modified,
                    GetDatabaseValues = () => dbPropertyValues,
                    GetDatabaseValuesAsync = () =>
                    {
                        return Task.FromResult<DbPropertyValues>(dbPropertyValues);
                    }
                };
                var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                {
                    EntityGet = () => created,
                    StateGet = () => EntityState.Modified,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(entityEntry);
                    return entries;
                };
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<SimpleEntity>((ctx, e) =>
                {
                    return typedEntityEntry;
                });

                Action testAndReset = () =>
                {
                    Assert.AreEqual(1, saveAction.RelatedEntityDocumentKeys.Count);
                    var expectedDocumentKey = new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, created.Id);
                    Assert.AreEqual(expectedDocumentKey, saveAction.RelatedEntityDocumentKeys.First());
                    saveAction.RelatedEntityDocumentKeys.Clear();
                };

                saveAction.BeforeSaveChanges(context);
                testAndReset();

                await saveAction.BeforeSaveChangesAsync(context);
                testAndReset();
            }
        }
    }
}
