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

        #region Location Existence Validation Tests
        [TestMethod]
        public async Task TestCheckAllLocationsExist_NoIdsGiven()
        {
            var ids = new List<int>();
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllLocationsExist(ids);
            var serviceResultsAsync = await service.CheckAllLocationsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllLocationsExist_NoLocationInContext()
        {
            var ids = new List<int> { 1 };
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllLocationsExist(ids);
            var serviceResultsAsync = await service.CheckAllLocationsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllLocationsExist_NoLocationsByIdInContext()
        {
            var ids = new List<int> { 1 };
            context.Locations.Add(new Location
            {
                LocationId = 0
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllLocationsExist(ids);
            var serviceResultsAsync = await service.CheckAllLocationsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllLocationsExist_AllLocationsExist()
        {
            var ids = new List<int> { 1, 2 };
            context.Locations.Add(new Location
            {
                LocationId = 1
            });
            context.Locations.Add(new Location
            {
                LocationId = 2
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllLocationsExist(ids);
            var serviceResultsAsync = await service.CheckAllLocationsExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

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

        #region Categories Existence Validation Tests
        [TestMethod]
        public async Task TestCheckAllCategoriesExist_NoIdsGiven()
        {
            var ids = new List<int>();
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllCategoriesExist(ids);
            var serviceResultsAsync = await service.CheckAllCategoriesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllCategoriesExist_NoCategoriesInContext()
        {
            var ids = new List<int> { 1 };
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllCategoriesExist(ids);
            var serviceResultsAsync = await service.CheckAllCategoriesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllCategoriesExist_NoCategoriesByIdInContext()
        {
            var ids = new List<int> { 1 };
            context.Categories.Add(new Category
            {
                CategoryId = 0
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllCategoriesExist(ids);
            var serviceResultsAsync = await service.CheckAllCategoriesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllCategoriesExist_AllCategoriesExist()
        {
            var ids = new List<int> { 1, 2 };
            context.Categories.Add(new Category
            {
                CategoryId = 1
            });
            context.Categories.Add(new Category
            {
                CategoryId = 2
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllCategoriesExist(ids);
            var serviceResultsAsync = await service.CheckAllCategoriesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion

        #region Objective Existence Validation Tests
        [TestMethod]
        public async Task TestCheckAllObjectivesExist_NoIdsGiven()
        {
            var ids = new List<int>();
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllObjectivesExist(ids);
            var serviceResultsAsync = await service.CheckAllObjectivesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllObjectivesExist_NoObjectivesInContext()
        {
            var ids = new List<int> { 1 };
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllObjectivesExist(ids);
            var serviceResultsAsync = await service.CheckAllObjectivesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllObjectivesExist_NoObjectivesByIdInContext()
        {
            var ids = new List<int> { 1 };
            context.Objectives.Add(new Objective
            {
                ObjectiveId = 0
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsFalse(results);
            };
            var serviceResults = service.CheckAllObjectivesExist(ids);
            var serviceResultsAsync = await service.CheckAllObjectivesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestCheckAllObjectivesExist_AllObjectivesExist()
        {
            var ids = new List<int> { 1, 2 };
            context.Objectives.Add(new Objective
            {
                ObjectiveId = 1
            });
            context.Objectives.Add(new Objective
            {
                ObjectiveId = 2
            });
            Action<bool> tester = (results) =>
            {
                Assert.IsTrue(results);
            };
            var serviceResults = service.CheckAllObjectivesExist(ids);
            var serviceResultsAsync = await service.CheckAllObjectivesExistAsync(ids);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion

        [TestMethod]
        public void TestSetParticipants_IsDetached()
        {
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Participant>(It.IsAny<Participant>())).Returns(() =>
            {
                return state;
            });
            var original = new Participant { ParticipantId = 1 };

            var project = new Project();
            project.Participants.Add(original);

            var newParticipant = new Participant { ParticipantId = 2 };
            var newParticipantIds = new List<int> { newParticipant.ParticipantId };
            service.SetParticipants<Project>(newParticipantIds, project, x => x.Participants);
            Assert.AreEqual(1, project.Participants.Count);
            Assert.AreEqual(newParticipant.ParticipantId, project.Participants.First().ParticipantId);

        }

        [TestMethod]
        public void TestSetParticipants_IsAdded()
        {
            var state = System.Data.Entity.EntityState.Added;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Location>(It.IsAny<Location>())).Returns(() =>
            {
                return state;
            });
            var original = new Participant { ParticipantId = 1 };

            var project = new Project();
            project.Participants.Add(original);

            var newParticipant = new Participant { ParticipantId = 2 };
            var newParticipantIds = new List<int> { newParticipant.ParticipantId };
            service.SetParticipants<Project>(newParticipantIds, project, x => x.Participants);
            Assert.AreEqual(1, project.Participants.Count);
            Assert.AreEqual(newParticipant.ParticipantId, project.Participants.First().ParticipantId);

        }

        [TestMethod]
        public void TestSetParticipants_IsLocal()
        {
            var state = System.Data.Entity.EntityState.Added;

            contextMock.Setup(x => x.GetEntityState<Participant>(It.IsAny<Participant>())).Returns(() =>
            {
                return state;
            });
            var original = new Participant { ParticipantId = 1 };

            var project = new Project();
            project.Participants.Add(original);

            var newParticipant = new Participant { ParticipantId = 2 };
            var newParticipantIds = new List<int> { newParticipant.ParticipantId };
            contextMock.Setup(x => x.GetLocalEntity<Participant>(It.IsAny<Func<Participant, bool>>())).Returns(newParticipant);
            service.SetParticipants<Project>(newParticipantIds, project, x => x.Participants);
            Assert.AreEqual(1, project.Participants.Count);
            Assert.AreEqual(newParticipant.ParticipantId, project.Participants.First().ParticipantId);
        }

        [TestMethod]
        public void TestSetLocations_IsDetached()
        {
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Location>(It.IsAny<Location>())).Returns(() =>
            {
                return state;
            });
            var original = new Location { LocationId = 1 };

            var project = new Project();
            project.Locations.Add(original);

            var newLocation = new Location { LocationId = 2 };
            var newLocationIds = new List<int> { newLocation.LocationId };
            service.SetLocations<Project>(newLocationIds, project, x => x.Locations);
            Assert.AreEqual(1, project.Locations.Count);
            Assert.AreEqual(newLocation.LocationId, project.Locations.First().LocationId);

        }

        [TestMethod]
        public void TestSetLocations_IsAdded()
        {
            var state = System.Data.Entity.EntityState.Added;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Location>(It.IsAny<Location>())).Returns(() =>
            {
                return state;
            });
            var original = new Location { LocationId = 1 };

            var project = new Project();
            project.Locations.Add(original);

            var newLocation = new Location { LocationId = 2 };
            var newLocationIds = new List<int> { newLocation.LocationId };
            service.SetLocations<Project>(newLocationIds, project, x => x.Locations);
            Assert.AreEqual(1, project.Locations.Count);
            Assert.AreEqual(newLocation.LocationId, project.Locations.First().LocationId);

        }

        [TestMethod]
        public void TestSetLocations_IsLocal()
        {
            var state = System.Data.Entity.EntityState.Added;

            contextMock.Setup(x => x.GetEntityState<Location>(It.IsAny<Location>())).Returns(() =>
            {
                return state;
            });
            var original = new Location { LocationId = 1 };

            var project = new Project();
            project.Locations.Add(original);

            var newLocation = new Location { LocationId = 2 };
            var newLocationIds = new List<int> { newLocation.LocationId };
            contextMock.Setup(x => x.GetLocalEntity<Location>(It.IsAny<Func<Location, bool>>())).Returns(newLocation);
            service.SetLocations<Project>(newLocationIds, project, x => x.Locations);
            Assert.AreEqual(1, project.Locations.Count);
            Assert.AreEqual(newLocation.LocationId, project.Locations.First().LocationId);
        }

        [TestMethod]
        public void TestSetObjectives_IsDetached()
        {
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Objective>(It.IsAny<Objective>())).Returns(() =>
            {
                return state;
            });
            var original = new Objective { ObjectiveId = 1 };

            var program = new Program();
            program.Objectives.Add(original);

            var newObjective = new Objective { ObjectiveId = 2 };
            var newObjectiveIds = new List<int> { newObjective.ObjectiveId };
            service.SetObjectives(newObjectiveIds, program);
            Assert.AreEqual(1, program.Objectives.Count);
            Assert.AreEqual(newObjective.ObjectiveId, program.Objectives.First().ObjectiveId);

        }

        [TestMethod]
        public void TestSetObjectives_IsAdded()
        {
            var state = System.Data.Entity.EntityState.Added;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Objective>(It.IsAny<Objective>())).Returns(() =>
            {
                return state;
            });
            var original = new Objective { ObjectiveId = 1 };

            var program = new Program();
            program.Objectives.Add(original);

            var newObjective = new Objective { ObjectiveId = 2 };
            var newObjectiveIds = new List<int> { newObjective.ObjectiveId };
            service.SetObjectives(newObjectiveIds, program);
            Assert.AreEqual(1, program.Objectives.Count);
            Assert.AreEqual(newObjective.ObjectiveId, program.Objectives.First().ObjectiveId);

        }

        [TestMethod]
        public void TestSetObjectives_IsLocal()
        {
            var state = System.Data.Entity.EntityState.Added;

            contextMock.Setup(x => x.GetEntityState<Objective>(It.IsAny<Objective>())).Returns(() =>
            {
                return state;
            });
            var original = new Objective { ObjectiveId = 1 };

            var program = new Program();
            program.Objectives.Add(original);

            var newObjective = new Objective { ObjectiveId = 2 };
            var newObjectiveIds = new List<int> { newObjective.ObjectiveId };
            contextMock.Setup(x => x.GetLocalEntity<Objective>(It.IsAny<Func<Objective, bool>>())).Returns(newObjective);
            service.SetObjectives(newObjectiveIds, program);
            Assert.AreEqual(1, program.Objectives.Count);
            Assert.AreEqual(newObjective.ObjectiveId, program.Objectives.First().ObjectiveId);
        }

        [TestMethod]
        public void TestSetCategories_IsDetached()
        {
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Category>(It.IsAny<Category>())).Returns(() =>
            {
                return state;
            });
            var original = new Category { CategoryId = 1 };

            var program = new Program();
            program.Categories.Add(original);

            var newCategory = new Category { CategoryId = 2 };
            var newCategoryIds = new List<int> { newCategory.CategoryId };
            service.SetCategories(newCategoryIds, program);
            Assert.AreEqual(1, program.Categories.Count);
            Assert.AreEqual(newCategory.CategoryId, program.Categories.First().CategoryId);

        }

        [TestMethod]
        public void TestSetCategories_IsAdded()
        {
            var state = System.Data.Entity.EntityState.Added;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Category>(It.IsAny<Category>())).Returns(() =>
            {
                return state;
            });
            var original = new Category { CategoryId = 1 };

            var program = new Program();
            program.Categories.Add(original);

            var newCategory = new Category { CategoryId = 2 };
            var newCategoryIds = new List<int> { newCategory.CategoryId };
            service.SetCategories(newCategoryIds, program);
            Assert.AreEqual(1, program.Categories.Count);
            Assert.AreEqual(newCategory.CategoryId, program.Categories.First().CategoryId);

        }

        [TestMethod]
        public void TestSetCategories_IsLocal()
        {
            var state = System.Data.Entity.EntityState.Added;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Category>(It.IsAny<Category>())).Returns(() =>
            {
                return state;
            });
            var original = new Category { CategoryId = 1 };

            var program = new Program();
            program.Categories.Add(original);

            var newCategory = new Category { CategoryId = 2 };
            var newCategoryIds = new List<int> { newCategory.CategoryId };
            contextMock.Setup(x => x.GetLocalEntity<Category>(It.IsAny<Func<Category, bool>>())).Returns(newCategory);
            service.SetCategories(newCategoryIds, program);
            Assert.AreEqual(1, program.Categories.Count);
            Assert.AreEqual(newCategory.CategoryId, program.Categories.First().CategoryId);

        }


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
        public void TestSetGoals_IsLocal()
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
            contextMock.Setup(x => x.GetLocalEntity<Goal>(It.IsAny<Func<Goal, bool>>())).Returns(newGoal);
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
            var state = System.Data.Entity.EntityState.Added;
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
        public void TestSetThemes_IsLocal()
        {
            var state = System.Data.Entity.EntityState.Detached;
            var original = new Theme { ThemeId = 1 };

            var program = new Program();
            program.Themes.Add(original);

            var newTheme = new Theme { ThemeId = 2 };
            var newThemeIds = new List<int> { newTheme.ThemeId };
            Action<Func<Theme, bool>> callbackTester = (f) =>
            {
                var testThemes = new List<Theme> { newTheme };

                Assert.IsTrue(Object.ReferenceEquals(newTheme, testThemes.Where(f).First()));
                testThemes.Clear();
                testThemes.Add(new Theme
                {
                    ThemeId = newTheme.ThemeId - 1
                });
                Assert.AreEqual(0, testThemes.Where(f).Count());
            };
            contextMock.Setup(x => x.GetLocalEntity<Theme>(It.IsAny<Func<Theme, bool>>())).Callback(callbackTester).Returns(newTheme);
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

        [TestMethod]
        public void TestSetPointsOfContact_Local()
        {
            var state = System.Data.Entity.EntityState.Detached;
            var original = new Contact { ContactId = 1 };

            var program = new Program();
            program.Contacts.Add(original);

            var newContact = new Contact { ContactId = 2 };
            var newContactIds = new List<int> { newContact.ContactId };
            Action<Func<Contact, bool>> callbackTester = (f) =>
            {
                var testContacts = new List<Contact> { newContact };

                Assert.IsTrue(Object.ReferenceEquals(newContact, testContacts.Where(f).First()));
                testContacts.Clear();
                testContacts.Add(new Contact
                {
                    ContactId = newContact.ContactId - 1
                });
                Assert.AreEqual(0, testContacts.Where(f).Count());
            };

            contextMock.Setup(x => x.GetLocalEntity<Contact>(It.IsAny<Func<Contact, bool>>())).Callback(callbackTester)
            .Returns(newContact);
            service.SetPointOfContacts(newContactIds, program);
            Assert.AreEqual(1, program.Contacts.Count);
            Assert.AreEqual(newContact.ContactId, program.Contacts.First().ContactId);
        }


        [TestMethod]
        public void TestSetRegions_IsDetached()
        {
            var state = System.Data.Entity.EntityState.Detached;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Location>(It.IsAny<Location>())).Returns(() =>
            {
                return state;
            });
            var original = new Location { LocationId = 1 };

            var program = new Program();
            program.Regions.Add(original);

            var newRegion = new Location { LocationId = 2 };
            var newRegionIds = new List<int> { newRegion.LocationId };
            service.SetRegions(newRegionIds, program);
            Assert.AreEqual(1, program.Regions.Count);
            Assert.AreEqual(newRegion.LocationId, program.Regions.First().LocationId);
        }

        [TestMethod]
        public void TestSetRegions_IsAdded()
        {
            var state = System.Data.Entity.EntityState.Added;
            contextMock.Setup(x => x.GetEntityState(It.IsAny<object>())).Returns(() =>
            {
                return state;
            });
            contextMock.Setup(x => x.GetEntityState<Location>(It.IsAny<Location>())).Returns(() =>
            {
                return state;
            });
            var original = new Location { LocationId = 1 };

            var program = new Program();
            program.Regions.Add(original);

            var newRegion = new Location { LocationId = 2 };
            var newRegionIds = new List<int> { newRegion.LocationId };
            service.SetRegions(newRegionIds, program);
            Assert.AreEqual(1, program.Regions.Count);
            Assert.AreEqual(newRegion.LocationId, program.Regions.First().LocationId);
        }

        [TestMethod]
        public void TestSetRegions_Local()
        {
            var state = System.Data.Entity.EntityState.Added;
            var original = new Location { LocationId = 1 };

            var program = new Program();
            program.Regions.Add(original);

            var newRegion = new Location { LocationId = 2 };
            var newRegionIds = new List<int> { newRegion.LocationId };
            Action<Func<Location, bool>> callbackTester = (f) =>
            {
                var testLocations = new List<Location> { newRegion };

                Assert.IsTrue(Object.ReferenceEquals(newRegion, testLocations.Where(f).First()));
                testLocations.Clear();
                testLocations.Add(new Location
                {
                    LocationId = newRegion.LocationId - 1
                });
                Assert.AreEqual(0, testLocations.Where(f).Count());
            };
            contextMock.Setup(x => x.GetLocalEntity<Location>(It.IsAny<Func<Location, bool>>())).Callback(callbackTester).Returns(newRegion);
            service.SetRegions(newRegionIds, program);
            Assert.AreEqual(1, program.Regions.Count);
            Assert.AreEqual(newRegion.LocationId, program.Regions.First().LocationId);
        }
    }
}
