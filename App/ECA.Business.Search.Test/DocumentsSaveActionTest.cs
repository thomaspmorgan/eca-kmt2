using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using System.Collections.Specialized;
using ECA.Core.Settings;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class DocumentsSaveActionTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private TestContext context;
        private SimpleEntityDocumentSaveAction saveAction;


        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            context = new TestContext();
            saveAction = new SimpleEntityDocumentSaveAction(settings);
        }

        [TestMethod]
        public void TestGetBatchMessage()
        {
            var created = new List<SimpleEntity>();
            created.Add(new SimpleEntity
            {
                Id = 1,
            });

            var modified = new List<SimpleEntity>();
            modified.Add(new SimpleEntity
            {
                Id = 2
            });

            var deleted = new List<SimpleEntity>();
            deleted.Add(new SimpleEntity
            {
                Id = 3
            });

            var createdKey = new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, created.First().Id);
            var deletedKey = new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, deleted.First().Id);
            var modifiedKey = new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, modified.First().Id);

            saveAction.CreatedEntities.AddRange(created);
            saveAction.DeletedEntities.AddRange(deleted);
            saveAction.ModifiedEntities.AddRange(modified);

            saveAction.DocumentKeys[created.First()] = createdKey;
            saveAction.DocumentKeys[modified.First()] = modifiedKey;
            saveAction.DocumentKeys[deleted.First()] = deletedKey;

            var message = saveAction.GetBatchMessage();
            Assert.AreEqual(1, message.CreatedDocuments.Count());
            Assert.AreEqual(1, message.DeletedDocuments.Count());
            Assert.AreEqual(1, message.ModifiedDocuments.Count());

            Assert.AreEqual(createdKey.ToString(), message.CreatedDocuments.First());
            Assert.AreEqual(deletedKey.ToString(), message.DeletedDocuments.First());
            Assert.AreEqual(modifiedKey.ToString(), message.ModifiedDocuments.First());
        }

        #region Document Entities

        [TestMethod]
        public void TestGetCreatedDocumentEntities_NoEntitiesExcluded()
        {
            using (ShimsContext.Create())
            {
                var entity = new SimpleEntity
                {
                    Id = 1,
                };
                saveAction.IsCreatedEntityActuallyExcluded = false;
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.SimpleEntities.Add(entity);
                var entities = saveAction.GetCreatedDocumentEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedDocumentEntities_EntitiesExcluded()
        {
            using (ShimsContext.Create())
            {
                var entity = new SimpleEntity
                {
                    Id = 1,
                };
                saveAction.IsCreatedEntityActuallyExcluded = true;
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.SimpleEntities.Add(entity);
                var entities = saveAction.GetCreatedDocumentEntities(context);
                Assert.AreEqual(0, entities.Count);
            }
        }


        [TestMethod]
        public void TestGetModifiedDocumentEntities_NoEntitiesExcluded()
        {
            using (ShimsContext.Create())
            {
                var entity = new SimpleEntity
                {
                    Id = 1,
                };
                saveAction.IsModifiedEntityActuallyExcluded = false;
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Modified
                    });
                    return entries;
                };
                context.SimpleEntities.Add(entity);
                var entities = saveAction.GetModifiedDocumentEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedDocumentEntities_EntitiesExcluded()
        {
            using (ShimsContext.Create())
            {
                var entity = new SimpleEntity
                {
                    Id = 1,
                };
                saveAction.IsModifiedEntityActuallyExcluded = true;
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Modified
                    });
                    return entries;
                };
                context.SimpleEntities.Add(entity);
                var entities = saveAction.GetModifiedDocumentEntities(context);
                Assert.AreEqual(0, entities.Count);
            }
        }

        [TestMethod]
        public void TestGetDeletedDocumentEntities_NoEntitiesExcluded()
        {
            using (ShimsContext.Create())
            {
                var entity = new SimpleEntity
                {
                    Id = 1,
                };
                saveAction.IsDeletedEntityActuallyExcluded = false;
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Deleted
                    });
                    return entries;
                };
                context.SimpleEntities.Add(entity);
                var entities = saveAction.GetDeletedDocumentEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetDeletedDocumentEntities_EntitiesExcluded()
        {
            using (ShimsContext.Create())
            {
                var entity = new SimpleEntity
                {
                    Id = 1,
                };
                saveAction.IsDeletedEntityActuallyExcluded = true;
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Deleted
                    });
                    return entries;
                };
                context.SimpleEntities.Add(entity);
                var entities = saveAction.GetDeletedDocumentEntities(context);
                Assert.AreEqual(0, entities.Count);
            }
        }

        [TestMethod]
        public void TestGetDocumentEntities_AddedEntityState()
        {
            using (ShimsContext.Create())
            {
                var entity = new SimpleEntity
                {
                    Id = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.SimpleEntities.Add(entity);
                var entities = saveAction.GetDocumentEntities(context, EntityState.Added);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetDocumentEntities_NoEntitiesOfState()
        {
            using (ShimsContext.Create())
            {
                var entity = new SimpleEntity
                {
                    Id = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Modified
                    });
                    return entries;
                };
                var entities = saveAction.GetDocumentEntities(context, EntityState.Added);
                Assert.AreEqual(0, entities.Count);

            }
        }
        #endregion

        #region Before Save Changes
        [TestMethod]
        public void TestBeforeSaveChanges()
        {
            using (ShimsContext.Create())
            {
                var entity = new SimpleEntity
                {
                    Id = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.SimpleEntities.Add(entity);
                saveAction.BeforeSaveChanges((DbContext)context);
                Assert.AreEqual(1, saveAction.CreatedEntities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, saveAction.CreatedEntities.First()));
            }
        }

        [TestMethod]
        public async Task TestBeforeSaveChangesAsync()
        {
            using (ShimsContext.Create())
            {
                var entity = new SimpleEntity
                {
                    Id = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.SimpleEntities.Add(entity);
                await saveAction.BeforeSaveChangesAsync((DbContext)context);
                Assert.AreEqual(1, saveAction.CreatedEntities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, saveAction.CreatedEntities.First()));
            }
        }

        #endregion

        #region After Save Changes
        [TestMethod]
        public async Task TestAfterSaveChangesAsync()
        {
            using (ShimsContext.Create())
            {
                var connectionString = "connection string";
                var queueName = "value";
                appSettings.Add(AppSettings.SEARCH_INDEX_QUEUE_NAME_KEY, queueName);
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.AZURE_WEB_JOBS_STORAGE_KEY, connectionString));
                var created = new List<SimpleEntity>();
                created.Add(new SimpleEntity
                {
                    Id = 1,
                });

                saveAction.CreatedEntities.AddRange(created);
                saveAction.DocumentKeys[created.First()] = new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, created.First().Id);
                var fakeQueue = new Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueue
                {   
                    CreateIfNotExistsAsync = () =>
                    {
                        return Task.FromResult<bool>(true);
                    },
                    AddMessageAsyncCloudQueueMessage = (msg) =>
                    {
                        var deserializedMessage = JsonConvert.DeserializeObject<IndexDocumentBatchMessage>(msg.AsString);
                        Assert.AreEqual(1, deserializedMessage.CreatedDocuments.Count());
                        Assert.AreEqual(0, deserializedMessage.DeletedDocuments.Count());
                        Assert.AreEqual(0, deserializedMessage.ModifiedDocuments.Count());
                        return Task.FromResult<object>(null);
                    },
                };

                Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueueClient.AllInstances.GetQueueReferenceString = (client, qName) =>
                {
                    Assert.AreEqual(queueName, qName);
                    return fakeQueue;
                };

                Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.AllInstances.CreateCloudQueueClient = (acct) =>
                {
                    return new Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueueClient();
                };

                Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.ParseString = (s) =>
                {
                    Assert.AreEqual(connectionString, s);
                    return new Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount();
                };

                await saveAction.AfterSaveChangesAsync((DbContext)context);
            }
        }

        [TestMethod]
        public void TestAfterSaveChanges()
        {
            using (ShimsContext.Create())
            {
                var connectionString = "connection string";
                var queueName = "value";
                appSettings.Add(AppSettings.SEARCH_INDEX_QUEUE_NAME_KEY, queueName);
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.AZURE_WEB_JOBS_STORAGE_KEY, connectionString));
                var created = new List<SimpleEntity>();
                created.Add(new SimpleEntity
                {
                    Id = 1,
                });

                saveAction.CreatedEntities.AddRange(created);
                saveAction.DocumentKeys[created.First()] = new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, created.First().Id);
                var fakeQueue = new Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueue
                {
                    CreateIfNotExistsQueueRequestOptionsOperationContext = (opt, ctx) =>
                    {
                        return true;
                    },
                    AddMessageCloudQueueMessageNullableOfTimeSpanNullableOfTimeSpanQueueRequestOptionsOperationContext = (msg, ttl,delay, options, ctx) =>
                    {
                        var deserializedMessage = JsonConvert.DeserializeObject<IndexDocumentBatchMessage>(msg.AsString);
                        Assert.AreEqual(1, deserializedMessage.CreatedDocuments.Count());
                        Assert.AreEqual(0, deserializedMessage.DeletedDocuments.Count());
                        Assert.AreEqual(0, deserializedMessage.ModifiedDocuments.Count());
                    },
                };

                Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueueClient.AllInstances.GetQueueReferenceString = (client, qName) =>
                {
                    Assert.AreEqual(queueName, qName);
                    return fakeQueue;
                };

                Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.AllInstances.CreateCloudQueueClient = (acct) =>
                {
                    return new Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueueClient();
                };

                Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.ParseString = (s) =>
                {
                    Assert.AreEqual(connectionString, s);
                    return new Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount();
                };

                saveAction.AfterSaveChanges((DbContext)context);
            }
        }


        [TestMethod]
        public async Task TestAfterSaveChangesAsync_NoMessages()
        {
            using (ShimsContext.Create())
            {
                var createIfNotExistsCalled = false;
                var addMessageCalled = false;
                var connectionString = "connection string";
                var queueName = "value";
                appSettings.Add(AppSettings.SEARCH_INDEX_QUEUE_NAME_KEY, queueName);
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.AZURE_WEB_JOBS_STORAGE_KEY, connectionString));
                var fakeQueue = new Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueue
                {
                    CreateIfNotExistsAsync = () =>
                    {
                        createIfNotExistsCalled = true;
                        return Task.FromResult<bool>(true);
                    },
                    AddMessageAsyncCloudQueueMessage = (msg) =>
                    {
                        addMessageCalled = true;                       
                        return Task.FromResult<object>(null);
                    },
                };

                Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueueClient.AllInstances.GetQueueReferenceString = (client, qName) =>
                {
                    Assert.AreEqual(queueName, qName);
                    return fakeQueue;
                };

                Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.AllInstances.CreateCloudQueueClient = (acct) =>
                {
                    return new Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueueClient();
                };

                Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.ParseString = (s) =>
                {
                    Assert.AreEqual(connectionString, s);
                    return new Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount();
                };

                await saveAction.AfterSaveChangesAsync((DbContext)context);
                Assert.IsFalse(createIfNotExistsCalled);
                Assert.IsFalse(addMessageCalled);
            }
        }

        [TestMethod]
        public void TestAfterSaveChanges_NoMessages()
        {
            using (ShimsContext.Create())
            {
                var createIfNotExistsCalled = false;
                var addMessageCalled = false;
                var connectionString = "connection string";
                var queueName = "value";
                appSettings.Add(AppSettings.SEARCH_INDEX_QUEUE_NAME_KEY, queueName);
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.AZURE_WEB_JOBS_STORAGE_KEY, connectionString));
                var fakeQueue = new Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueue
                {
                    CreateIfNotExistsQueueRequestOptionsOperationContext = (opt, ctx) =>
                    {
                        createIfNotExistsCalled = true;
                        return true;
                    },
                    AddMessageCloudQueueMessageNullableOfTimeSpanNullableOfTimeSpanQueueRequestOptionsOperationContext = (msg, ttl, delay, options, ctx) =>
                    {
                        addMessageCalled = true;
                    },
                };

                Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueueClient.AllInstances.GetQueueReferenceString = (client, qName) =>
                {
                    Assert.AreEqual(queueName, qName);
                    return fakeQueue;
                };

                Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.AllInstances.CreateCloudQueueClient = (acct) =>
                {
                    return new Microsoft.WindowsAzure.Storage.Queue.Fakes.ShimCloudQueueClient();
                };

                Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.ParseString = (s) =>
                {
                    Assert.AreEqual(connectionString, s);
                    return new Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount();
                };

                saveAction.AfterSaveChanges((DbContext)context);
                Assert.IsFalse(createIfNotExistsCalled);
                Assert.IsFalse(addMessageCalled);
            }
        }
        #endregion
    }
}
