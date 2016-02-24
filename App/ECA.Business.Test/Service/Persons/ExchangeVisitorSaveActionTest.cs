using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using Microsoft.QualityTools.Testing.Fakes;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using ECA.Business.Service.Persons;
using Moq;

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

    [TestClass]
    public class ExchangeVisitorSaveActionTest
    {
        private TestEcaContext context;
        private Mock<IExchangeVisitorValidationService> exchangeVisitorValidationService;
        private ExchangeVisitorSaveAction saveAction;

        [TestInitialize]
        public void TestInit()
        {
            exchangeVisitorValidationService = new Mock<IExchangeVisitorValidationService>();
            context = new TestEcaContext();
            saveAction = new ExchangeVisitorSaveAction(exchangeVisitorValidationService.Object);
        }
        #region Modified Entities

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
    }
}
