using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECA.Business.Service;
using ECA.Data;
using Moq;
using Microsoft.QualityTools.Testing.Fakes;
using System.Data;

namespace ECA.Business.Test.Service
{
    [TestClass]
    public class EcaServiceTest
    {
        private Mock<TestEcaContext> contextMock;
        private TestEcaContext context;
        private EcaService service;

        [TestInitialize]
        public void TestInit()
        {
            contextMock = new Mock<TestEcaContext>();
            context = contextMock.Object;
            service = new EcaService(context);
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
        public void TestSetGoals_IsDetached()
        {
            var state = System.Data.Entity.EntityState.Detached; 
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() => 
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Goal>(It.IsAny<Goal>())).Returns(() =>
            {
                return state;
            });
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
        public void TestSetGoals_IsAdded()
        {
            var state = System.Data.Entity.EntityState.Added;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Goal>(It.IsAny<Goal>())).Returns(() =>
            {
                return state;
            });
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
        public void TestSetThemes_IsDetached()
        {
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Theme>(It.IsAny<Theme>())).Returns(() =>
            {
                return state;
            });
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
        public void TestSetThemes_IsAdded()
        {
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Theme>(It.IsAny<Theme>())).Returns(() =>
            {
                return state;
            });
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
        public void TestSetPointsOfContact_IsDetached()
        {
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Contact>(It.IsAny<Contact>())).Returns(() =>
            {
                return state;
            });
            var original = new Contact { ContactId = 1 };

            var program = new Program();
            program.Contacts.Add(original);

            var newContact = new Contact { ContactId = 2 };
            var newContactIds = new List<int> { newContact.ContactId };
            service.SetPointOfContacts(newContactIds, program);
            Assert.AreEqual(1, program.Contacts.Count);
            Assert.AreEqual(newContact.ContactId, program.Contacts.First().ContactId);
        }

        [TestMethod]
        public void TestSetPointsOfContact_IsAdded()
        {
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Contact>(It.IsAny<Contact>())).Returns(() =>
            {
                return state;
            });
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
