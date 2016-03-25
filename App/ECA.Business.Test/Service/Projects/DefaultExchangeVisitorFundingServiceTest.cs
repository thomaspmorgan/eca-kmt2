using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Projects;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Projects
{
    [TestClass]
    public class DefaultExchangeVisitorFundingServiceTest
    {
        private TestEcaContext context;
        private DefaultExchangeVisitorFundingService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new DefaultExchangeVisitorFundingService(context);
        }

        #region Get
        [TestMethod]
        public async Task TestGetDefaultExchangeVisitorFundingAsync_CheckDefaultFunding()
        {
            var model = new DefaultExchangeVisitorFunding
            {
                ProjectId = 1
            };
            context.DefaultExchangeVisitorFunding.Add(model);
            var result = await service.GetDefaultExchangeVisitorFundingAsync(model.ProjectId);

            Assert.AreEqual(model.ProjectId, result.ProjectId);
            Assert.AreEqual(0, result.FundingSponsor);
            Assert.AreEqual(0, result.FundingPersonal);
            Assert.AreEqual(0, result.FundingVisGovt);
            Assert.AreEqual(0, result.FundingVisBNC);
            Assert.AreEqual(0, result.FundingGovtAgency1);
            Assert.AreEqual(0, result.FundingGovtAgency2);
            Assert.AreEqual(0, result.FundingIntlOrg1);
            Assert.AreEqual(0, result.FundingIntlOrg2);
            Assert.AreEqual(0, result.FundingOther);
            Assert.AreEqual(0, result.FundingTotal);
        }

        [TestMethod]
        public async Task TestGetDefaultExchangeVisitorFundingAsync_CheckFunding()
        {
            var model = new DefaultExchangeVisitorFunding
            {
                ProjectId = 1,
                FundingSponsor = 1,
                FundingPersonal = 2,
                FundingVisGovt = 3,
                FundingVisBNC = 4,
                FundingGovtAgency1 = 5,
                FundingGovtAgency2 = 6,
                FundingIntlOrg1 = 7,
                FundingIntlOrg2 = 8,
                FundingOther = 9,
                FundingTotal = 45
            };
            context.DefaultExchangeVisitorFunding.Add(model);
            var result = await service.GetDefaultExchangeVisitorFundingAsync(model.ProjectId);

            Assert.AreEqual(1, result.FundingSponsor);
            Assert.AreEqual(2, result.FundingPersonal);
            Assert.AreEqual(3, result.FundingVisGovt);
            Assert.AreEqual(4, result.FundingVisBNC);
            Assert.AreEqual(5, result.FundingGovtAgency1);
            Assert.AreEqual(6, result.FundingGovtAgency2);
            Assert.AreEqual(7, result.FundingIntlOrg1);
            Assert.AreEqual(8, result.FundingIntlOrg2);
            Assert.AreEqual(9, result.FundingOther);
            Assert.AreEqual(45, result.FundingTotal);
        }

        [TestMethod]
        public async Task TestGetDefaultExchangeVisitorFundingAsync_NullAgenciesAndOrgs()
        {
            var model = new DefaultExchangeVisitorFunding
            {
                ProjectId = 1
            };
            context.DefaultExchangeVisitorFunding.Add(model);
            var result = await service.GetDefaultExchangeVisitorFundingAsync(model.ProjectId);

            Assert.IsNull(result.GovtAgency1Id);
            Assert.IsNull(result.GovtAgency2Id);
            Assert.IsNull(result.IntlOrg1Id);
            Assert.IsNull(result.IntlOrg2Id);
        }

        [TestMethod]
        public async Task TestGetDefaultExchangeVisitorFundingAsync_CheckAgenciesAndOrgs()
        {
            var govtAgency1 = new USGovernmentAgency {
                AgencyId = 1,
                Description = "govtAgency1"
            };

            var govtAgency2 = new USGovernmentAgency {
                AgencyId = 2,
                Description = "govtAgency2"
            };

            var intlOrg1 = new InternationalOrganization
            {
                OrganizationId = 1,
                Description = "intlOrg1"
            };

            var intlOrg2 = new InternationalOrganization
            {
                OrganizationId = 2,
                Description = "intlOrg2"
            };

            context.USGovernmentAgencies.Add(govtAgency1);
            context.USGovernmentAgencies.Add(govtAgency2);
            context.InternationalOrganizations.Add(intlOrg1);
            context.InternationalOrganizations.Add(intlOrg2);

            var model = new DefaultExchangeVisitorFunding
            {
                ProjectId = 1,
                GovtAgency1Id = govtAgency1.AgencyId,
                GovtAgency1 = govtAgency1,
                GovtAgency1OtherName = "govtAgency1OtherName",
                GovtAgency2Id = govtAgency2.AgencyId,
                GovtAgency2 = govtAgency2,
                GovtAgency2OtherName = "govtAgency2OtherName",
                IntlOrg1Id = intlOrg1.OrganizationId,
                IntlOrg1 = intlOrg1,
                IntlOrg1OtherName = "intlOrg1OtherName",
                IntlOrg2Id = intlOrg2.OrganizationId,
                IntlOrg2 = intlOrg2,
                IntlOrg2OtherName = "intlOrg2OtherName"
            };
            context.DefaultExchangeVisitorFunding.Add(model);
            var result = await service.GetDefaultExchangeVisitorFundingAsync(model.ProjectId);

            Assert.AreEqual(govtAgency1.AgencyId, result.GovtAgency1Id);
            Assert.AreEqual(govtAgency1.Description, result.GovtAgency1Name);
            Assert.AreEqual(model.GovtAgency1OtherName, result.GovtAgency1OtherName);
            Assert.AreEqual(govtAgency2.AgencyId, result.GovtAgency2Id);
            Assert.AreEqual(govtAgency2.Description, result.GovtAgency2Name);
            Assert.AreEqual(model.GovtAgency2OtherName, result.GovtAgency2OtherName);
            Assert.AreEqual(intlOrg1.OrganizationId, result.IntlOrg1Id);
            Assert.AreEqual(intlOrg1.Description, result.IntlOrg1Name);
            Assert.AreEqual(model.IntlOrg1OtherName, result.IntlOrg1OtherName);
            Assert.AreEqual(intlOrg2.OrganizationId, result.IntlOrg2Id);
            Assert.AreEqual(intlOrg2.Description, result.IntlOrg2Name);
            Assert.AreEqual(model.IntlOrg2OtherName, result.IntlOrg2OtherName);
        }

        [TestMethod]
        public async Task TestGetDefaultExchangeVisitorFundingAsync_CheckOtherName()
        {
            var model = new DefaultExchangeVisitorFunding
            {
                ProjectId = 1,
                OtherName = "otherName"
            };
            context.DefaultExchangeVisitorFunding.Add(model);
            var result = await service.GetDefaultExchangeVisitorFundingAsync(model.ProjectId);

            Assert.AreEqual(model.OtherName, result.OtherName);
        }
        #endregion

        #region Put
        [TestMethod]
        [ExpectedException(typeof(ModelNotFoundException))]
        public async Task TestUpdateAsync_ModelNotFound()
        {
            var updatedDefaultExchangeVisitorFunding = new UpdatedDefaultExchangeVisitorFunding(
                new Business.Service.User(1),
                1,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);
            await service.UpdateAsync(updatedDefaultExchangeVisitorFunding);
        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckProperties()
        {
            var govtAgency1 = new USGovernmentAgency
            {
                AgencyId = 1,
                Description = "govtAgency1"
            };

            var govtAgency2 = new USGovernmentAgency
            {
                AgencyId = 2,
                Description = "govtAgency2"
            };

            context.USGovernmentAgencies.Add(govtAgency1);
            context.USGovernmentAgencies.Add(govtAgency2);

            var intlOrg1 = new InternationalOrganization
            {
                OrganizationId = 1,
                Description = "intlOrg1"
            };

            var intlOrg2 = new InternationalOrganization
            {
                OrganizationId = 2,
                Description = "intlOrg2"
            };

            context.InternationalOrganizations.Add(intlOrg1);
            context.InternationalOrganizations.Add(intlOrg2);

            var initialExchangeVisitorFunding = new DefaultExchangeVisitorFunding
            {
                ProjectId = 1,
                FundingSponsor = 0,
                FundingPersonal = 0,
                FundingVisGovt = 0,
                FundingVisBNC = 0,
                FundingGovtAgency1 = 0,
                FundingGovtAgency2 = 0,
                FundingIntlOrg1 = 0,
                FundingIntlOrg2 = 0,
                FundingOther = 0,
                FundingTotal = 0
            };
            context.DefaultExchangeVisitorFunding.Add(initialExchangeVisitorFunding);

            var updatedDefaultExchangeVisitorFunding = new UpdatedDefaultExchangeVisitorFunding(
               new Business.Service.User(1),
               1,
               2,
               3,
               4,
               5,
               6,
               govtAgency1.AgencyId,
               "Govt Agency 1",
               7,
               govtAgency2.AgencyId,
               "Govt Agency 2",
               8,
               intlOrg1.OrganizationId,
               "Intl Org 1",
               9,
               intlOrg2.OrganizationId,
               "Intl Org 2",
               10,
               "Other Name",
               55);

            await service.UpdateAsync(updatedDefaultExchangeVisitorFunding);

            var defaultExchangeVisitorFunding = context.DefaultExchangeVisitorFunding.Find(initialExchangeVisitorFunding.ProjectId);

            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.ProjectId, defaultExchangeVisitorFunding.ProjectId);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.FundingSponsor, defaultExchangeVisitorFunding.FundingSponsor);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.FundingPersonal, defaultExchangeVisitorFunding.FundingPersonal);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.FundingVisGovt, defaultExchangeVisitorFunding.FundingVisGovt);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.FundingVisBNC, defaultExchangeVisitorFunding.FundingVisBNC);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.FundingGovtAgency1, defaultExchangeVisitorFunding.FundingGovtAgency1);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.FundingGovtAgency2, defaultExchangeVisitorFunding.FundingGovtAgency2);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.FundingIntlOrg1, defaultExchangeVisitorFunding.FundingIntlOrg1);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.FundingIntlOrg2, defaultExchangeVisitorFunding.FundingIntlOrg2);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.FundingOther, defaultExchangeVisitorFunding.FundingOther);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.FundingTotal, defaultExchangeVisitorFunding.FundingTotal);

            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.GovtAgency1OtherName, defaultExchangeVisitorFunding.GovtAgency1OtherName);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.GovtAgency2OtherName, defaultExchangeVisitorFunding.GovtAgency2OtherName);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.IntlOrg1OtherName, defaultExchangeVisitorFunding.IntlOrg1OtherName);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.IntlOrg2OtherName, defaultExchangeVisitorFunding.IntlOrg2OtherName);
            Assert.AreEqual(updatedDefaultExchangeVisitorFunding.OtherName, defaultExchangeVisitorFunding.OtherName);

            Assert.AreEqual(govtAgency1.AgencyId, defaultExchangeVisitorFunding.GovtAgency1Id);
            Assert.AreEqual(govtAgency2.AgencyId, defaultExchangeVisitorFunding.GovtAgency2Id);
            Assert.AreEqual(intlOrg1.OrganizationId, defaultExchangeVisitorFunding.IntlOrg1Id);
            Assert.AreEqual(intlOrg2.OrganizationId, defaultExchangeVisitorFunding.IntlOrg2Id);
        }
        #endregion
    }
}
