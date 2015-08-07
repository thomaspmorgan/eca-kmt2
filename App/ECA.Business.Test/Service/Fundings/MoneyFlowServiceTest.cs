using ECA.Business.Models.Fundings;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Fundings;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Exceptions;
using ECA.Core.Generation;
using ECA.Core.Query;
using ECA.Data;
using FluentAssertions;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Fundings
{
    [TestClass]
    public class MoneyFlowServiceTest
    {
        private TestEcaContext context;
        private MoneyFlowService service;
        private Mock<IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity>> validator;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            validator = new Mock<IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity>>();
            service = new MoneyFlowService(context, validator.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get MoneyFlows By Project Id
        [TestMethod]
        public async Task TestGetMoneyFlowsByProjectId()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var budgeted = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Budgeted.Value
            };

            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Outgoing"
            };

            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "Incoming"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProjectId = recipientId,
                SourceProject = sourceProject,
                RecipientProject = recipientProject,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = budgeted,
                MoneyFlowStatusId = budgeted.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Projects.Add(recipientProject);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlowStatuses.Add(budgeted);

            Action<PagedQueryResults<MoneyFlowDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.IsNotNull(results.Results.First());
                Assert.AreEqual(moneyFlow.MoneyFlowId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetMoneyFlowsByProjectId(sourceProject.ProjectId, queryOperator);
            var serviceResultsAsync = await service.GetMoneyFlowsByProjectIdAsync(sourceProject.ProjectId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Get MoneyFlows By Program Id
        [TestMethod]
        public async Task TestGetMoneyFlowsByProgramId_CheckProperties()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var programType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Program.Id,
                TypeName = MoneyFlowSourceRecipientType.Program.Value
            };
            var budgeted = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Budgeted.Value
            };

            var sourceId = 1;
            var recipientId = 2;
            var sourceProgram = new Program
            {
                ProgramId = sourceId,
                Name = "Outgoing"
            };

            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Incoming"

            };

            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProgramId = recipientId,
                SourceProgram = sourceProgram,
                RecipientProgram = recipientProgram,
                SourceType = programType,
                SourceTypeId = programType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = budgeted,
                MoneyFlowStatusId = budgeted.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Programs.Add(sourceProgram);
            context.Programs.Add(recipientProgram);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(programType);
            context.MoneyFlowStatuses.Add(budgeted);

            Action<PagedQueryResults<MoneyFlowDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.IsNotNull(results.Results.First());
                Assert.AreEqual(moneyFlow.MoneyFlowId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetMoneyFlowsByProgramId(sourceProgram.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetMoneyFlowsByProgramIdAsync(sourceProgram.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);

        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestCreate_CheckProperties()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetType = (c, type) =>
                {
                    Debug.Assert(type == typeof(Project));
                    var dbSet = new System.Data.Entity.Fakes.StubDbSet();
                    dbSet.FindObjectArray = (keys) =>
                    {
                        return (Object)context.Projects.Find(keys);
                    };
                    dbSet.FindAsyncObjectArray = (keys) =>
                    {
                        return Task.FromResult<object>(dbSet.Find(keys));
                    };
                    return dbSet;
                };
                var sourceProject = new Project
                {
                    ProjectId = 1,
                };
                var recipientProject = new Project
                {
                    ProjectId = 2
                };
                context.SetupActions.Add(() =>
                {
                    context.Projects.Add(sourceProject);
                    context.Projects.Add(recipientProject);
                });
                var userId = 1;
                var creator = new User(userId);
                var description = "description";
                decimal value = 1.90m;
                int moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
                var now = DateTimeOffset.UtcNow;
                var fiscalYear = 2015;
                var projectMoneyFlowTypeId = MoneyFlowSourceRecipientType.Project.Id;

                var additionalMoneyFlow = new AdditionalMoneyFlow(
                    createdBy: creator,
                    description: description,
                    value: value,
                    moneyFlowStatusId: moneyFlowStatusId,
                    transactionDate: now,
                    fiscalYear: fiscalYear,
                    sourceEntityId: sourceProject.ProjectId,
                    sourceEntityTypeId: projectMoneyFlowTypeId,
                    recipientEntityId: recipientProject.ProjectId,
                    recipientEntityTypeId: projectMoneyFlowTypeId
                    );

                Action tester = () =>
                {
                    Assert.AreEqual(1, context.MoneyFlows.Count());
                    var moneyFlow = context.MoneyFlows.First();
                    Assert.AreEqual(description, moneyFlow.Description);
                    Assert.AreEqual(value, moneyFlow.Value);
                    Assert.AreEqual(moneyFlowStatusId, moneyFlow.MoneyFlowStatusId);
                    Assert.AreEqual(now, moneyFlow.TransactionDate);
                    Assert.AreEqual(fiscalYear, moneyFlow.FiscalYear);
                    Assert.AreEqual(sourceProject.ProjectId, moneyFlow.SourceProjectId);
                    Assert.AreEqual(recipientProject.ProjectId, moneyFlow.RecipientProjectId);
                    Assert.AreEqual(userId, moneyFlow.History.CreatedBy);
                    Assert.AreEqual(userId, moneyFlow.History.RevisedBy);
                    DateTimeOffset.UtcNow.Should().BeCloseTo(moneyFlow.History.CreatedOn, 2000);
                    DateTimeOffset.UtcNow.Should().BeCloseTo(moneyFlow.History.RevisedOn, 2000);

                    Assert.IsFalse(moneyFlow.SourceItineraryStopId.HasValue);
                    Assert.IsFalse(moneyFlow.SourceOrganizationId.HasValue);
                    Assert.IsFalse(moneyFlow.SourceParticipantId.HasValue);
                    Assert.IsFalse(moneyFlow.SourceProgramId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientAccommodationId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientItineraryStopId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientOrganizationId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientParticipantId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientProgramId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientTransportationId.HasValue);
                };

                context.Revert();
                service.Create(additionalMoneyFlow);
                tester();

                context.Revert();
                await service.CreateAsync(additionalMoneyFlow);
                tester();

                validator.Verify(x => x.ValidateCreate(It.IsAny<MoneyFlowServiceCreateValidationEntity>()), Times.Exactly(2));
            }
        }

        [TestMethod]
        public async Task TestCreate_SourceDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetType = (c, type) =>
                {
                    Debug.Assert(type == typeof(Project));
                    var dbSet = new System.Data.Entity.Fakes.StubDbSet();
                    dbSet.FindObjectArray = (keys) =>
                    {
                        return (Object)context.Projects.Find(keys);
                    };
                    dbSet.FindAsyncObjectArray = (keys) =>
                    {
                        return Task.FromResult<object>(dbSet.Find(keys));
                    };
                    return dbSet;
                };
                var recipientProject = new Project
                {
                    ProjectId = 2
                };
                context.SetupActions.Add(() =>
                {
                    context.Projects.Add(recipientProject);
                });
                var userId = 1;
                var creator = new User(userId);
                var description = "description";
                decimal value = 1.90m;
                int moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
                var now = DateTimeOffset.UtcNow;
                var fiscalYear = 2015;
                var projectMoneyFlowTypeId = MoneyFlowSourceRecipientType.Project.Id;

                var additionalMoneyFlow = new AdditionalMoneyFlow(
                    createdBy: creator,
                    description: description,
                    value: value,
                    moneyFlowStatusId: moneyFlowStatusId,
                    transactionDate: now,
                    fiscalYear: fiscalYear,
                    sourceEntityId: -1,
                    sourceEntityTypeId: projectMoneyFlowTypeId,
                    recipientEntityId: recipientProject.ProjectId,
                    recipientEntityTypeId: projectMoneyFlowTypeId
                    );
                context.Revert();
                Func<Task> f = () =>
                {
                    return service.CreateAsync(additionalMoneyFlow);
                };

                service.Invoking(x => x.Create(additionalMoneyFlow)).ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The entity of type [{0}] with id [{1}] was not found.", "Project", -1));
                f.ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The entity of type [{0}] with id [{1}] was not found.", "Project", -1));
            }
        }

        [TestMethod]
        public async Task TestCreate_RecipientDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetType = (c, type) =>
                {
                    Debug.Assert(type == typeof(Project));
                    var dbSet = new System.Data.Entity.Fakes.StubDbSet();
                    dbSet.FindObjectArray = (keys) =>
                    {
                        return (Object)context.Projects.Find(keys);
                    };
                    dbSet.FindAsyncObjectArray = (keys) =>
                    {
                        return Task.FromResult<object>(dbSet.Find(keys));
                    };
                    return dbSet;
                };
                var sourceProject = new Project
                {
                    ProjectId = 1
                };
                context.SetupActions.Add(() =>
                {
                    context.Projects.Add(sourceProject);
                });
                var userId = 1;
                var creator = new User(userId);
                var description = "description";
                decimal value = 1.90m;
                int moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
                var now = DateTimeOffset.UtcNow;
                var fiscalYear = 2015;
                var projectMoneyFlowTypeId = MoneyFlowSourceRecipientType.Project.Id;

                var additionalMoneyFlow = new AdditionalMoneyFlow(
                    createdBy: creator,
                    description: description,
                    value: value,
                    moneyFlowStatusId: moneyFlowStatusId,
                    transactionDate: now,
                    fiscalYear: fiscalYear,
                    sourceEntityId: sourceProject.ProjectId,
                    sourceEntityTypeId: projectMoneyFlowTypeId,
                    recipientEntityId: -1,
                    recipientEntityTypeId: projectMoneyFlowTypeId
                    );
                context.Revert();
                Func<Task> f = () =>
                {
                    return service.CreateAsync(additionalMoneyFlow);
                };

                service.Invoking(x => x.Create(additionalMoneyFlow)).ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The entity of type [{0}] with id [{1}] was not found.", "Project", -1));
                f.ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The entity of type [{0}] with id [{1}] was not found.", "Project", -1));
            }
        }
        #endregion

        [TestMethod]
        public void TestGetMoneyFlowType()
        {
            var expectedMapping = new Dictionary<int, Type>();
            expectedMapping.Add(MoneyFlowSourceRecipientType.ItineraryStop.Id, typeof(ItineraryStop));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Organization.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Participant.Id, typeof(Participant));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Program.Id, typeof(Program));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Project.Id, typeof(Project));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Post.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Accomodation.Id, typeof(Accommodation));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Transportation.Id, typeof(Transportation));

            expectedMapping.Add(MoneyFlowSourceRecipientType.Expense.Id, null);

            var staticProperties = typeof(MoneyFlowSourceRecipientType).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            foreach (var staticLookup in allStaticLookups)
            {
                Assert.IsTrue(expectedMapping.ContainsKey(staticLookup.Id), "The money flow source recipient type " + staticLookup.Value + " is not tested.");
            }

            foreach (var key in expectedMapping.Keys)
            {
                Assert.AreEqual(expectedMapping[key], service.GetMoneyFlowType(key), "The key " + key + " is not mapped.");
            }
        }

        [TestMethod]
        public void TestIsMoneyFlowType()
        {
            var expectedMapping = new Dictionary<int, Type>();
            expectedMapping.Add(MoneyFlowSourceRecipientType.ItineraryStop.Id, typeof(ItineraryStop));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Organization.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Participant.Id, typeof(Participant));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Program.Id, typeof(Program));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Project.Id, typeof(Project));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Post.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Accomodation.Id, typeof(Accommodation));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Transportation.Id, typeof(Transportation));

            expectedMapping.Add(MoneyFlowSourceRecipientType.Expense.Id, null);

            var staticProperties = typeof(MoneyFlowSourceRecipientType).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            foreach (var staticLookup in allStaticLookups)
            {
                Assert.IsTrue(expectedMapping.ContainsKey(staticLookup.Id), "The money flow source recipient type " + staticLookup.Value + " is not tested.");
            }

            foreach (var key in expectedMapping.Keys)
            {
                var type = service.GetMoneyFlowType(key);
                var isExpectedToBeAType = type != null;
                Assert.AreEqual(isExpectedToBeAType, service.IsMoneyFlowType(key), "The type with key " + key + " did not have the expected IsIFundableType result " + isExpectedToBeAType);
            }
        }

        #region Delete
        [TestMethod]
        public async Task TestDelete()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var userId = 1;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            MoneyFlow moneyFlowToDelete = null;

            context.SetupActions.Add(() =>
            {
                moneyFlowToDelete = new MoneyFlow
                {
                    MoneyFlowId = moneyFlowId,
                    Description = "old desc",
                    FiscalYear = 1900,
                    MoneyFlowStatusId = -1,
                    MoneyFlowTypeId = -1,
                    TransactionDate = lastWeek,
                    Value = -1.0m,
                    SourceProjectId = sourceEntityId
                };
                moneyFlowToDelete.History.CreatedBy = userId;
                moneyFlowToDelete.History.RevisedBy = userId;
                moneyFlowToDelete.History.CreatedOn = yesterday;
                moneyFlowToDelete.History.RevisedOn = yesterday;
                context.MoneyFlows.Add(moneyFlowToDelete);
            });
            Action beforeTester = () =>
            {
                Assert.AreEqual(1, context.MoneyFlows.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(0, context.MoneyFlows.Count());
            };
            var instance = new DeletedMoneyFlow(new User(userId), moneyFlowId, sourceEntityId);

            context.Revert();
            beforeTester();
            service.Delete(instance);
            afterTester();

            context.Revert();
            beforeTester();
            await service.DeleteAsync(instance);
            afterTester();
        }

        [TestMethod]
        public async Task TestDelete_DifferentMoneyFlowId()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var userId = 1;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            MoneyFlow moneyFlowToDelete = null;
            MoneyFlow otherMoneyFlow = null;

            context.SetupActions.Add(() =>
            {
                moneyFlowToDelete = new MoneyFlow
                {
                    MoneyFlowId = moneyFlowId,
                    Description = "old desc",
                    FiscalYear = 1900,
                    MoneyFlowStatusId = -1,
                    MoneyFlowTypeId = -1,
                    TransactionDate = lastWeek,
                    Value = -1.0m,
                    SourceProjectId = sourceEntityId
                };
                otherMoneyFlow = new MoneyFlow
                {
                    MoneyFlowId = moneyFlowId + 1,
                    Description = "old desc",
                    FiscalYear = 1900,
                    MoneyFlowStatusId = -1,
                    MoneyFlowTypeId = -1,
                    TransactionDate = lastWeek,
                    Value = -1.0m,
                    SourceProjectId = sourceEntityId + 1
                };
                moneyFlowToDelete.History.CreatedBy = userId;
                moneyFlowToDelete.History.RevisedBy = userId;
                moneyFlowToDelete.History.CreatedOn = yesterday;
                moneyFlowToDelete.History.RevisedOn = yesterday;

                otherMoneyFlow.History.CreatedBy = userId;
                otherMoneyFlow.History.RevisedBy = userId;
                otherMoneyFlow.History.CreatedOn = yesterday;
                otherMoneyFlow.History.RevisedOn = yesterday;
                context.MoneyFlows.Add(moneyFlowToDelete);
                context.MoneyFlows.Add(otherMoneyFlow);
            });
            Action beforeTester = () =>
            {
                Assert.AreEqual(2, context.MoneyFlows.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.MoneyFlows.Count());
                Assert.IsTrue(Object.ReferenceEquals(otherMoneyFlow, context.MoneyFlows.First()));
            };
            var instance = new DeletedMoneyFlow(new User(userId), moneyFlowId, sourceEntityId);

            context.Revert();
            beforeTester();
            service.Delete(instance);
            afterTester();

            context.Revert();
            beforeTester();
            await service.DeleteAsync(instance);
            afterTester();
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceProjectIdDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceProjectId = -1
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceProgramIdDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceProgramId = -1
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceItineraryDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceItineraryStopId = -1
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceOrganizationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceOrganizationId = -1
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceParticipantDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceParticipantId = -1
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientAccommodationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientAccommodationId = -1
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientItineraryStopDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientItineraryStopId = -1
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientOrganizationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientOrganizationId = -1
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientParticipantDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientParticipantId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientProgramDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientProgramId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientProjectDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientProjectId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientTransportationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientTransportationId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }


        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdate()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var creatorId = 1;
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            MoneyFlow moneyFlowToUpdate = null;

            context.SetupActions.Add(() =>
            {
                moneyFlowToUpdate = new MoneyFlow
                {
                    MoneyFlowId = moneyFlowId,
                    Description = "old desc",
                    FiscalYear = 1900,
                    MoneyFlowStatusId = -1,
                    MoneyFlowTypeId = -1,
                    TransactionDate = lastWeek,
                    Value = -1.0m,
                    SourceProjectId = sourceEntityId
                };
                moneyFlowToUpdate.History.CreatedBy = creatorId;
                moneyFlowToUpdate.History.RevisedBy = creatorId;
                moneyFlowToUpdate.History.CreatedOn = yesterday;
                moneyFlowToUpdate.History.RevisedOn = yesterday;
                context.MoneyFlows.Add(moneyFlowToUpdate);
            });

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015,
                sourceOrRecipientEntityId: sourceEntityId
                );

            Action tester = () =>
            {
                Assert.AreEqual(1, context.MoneyFlows.Count());
                Assert.AreEqual(updatedMoneyFlow.Description, moneyFlowToUpdate.Description);
                Assert.AreEqual(updatedMoneyFlow.Value, moneyFlowToUpdate.Value);
                Assert.AreEqual(updatedMoneyFlow.MoneyFlowStatusId, moneyFlowToUpdate.MoneyFlowStatusId);
                Assert.AreEqual(updatedMoneyFlow.TransactionDate, moneyFlowToUpdate.TransactionDate);
                Assert.AreEqual(updatedMoneyFlow.FiscalYear, moneyFlowToUpdate.FiscalYear);
                Assert.AreEqual(creatorId, moneyFlowToUpdate.History.CreatedBy);
                Assert.AreEqual(revisorId, moneyFlowToUpdate.History.RevisedBy);
                Assert.AreEqual(yesterday, moneyFlowToUpdate.History.CreatedOn);
                DateTimeOffset.UtcNow.Should().BeCloseTo(moneyFlowToUpdate.History.RevisedOn, 2000);
            };
            context.Revert();
            service.Update(updatedMoneyFlow);
            tester();
            validator.Verify(x => x.ValidateUpdate(It.IsAny<MoneyFlowServiceUpdateValidationEntity>()), Times.Once());


            context.Revert();
            await service.UpdateAsync(updatedMoneyFlow);
            tester();
            validator.Verify(x => x.ValidateUpdate(It.IsAny<MoneyFlowServiceUpdateValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithGivenIdDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The money flow with id [{0}] and source entity id [{1}] was not found.", updatedMoneyFlow.Id, updatedMoneyFlow.SourceOrRecipientEntityId));
            f.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The money flow with id [{0}] and source entity id [{1}] was not found.", updatedMoneyFlow.Id, updatedMoneyFlow.SourceOrRecipientEntityId));
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceProjectIdDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceProjectId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceProgramIdDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceProgramId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceItineraryDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceItineraryStopId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceOrganizationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceOrganizationId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceParticipantDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceParticipantId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientAccommodationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientAccommodationId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientItineraryStopDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientItineraryStopId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientOrganizationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientOrganizationId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientParticipantDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientParticipantId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientProgramDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientProgramId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientProjectDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientProjectId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientTransportationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientTransportationId = -1
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }
        #endregion

    }
}
