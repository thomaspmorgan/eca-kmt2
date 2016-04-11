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
using ECA.Core.DynamicLinq;

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

    public class PhoneNumberProxyClass : ECA.Data.PhoneNumber
    {

    }

    public class EmailAddressProxyClass : ECA.Data.EmailAddress
    {

    }

    public class AddressProxyClass : ECA.Data.Address
    {

    }

    public class LocationProxyClass : ECA.Data.Location
    {

    }

    public class PersonDependentProxyClass : ECA.Data.PersonDependent
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
        public void TestGetModifiedParticipants_PersonDependent()
        {
            using (ShimsContext.Create())
            {
                var entity = new PersonDependentProxyClass
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
                context.PersonDependents.Add(entity);
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
        public void TestGetModifiedParticipants_EmailAddress()
        {
            using (ShimsContext.Create())
            {
                var entity = new EmailAddress
                {
                    EmailAddressId = 1,
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
                context.EmailAddresses.Add(entity);
                var entities = saveAction.GetModifiedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedParticipants_EmailAddressProxyClass()
        {
            using (ShimsContext.Create())
            {
                var entity = new EmailAddressProxyClass
                {
                    EmailAddressId = 1,
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
                context.EmailAddresses.Add(entity);
                var entities = saveAction.GetModifiedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedParticipants_PhoneNumber()
        {
            using (ShimsContext.Create())
            {
                var entity = new PhoneNumber
                {
                    PhoneNumberId = 1,
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
                context.PhoneNumbers.Add(entity);
                var entities = saveAction.GetModifiedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedParticipants_PhoneNumberProxyClass()
        {
            using (ShimsContext.Create())
            {
                var entity = new PhoneNumberProxyClass
                {
                    PhoneNumberId = 1,
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
                context.PhoneNumbers.Add(entity);
                var entities = saveAction.GetModifiedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedParticipants_Address()
        {
            using (ShimsContext.Create())
            {
                var entity = new Address
                {
                    AddressId = 1,
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
                context.Addresses.Add(entity);
                var entities = saveAction.GetModifiedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedParticipants_AddressProxyClass()
        {
            using (ShimsContext.Create())
            {
                var entity = new AddressProxyClass
                {
                    AddressId = 1,
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
                context.Addresses.Add(entity);
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
        public void TestGetCreatedEntities_Participant()
        {
            using (ShimsContext.Create())
            {
                var entity = new Participant
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
        public void TestGetCreatedEntities_ParticipantProxy()
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
        public void TestGetCreatedEntities_Person()
        {
            using (ShimsContext.Create())
            {
                var entity = new Person
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
        public void TestGetCreatedEntities_PersonProxy()
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
        public void TestGetCreatedEntities_PersonDependent()
        {
            using (ShimsContext.Create())
            {
                var entity = new PersonDependent
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
                context.PersonDependents.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedEntities_PersonDependentProxy()
        {
            using (ShimsContext.Create())
            {
                var entity = new PersonDependentProxyClass
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
                context.PersonDependents.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedEntities_ParticipantPerson()
        {
            using (ShimsContext.Create())
            {
                var entity = new ParticipantPerson
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
        public void TestGetCreatedEntities_ParticipantPersonProxy()
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
        public void TestGetCreatedEntities_ParticipantPersonExchangeVisitor()
        {
            using (ShimsContext.Create())
            {
                var entity = new ParticipantExchangeVisitor
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
        public void TestGetCreatedEntities_ParticipantPersonExchangeVisitorProxy()
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
        public void TestGetCreatedEntities_Address()
        {
            using (ShimsContext.Create())
            {
                var entity = new Address
                {
                    AddressId = 1,
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
                context.Addresses.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedEntities_AddressProxy()
        {
            using (ShimsContext.Create())
            {
                var entity = new AddressProxyClass
                {
                    AddressId = 1,
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
                context.Addresses.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedEntities_EmailAddress()
        {
            using (ShimsContext.Create())
            {
                var entity = new EmailAddress
                {
                    EmailAddressId = 1,
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
                context.EmailAddresses.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedEntities_EmailAddressProxy()
        {
            using (ShimsContext.Create())
            {
                var entity = new EmailAddressProxyClass
                {
                    EmailAddressId = 1,
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
                context.EmailAddresses.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedEntities_PhoneNumber()
        {
            using (ShimsContext.Create())
            {
                var entity = new PhoneNumber
                {
                    PhoneNumberId = 1,
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
                context.PhoneNumbers.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedEntities_PhoneNumberProxy()
        {
            using (ShimsContext.Create())
            {
                var entity = new PhoneNumberProxyClass
                {
                    PhoneNumberId = 1,
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
                context.PhoneNumbers.Add(entity);
                var entities = saveAction.GetCreatedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetCreatedEntities_EntityIsNotAParticipantRelatedEntity()
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

        #region Deleted Entities
        [TestMethod]
        public void TestGetDeletedEntities_EntityTypeIsNotParticipantRelatedEntity()
        {
            using (ShimsContext.Create())
            {
                var entity = new Project
                {
                    ProjectId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Deleted
                    });
                    return entries;
                };
                context.Projects.Add(entity);
                var entities = saveAction.GetDeletedEntities(context);
                Assert.AreEqual(0, entities.Count);
            }
        }

        [TestMethod]
        public void TestGetDeletedEntities_Address()
        {
            using (ShimsContext.Create())
            {
                var entity = new Address
                {
                    AddressId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Deleted
                    });
                    return entries;
                };
                context.Addresses.Add(entity);
                var entities = saveAction.GetDeletedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetDeletedEntities_AddressProxy()
        {
            using (ShimsContext.Create())
            {
                var entity = new AddressProxyClass
                {
                    AddressId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Deleted
                    });
                    return entries;
                };
                context.Addresses.Add(entity);
                var entities = saveAction.GetDeletedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetDeletedEntities_PhoneNumber()
        {
            using (ShimsContext.Create())
            {
                var entity = new PhoneNumber
                {
                    PhoneNumberId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Deleted
                    });
                    return entries;
                };
                context.PhoneNumbers.Add(entity);
                var entities = saveAction.GetDeletedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetDeletedEntities_PhoneNumberProxy()
        {
            using (ShimsContext.Create())
            {
                var entity = new PhoneNumberProxyClass
                {
                    PhoneNumberId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Deleted
                    });
                    return entries;
                };
                context.PhoneNumbers.Add(entity);
                var entities = saveAction.GetDeletedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetDeletedEntities_EmailAddress()
        {
            using (ShimsContext.Create())
            {
                var entity = new EmailAddress
                {
                    EmailAddressId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Deleted
                    });
                    return entries;
                };
                context.EmailAddresses.Add(entity);
                var entities = saveAction.GetDeletedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetDeletedEntities_EmailAddressProxyClass()
        {
            using (ShimsContext.Create())
            {
                var entity = new EmailAddressProxyClass
                {
                    EmailAddressId = 1,
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Deleted
                    });
                    return entries;
                };
                context.EmailAddresses.Add(entity);
                var entities = saveAction.GetDeletedEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        #endregion

        #region GetCreatedAndModifiedEntityTypes
        [TestMethod]
        public void GetCreatedAndModifiedEntityTypes()
        {
            var types = saveAction.GetCreatedAndModifiedEntityTypes();
            Assert.AreEqual(9, types.Count);
            Assert.IsTrue(types.Contains(typeof(Location)));
            Assert.IsTrue(types.Contains(typeof(Participant)));
            Assert.IsTrue(types.Contains(typeof(Person)));
            Assert.IsTrue(types.Contains(typeof(ParticipantPerson)));
            Assert.IsTrue(types.Contains(typeof(ParticipantExchangeVisitor)));
            Assert.IsTrue(types.Contains(typeof(PhoneNumber)));
            Assert.IsTrue(types.Contains(typeof(EmailAddress)));
            Assert.IsTrue(types.Contains(typeof(Address)));
            Assert.IsTrue(types.Contains(typeof(PersonDependent)));
        }
        #endregion

        #region GetDeletedEntityTypes
        [TestMethod]
        public void GetDeletedEntityTypes()
        {
            var types = saveAction.GetDeletedEntityTypes();
            Assert.AreEqual(3, types.Count);
            Assert.IsTrue(types.Contains(typeof(PhoneNumber)));
            Assert.IsTrue(types.Contains(typeof(EmailAddress)));
            Assert.IsTrue(types.Contains(typeof(Address)));
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
        public async Task TestGetParticipantIds_Person()
        {
            using (ShimsContext.Create())
            {
                var participantId = 2;

                var list = new List<object>();

                var person = new PersonProxyClass
                {
                    PersonId = 1,
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
        public async Task TestGetParticipantIds_PersonDependent()
        {
            using (ShimsContext.Create())
            {
                var participantId = 2;

                var list = new List<object>();
                
                var person = new PersonProxyClass
                {
                    PersonId = 1,
                };
                var personDependent = new PersonDependentProxyClass
                {
                    DependentId = 1,
                    Person = person,
                    PersonId = person.PersonId
                };
                list.Add(personDependent);
                context.People.Add(person);
                saveAction.Context = context;
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
        public async Task TestGetParticipantIds_Person_IsNotParticipating()
        {
            using (ShimsContext.Create())
            {

                var list = new List<object>();

                var person = new PersonProxyClass
                {
                    PersonId = 1,
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
        public async Task TestGetParticipantIds_Person_PersonDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                var list = new List<object>();

                var person = new PersonProxyClass
                {
                    PersonId = 1,
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
        public async Task TestGetParticipantIds_PhoneNumber()
        {
            using (ShimsContext.Create())
            {
                var participantId = 2;

                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                };
                var phoneNumber = new PhoneNumberProxyClass
                {
                    PhoneNumberId = 3,
                    Person = participatingPerson,
                    PersonId = participatingPerson.PersonId
                };
                list.Add(phoneNumber);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
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

                context.People.Add(participatingPerson);
                context.PhoneNumbers.Add(phoneNumber);
                saveAction.Context = context;
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
        public async Task TestGetParticipantIds_PhoneNumberDoesNotBelongToPerson()
        {
            using (ShimsContext.Create())
            {
                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                };
                var phoneNumber = new PhoneNumberProxyClass
                {
                    PhoneNumberId = 3,
                    Person = new Person(),
                    PersonId = participatingPerson.PersonId + 1
                };
                list.Add(phoneNumber);
                context.People.Add(participatingPerson);
                context.PhoneNumbers.Add(phoneNumber);
                saveAction.Context = context;
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
        public async Task TestGetParticipantIds_EmailAddress_BelongsToPerson()
        {
            using (ShimsContext.Create())
            {
                var participantId = 2;

                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                };
                var email = new EmailAddressProxyClass
                {
                    EmailAddressId = 3,
                    Person = participatingPerson,
                    PersonId = participatingPerson.PersonId
                };
                list.Add(email);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
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

                context.People.Add(participatingPerson);
                context.EmailAddresses.Add(email);
                saveAction.Context = context;
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
        public async Task TestGetParticipantIds_EmailAddress_BelongsToDependent()
        {
            using (ShimsContext.Create())
            {
                var participantId = 2;

                var list = new List<object>();
                var person = new Person
                {
                    PersonId = 20,
                };
                var personDependent = new PersonDependentProxyClass
                {
                    DependentId = 1,
                    PersonId = person.PersonId,
                    Person = person
                };
                var email = new EmailAddressProxyClass
                {
                    EmailAddressId = 3,
                    Dependent = personDependent,
                    DependentId = personDependent.DependentId
                };
                list.Add(email);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
                {
                    var peopleDtos = new List<SimplePersonDTO>();
                    peopleDtos.Add(new SimplePersonDTO
                    {
                        PersonId = personDependent.PersonId,
                        ParticipantId = participantId
                    });
                    return peopleDtos.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SimplePersonDTO>((src) =>
                {
                    return Task<SimplePersonDTO>.FromResult(src.FirstOrDefault());
                });
                context.People.Add(person);
                context.PersonDependents.Add(personDependent);
                context.EmailAddresses.Add(email);
                saveAction.Context = context;
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
        public async Task TestGetParticipantIds_EmailAddressDoesNotBelongToPerson()
        {
            using (ShimsContext.Create())
            {
                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                };
                var email = new EmailAddressProxyClass
                {
                    EmailAddressId = 3,
                    Person = new Person(),
                    PersonId = participatingPerson.PersonId + 1
                };
                list.Add(email);
                context.People.Add(participatingPerson);
                context.EmailAddresses.Add(email);
                saveAction.Context = context;
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
        public async Task TestGetParticipantIds_Address()
        {
            using (ShimsContext.Create())
            {
                var participantId = 2;

                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                };
                var address = new AddressProxyClass
                {
                    AddressId = 3,
                    Person = participatingPerson,
                    PersonId = participatingPerson.PersonId
                };
                list.Add(address);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
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
                context.People.Add(participatingPerson);
                context.Addresses.Add(address);
                saveAction.Context = context;
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
        public async Task TestGetParticipantIds_AddressDoesNotBelongToPerson()
        {
            using (ShimsContext.Create())
            {
                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                };
                var address = new AddressProxyClass
                {
                    AddressId = 3,
                    Person = new Person(),
                    PersonId = participatingPerson.PersonId + 1
                };
                list.Add(address);
                context.People.Add(participatingPerson);
                context.Addresses.Add(address);
                saveAction.Context = context;
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
        public async Task TestGetParticipantIds_LocationThatHasAddress()
        {
            using (ShimsContext.Create())
            {
                var participantId = 2;

                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                };

                var location = new LocationProxyClass
                {
                    LocationId = 1,
                };
                var address = new AddressProxyClass
                {
                    AddressId = 3,
                    Person = participatingPerson,
                    PersonId = participatingPerson.PersonId,
                    LocationId = location.LocationId,
                    Location = location
                };
                list.Add(location);
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
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
                context.People.Add(participatingPerson);
                context.Addresses.Add(address);
                context.Locations.Add(location);
                saveAction.Context = context;
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
        public async Task TestGetParticipantIds_LocationThatDoesNotHaveAddress()
        {
            using (ShimsContext.Create())
            {
                var list = new List<object>();
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                };
                var location = new LocationProxyClass
                {
                    LocationId = 3,
                };
                list.Add(location);
                context.People.Add(participatingPerson);
                context.Locations.Add(location);
                saveAction.Context = context;
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
        public async Task TestGetParticipantIds_TheObjectTypeIsNotSupported()
        {
            var list = new List<object>();
            var moneyFlow = new MoneyFlow();
            list.Add(moneyFlow);
            var message = String.Format("The object type [{0}] is not supported.", moneyFlow.GetType().Name);
            Action a = () => saveAction.GetParticipantIds(list);
            Func<Task> f = () => saveAction.GetParticipantIdsAsync(list);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
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
            var participantPerson = new ParticipantPersonProxyClass
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId
            };
            participant.ParticipantPerson = participantPerson;
            context.ParticipantPersons.Add(participantPerson);
            context.Participants.Add(participant);
            saveAction.CreatedObjects.Add(participant);
            saveAction.Context = context;
            Assert.AreEqual(1, saveAction.CreatedObjects.Count);
            Assert.AreEqual(0, saveAction.ModifiedObjects.Count);

            Action<int, int> callback = (projectId, participantId) =>
            {
                Assert.AreEqual(participant.ProjectId, projectId);
                Assert.AreEqual(participant.ParticipantId, participantId);
            };
            exchangeVisitorValidationService.Setup(x => x.RunParticipantSevisValidation(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(default(ParticipantPersonSevisCommStatus))
                .Callback(callback);
            exchangeVisitorValidationService.Setup(x => x.RunParticipantSevisValidationAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<ParticipantPersonSevisCommStatus>(null))
                .Callback(callback);

            saveAction.AfterSaveChanges(context);
            Assert.AreEqual(1, context.SaveChangesCalledCount);

            await saveAction.AfterSaveChangesAsync(context);
            Assert.AreEqual(2, context.SaveChangesCalledCount);

            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidation(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidationAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestAfterSaveChanges_ParticipantDoesNotHaveParticipantPerson()
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

            Action<int, int> callback = (projectId, participantId) =>
            {
                Assert.AreEqual(participant.ProjectId, projectId);
                Assert.AreEqual(participant.ParticipantId, participantId);
            };

            saveAction.AfterSaveChanges(context);
            Assert.AreEqual(0, context.SaveChangesCalledCount);

            await saveAction.AfterSaveChangesAsync(context);
            Assert.AreEqual(0, context.SaveChangesCalledCount);

            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidation(It.IsAny<int>(), It.IsAny<int>()), Times.Never());
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidationAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never());
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

            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidation(It.IsAny<int>(), It.IsAny<int>()), Times.Never());
            exchangeVisitorValidationService.Verify(x => x.RunParticipantSevisValidationAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never());
        }
        #endregion

        #region BeforeSaveChanges
        [TestMethod]
        public async Task TestBeforeChanges_AddedEntity()
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
                    Assert.AreEqual(0, saveAction.ParticipantIds.Count);
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

        [TestMethod]
        public async Task TestBeforeChanges_ModifiedEntity()
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
                Action tester = () =>
                {
                    Assert.AreEqual(1, saveAction.ModifiedObjects.Count);
                    Assert.IsTrue(Object.ReferenceEquals(entity, saveAction.ModifiedObjects.First()));
                    Assert.AreEqual(0, saveAction.ParticipantIds.Count);
                };
                context.SetupActions.Add(() =>
                {
                    saveAction.ModifiedObjects.Clear();
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

        [TestMethod]
        public async Task TestBeforeChanges_DeletedEntity_HasPersonId_HasParticipantId()
        {
            using (ShimsContext.Create())
            {
                var participantId = 2;
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                };
                var address = new Address
                {
                    AddressId = 3,
                    Person = participatingPerson,
                    PersonId = participatingPerson.PersonId
                };
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
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

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.PersonId), property);
                    return address.PersonId;
                });
                var addressDbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>();
                addressDbEntityEntry.GetDatabaseValues = () => propertyValues;
                addressDbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);
                addressDbEntityEntry.EntityGet = () => address;
                addressDbEntityEntry.StateGet = () => EntityState.Deleted;

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);
                dbEntityEntry.EntityGet = () => address;
                dbEntityEntry.StateGet = () => EntityState.Deleted;

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, addr) =>
                {
                    return addressDbEntityEntry;
                });

                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (chgTracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(dbEntityEntry);
                    return entries;
                };

                context.GetLocalDelegate = () => address;

                Action tester = () =>
                {
                    Assert.AreEqual(1, saveAction.DeletedObjects.Count);
                    Assert.IsTrue(Object.ReferenceEquals(address, saveAction.DeletedObjects.First()));
                    Assert.AreEqual(1, saveAction.ParticipantIds.Count);
                };
                context.SetupActions.Add(() =>
                {
                    saveAction.DeletedObjects.Clear();
                    context.Addresses.Add(address);
                    context.People.Add(participatingPerson);
                });

                context.Revert();
                saveAction.BeforeSaveChanges(context);
                tester();

                context.Revert();
                await saveAction.BeforeSaveChangesAsync(context);
                tester();
            }
        }

        [TestMethod]
        public async Task TestBeforeChanges_DeletedEntity_HasPersonId_DoesNotHaveParticipantId()
        {
            using (ShimsContext.Create())
            {
                var participatingPerson = new PersonProxyClass
                {
                    PersonId = 2,
                };
                var address = new Address
                {
                    AddressId = 3,
                    Person = participatingPerson,
                    PersonId = participatingPerson.PersonId
                };
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
                {
                    var peopleDtos = new List<SimplePersonDTO>();
                    return peopleDtos.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SimplePersonDTO>((src) =>
                {
                    return Task<SimplePersonDTO>.FromResult(src.FirstOrDefault());
                });

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.PersonId), property);
                    return address.PersonId;
                });
                var addressDbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>();
                addressDbEntityEntry.GetDatabaseValues = () => propertyValues;
                addressDbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);
                addressDbEntityEntry.EntityGet = () => address;
                addressDbEntityEntry.StateGet = () => EntityState.Deleted;

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);
                dbEntityEntry.EntityGet = () => address;
                dbEntityEntry.StateGet = () => EntityState.Deleted;

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, addr) =>
                {
                    return addressDbEntityEntry;
                });

                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (chgTracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(dbEntityEntry);
                    return entries;
                };

                context.GetLocalDelegate = () => address;

                Action tester = () =>
                {
                    Assert.AreEqual(1, saveAction.DeletedObjects.Count);
                    Assert.IsTrue(Object.ReferenceEquals(address, saveAction.DeletedObjects.First()));
                    Assert.AreEqual(0, saveAction.ParticipantIds.Count);
                };
                context.SetupActions.Add(() =>
                {
                    saveAction.DeletedObjects.Clear();
                    context.Addresses.Add(address);
                    context.People.Add(participatingPerson);
                });

                context.Revert();
                saveAction.BeforeSaveChanges(context);
                tester();

                context.Revert();
                await saveAction.BeforeSaveChangesAsync(context);
                tester();
            }
        }

        [TestMethod]
        public async Task TestBeforeChanges_DeletedEntity_DoesNotHavePersonId()
        {
            using (ShimsContext.Create())
            {
                var address = new Address
                {
                    AddressId = 3,
                };
                ECA.Business.Queries.Persons.Fakes.ShimPersonQueries.CreateGetSimplePersonDTOsQueryEcaContext = (ctx) =>
                {
                    var peopleDtos = new List<SimplePersonDTO>();
                    return peopleDtos.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SimplePersonDTO>((src) =>
                {
                    return Task<SimplePersonDTO>.FromResult(src.FirstOrDefault());
                });

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.PersonId), property);
                    return address.PersonId;
                });
                var addressDbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>();
                addressDbEntityEntry.GetDatabaseValues = () => propertyValues;
                addressDbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);
                addressDbEntityEntry.EntityGet = () => address;
                addressDbEntityEntry.StateGet = () => EntityState.Deleted;

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);
                dbEntityEntry.EntityGet = () => address;
                dbEntityEntry.StateGet = () => EntityState.Deleted;

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, addr) =>
                {
                    return addressDbEntityEntry;
                });

                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (chgTracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(dbEntityEntry);
                    return entries;
                };

                context.GetLocalDelegate = () => address;

                Action tester = () =>
                {
                    Assert.AreEqual(1, saveAction.DeletedObjects.Count);
                    Assert.IsTrue(Object.ReferenceEquals(address, saveAction.DeletedObjects.First()));
                    Assert.AreEqual(0, saveAction.ParticipantIds.Count);
                };
                context.SetupActions.Add(() =>
                {
                    saveAction.DeletedObjects.Clear();
                    context.Addresses.Add(address);
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

        #region GetPersonIdByEntity
        [TestMethod]
        public async Task TestGetPersonIdByEntity()
        {
            using (ShimsContext.Create())
            {
                var address = new Address
                {
                    AddressId = 1,
                    PersonId = 2
                };
                context.GetLocalDelegate = () => address;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.PersonId), property);
                    return address.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(address.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByEntity<Address>(context, address, x => x.PersonId);
                var resultAsync = await saveAction.GetPersonIdByEntityAsync<Address>(context, address, x => x.PersonId);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByEntity_PersonDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                var address = new Address
                {
                    AddressId = 1,
                };
                context.GetLocalDelegate = () => address;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.PersonId), property);
                    return address.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.IsNull(personId);
                };

                var result = saveAction.GetPersonIdByEntity<Address>(context, address, x => x.PersonId);
                var resultAsync = await saveAction.GetPersonIdByEntityAsync<Address>(context, address, x => x.PersonId);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByAddress()
        {
            using (ShimsContext.Create())
            {
                var address = new Address
                {
                    AddressId = 1,
                    PersonId = 2
                };
                context.GetLocalDelegate = () => address;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.PersonId), property);
                    return address.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(address.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByAddress(context, address);
                var resultAsync = await saveAction.GetPersonIdByAddressAsync(context, address);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByPhoneNumber()
        {
            using (ShimsContext.Create())
            {
                var phoneNumber = new PhoneNumber
                {
                    PhoneNumberId = 1,
                    PersonId = 2
                };
                context.GetLocalDelegate = () => phoneNumber;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<PhoneNumber>(x => x.PersonId), property);
                    return phoneNumber.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<PhoneNumber>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<PhoneNumber>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(phoneNumber.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByPhoneNumber(context, phoneNumber);
                var resultAsync = await saveAction.GetPersonIdByPhoneNumberAsync(context, phoneNumber);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByEmailAddress_EmailIsForPerson()
        {
            using (ShimsContext.Create())
            {
                var email = new EmailAddress
                {
                    EmailAddressId = 1,
                    PersonId = 2
                };
                context.GetLocalDelegate = () => email;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<EmailAddress>(x => x.PersonId), property);
                    return email.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<EmailAddress>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<EmailAddress>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(email.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByEmailAddress(context, email);
                var resultAsync = await saveAction.GetPersonIdByEmailAddressAsync(context, email);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByEmailAddress_EmailIsForPersonDependent()
        {
            using (ShimsContext.Create())
            {
                var person = new Person
                {
                    PersonId = 100
                };
                var dependent = new PersonDependent
                {
                    DependentId = 2,
                    PersonId = person.PersonId,
                    Person = person
                };
                var email = new EmailAddress
                {
                    EmailAddressId = 1,
                    DependentId = 2,
                    Dependent = dependent
                };
                context.GetLocalDelegate = () => email;
                context.People.Add(person);
                context.PersonDependents.Add(dependent);
                context.EmailAddresses.Add(email);
                saveAction.Context = context;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<EmailAddress>(x => x.DependentId), property);
                    return email.DependentId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<EmailAddress>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<EmailAddress>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(person.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByEmailAddress(context, email);
                var resultAsync = await saveAction.GetPersonIdByEmailAddressAsync(context, email);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByEmailAddress_EmailIsForPersonDependent_DependentDoesNotHavePerson()
        {
            using (ShimsContext.Create())
            {
                var dependent = new PersonDependent
                {
                    DependentId = 2,
                };
                var email = new EmailAddress
                {
                    EmailAddressId = 1,
                    DependentId = 2,
                    Dependent = dependent
                };
                context.GetLocalDelegate = () => email;
                context.PersonDependents.Add(dependent);
                context.EmailAddresses.Add(email);
                saveAction.Context = context;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<EmailAddress>(x => x.DependentId), property);
                    return email.DependentId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<EmailAddress>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<EmailAddress>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.IsNull(personId);
                };

                var result = saveAction.GetPersonIdByEmailAddress(context, email);
                var resultAsync = await saveAction.GetPersonIdByEmailAddressAsync(context, email);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByObject_EmailAddress()
        {
            using (ShimsContext.Create())
            {
                var email = new EmailAddress
                {
                    EmailAddressId = 1,
                    PersonId = 2
                };
                context.GetLocalDelegate = () => email;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<EmailAddress>(x => x.PersonId), property);
                    return email.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<EmailAddress>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<EmailAddress>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(email.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByObject(context, email);
                var resultAsync = await saveAction.GetPersonIdByObjectAsync(context, email);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByObject_EmailAddressProxyClass()
        {
            using (ShimsContext.Create())
            {
                var email = new EmailAddressProxyClass
                {
                    EmailAddressId = 1,
                    PersonId = 2
                };
                context.GetLocalDelegate = () => email;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<EmailAddress>(x => x.PersonId), property);
                    return email.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<EmailAddress>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<EmailAddress>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(email.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByObject(context, email);
                var resultAsync = await saveAction.GetPersonIdByObjectAsync(context, email);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByObject_Address()
        {
            using (ShimsContext.Create())
            {
                var address = new Address
                {
                    AddressId = 1,
                    PersonId = 2
                };
                context.GetLocalDelegate = () => address;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.PersonId), property);
                    return address.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(address.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByObject(context, address);
                var resultAsync = await saveAction.GetPersonIdByObjectAsync(context, address);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByObject_AddressProxyClass()
        {
            using (ShimsContext.Create())
            {
                var address = new AddressProxyClass
                {
                    AddressId = 1,
                    PersonId = 2
                };
                context.GetLocalDelegate = () => address;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.PersonId), property);
                    return address.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<Address>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<Address>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(address.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByObject(context, address);
                var resultAsync = await saveAction.GetPersonIdByObjectAsync(context, address);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByObject_PhoneNumber()
        {
            using (ShimsContext.Create())
            {
                var phoneNumber = new PhoneNumber
                {
                    PhoneNumberId = 1,
                    PersonId = 2
                };
                context.GetLocalDelegate = () => phoneNumber;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<PhoneNumber>(x => x.PersonId), property);
                    return phoneNumber.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<PhoneNumber>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<PhoneNumber>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(phoneNumber.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByObject(context, phoneNumber);
                var resultAsync = await saveAction.GetPersonIdByObjectAsync(context, phoneNumber);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByObject_PhoneNumberProxyClass()
        {
            using (ShimsContext.Create())
            {
                var phoneNumber = new PhoneNumberProxyClass
                {
                    PhoneNumberId = 1,
                    PersonId = 2
                };
                context.GetLocalDelegate = () => phoneNumber;

                var propertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                propertyValues.GetValueOf1String<int?>((property) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<PhoneNumber>(x => x.PersonId), property);
                    return phoneNumber.PersonId;
                });

                var dbEntityEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry<PhoneNumber>();
                dbEntityEntry.GetDatabaseValues = () => propertyValues;
                dbEntityEntry.GetDatabaseValuesAsync = () => Task.FromResult<DbPropertyValues>(propertyValues);

                System.Data.Entity.Fakes.ShimDbContext.AllInstances.EntryOf1M0<PhoneNumber>((ctx, addr) =>
                {
                    return dbEntityEntry;
                });

                Action<int?> tester = (personId) =>
                {
                    Assert.AreEqual(phoneNumber.PersonId, personId);
                };

                var result = saveAction.GetPersonIdByObject(context, phoneNumber);
                var resultAsync = await saveAction.GetPersonIdByObjectAsync(context, phoneNumber);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetPersonIdByObject_ObjectTypeNotSupported()
        {
            var obj = 1;
            Action a = () => saveAction.GetPersonIdByObject(context, obj);
            Func<Task> f = () => saveAction.GetPersonIdByObjectAsync(context, obj);

            var expectedMessage = String.Format("The object type [{0}] is not supported.", obj.GetType().Name);
            a.ShouldThrow<NotSupportedException>().WithMessage(expectedMessage);
            f.ShouldThrow<NotSupportedException>().WithMessage(expectedMessage);
        }
        #endregion

        [TestMethod]
        public void TestGetUnionedObjects()
        {
            var a = 1;
            var b = 2;
            var c = 3;
            saveAction.CreatedObjects.Add(a);
            saveAction.ModifiedObjects.Add(b);
            saveAction.DeletedObjects.Add(c);
            var list = saveAction.GetCreatedAndModifiedEntities();
            Assert.IsTrue(list.Contains(a));
            Assert.IsTrue(list.Contains(b));
            Assert.IsTrue(list.Contains(c));
        }
    }
}

