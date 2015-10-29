using System.Linq;
using ECA.Business.Queries.Admin;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Admin;

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

        //[TestMethod]
        //public void TestCreateGetProgramCountryCountQuery()
        //{
        //    var active = new ProgramStatus
        //    {
        //        ProgramStatusId = ProgramStatus.Active.Id,
        //        Status = ProgramStatus.Active.Value
        //    };

        //    var organizationType = new OrganizationType
        //    {
        //        OrganizationTypeId = OrganizationType.Office.Id,
        //        OrganizationTypeName = OrganizationType.Office.Value
        //    };

        //    var org1 = new Organization
        //    {
        //        OrganizationId = 1,
        //        OrganizationTypeId = organizationType.OrganizationTypeId,
        //        Name = "prog owner 1",
        //        OfficeSymbol = "officesymbol",
        //    };

        //    var locations = new List<Location>();
        //    var regions = new List<Location>();

        //    var region1 = new Location()
        //    {
        //        LocationId = 1,
        //        LocationTypeId = 2
        //    };
        //    regions.Add(region1);

        //    var country1 = new Location()
        //    {
        //        LocationId = 2,
        //        RegionId = 1,
        //        LocationTypeId = 3
        //    };
        //    locations.Add(country1);

        //    var country2 = new Location()
        //    {
        //        LocationId = 3,
        //        RegionId = 1,
        //        LocationTypeId = 3
        //    };
        //    locations.Add(country2);

        //    var country3 = new Location()
        //    {
        //        LocationId = 4,
        //        RegionId = 1,
        //        LocationTypeId = 3
        //    };
        //    locations.Add(country3);

        //    var program1 = new Program
        //    {
        //        ProgramId = 1008,
        //        Name = "program 1",
        //        Owner = org1,
        //        OwnerId = org1.OrganizationId,
        //        ProgramStatus = active,
        //        ProgramStatusId = active.ProgramStatusId,
        //        Regions = regions,
        //        Locations = locations
        //    };

        //    context.OrganizationTypes.Add(organizationType);
        //    context.Organizations.Add(org1);
        //    context.ProgramStatuses.Add(active);
        //    context.Programs.Add(program1);
        //    context.Locations.Add(region1);
        //    context.Locations.Add(country1);
        //    context.Locations.Add(country2);
        //    context.Locations.Add(country3);

        //    var results = SnapshotQueries.CreateGetProgramCountryCountQuery(context, program1.ProgramId);
        //    Assert.AreEqual(3, results.Result.DataValue);
        //}
    }
}
