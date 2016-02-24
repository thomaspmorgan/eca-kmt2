using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using Microsoft.QualityTools.Testing.Fakes;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using ECA.Business.Service.Persons;
using Moq;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Persons
{
    public class ParticipantProxyClass : Participant
    {

    }

    public class ParticipantPersonProxyClass : ParticipantPerson
    {

    }

    public class ParticipantExchangeVisitorProxyClass : ECA.Data.ParticipantExchangeVisitor
    {

    }

    public class ProjectProxyClass : ECA.Data.Project
    {

    }

    public class PersonProxyClass : ECA.Data.Person
    {

    }

    [TestClass]
    public class ExchangeVisitorSaveActionTest
    {
        private TestEcaContext context;
        private Func<User> userProvider;
        private Mock<IExchangeVisitorValidationService> exchangeVisitorValidationService;
        private ExchangeVisitorSaveAction saveAction;

        [TestInitialize]
        public void TestInit()
        {
            exchangeVisitorValidationService = new Mock<IExchangeVisitorValidationService>();
            context = new TestEcaContext();
            userProvider = () => new User(1);
            saveAction = new ExchangeVisitorSaveAction(exchangeVisitorValidationService.Object, userProvider);
        }
        #region Constructor
        [TestMethod]
        public void TestConstructor()
        {
            var testSaveAction = new ExchangeVisitorSaveAction(exchangeVisitorValidationService.Object, userProvider);
            Assert.AreEqual(userProvider().Id, testSaveAction.User.Id);
        }
        #endregion

        #region Modified Entities
        [TestMethod]
        public void TestGetModifiedParticipants_Participant()
        {
            using (ShimsContext.Create())
            {
                var entity = new ParticipantProxyClass
                {
                    ParticipantId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Modified
                    });
                    return entries;
                };
                context.Participants.Add(entity);
                var entities = saveAction.GetModifiedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedParticipants_Person()
        {
            using (ShimsContext.Create())
            {
                var entity = new PersonProxyClass
                {
                    PersonId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Modified
                    });
                    return entries;
                };
                context.People.Add(entity);
                var entities = saveAction.GetModifiedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedParticipants_ParticipantPerson()
        {
            using (ShimsContext.Create())
            {
                var entity = new ParticipantPersonProxyClass
                {
                    ParticipantId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Modified
                    });
                    return entries;
                };
                context.ParticipantPersons.Add(entity);
                var entities = saveAction.GetModifiedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedParticipants_ParticipantPersonExchangeVisitor()
        {
            using (ShimsContext.Create())
            {
                var entity = new ParticipantExchangeVisitorProxyClass
                {
                    ParticipantId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Modified
                    });
                    return entries;
                };
                context.ParticipantExchangeVisitors.Add(entity);
                var entities = saveAction.GetModifiedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedParticipants_EntityIsNotAParticipantRelatedEntity()
        {
            using (ShimsContext.Create())
            {
                var entity = new ProjectProxyClass
                {

                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Modified
                    });
                    return entries;
                };
                context.Projects.Add(entity);
                var entities = saveAction.GetModifiedEntities(context);
                Assert.AreEqual(0, entities.Count);
            }
        }
        #endregion

        #region Created Entities
        [TestMethod]
        public void TestGetCreatedParticipants_Participant()
        {
            using (ShimsContext.Create())
            {
                var entity = new ParticipantProxyClass
                {
                    ParticipantId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.Participants.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedParticipants_Person()
        {
            using (ShimsContext.Create())
            {
                var entity = new PersonProxyClass
                {
                    PersonId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.People.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedParticipants_ParticipantPerson()
        {
            using (ShimsContext.Create())
            {
                var entity = new ParticipantPersonProxyClass
                {
                    ParticipantId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.ParticipantPersons.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedParticipants_ParticipantPersonExchangeVisitor()
        {
            using (ShimsContext.Create())
            {
                var entity = new ParticipantExchangeVisitorProxyClass
                {
                    ParticipantId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.ParticipantExchangeVisitors.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedParticipants_EntityIsNotAParticipantRelatedEntity()
        {
            using (ShimsContext.Create())
            {
                var entity = new ProjectProxyClass
                {

                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.Projects.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(0, entities.Count);
            }
        }
        #endregion

        #region GetParticipantTypes
        [TestMethod]
        public void TestParticipantTypes()
        {
            var types = saveAction.GetParticipantTypes();
            Assert.AreEqual(4, types.Count);
            Assert.IsTrue(types.Contains(typeof(Participant)));
            Assert.IsTrue(types.Contains(typeof(Person)));
            Assert.IsTrue(types.Contains(typeof(ParticipantPerson)));
            Assert.IsTrue(types.Contains(typeof(ParticipantExchangeVisitor)));
        }
        #endregion

        #region GetParticipantIds
        [TestMethod]
        public async Task TestGetParticipantIds_CheckDistinctList()
        {
            var list = new List<object>();
            var participant = new ParticipantProxyClass
            {
                ParticipantId = 1
            };
            list.Add(participant);
            list.Add(participant);

            Action<List<int>> tester = (ids) =>
            {
                Assert.AreEqual(2, list.Count);
                Assert.AreEqual(1, ids.Count);
                Assert.AreEqual(participant.ParticipantId, ids.First());
            };
            var participantIds = saveAction.GetParticipantIds(list);
            var participantIdsAsync = await saveAction.GetParticipantIdsAsync(list);
            tester(participantIds);
            tester(participantIdsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantIds_Participant()
        {
            var list = new List<object>();
            var participant = new ParticipantProxyClass
            {
                ParticipantId = 1
            };
            list.Add(participant);

            Action<List<int>> tester = (ids) =>
            {
                Assert.AreEqual(1, ids.Count);
                Assert.AreEqual(participant.ParticipantId, ids.First());
            };
            var participantIds = saveAction.GetParticipantIds(list);
            var participantIdsAsync = await saveAction.GetParticipantIdsAsync(list);
            tester(participantIds);
            tester(participantIdsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantIds_ParticipantPerson()
        {
            var list = new List<object>();
            var participant = new ParticipantPersonProxyClass
            {
                ParticipantId = 1
            };
            list.Add(participant);

            Action<List<int>> tester = (ids) =>
            {
                Assert.AreEqual(1, ids.Count);
                Assert.AreEqual(participant.ParticipantId, ids.First());
            };
            var participantIds = saveAction.GetParticipantIds(list);
            var participantIdsAsync = await saveAction.GetParticipantIdsAsync(list);
            tester(participantIds);
            tester(participantIdsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantIds_ParticipantExchangeVisitor()
        {
            var list = new List<object>();
            var participant = new ParticipantExchangeVisitorProxyClass
            {
                ParticipantId = 1
            };
            list.Add(participant);

            Action<List<int>> tester = (ids) =>
            {
                Assert.AreEqual(1, ids.Count);
                Assert.AreEqual(participant.ParticipantId, ids.First());
            };
            var participantIds = saveAction.GetParticipantIds(list);
            var participantIdsAsync = await saveAction.GetParticipantIdsAsync(list);
            tester(participantIds);
            tester(participantIdsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantIds_Person_IsNotDependent()
        {
            using (ShimsContext.Create())
            {
                var participantId = 2;

                var list = new List<object>();

                var person = new PersonProxyClass
                {
                    PersonId = 1,
                    PersonTypeId = PersonType.Participant.Id
                };
                list.Add(person);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
                {
                    var peopleDtos = new List<SimplePersonDTO>();
                    peopleDtos.Add(new SimplePersonDTO
                    {
                        PersonId = person.PersonId,
                        ParticipantId = participantId
                    });
                    return peopleDtos.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SimplePersonDTO>((src) =>
                {
                    return Task<SimplePersonDTO>.FromResult(src.FirstOrDefault());
                });
                Action<List<int>> tester = (ids) =>
                {
                    Assert.AreEqual(1, ids.Count);
                    Assert.AreEqual(participantId, ids.First());
                };
                var participantIds = saveAction.GetParticipantIds(list);
                var participantIdsAsync = await saveAction.GetParticipantIdsAsync(list);
                tester(participantIds);
                tester(participantIdsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetParticipantIds_Person_IsNotDependent_IsNotParticipating()
        {
            using (ShimsContext.Create())
            {

                var list = new List<object>();

                var person = new PersonProxyClass
                {
                    PersonId = 1,
                    PersonTypeId = PersonType.Participant.Id
                };
                list.Add(person);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
                {
                    var peopleDtos = new List<SimplePersonDTO>();
                    peopleDtos.Add(new SimplePersonDTO
                    {
                        PersonId = person.PersonId,
                        ParticipantId = null
                    });
                    return peopleDtos.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SimplePersonDTO>((src) =>
                {
                    return Task<SimplePersonDTO>.FromResult(src.FirstOrDefault());
                });
                Action<List<int>> tester = (ids) =>
                {
                    Assert.AreEqual(0, ids.Count);
                };
                var participantIds = saveAction.GetParticipantIds(list);
                var participantIdsAsync = await saveAction.GetParticipantIdsAsync(list);
                tester(participantIds);
                tester(participantIdsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetParticipantIds_Person_IsNotDependent_PersonDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                var list = new List<object>();

                var person = new PersonProxyClass
                {
                    PersonId = 1,
                    PersonTypeId = PersonType.Participant.Id
                };
                list.Add(person);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
                {
                    var peopleDtos = new List<SimplePersonDTO>();
                    return peopleDtos.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SimplePersonDTO>((src) =>
                {
                    return Task<SimplePersonDTO>.FromResult(src.FirstOrDefault());
                });
                Action<List<int>> tester = (ids) =>
                {
                    Assert.AreEqual(0, ids.Count);
                };
                var participantIds = saveAction.GetParticipantIds(list);
                var participantIdsAsync = await saveAction.GetParticipantIdsAsync(list);
                tester(participantIds);
                tester(participantIdsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetParticipantIds_Person_IsDependent()
        {
            using (ShimsContext.Create())
            {
                var participantId = 2;

                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                    PersonTypeId = PersonType.Participant.Id
                };
                var dependent = new PersonProxyClass
                {
                    PersonId = 1,
                    PersonTypeId = PersonType.Dependent.Id
                };
                list.Add(dependent);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetRelatedPersonByDependentFamilyMemberQueryEcaContextInt32 = (ctx, personId) =>
                {
                    var peopleDtos = new List<SimplePersonDTO>();
                    peopleDtos.Add(new SimplePersonDTO
                    {
                        PersonId = participatingPerson.PersonId,
                        ParticipantId = participantId
                    });
                    return peopleDtos.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SimplePersonDTO>((src) =>
                {
                    return Task<SimplePersonDTO>.FromResult(src.FirstOrDefault());
                });
                Action<List<int>> tester = (ids) =>
                {
                    Assert.AreEqual(1, ids.Count);
                    Assert.AreEqual(participantId, ids.First());
                };
                var participantIds = saveAction.GetParticipantIds(list);
                var participantIdsAsync = await saveAction.GetParticipantIdsAsync(list);
                tester(participantIds);
                tester(participantIdsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetParticipantIds_Person_PersonTypeIsNotSupported()
        {
            var list = new List<object>();
            var person = new PersonProxyClass
            {
                PersonId = 2,
                PersonTypeId = -1
            };
            list.Add(person);
            var message = "The person by person type is not supported.";
            Action a = () => saveAction.GetParticipantIds(list);
            Func<Task> f = () => saveAction.GetParticipantIdsAsync(list);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetParticipantIds_TheObjectTypeIsNotSupported()
        {
            var list = new List<object>();
            var project = new ProjectProxyClass();
            list.Add(project);
            var message = String.Format("The object type [{0}] is not supported.", project.GetType().BaseType.Name);
            Action a = () => saveAction.GetParticipantIds(list);
            Func<Task> f = () => saveAction.GetParticipantIdsAsync(list);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetParticipantIds_Person_IsDependent_IsNotParticipating()
        {
            using (ShimsContext.Create())
            {
                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                    PersonTypeId = PersonType.Participant.Id
                };
                var dependent = new PersonProxyClass
                {
                    PersonId = 1,
                    PersonTypeId = PersonType.Dependent.Id
                };
                list.Add(dependent);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetRelatedPersonByDependentFamilyMemberQueryEcaContextInt32 = (ctx, personId) =>
                {
                    var peopleDtos = new List<SimplePersonDTO>();
                    peopleDtos.Add(new SimplePersonDTO
                    {
                        PersonId = participatingPerson.PersonId,
                        ParticipantId = null
                    });
                    return peopleDtos.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SimplePersonDTO>((src) =>
                {
                    return Task<SimplePersonDTO>.FromResult(src.FirstOrDefault());
                });
                Action<List<int>> tester = (ids) =>
                {
                    Assert.AreEqual(0, ids.Count);
                };
                var participantIds = saveAction.GetParticipantIds(list);
                var participantIdsAsync = await saveAction.GetParticipantIdsAsync(list);
                tester(participantIds);
                tester(participantIdsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetParticipantIds_Person_IsDependent_PersonDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                    PersonTypeId = PersonType.Participant.Id
                };
                var dependent = new PersonProxyClass
                {
                    PersonId = 1,
                    PersonTypeId = PersonType.Dependent.Id
                };
                list.Add(dependent);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetRelatedPersonByDependentFamilyMemberQueryEcaContextInt32 = (ctx, personId) =>
                {
                    var peopleDtos = new List<SimplePersonDTO>();
                    return peopleDtos.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SimplePersonDTO>((src) =>
                {
                    return Task<SimplePersonDTO>.FromResult(src.FirstOrDefault());
                });
                Action<List<int>> tester = (ids) =>
                {
                    Assert.AreEqual(0, ids.Count);
                };
                var participantIds = saveAction.GetParticipantIds(list);
                var participantIdsAsync = await saveAction.GetParticipantIdsAsync(list);
                tester(participantIds);
                tester(participantIdsAsync);
            }
        }
        #endregion

        #region AfterSaveChanges
        [TestMethod]
        public async Task TestAfterSaveChanges()
        {
            Assert.AreEqual(0, context.SaveChangesCalledCount);
            var participant = new ParticipantProxyClass
            {
                ParticipantId = 1,
                ProjectId = 2
            };
            context.Participants.Add(participant);
            saveAction.CreatedObjects.Add(participant);
            saveAction.Context = context;
            Assert.AreEqual(1, saveAction.CreatedObjects.Count);
            Assert.AreEqual(0, saveAction.ModifiedObjects.Count);

            Action<User, int, int> callback = (usr, projectId, participantId) =>
            {
                Assert.AreEqual(userProvider().Id, usr.Id);
                Assert.AreEqual(participant.ProjectId, projectId);
                Assert.AreEqual(participant.ParticipantId, participantId);
            };
            exchangeVisitorValidationService.Setup(x => x.RunParticipantSevisValidation(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(default(ParticipantPersonSevisCommStatus))
                .Callback(callback);
            exchangeVisitorValidationService.Setup(x => x.RunParticipantSevisValidationAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<ParticipantPersonSevisCommStatus>(null))
                .Callback(callback);

            saveAction.AfterSaveChanges(context);
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            await saveAction.AfterSaveChangesAsync(context);
            Assert.AreEqual(2, context.SaveChangesCalledCount);

            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidation(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidationAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestAfterSaveChanges_NoParticipantObjectsChanged()
        {
            Assert.AreEqual(0, context.SaveChangesCalledCount);
            Assert.AreEqual(0, saveAction.CreatedObjects.Count);
            Assert.AreEqual(0, saveAction.ModifiedObjects.Count);
            saveAction.Context = context;

            saveAction.AfterSaveChanges(context);            
            Assert.AreEqual(0, context.SaveChangesCalledCount);

            await saveAction.AfterSaveChangesAsync(context);
            Assert.AreEqual(0, context.SaveChangesCalledCount);

            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidation(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never());
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidationAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never());
        }
        #endregion

        #region BeforeSaveChanges
        [TestMethod]
        public async Task TestBeforeChanges()
        {
            using (ShimsContext.Create())
            {
                var entity = new ParticipantProxyClass
                {
                    ParticipantId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                Action tester = () =>
                {
                    Assert.AreEqual(1, saveAction.CreatedObjects.Count);
                    Assert.IsTrue(Object.ReferenceEquals(entity, saveAction.CreatedObjects.First()));
                };
                context.SetupActions.Add(() =>
                {
                    saveAction.CreatedObjects.Clear();
                    context.Participants.Add(entity);
                });

                context.Revert();
                saveAction.BeforeSaveChanges(context);
                tester();

                context.Revert();
                await saveAction.BeforeSaveChangesAsync(context);
                tester();
            }
        }
        #endregion

        [TestMethod]
        public void TestGetUnionedCreatedAndModifiedObjects()
        {
            var a = 1;
            var b = 2;
            saveAction.CreatedObjects.Add(a);
            saveAction.ModifiedObjects.Add(b);
            var list = saveAction.GetUnionedCreatedAndModifiedObjects();
            Assert.IsTrue(list.Contains(a));
            Assert.IsTrue(list.Contains(b));
        }
    }
}
