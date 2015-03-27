using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECA.Business.Service;
using ECA.Core.Logging;
using ECA.Data;

namespace ECA.Business.Test.Service
{
    [TestClass]
    public class EcaServiceTest
    {
        private TestEcaContext context;
        private EcaService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new EcaService(context, new TraceLogger());
        }

        #region Contact Existence Validation Tests
        [TestMethod]
        public async Task TestCheckAllContactsExist_NoIdsGiven()
        {
            var ids = new List<int>();
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllContactsExist(ids);
            var serviceResultsAsync = await service.CheckAllContactsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllContactsExist_NoContactsInContext()
        {
            var ids = new List<int> { 1 };
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllContactsExist(ids);
            var serviceResultsAsync = await service.CheckAllContactsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllContactsExist_NoContactsByIdInContext()
        {
            var ids = new List<int> { 1 };
            context.Contacts.Add(new Contact
            {
                ContactId = 0
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllContactsExist(ids);
            var serviceResultsAsync = await service.CheckAllContactsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllContactsExist_AllContactsExist()
        {
            var ids = new List<int> { 1, 2 };
            context.Contacts.Add(new Contact
            {
                ContactId = 1
            });
            context.Contacts.Add(new Contact
            {
                ContactId = 2
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllContactsExist(ids);
            var serviceResultsAsync = await service.CheckAllContactsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion

        #region Theme Existence Validation Tests
        [TestMethod]
        public async Task TestCheckAllThemesExist_NoIdsGiven()
        {
            var ids = new List<int>();
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllThemesExist(ids);
            var serviceResultsAsync = await service.CheckAllThemesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllThemesExist_NoThemeInContext()
        {
            var ids = new List<int> { 1 };
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllThemesExist(ids);
            var serviceResultsAsync = await service.CheckAllThemesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllThemesExist_NoThemesByIdInContext()
        {
            var ids = new List<int> { 1 };
            context.Themes.Add(new Theme
            {
                ThemeId = 0
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllThemesExist(ids);
            var serviceResultsAsync = await service.CheckAllThemesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllThemesExist_AllThemesExist()
        {
            var ids = new List<int> { 1, 2 };
            context.Themes.Add(new Theme
            {
                ThemeId = 1
            });
            context.Themes.Add(new Theme
            {
                ThemeId = 2
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllThemesExist(ids);
            var serviceResultsAsync = await service.CheckAllThemesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Goals Existence Validation Tests
        [TestMethod]
        public async Task TestCheckAllGoalsExist_NoIdsGiven()
        {
            var ids = new List<int>();
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllGoalsExist(ids);
            var serviceResultsAsync = await service.CheckAllGoalsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllGoalsExist_NoThemeInContext()
        {
            var ids = new List<int> { 1 };
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllGoalsExist(ids);
            var serviceResultsAsync = await service.CheckAllGoalsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllGoalsExist_NoThemesByIdInContext()
        {
            var ids = new List<int> { 1 };
            context.Themes.Add(new Theme
            {
                ThemeId = 0
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllGoalsExist(ids);
            var serviceResultsAsync = await service.CheckAllGoalsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllGoalsExist_AllThemesExist()
        {
            var ids = new List<int> { 1, 2 };
            context.Goals.Add(new Goal
            {
                GoalId = 1
            });
            context.Goals.Add(new Goal
            {
                GoalId = 2
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllGoalsExist(ids);
            var serviceResultsAsync = await service.CheckAllGoalsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        [TestMethod]
        public void TestSetGoals()
        {
            var original = new Goal { GoalId = 1 };

            var program = new Program();
            program.Goals.Add(original);

            var newGoal = new Goal { GoalId = 2 };
            var newGoalIds = new List<int> { newGoal.GoalId };
            service.SetGoals(newGoalIds, program);
            Assert.AreEqual(1, program.Goals.Count);
            Assert.AreEqual(newGoal.GoalId, program.Goals.First().GoalId);

        }

        [TestMethod]
        public void TestSetThemes()
        {
            var original = new Theme { ThemeId = 1 };

            var program = new Program();
            program.Themes.Add(original);

            var newTheme = new Theme { ThemeId = 2 };
            var newThemeIds = new List<int> { newTheme.ThemeId };
            service.SetThemes(newThemeIds, program);
            Assert.AreEqual(1, program.Themes.Count);
            Assert.AreEqual(newTheme.ThemeId, program.Themes.First().ThemeId);

        }


        [TestMethod]
        public void TestSetPointsOfContact()
        {
            var original = new Contact { ContactId = 1 };

            var program = new Program();
            program.Contacts.Add(original);

            var newContact = new Contact { ContactId = 2 };
            var newContactIds = new List<int> { newContact.ContactId };
            service.SetPointOfContacts(newContactIds, program);
            Assert.AreEqual(1, program.Contacts.Count);
            Assert.AreEqual(newContact.ContactId, program.Contacts.First().ContactId);
        }


    }
}
