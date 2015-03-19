using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.Logging;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class OfficeServiceTest
    {
        private TestEcaContext context;
        private OfficeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new OfficeService(context, new TraceLogger());
        }

        #region Get
        [TestMethod]
        public async Task TestGetOfficeById_CheckProperties()
        {
            var now = DateTimeOffset.UtcNow;
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Office.Id,
                Name = "office",
                Description = "office desc",
                OfficeSymbol = "symbol"
                
            };
            office.History.RevisedOn = now;
            var contact = new Contact
            {
                ContactId = 2,
                FullName = "full name",
            };
            var program = new Program
            {
                ProgramId = 3,
                Owner = office
            };
            var goal = new Goal
            {
                GoalId = 4,
                GoalName = "goal"
            };
            var theme = new Theme
            {
                ThemeId = 5,
                ThemeName = "theme"
            };
            var focus = new Focus
            {
                FocusId = 6,
                FocusName = "focus"
            };

            office.OwnerPrograms.Add(program);
            office.Contacts.Add(contact);
            program.Focus = focus;
            program.Themes.Add(theme);
            program.Goals.Add(goal);

            context.Organizations.Add(office);
            context.Contacts.Add(contact);
            context.Programs.Add(program);
            context.Goals.Add(goal);
            context.Themes.Add(theme);
            context.Foci.Add(focus);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(office.OrganizationId, dto.Id);
                Assert.AreEqual(office.Name, dto.Name);
                Assert.AreEqual(office.OfficeSymbol, dto.OfficeSymbol);
                Assert.AreEqual(office.Description, dto.Description);
                Assert.AreEqual(office.History.RevisedOn, dto.RevisedOn);

                Assert.AreEqual(1, dto.Contacts.Count());
                Assert.AreEqual(contact.ContactId, dto.Contacts.First().Id);
                Assert.AreEqual(contact.FullName, dto.Contacts.First().Value);

                Assert.AreEqual(1, dto.Goals.Count());
                Assert.AreEqual(goal.GoalId, dto.Goals.First().Id);
                Assert.AreEqual(goal.GoalName, dto.Goals.First().Value);

                Assert.AreEqual(1, dto.Themes.Count());
                Assert.AreEqual(theme.ThemeId, dto.Themes.First().Id);
                Assert.AreEqual(theme.ThemeName, dto.Themes.First().Value);

                Assert.AreEqual(1, dto.Foci.Count());
                Assert.AreEqual(focus.FocusId, dto.Foci.First().Id);
                Assert.AreEqual(focus.FocusName, dto.Foci.First().Value);
            };

            var serviceResults = service.GetOfficeById(office.OrganizationTypeId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_OfficeDoesNotExist()
        {
            var org = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Office.Id
            };
            context.Organizations.Add(org);
            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };

            var serviceResults = service.GetOfficeById(org.OrganizationId + 1);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(org.OrganizationId + 1);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_NoPrograms()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Office.Id,
                Name = "office",
                Description = "office desc"
            };
            context.Organizations.Add(office);
            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(office.OrganizationId, dto.Id);
                Assert.AreEqual(office.Name, dto.Name);
                Assert.AreEqual(office.Description, dto.Description);
                Assert.AreEqual(0, dto.Contacts.Count());
                Assert.AreEqual(0, dto.Goals.Count());
                Assert.AreEqual(0, dto.Themes.Count());
                Assert.AreEqual(0, dto.Foci.Count());
            };

            var serviceResults = service.GetOfficeById(office.OrganizationTypeId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_MultiplePrograms()
        {
            //multiple programs are sharing the same themes and goals so make sure we're getting distinct
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Office.Id,
                Name = "office",
                Description = "office desc"
            };
            var contact = new Contact
            {
                ContactId = 2,
                FullName = "full name",
            };
            var program = new Program
            {
                ProgramId = 3,
                Owner = office
            };
            var program2 = new Program
            {
                ProgramId = 10,
                Owner = office
            };
            var goal = new Goal
            {
                GoalId = 4,
                GoalName = "goal"
            };
            var theme = new Theme
            {
                ThemeId = 5,
                ThemeName = "theme"
            };
            var focus = new Focus
            {
                FocusId = 6,
                FocusName = "focus"
            };

            office.OwnerPrograms.Add(program);
            office.OwnerPrograms.Add(program2);
            office.Contacts.Add(contact);
            program.Themes.Add(theme);
            program2.Themes.Add(theme);

            program.Goals.Add(goal);            
            program2.Goals.Add(goal);

            program.Focus = focus;
            program2.Focus = focus;


            context.Organizations.Add(office);
            context.Contacts.Add(contact);
            context.Programs.Add(program);
            context.Programs.Add(program2);
            context.Goals.Add(goal);
            context.Themes.Add(theme);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(office.OrganizationId, dto.Id);
                Assert.AreEqual(office.Name, dto.Name);
                Assert.AreEqual(office.Description, dto.Description);

                Assert.AreEqual(1, dto.Contacts.Count());
                Assert.AreEqual(1, dto.Goals.Count());
                Assert.AreEqual(1, dto.Themes.Count());
                Assert.AreEqual(1, dto.Foci.Count());
            };

            var serviceResults = service.GetOfficeById(office.OrganizationTypeId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_OrganizationIsNotOffice()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Individual.Id,
                Name = "office",
                Description = "office desc"
            };
            context.Organizations.Add(office);            

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };

            var serviceResults = service.GetOfficeById(office.OrganizationId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_OrganizationIsAnOffice()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Office.Id,
                Name = "office",
                Description = "office desc"
            };
            context.Organizations.Add(office);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
            };

            var serviceResults = service.GetOfficeById(office.OrganizationId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_OrganizationIsADivison()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Division.Id,
                Name = "office",
                Description = "office desc"
            };
            context.Organizations.Add(office);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
            };

            var serviceResults = service.GetOfficeById(office.OrganizationId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOfficeById_OrganizationIsABranch()
        {
            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = OrganizationType.Branch.Id,
                Name = "office",
                Description = "office desc"
            };
            context.Organizations.Add(office);

            Action<OfficeDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
            };

            var serviceResults = service.GetOfficeById(office.OrganizationId);
            var serviceResultsAsync = await service.GetOfficeByIdAsync(office.OrganizationId);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion


    }
}
