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

        [TestMethod]
        public void TestCreateGetProgramCountryCountQuery()
        {
            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var typeRegion = new LocationType
            {
                LocationTypeId = LocationType.Region.Id,
                LocationTypeName = LocationType.Region.Value
            };
            var typeCountry = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };

            var region1 = new Location()
            {
                LocationId = 1,
                LocationType = typeRegion,
                LocationTypeId = typeRegion.LocationTypeId
            };

            var country1 = new Location()
            {
                LocationId = 2,
                RegionId = 1,
                LocationType = typeCountry,
                LocationTypeId = typeCountry.LocationTypeId
            };

            var country2 = new Location()
            {
                LocationId = 3,
                RegionId = 1,
                LocationType = typeCountry,
                LocationTypeId = typeCountry.LocationTypeId
            };

            var country3 = new Location()
            {
                LocationId = 4,
                RegionId = 1,
                LocationType = typeCountry,
                LocationTypeId = typeCountry.LocationTypeId
            };

            var program1 = new Program
            {
                ProgramId = 1,
                Name = "program 1",
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId
            };

            program1.Regions.Add(region1);
            program1.Locations.Add(country1);
            program1.Locations.Add(country2);
            program1.Locations.Add(country3);
            context.ProgramStatuses.Add(active);
            context.Programs.Add(program1);
            context.LocationTypes.Add(typeRegion);
            context.LocationTypes.Add(typeCountry);
            context.Locations.Add(region1);
            context.Locations.Add(country1);
            context.Locations.Add(country2);
            context.Locations.Add(country3);

            List<int> programIds = new List<int>();
            programIds.Add(program1.ProgramId);

            var results = SnapshotQueries.CreateGetProgramCountriesByProgramIdsQuery(context, programIds);
            Assert.AreEqual(3, results.Count());
        }
    }
}
