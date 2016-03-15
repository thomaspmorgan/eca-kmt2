using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Projects;
using System.Threading.Tasks;
using ECA.Data;

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

    }
}
