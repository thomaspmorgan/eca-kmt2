using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using Moq;
using System.Reflection;
using ECA.Data;
using ECA.Core.Generation;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class OfficeDocumentServiceTest
    {
        private InMemoryEcaContext context;
        private OfficeDocumentService service;
        private Mock<IIndexService> indexService;
        private Mock<IIndexNotificationService> notificationService;
        private int batchSize;


        [TestInitialize]
        public void TestInit()
        {
            batchSize = 1;
            context = new InMemoryEcaContext();
            indexService = new Mock<IIndexService>();
            notificationService = new Mock<IIndexNotificationService>();
            service = new OfficeDocumentService(context, indexService.Object, notificationService.Object, batchSize);
        }


        [TestMethod]
        public void TestConstructor()
        {
            batchSize = 1;
            service = new OfficeDocumentService(context, indexService.Object, notificationService.Object, batchSize);
            Assert.AreEqual(batchSize, service.GetBatchSize());

            var batchSizeField = typeof(OfficeDocumentService).BaseType.GetField("batchSize", BindingFlags.Instance | BindingFlags.NonPublic);
            var batchSizeValue = batchSizeField.GetValue(service);
            Assert.AreEqual(batchSize, batchSizeValue);
        }

        [TestMethod]
        public void TestCreateGetDocumentsQuery_CheckOrgTypes()
        {
            var lastRevised = DateTimeOffset.UtcNow;
            var orgType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var org = new Organization
            {
                Description = "description",
                Name = "name",
                OrganizationId = 1,
                OrganizationType = orgType,
                OrganizationTypeId = orgType.OrganizationTypeId,
                Website = "website",
            };
            org.History.RevisedOn = lastRevised;
            context.Organizations.Add(org);
            context.OrganizationTypes.Add(orgType);


            var staticProperties = typeof(OrganizationType).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            var query = service.CreateGetDocumentsQuery();
            foreach (var orgTypeLookup in allStaticLookups)
            {
                var isOffice = false;
                if (Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(orgTypeLookup.Id))
                {
                    isOffice = true;
                }
                orgType.OrganizationTypeId = orgTypeLookup.Id;
                orgType.OrganizationTypeName = orgTypeLookup.Value;
                org.OrganizationTypeId = orgTypeLookup.Id;
                if (isOffice)
                {
                    Assert.AreEqual(1, query.Count());
                }
                else
                {
                    Assert.AreEqual(0, query.Count());
                }
            }
        }

        [TestMethod]
        public void TestCreateGetDocumentsByIdQuery_CheckOrgTypes()
        {
            var lastRevised = DateTimeOffset.UtcNow;
            var orgType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var org = new Organization
            {
                Description = "description",
                Name = "name",
                OrganizationId = 1,
                OrganizationType = orgType,
                OrganizationTypeId = orgType.OrganizationTypeId,
                Website = "website",
            };
            org.History.RevisedOn = lastRevised;
            context.Organizations.Add(org);
            context.OrganizationTypes.Add(orgType);


            var staticProperties = typeof(OrganizationType).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            var query = service.CreateGetDocumentByIdQuery(org.OrganizationId);
            foreach (var orgTypeLookup in allStaticLookups)
            {
                var isOffice = false;
                if (Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(orgTypeLookup.Id))
                {
                    isOffice = true;
                }
                orgType.OrganizationTypeId = orgTypeLookup.Id;
                orgType.OrganizationTypeName = orgTypeLookup.Value;
                org.OrganizationTypeId = orgTypeLookup.Id;
                if (isOffice)
                {
                    Assert.AreEqual(1, query.Count());
                }
                else
                {
                    Assert.AreEqual(0, query.Count());
                }
            }
        }
    }
}
