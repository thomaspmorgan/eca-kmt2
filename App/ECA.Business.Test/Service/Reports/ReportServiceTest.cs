using ECA.Business.Exceptions;
using ECA.Business.Queries.Models.Reports;
using ECA.Business.Service;
using ECA.Business.Service.Reports;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Reports
{
    [TestClass]
    public class ReportServiceTest
    {
        private TestEcaContext context;
        private ReportService service;


        [TestInitialize]
        public void TestInit()
        {

            context = new TestEcaContext();
            service = new ReportService(context);
        }


        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public void TestGetProgramAwards()
        {
            float value1 = 500.26F;
            float value2 = 750.73F;
            float value3 = 1204.13F;
            float value4 = 25.31F;
            int locationId = 1;
            int countryId = 500;
            int programId = 501;
            DateTime date1 = new DateTime(2014, 1, 5);

            var locationInFrance = new Location
            {
                LocationId = locationId,
                LocationName = "Hotel du Champ de Mars",
                Street1 = "104 Champ de Mars",
                LocationTypeId = LocationType.Address.Id,
                CountryId = countryId
            };

            var moneyFlow1 = new MoneyFlow
            {
                MoneyFlowId = 1,
                Value = value1,
                RecipientProjectId = 1,
            };

            var moneyFlow2 = new MoneyFlow
            {
                MoneyFlowId = 2,
                Value = value2,
                RecipientProjectId = 2
            };

            var moneyFlow3 = new MoneyFlow
            {
                MoneyFlowId = 3,
                Value = value3,
                RecipientProjectId = 3
            };

            var moneyFlow4 = new MoneyFlow
            {
                MoneyFlowId = 5,
                Value = value4,
                RecipientProjectId = 4
            };

            var program1 = new Program
            {
                ProgramId = programId
            };

            var project1 = new Project
            {
                ProjectId = 1,
                Name = "Test Project 1",
                Description = "This is a long description",
                StartDate = new DateTimeOffset(date1),
                ProgramId = programId
            };

            var project2 = new Project
            {
                ProjectId = 2,
                Name = "Test Project 2",
                Description = "This is a long description",
                StartDate = new DateTimeOffset(date1),
                ProgramId = programId
            };

            var project3 = new Project
            {
                ProjectId = 3,
                Name = "Test Project 3",
                Description = "This is a long description",
                StartDate = new DateTimeOffset(date1),
                ProgramId = programId
            };


            var projectAward1 = new ProjectAwardDTO
            {
                Award = value1,
                Summary = project1.Description,
                Title = project1.Name,
                Year = project1.StartDate.Year
            };

            var projectAward2 = new ProjectAwardDTO
            {
                Award = value2,
                Summary = project2.Description,
                Title = project2.Name,
                Year = project2.StartDate.Year
            };

            var projectAward3 = new ProjectAwardDTO
            {
                Award = value3 + value4,
                Summary = project3.Description,
                Title = project3.Name,
                Year = project3.StartDate.Year
            };

            var projectAwards = new List<ProjectAwardDTO>();

            projectAwards.Add(projectAward1);
            projectAwards.Add(projectAward2);
            projectAwards.Add(projectAward3);

            project1.RecipientProjectMoneyFlows.Add(moneyFlow1);
            project2.RecipientProjectMoneyFlows.Add(moneyFlow2);
            project3.RecipientProjectMoneyFlows.Add(moneyFlow3);
            project3.RecipientProjectMoneyFlows.Add(moneyFlow4);

            project1.Locations.Add(locationInFrance);
            project2.Locations.Add(locationInFrance);
            project3.Locations.Add(locationInFrance);


            context.Projects.Add(project1);
            context.Projects.Add(project2);
            context.Projects.Add(project3);




            Action<IQueryable<ProjectAwardDTO>> tester = (resultAwards) =>
            {
                var resultList = resultAwards.ToList<ProjectAwardDTO>();
                CollectionAssert.AreEqual(resultList.Select(p => p.Year).ToList(), projectAwards.Select(p => p.Year).ToList());
                CollectionAssert.AreEqual(resultList.Select(p => p.Title).ToList(), projectAwards.Select(p => p.Title).ToList());
                CollectionAssert.AreEqual(resultList.Select(p => p.Summary).ToList(), projectAwards.Select(p => p.Summary).ToList());
                CollectionAssert.AreEqual(resultList.Select(p => p.Award).ToList(), projectAwards.Select(p => p.Award).ToList());
            };
            var result = service.GetProjectAwards(programId, countryId);
            tester(result);
        }

        [TestMethod]
        public void TestGetRegionAwards()
        {
            float value1 = 500.26F;
            float value2 = 750.73F;
            float value3 = 1204.13F;
            float value4 = 25.31F;
            int locationId1 = 1;
            int locationId2 = 2;
            int locationId3 = 3;
            int locationId4 = 4; 
            int regionIdAF = 5;
            int regionIdEUR = 6;
            int countryId1 = 500;
            int countryId2 = 501;
            int programId = 4;
            DateTime date1 = new DateTime(2014, 1, 5);

            var locationInFrance = new Location
            {
                LocationId = locationId1,
                LocationName = "Hotel du Champ de Mars",
                Street1 = "104 Champ de Mars",
                LocationTypeId = LocationType.Address.Id,
                CountryId = countryId1,
                RegionId = regionIdEUR
            };

            var locationInSouthAfrica = new Location
            {
                LocationId = locationId2,
                LocationName = "Hotel Prestige",
                Street1 = "104 Rex Ave",
                LocationTypeId = LocationType.Address.Id,
                CountryId = countryId2,
                RegionId = regionIdAF
            };

            var regionAF = new Location
            {
                LocationId = locationId3,
                LocationName = "AF",
                LocationTypeId = LocationType.Region.Id,
            };

            var regionEUR= new Location
            {
                LocationId = locationId4,
                LocationName = "EUR",
                LocationTypeId = LocationType.Region.Id,
            };

            var moneyFlow1 = new MoneyFlow
            {
                MoneyFlowId = 1,
                Value = value1,
                RecipientProjectId = 1,
                SourceProgramId = programId
            };

            var moneyFlow2 = new MoneyFlow
            {
                MoneyFlowId = 2,
                Value = value2,
                RecipientProjectId = 2,
                SourceProgramId = programId
            };

            var moneyFlow3 = new MoneyFlow
            {
                MoneyFlowId = 3,
                Value = value3,
                RecipientProjectId = 3,
                SourceProgramId = null
            };

            var moneyFlow4 = new MoneyFlow
            {
                MoneyFlowId = 5,
                Value = value4,
                RecipientProjectId = 4,
                SourceProgramId = programId
            };

            var program1 = new Program
            {
                ProgramId = programId
            };

            var project1 = new Project
            {
                ProjectId = 1,
                Name = "Test Project 1",
                Description = "This is a long description",
                StartDate = new DateTimeOffset(date1),
                ProgramId = programId
            };

            var project2 = new Project
            {
                ProjectId = 2,
                Name = "Test Project 2",
                Description = "This is a long description",
                StartDate = new DateTimeOffset(date1),
                ProgramId = programId
            };

            var project3 = new Project
            {
                ProjectId = 3,
                Name = "Test Project 3",
                Description = "This is a long description",
                StartDate = new DateTimeOffset(date1),
                ProgramId = programId
            };


            var regionAward1 = new RegionAwardDTO
            {
                Region = "EUR",
                ProgramValue = value1 + value2,
                OtherValue = 0.0F,
                Projects = 2,
                Average = (value1 + value2)/2
            };

            var regionAward2 = new RegionAwardDTO
            {
                Region = "AF",
                ProgramValue = value4,
                OtherValue = value3,
                Projects = 2,
                Average = (value3 + value4) / 2
            };

            var regionAwards = new List<RegionAwardDTO>();

            regionAwards.Add(regionAward1);
            regionAwards.Add(regionAward2);


            project1.SourceProjectMoneyFlows.Add(moneyFlow1);
            project2.SourceProjectMoneyFlows.Add(moneyFlow2);
            project3.SourceProjectMoneyFlows.Add(moneyFlow3);
            project3.SourceProjectMoneyFlows.Add(moneyFlow4);

            locationInSouthAfrica.Region = regionAF;
            locationInFrance.Region = regionEUR;

            context.Locations.Add(regionEUR);
            context.Locations.Add(regionAF);
            context.Locations.Add(locationInFrance);
            context.Locations.Add(locationInSouthAfrica);

            project1.Locations.Add(locationInFrance);
            project2.Locations.Add(locationInFrance);
            project3.Locations.Add(locationInSouthAfrica);


            context.Projects.Add(project1);
            context.Projects.Add(project2);
            context.Projects.Add(project3);

            moneyFlow1.RecipientProject = project1;
            moneyFlow2.RecipientProject = project2;
            moneyFlow3.RecipientProject = project3;
            moneyFlow4.RecipientProject = project3;

            context.MoneyFlows.Add(moneyFlow1);
            context.MoneyFlows.Add(moneyFlow2);
            context.MoneyFlows.Add(moneyFlow3);
            context.MoneyFlows.Add(moneyFlow4);




            Action<IQueryable<RegionAwardDTO>> tester = (resultAwards) =>
            {
                var resultList = resultAwards.ToList<RegionAwardDTO>();
                CollectionAssert.AreEqual(resultList.Select(p => p.Region).ToList(), regionAwards.Select(p => p.Region).ToList());
                CollectionAssert.AreEqual(resultList.Select(p => p.ProgramValue).ToList(), regionAwards.Select(p => p.ProgramValue).ToList());
                CollectionAssert.AreEqual(resultList.Select(p => p.OtherValue).ToList(), regionAwards.Select(p => p.OtherValue).ToList());
                CollectionAssert.AreEqual(resultList.Select(p => p.Projects).ToList(), regionAwards.Select(p => p.Projects).ToList());
                CollectionAssert.AreEqual(resultList.Select(p => p.Average).ToList(), regionAwards.Select(p => p.Average).ToList());
            };

            var result = service.GetRegionAwards(programId);
            tester(result);
        }
        #endregion
    }


}

