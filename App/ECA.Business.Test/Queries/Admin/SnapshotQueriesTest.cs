using System.Linq;
using ECA.Business.Queries.Admin;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Queries.Admin
{
    [TestClass]
    public class SnapshotQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetProgramCountryCountQuery()
        {

            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Office.Id,
                OrganizationTypeName = OrganizationType.Office.Value
            };

            var owner1 = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                Name = "owner 1",
                OfficeSymbol = "owner 1 symbol",
            };

            var program1 = new Program
            {
                ProgramId = 1008,
                Description = "desc1",
                History = new History
                {
                    CreatedBy = 1,
                },
                Name = "program 1",
                Owner = owner1,
                OwnerId = owner1.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId,
            };

            context.OrganizationTypes.Add(organizationType);
            context.Programs.Add(program1);
            context.Organizations.Add(owner1);
            context.ProgramStatuses.Add(active);

            var results = SnapshotQueries.CreateGetProgramCountryCountQuery(context, program1.ProgramId);
            Assert.AreNotEqual(0, results);
        }
    }
}
