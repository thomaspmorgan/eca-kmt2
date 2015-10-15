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
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class AddressToOrganizationDocumentSaveActionTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private AddressToOrganizationDocumentSaveAction saveAction;
        private InMemoryEcaContext context;


        [TestInitialize]
        public void TestInit()
        {
            context = new InMemoryEcaContext();
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            saveAction = new AddressToOrganizationDocumentSaveAction(settings);
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
        public async Task TestGetOrganizationDocumentKeys_DbPropertyValues_HasOrgId()
        {
            using (ShimsContext.Create())
            {
                var orgId = 10;
                var address = new Address
                {
                    OrganizationId = orgId
                };
                var dbPropertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                dbPropertyValues.GetValueOf1String<int?>((propertyName) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId), propertyName);
                    return orgId;
                });
                Action<List<DocumentKey>> tester = (documentKeys) =>
                {
                    Assert.AreEqual(1, documentKeys.Count);
                    var expectedDocumentKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
                    Assert.AreEqual(expectedDocumentKey, documentKeys.First());
                };
                var keys = saveAction.GetRelatedEntityDocumentKeysOfModifiedEntity(address, dbPropertyValues);
                var keysAsync = await saveAction.GetRelatedEntityDocumentKeysOfModifiedEntityAsync(address, dbPropertyValues);
                tester(keys);
                tester(keysAsync);
            }
        }

        [TestMethod]
        public async Task TestGetOrganizationDocumentKeys_DbPropertyValues_DoesNotHaveOrgId()
        {
            using (ShimsContext.Create())
            {
                var dbPropertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                dbPropertyValues.GetValueOf1String<int?>((propertyName) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId), propertyName);
                    return null;
                });
                Action<List<DocumentKey>> tester = (documentKeys) =>
                {
                    Assert.AreEqual(0, documentKeys.Count);
                };
                var keys = saveAction.GetRelatedEntityDocumentKeysOfModifiedEntity(null, dbPropertyValues);
                var keysAsync = await saveAction.GetRelatedEntityDocumentKeysOfModifiedEntityAsync(null, dbPropertyValues);
                tester(keys);
                tester(keysAsync);
            }
        }

        [TestMethod]
        public void TestGetOrganizationDocumentKeys_DoesNotHaveOrgIdOrOrgReference()
        {
            var address = new Address();
            Assert.IsFalse(address.OrganizationId.HasValue);
            Assert.IsNull(address.Organization);
            Action a = () => saveAction.GetOrganizationDocumentKeys(address);
            a.ShouldThrow<NotSupportedException>().WithMessage("Unable to determine organization document key from the given address.");
        }

        [TestMethod]
        public void TestGetOrganizationDocumentKeys_HasOrganizationId()
        {
            var orgId = 10;
            var address = new Address
            {
                OrganizationId = orgId
            };
            var expectedDocumentKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
            var documentKeys = saveAction.GetOrganizationDocumentKeys(address);
            Assert.AreEqual(1, documentKeys.Count);
            Assert.AreEqual(expectedDocumentKey, documentKeys.First());
        }

        [TestMethod]
        public void TestGetOrganizationDocumentKeys_HasOrganizationReference()
        {
            var orgId = 10;
            var address = new Address
            {
                Organization = new Organization
                {
                    OrganizationId = orgId
                }
            };
            var expectedDocumentKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
            var documentKeys = saveAction.GetOrganizationDocumentKeys(address);
            Assert.AreEqual(1, documentKeys.Count);
            Assert.AreEqual(expectedDocumentKey, documentKeys.First());
        }

        [TestMethod]
        public async Task TestGetRelatedEntityDocumentKeysOfAddedEntity()
        {
            var orgId = 10;
            var address = new Address
            {
                OrganizationId = orgId
            };
            Action<List<DocumentKey>> tester = (documentKeys) =>
            {
                Assert.AreEqual(1, documentKeys.Count);
                var expectedDocumentKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
                Assert.AreEqual(expectedDocumentKey, documentKeys.First());
            };
            var keys = saveAction.GetRelatedEntityDocumentKeysOfAddedEntity(address);
            var keysAsync = await saveAction.GetRelatedEntityDocumentKeysOfAddedEntityAsync(address);
            tester(keys);
            tester(keysAsync);
        }

        //[TestMethod]
        //public void TestGetDocumentKey_HasOrganizationId()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        var orgId = 10;
        //        var address = new Address
        //        {
        //            OrganizationId = orgId
        //        };
        //        var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>
        //        {
        //            OriginalValuesGet = () =>
        //            {
        //                var values = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
        //                values.GetValueOf1String<int?>((propertyName) =>
        //                {
        //                    var allowedPropertyNames = new List<string>
        //                    {
        //                        PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId),
        //                        PropertyHelper.GetPropertyName<Address>(x => x.PersonId),
        //                    };
        //                    Assert.IsTrue(allowedPropertyNames.Contains(propertyName));
        //                    return orgId;
        //                });
        //                return values;
        //            }
        //        };

        //        var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
        //        var testKey = saveAction.GetDocumentKey(address, entityEntry);
        //        Assert.AreEqual(expectedKey, testKey);
        //    }

        //}

        //[TestMethod]
        //public void TestGetDocumentKey_HasPersonId()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        var personId = 10;
        //        var address = new Address
        //        {
        //            PersonId = personId
        //        };
        //        var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>
        //        {
        //            OriginalValuesGet = () =>
        //            {
        //                var values = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
        //                values.GetValueOf1String<int?>((propertyName) =>
        //                {
        //                    var allowedPropertyNames = new List<string>
        //                    {
        //                        PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId),
        //                        PropertyHelper.GetPropertyName<Address>(x => x.PersonId),
        //                    };
        //                    Assert.IsTrue(allowedPropertyNames.Contains(propertyName));
        //                    //return null because I'm faking the organizationid does not have a value
        //                    return null;
        //                });
        //                return values;
        //            }
        //        };
        //        System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, add) =>
        //        {
        //            return entityEntry;
        //        });
        //        saveAction.Context = context;
        //        Action a = () => saveAction.GetDocumentKey(address, entityEntry);
        //        a.ShouldThrow<NotSupportedException>().WithMessage("Currently people are not indexed for searching; therefore, addresses related to people should not be indexed.  These address should be excluded.");
        //    }

        //}

        //[TestMethod]
        //public void TestGetBatchMessage_CreatedEntity()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        var orgId = 10;
        //        var address = new Address
        //        {
        //            OrganizationId = orgId
        //        };
        //        var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>
        //        {
        //            OriginalValuesGet = () =>
        //            {
        //                var values = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
        //                values.GetValueOf1String<int?>((propertyName) =>
        //                {
        //                    var allowedPropertyNames = new List<string>
        //                    {
        //                        PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId),
        //                        PropertyHelper.GetPropertyName<Address>(x => x.PersonId),
        //                    };
        //                    Assert.IsTrue(allowedPropertyNames.Contains(propertyName));
        //                    return orgId;
        //                });
        //                return values;
        //            }
        //        };

        //        System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, add) =>
        //        {
        //            return entityEntry;
        //        });
        //        saveAction.Context = context;
        //        saveAction.CreatedEntities.Add(address);

        //        var message = saveAction.GetBatchMessage();
        //        Assert.AreEqual(0, message.CreatedDocuments.Count());
        //        Assert.AreEqual(1, message.ModifiedDocuments.Count());
        //        Assert.AreEqual(0, message.DeletedDocuments.Count());

        //        var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
        //        Assert.AreEqual(expectedKey.ToString(), message.ModifiedDocuments.First());
        //    }

        //}

        //[TestMethod]
        //public void TestGetBatchMessage_DeletedEntity()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        var orgId = 10;
        //        var address = new Address
        //        {
        //            OrganizationId = orgId
        //        };
        //        var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>
        //        {
        //            OriginalValuesGet = () =>
        //            {
        //                var values = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
        //                values.GetValueOf1String<int?>((propertyName) =>
        //                {
        //                    var allowedPropertyNames = new List<string>
        //                    {
        //                        PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId),
        //                        PropertyHelper.GetPropertyName<Address>(x => x.PersonId),
        //                    };
        //                    Assert.IsTrue(allowedPropertyNames.Contains(propertyName));
        //                    return orgId;
        //                });
        //                return values;
        //            }
        //        };
        //        System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, add) =>
        //        {
        //            return entityEntry;
        //        });
        //        saveAction.Context = context;
        //        saveAction.DeletedEntities.Add(address);

        //        var message = saveAction.GetBatchMessage();
        //        Assert.AreEqual(0, message.CreatedDocuments.Count());
        //        Assert.AreEqual(1, message.ModifiedDocuments.Count());
        //        Assert.AreEqual(0, message.DeletedDocuments.Count());

        //        var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
        //        Assert.AreEqual(expectedKey.ToString(), message.ModifiedDocuments.First());
        //    }

        //}

        //[TestMethod]
        //public void TestGetBatchMessage_ModifiedEntity()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        var orgId = 10;
        //        var address = new Address
        //        {
        //            OrganizationId = orgId
        //        };
        //        var entityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>
        //        {
        //            OriginalValuesGet = () =>
        //            {
        //                var values = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
        //                values.GetValueOf1String<int?>((propertyName) =>
        //                {
        //                    var allowedPropertyNames = new List<string>
        //                    {
        //                        PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId),
        //                        PropertyHelper.GetPropertyName<Address>(x => x.PersonId),
        //                    };
        //                    Assert.IsTrue(allowedPropertyNames.Contains(propertyName));
        //                    return orgId;
        //                });
        //                return values;
        //            }
        //        };
        //        System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, add) =>
        //        {
        //            return entityEntry;
        //        });
        //        saveAction.Context = context;
        //        saveAction.ModifiedEntities.Add(address);

        //        var message = saveAction.GetBatchMessage();
        //        Assert.AreEqual(0, message.CreatedDocuments.Count());
        //        Assert.AreEqual(1, message.ModifiedDocuments.Count());
        //        Assert.AreEqual(0, message.DeletedDocuments.Count());

        //        var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
        //        Assert.AreEqual(expectedKey.ToString(), message.ModifiedDocuments.First());
        //    }

        //}
    }
}
