using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using System.Collections.Specialized;
using ECA.Core.Settings;
using System.Configuration;
using ECA.Data;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Core.DynamicLinq;
using System.Collections.Generic;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class AddressDocumentSaveActionTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private AddressDocumentSaveAction saveAction;
        private InMemoryEcaContext context;


        [TestInitialize]
        public void TestInit()
        {
            context = new InMemoryEcaContext();
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            saveAction = new AddressDocumentSaveAction(settings);
        }


        [TestMethod]
        public void TestIsCreatedEntityExcluded()
        {
            Assert.IsFalse(saveAction.IsCreatedEntityExcluded(new Address()));
        }

        [TestMethod]
        public void TestIsModifiedEntityExcluded()
        {
            Assert.IsFalse(saveAction.IsModifiedEntityExcluded(new Address()));
        }

        [TestMethod]
        public void TestIsDeletedEntityExcluded()
        {
            Assert.IsFalse(saveAction.IsDeletedEntityExcluded(new Address()));
        }

        [TestMethod]
        public void TestGetDocumentKey_HasOrganizationId()
        {
            using (ShimsContext.Create())
            {
                var orgId = 10;
                var address = new Address
                {
                    OrganizationId = orgId
                };
                var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>
                {
                    OriginalValuesGet = () =>
                    {
                        var values = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                        values.GetValueOf1String<int?>((propertyName) =>
                        {
                            var allowedPropertyNames = new List<string>
                            {
                                PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId),
                                PropertyHelper.GetPropertyName<Address>(x => x.PersonId),
                            };
                            Assert.IsTrue(allowedPropertyNames.Contains(propertyName));
                            return orgId;
                        });
                        return values;
                    }
                };

                var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
                var testKey = saveAction.GetDocumentKey(address, entityEntry);
                Assert.AreEqual(expectedKey, testKey);
            }

        }

        [TestMethod]
        public void TestGetDocumentKey_HasPersonId()
        {
            using (ShimsContext.Create())
            {
                var personId = 10;
                var address = new Address
                {
                    PersonId = personId
                };
                var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>
                {
                    OriginalValuesGet = () =>
                    {
                        var values = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                        values.GetValueOf1String<int?>((propertyName) =>
                        {
                            var allowedPropertyNames = new List<string>
                            {
                                PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId),
                                PropertyHelper.GetPropertyName<Address>(x => x.PersonId),
                            };
                            Assert.IsTrue(allowedPropertyNames.Contains(propertyName));
                            //return null because I'm faking the organizationid does not have a value
                            return null;
                        });
                        return values;
                    }
                };
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, add) =>
                {
                    return entityEntry;
                });
                saveAction.Context = context;
                Action a = () => saveAction.GetDocumentKey(address, entityEntry);
                a.ShouldThrow<NotSupportedException>().WithMessage("Currently people are not indexed for searching; therefore, addresses related to people should not be indexed.  These address should be excluded.");
            }

        }
        
        [TestMethod]
        public void TestGetBatchMessage_CreatedEntity()
        {
            using (ShimsContext.Create())
            {
                var orgId = 10;
                var address = new Address
                {
                    OrganizationId = orgId
                };
                var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>
                {
                    OriginalValuesGet = () =>
                    {
                        var values = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                        values.GetValueOf1String<int?>((propertyName) =>
                        {
                            var allowedPropertyNames = new List<string>
                            {
                                PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId),
                                PropertyHelper.GetPropertyName<Address>(x => x.PersonId),
                            };
                            Assert.IsTrue(allowedPropertyNames.Contains(propertyName));
                            return orgId;
                        });
                        return values;
                    }
                };
                
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, add) =>
                {
                    return entityEntry;
                });
                saveAction.Context = context;
                saveAction.CreatedEntities.Add(address);

                var message = saveAction.GetBatchMessage();
                Assert.AreEqual(0, message.CreatedDocuments.Count());
                Assert.AreEqual(1, message.ModifiedDocuments.Count());
                Assert.AreEqual(0, message.DeletedDocuments.Count());

                var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
                Assert.AreEqual(expectedKey.ToString(), message.ModifiedDocuments.First());
            }
                
        }

        [TestMethod]
        public void TestGetBatchMessage_DeletedEntity()
        {
            using (ShimsContext.Create())
            {
                var orgId = 10;
                var address = new Address
                {
                    OrganizationId = orgId
                };
                var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>
                {
                    OriginalValuesGet = () =>
                    {
                        var values = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                        values.GetValueOf1String<int?>((propertyName) =>
                        {
                            var allowedPropertyNames = new List<string>
                            {
                                PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId),
                                PropertyHelper.GetPropertyName<Address>(x => x.PersonId),
                            };
                            Assert.IsTrue(allowedPropertyNames.Contains(propertyName));
                            return orgId;
                        });
                        return values;
                    }
                };
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, add) =>
                {
                    return entityEntry;
                });
                saveAction.Context = context;
                saveAction.DeletedEntities.Add(address);

                var message = saveAction.GetBatchMessage();
                Assert.AreEqual(0, message.CreatedDocuments.Count());
                Assert.AreEqual(1, message.ModifiedDocuments.Count());
                Assert.AreEqual(0, message.DeletedDocuments.Count());

                var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
                Assert.AreEqual(expectedKey.ToString(), message.ModifiedDocuments.First());
            }
            
        }

        [TestMethod]
        public void TestGetBatchMessage_ModifiedEntity()
        {
            using (ShimsContext.Create())
            {
                var orgId = 10;
                var address = new Address
                {
                    OrganizationId = orgId
                };
                var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>
                {
                    OriginalValuesGet = () =>
                    {
                        var values = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                        values.GetValueOf1String<int?>((propertyName) =>
                        {
                            var allowedPropertyNames = new List<string>
                            {
                                PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId),
                                PropertyHelper.GetPropertyName<Address>(x => x.PersonId),
                            };
                            Assert.IsTrue(allowedPropertyNames.Contains(propertyName));
                            return orgId;
                        });
                        return values;
                    }
                };
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, add) =>
                {
                    return entityEntry;
                });
                saveAction.Context = context;
                saveAction.ModifiedEntities.Add(address);

                var message = saveAction.GetBatchMessage();
                Assert.AreEqual(0, message.CreatedDocuments.Count());
                Assert.AreEqual(1, message.ModifiedDocuments.Count());
                Assert.AreEqual(0, message.DeletedDocuments.Count());

                var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
                Assert.AreEqual(expectedKey.ToString(), message.ModifiedDocuments.First());
            }
                
        }
    }
}
