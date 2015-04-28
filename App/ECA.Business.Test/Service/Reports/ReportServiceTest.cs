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
            DateTime date1 = new DateTime(2014, 10, 4);

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

            var project1 = new Project
            {
                ProjectId = 1,
                Name = "Test Project 1",
                Description = "This is a long description",
                StartDate = new DateTimeOffset(date1)
            };

            var project2 = new Project
            {
                ProjectId = 2,
                Name = "Test Project 2",
                Description = "This is a long description",
                StartDate = new DateTimeOffset(date1)
            };

            var project3 = new Project
            {
                ProjectId = 3,
                Name = "Test Project 3",
                Description = "This is a long description",
                StartDate = new DateTimeOffset(date1)
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
            var result = service.GetProjectAwards(2014, countryId);
            tester(result);
        }
        #endregion
    }

}

