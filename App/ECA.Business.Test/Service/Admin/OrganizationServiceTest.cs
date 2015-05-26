using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using System.Linq;

namespace ECA.Business.Test.Service.Admin
{
    /// <summary>
    /// Summary description for OrganizationServiceTest
    /// </summary>
    [TestClass]
    public class OrganizationServiceTest
    {
        private TestEcaContext context;
        private OrganizationService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new OrganizationService(context);
        }

        /** Not working
        [TestMethod]
        public async Task TestGetOrganizations_CheckProperties()
        {
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Division.Id,
                OrganizationTypeName = OrganizationType.Division.Value
            };

            var organization = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                Name = "name",
                Description = "test",
                Status = "status",
                ParentOrganization = new Organization()
            };

            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(organization);

            var defaultSorter = new ExpressionSorter<SimpleOrganizationDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleOrganizationDTO>(0, 1, defaultSorter);
            var serviceResultsAsync = await service.GetOrganizationsAsync(queryOperator);

            Assert.AreEqual(1, serviceResultsAsync.Total);
        }
        **/
    }
}
