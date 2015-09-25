using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Fundings;
using ECA.Core.DynamicLinq;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Business.Test.Queries.Fundings
{
    [TestClass]
    public class MoneyFlowQueriesTest
    {
        private TestEcaContext context;
        private MoneyFlowSourceRecipientType itineraryStopType;
        private MoneyFlowSourceRecipientType organizationType;
        private MoneyFlowSourceRecipientType programType;
        private MoneyFlowSourceRecipientType projectType;
        private MoneyFlowSourceRecipientType participantType;
        private MoneyFlowSourceRecipientType accomodationType;
        private MoneyFlowSourceRecipientType transportationType;
        private MoneyFlowSourceRecipientType expenseType;
        private MoneyFlowSourceRecipientType postType;

        private MoneyFlowStatus actual;
        private MoneyFlowStatus appropriated;

        private MoneyFlowType outgoing;
        private MoneyFlowType incoming;


        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            itineraryStopType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.ItineraryStop.Id,
                TypeName = MoneyFlowSourceRecipientType.ItineraryStop.Value
            };
            organizationType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Organization.Id,
                TypeName = MoneyFlowSourceRecipientType.Organization.Value
            };
            programType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Program.Id,
                TypeName = MoneyFlowSourceRecipientType.Program.Value
            };
            projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            participantType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Participant.Id,
                TypeName = MoneyFlowSourceRecipientType.Participant.Value
            };
            accomodationType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Accomodation.Id,
                TypeName = MoneyFlowSourceRecipientType.Accomodation.Value
            };
            transportationType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Transportation.Id,
                TypeName = MoneyFlowSourceRecipientType.Transportation.Value
            };
            expenseType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Expense.Id,
                TypeName = MoneyFlowSourceRecipientType.Expense.Value
            };
            postType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Post.Id,
                TypeName = MoneyFlowSourceRecipientType.Post.Value
            };
            actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            appropriated = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Appropriated.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Appropriated.Value
            };
            outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            incoming = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Incoming.Id,
                MoneyFlowTypeName = MoneyFlowType.Incoming.Value
            };
            context.SetupActions.Add(() =>
            {
                context.MoneyFlowSourceRecipientTypes.Add(itineraryStopType);
                context.MoneyFlowSourceRecipientTypes.Add(organizationType);
                context.MoneyFlowSourceRecipientTypes.Add(programType);
                context.MoneyFlowSourceRecipientTypes.Add(projectType);
                context.MoneyFlowSourceRecipientTypes.Add(participantType);
                context.MoneyFlowSourceRecipientTypes.Add(accomodationType);
                context.MoneyFlowSourceRecipientTypes.Add(transportationType);
                context.MoneyFlowSourceRecipientTypes.Add(expenseType);
                context.MoneyFlowSourceRecipientTypes.Add(postType);

                context.MoneyFlowStatuses.Add(actual);
                context.MoneyFlowStatuses.Add(appropriated);

                context.MoneyFlowTypes.Add(incoming);
                context.MoneyFlowTypes.Add(outgoing);
            });
            context.Revert();
        }

        #region Create Get Outgoing Money Flow DTOs Query
        [TestMethod]
        public void TestCreateOutgoingMoneyFlowDTOsQuery_CheckProperties()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProgram = new Program
            {
                ProgramId = sourceId,
                Name = "program"
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = sourceProgram,
                RecipientProject = recipientProject,
                SourceType = programType,
                SourceTypeId = programType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(recipientProject);
            context.Programs.Add(sourceProgram);
            var results = MoneyFlowQueries.CreateOutgoingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            
            Assert.AreEqual(-moneyFlow.Value, dto.Amount);
            Assert.AreEqual(moneyFlow.Description, dto.Description);
            
            Assert.AreEqual(recipientId, dto.SourceRecipientEntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Program.Id, dto.EntityTypeId);
            Assert.AreEqual(moneyFlow.FiscalYear, dto.FiscalYear);
            Assert.AreEqual(moneyFlow.MoneyFlowId, dto.Id);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Project.Value, dto.SourceRecipientTypeName);
            Assert.AreEqual(actual.MoneyFlowStatusName, dto.MoneyFlowStatus);
            Assert.AreEqual(moneyFlow.MoneyFlowStatusId, dto.MoneyFlowStatusId);
            Assert.AreEqual(moneyFlow.TransactionDate, dto.TransactionDate);
            Assert.AreEqual(outgoing.MoneyFlowTypeName, dto.MoneyFlowType);

            Assert.AreEqual(sourceId, dto.EntityId);
            Assert.AreEqual(programType.MoneyFlowSourceRecipientTypeId, dto.EntityTypeId);
            Assert.AreEqual(recipientId, dto.SourceRecipientEntityId);
            Assert.AreEqual(recipientProject.Name, dto.SourceRecipientName);
            Assert.AreEqual(projectType.TypeName, dto.SourceRecipientTypeName);
            Assert.AreEqual(projectType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateOutgoingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_ItineraryStop()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceItineraryStop = new ItineraryStop
            {
                ItineraryStopId = sourceId,
            };
            var recipientItineraryStop = new ItineraryStop
            {
                ItineraryStopId = recipientId
            };

            var moneyFlow = new MoneyFlow
            {
                SourceItineraryStopId = sourceId,
                RecipientItineraryStopId = recipientId,
                SourceItineraryStop = sourceItineraryStop,
                RecipientItineraryStop = recipientItineraryStop,
                SourceType = itineraryStopType,
                SourceTypeId = itineraryStopType.MoneyFlowSourceRecipientTypeId,
                RecipientType = itineraryStopType,
                RecipientTypeId = itineraryStopType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.ItineraryStops.Add(sourceItineraryStop);
            context.ItineraryStops.Add(recipientItineraryStop);
            var results = MoneyFlowQueries.CreateOutgoingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(sourceId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.ItineraryStop.Id, dto.EntityTypeId);

            Assert.AreEqual(MoneyFlowQueries.ITINERARY_NAME, dto.SourceRecipientName);
            Assert.AreEqual(recipientId, dto.SourceRecipientEntityId);
            Assert.AreEqual(itineraryStopType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(itineraryStopType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateOutgoingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_Organization()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceOrg = new Organization
            {
                OrganizationId = sourceId,
                Name = "Incoming"
            };
            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Outgoing"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceOrganization = sourceOrg,
                RecipientOrganization = recipientOrg,
                SourceType = organizationType,
                SourceTypeId = organizationType.MoneyFlowSourceRecipientTypeId,
                RecipientType = organizationType,
                RecipientTypeId = organizationType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrg);
            context.Organizations.Add(recipientOrg);
            var results = MoneyFlowQueries.CreateOutgoingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(sourceId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Organization.Id, dto.EntityTypeId);

            Assert.AreEqual(recipientOrg.Name, dto.SourceRecipientName);
            Assert.AreEqual(recipientId, dto.SourceRecipientEntityId);
            Assert.AreEqual(organizationType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(organizationType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateOutgoingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_OrganizationParticipant()
        {
            var sourceId = 1;
            var recipientId = 2;
            var orgParticipantType = new ParticipantType
            {
                ParticipantTypeId = 1,
                Name = "name"
            };
            var sourceParticipant = new Participant
            {
                ParticipantId = sourceId,
                ParticipantTypeId = orgParticipantType.ParticipantTypeId,
                ParticipantType = orgParticipantType
            };
            var sourceOrg = new Organization
            {
                OrganizationId = sourceId + 1,
                Name = "Incoming"
            };
            sourceParticipant.Organization = sourceOrg;
            sourceParticipant.OrganizationId = sourceOrg.OrganizationId;

            var recipientParticipant = new Participant
            {
                ParticipantId = recipientId,
                ParticipantTypeId = orgParticipantType.ParticipantTypeId,
                ParticipantType = orgParticipantType

            };
            var recipientOrg = new Organization
            {
                OrganizationId = recipientId + 1,
                Name = "Outgoing"
            };
            recipientParticipant.Organization = recipientOrg;
            recipientParticipant.OrganizationId = recipientOrg.OrganizationId;

            var moneyFlow = new MoneyFlow
            {
                SourceParticipantId = sourceId,
                RecipientParticipantId = recipientId,
                SourceParticipant = sourceParticipant,
                RecipientParticipant = recipientParticipant,
                SourceType = participantType,
                SourceTypeId = participantType.MoneyFlowSourceRecipientTypeId,
                RecipientType = participantType,
                RecipientTypeId = participantType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.ParticipantTypes.Add(orgParticipantType);
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrg);
            context.Organizations.Add(recipientOrg);
            context.Participants.Add(sourceParticipant);
            context.Participants.Add(recipientParticipant);
            var results = MoneyFlowQueries.CreateOutgoingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(sourceId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Participant.Id, dto.EntityTypeId);

            Assert.AreEqual(recipientOrg.Name, dto.SourceRecipientName);
            Assert.AreEqual(recipientId, dto.SourceRecipientEntityId);
            Assert.AreEqual(participantType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(participantType.TypeName, dto.SourceRecipientTypeName);
            Assert.AreEqual(orgParticipantType.ParticipantTypeId, dto.ParticipantTypeId);
            Assert.AreEqual(orgParticipantType.Name, dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateOutgoingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_PersonParticipant()
        {
            var sourceId = 1;
            var recipientId = 2;
            var personParticipantType = new ParticipantType
            {
                ParticipantTypeId = 1,
                Name = "name"
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var sourceParticipant = new Participant
            {
                ParticipantId = sourceId,
                ParticipantTypeId = personParticipantType.ParticipantTypeId,
                ParticipantType = personParticipantType
            };
            var sourcePerson = new Person
            {
                PersonId = sourceId + 1,
                FirstName = "first1",
                LastName = "last2",
                GenderId = gender.GenderId,
                Gender = gender
            };
            sourceParticipant.Person = sourcePerson;
            sourceParticipant.PersonId = sourcePerson.PersonId;

            var recipientParticipant = new Participant
            {
                ParticipantId = recipientId,
                ParticipantTypeId = personParticipantType.ParticipantTypeId,
                ParticipantType = personParticipantType

            };
            var recipientPerson = new Person
            {
                PersonId = recipientId + 1,
                FirstName = "first2",
                LastName = "last2",
                Gender = gender,
                GenderId = gender.GenderId
            };
            recipientParticipant.Person = recipientPerson;
            recipientParticipant.PersonId = recipientPerson.PersonId;

            var moneyFlow = new MoneyFlow
            {
                SourceParticipantId = sourceId,
                RecipientParticipantId = recipientId,
                SourceParticipant = sourceParticipant,
                RecipientParticipant = recipientParticipant,
                SourceType = participantType,
                SourceTypeId = participantType.MoneyFlowSourceRecipientTypeId,
                RecipientType = participantType,
                RecipientTypeId = participantType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.ParticipantTypes.Add(personParticipantType);
            context.Genders.Add(gender);
            context.MoneyFlows.Add(moneyFlow);
            context.People.Add(sourcePerson);
            context.People.Add(recipientPerson);
            context.Participants.Add(sourceParticipant);
            context.Participants.Add(recipientParticipant);
            var results = MoneyFlowQueries.CreateOutgoingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();

            Assert.AreEqual(sourceId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Participant.Id, dto.EntityTypeId);

            Assert.AreEqual(recipientPerson.FirstName + " " + recipientPerson.LastName, dto.SourceRecipientName);
            Assert.AreEqual(recipientId, dto.SourceRecipientEntityId);
            Assert.AreEqual(participantType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(participantType.TypeName, dto.SourceRecipientTypeName);
            Assert.AreEqual(personParticipantType.ParticipantTypeId, dto.ParticipantTypeId);
            Assert.AreEqual(personParticipantType.Name, dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateOutgoingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_Program()
        {
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

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Programs.Add(sourceProgram);
            context.Programs.Add(recipientProgram);
            var results = MoneyFlowQueries.CreateOutgoingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(sourceId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Program.Id, dto.EntityTypeId);

            Assert.AreEqual(recipientProgram.Name, dto.SourceRecipientName);
            Assert.AreEqual(recipientId, dto.SourceRecipientEntityId);
            Assert.AreEqual(programType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(programType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateOutgoingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_Project()
        {
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

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Projects.Add(recipientProject);
            var results = MoneyFlowQueries.CreateOutgoingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(sourceId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Project.Id, dto.EntityTypeId);

            Assert.AreEqual(recipientProject.Name, dto.SourceRecipientName);
            Assert.AreEqual(recipientId, dto.SourceRecipientEntityId);
            Assert.AreEqual(projectType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(projectType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateOutgoingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_Transportation()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Outgoing"
            };

            var recipientTransportation = new Transportation
            {
                TransportationId = recipientId,
                Carrier = new Organization
                {
                    Name = "Incoming"
                }
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientTransportationId = recipientId,
                SourceProject = sourceProject,
                RecipientTransportation = recipientTransportation,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = transportationType,
                RecipientTypeId = transportationType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Transportations.Add(recipientTransportation);
            var results = MoneyFlowQueries.CreateOutgoingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(sourceId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Project.Id, dto.EntityTypeId);

            Assert.AreEqual(recipientTransportation.Carrier.Name, dto.SourceRecipientName);
            Assert.AreEqual(recipientId, dto.SourceRecipientEntityId);
            Assert.AreEqual(transportationType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(transportationType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateOutgoingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_Accomodation()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Outgoing"
            };

            var accomodation = new Accommodation
            {
                AccommodationId = recipientId,
                Host = new Organization
                {
                    Name = "Incoming"
                }
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientAccommodationId = recipientId,
                SourceProject = sourceProject,
                RecipientAccommodation = accomodation,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = accomodationType,
                RecipientTypeId = accomodationType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Accommodations.Add(accomodation);
            var results = MoneyFlowQueries.CreateOutgoingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(sourceId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Project.Id, dto.EntityTypeId);

            Assert.AreEqual(accomodation.Host.Name, dto.SourceRecipientName);
            Assert.AreEqual(recipientId, dto.SourceRecipientEntityId);            
            Assert.AreEqual(accomodationType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(accomodationType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateOutgoingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_Expense()
        {
            var sourceId = 1;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Outgoing"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                SourceProject = sourceProject,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = expenseType,
                RecipientTypeId = expenseType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            var results = MoneyFlowQueries.CreateOutgoingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(sourceId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Project.Id, dto.EntityTypeId);

            Assert.IsNull(dto.SourceRecipientName);
            Assert.IsNull(dto.SourceRecipientEntityId);
            Assert.AreEqual(expenseType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(expenseType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        #endregion

        #region Create Get Incoming Money Flow DTOs Query
        [TestMethod]
        public void TestCreateIncomingMoneyFlowDTOsQuery_CheckProperties()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProgram = new Program
            {
                ProgramId = sourceId,
                Name = "program"
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name= "project"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = sourceProgram,
                RecipientProject = recipientProject,
                SourceType = programType,
                SourceTypeId = programType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Programs.Add(sourceProgram);
            context.Projects.Add(recipientProject);
            var results = MoneyFlowQueries.CreateIncomingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            
            Assert.AreEqual(moneyFlow.Value, dto.Amount);
            Assert.AreEqual(moneyFlow.Description, dto.Description);
            Assert.AreEqual(moneyFlow.FiscalYear, dto.FiscalYear);
            Assert.AreEqual(moneyFlow.MoneyFlowId, dto.Id);
            Assert.AreEqual(actual.MoneyFlowStatusName, dto.MoneyFlowStatus);
            Assert.AreEqual(moneyFlow.MoneyFlowStatusId, dto.MoneyFlowStatusId);
            Assert.AreEqual(moneyFlow.TransactionDate, dto.TransactionDate);
            Assert.AreEqual(incoming.MoneyFlowTypeName, dto.MoneyFlowType);

            Assert.AreEqual(recipientId, dto.EntityId);
            Assert.AreEqual(projectType.MoneyFlowSourceRecipientTypeId, dto.EntityTypeId);
            Assert.AreEqual(sourceId, dto.SourceRecipientEntityId);
            Assert.AreEqual(sourceProgram.Name, dto.SourceRecipientName);
            Assert.AreEqual(programType.TypeName, dto.SourceRecipientTypeName);
            Assert.AreEqual(programType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateIncomingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_ItineraryStop()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceItineraryStop = new ItineraryStop
            {
                ItineraryStopId = sourceId,
            };
            var recipientItineraryStop = new ItineraryStop
            {
                ItineraryStopId = recipientId
            };

            var moneyFlow = new MoneyFlow
            {
                SourceItineraryStopId = sourceId,
                RecipientItineraryStopId = recipientId,
                SourceItineraryStop = sourceItineraryStop,
                RecipientItineraryStop = recipientItineraryStop,
                SourceType = itineraryStopType,
                SourceTypeId = itineraryStopType.MoneyFlowSourceRecipientTypeId,
                RecipientType = itineraryStopType,
                RecipientTypeId = itineraryStopType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.ItineraryStops.Add(sourceItineraryStop);
            context.ItineraryStops.Add(recipientItineraryStop);
            var results = MoneyFlowQueries.CreateIncomingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            
            Assert.AreEqual(recipientItineraryStop.ItineraryStopId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.ItineraryStop.Id, dto.EntityTypeId);

            Assert.AreEqual(MoneyFlowQueries.ITINERARY_NAME, dto.SourceRecipientName);
            Assert.AreEqual(sourceId, dto.SourceRecipientEntityId);
            Assert.AreEqual(itineraryStopType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(itineraryStopType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateIncomingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_Organization()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceOrg = new Organization
            {
                OrganizationId = sourceId,
                Name = "Incoming"
            };
            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Outgoing"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceOrganization = sourceOrg,
                RecipientOrganization = recipientOrg,
                SourceType = organizationType,
                SourceTypeId = organizationType.MoneyFlowSourceRecipientTypeId,
                RecipientType = organizationType,
                RecipientTypeId = organizationType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrg);
            context.Organizations.Add(recipientOrg);
            var results = MoneyFlowQueries.CreateIncomingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(recipientId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Organization.Id, dto.EntityTypeId);

            Assert.AreEqual(sourceOrg.Name, dto.SourceRecipientName);
            Assert.AreEqual(sourceId, dto.SourceRecipientEntityId);
            Assert.AreEqual(organizationType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(organizationType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateIncomingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_OrganizationParticipant()
        {
            var sourceId = 1;
            var recipientId = 2;
            var orgParticipantType = new ParticipantType
            {
                ParticipantTypeId = 1,
                Name = "type"
            };
            var sourceParticipant = new Participant
            {
                ParticipantId = sourceId,
                ParticipantTypeId = orgParticipantType.ParticipantTypeId,
                ParticipantType = orgParticipantType
            };
            var sourceOrg = new Organization
            {
                OrganizationId = sourceId + 1,
                Name = "Incoming"
            };
            sourceParticipant.Organization = sourceOrg;
            sourceParticipant.OrganizationId = sourceOrg.OrganizationId;

            var recipientParticipant = new Participant
            {
                ParticipantId = recipientId,
                ParticipantTypeId = orgParticipantType.ParticipantTypeId,
                ParticipantType = orgParticipantType

            };
            var recipientOrg = new Organization
            {
                OrganizationId = recipientId + 1,
                Name = "Outgoing"
            };
            recipientParticipant.Organization = recipientOrg;
            recipientParticipant.OrganizationId = recipientOrg.OrganizationId;

            var moneyFlow = new MoneyFlow
            {
                SourceParticipantId = sourceId,
                RecipientParticipantId = recipientId,
                SourceParticipant = sourceParticipant,
                RecipientParticipant = recipientParticipant,
                SourceType = participantType,
                SourceTypeId = participantType.MoneyFlowSourceRecipientTypeId,
                RecipientType = participantType,
                RecipientTypeId = participantType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.ParticipantTypes.Add(orgParticipantType);
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrg);
            context.Organizations.Add(recipientOrg);
            context.Participants.Add(sourceParticipant);
            context.Participants.Add(recipientParticipant);
            var results = MoneyFlowQueries.CreateIncomingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(recipientId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Participant.Id, dto.EntityTypeId);

            Assert.AreEqual(sourceOrg.Name, dto.SourceRecipientName);
            Assert.AreEqual(sourceId, dto.SourceRecipientEntityId);
            Assert.AreEqual(participantType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(participantType.TypeName, dto.SourceRecipientTypeName);
            Assert.AreEqual(orgParticipantType.ParticipantTypeId, dto.ParticipantTypeId);
            Assert.AreEqual(orgParticipantType.Name, dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateIncomingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_PersonParticipant()
        {
            var sourceId = 1;
            var recipientId = 2;
            var personParticipantType = new ParticipantType
            {
                ParticipantTypeId = 1,
                Name = "type"
            };
            var sourceParticipant = new Participant
            {
                ParticipantId = sourceId,
                ParticipantTypeId = personParticipantType.ParticipantTypeId,
                ParticipantType = personParticipantType
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var sourcePerson = new Person
            {
                PersonId = sourceId + 1,
                FirstName = "first1",
                LastName = "last2",
                Gender = gender,
                GenderId = gender.GenderId
            };
            sourceParticipant.Person = sourcePerson;
            sourceParticipant.PersonId = sourcePerson.PersonId;

            var recipientParticipant = new Participant
            {
                ParticipantId = recipientId,
                ParticipantTypeId = personParticipantType.ParticipantTypeId,
                ParticipantType = personParticipantType

            };
            var recipientPerson = new Person
            {
                PersonId = recipientId + 1,
                FirstName = "first2",
                LastName = "last2",
                GenderId = gender.GenderId,
                Gender = gender
            };
            recipientParticipant.Person = recipientPerson;
            recipientParticipant.PersonId = recipientPerson.PersonId;

            var moneyFlow = new MoneyFlow
            {
                SourceParticipantId = sourceId,
                RecipientParticipantId = recipientId,
                SourceParticipant = sourceParticipant,
                RecipientParticipant = recipientParticipant,
                SourceType = participantType,
                SourceTypeId = participantType.MoneyFlowSourceRecipientTypeId,
                RecipientType = participantType,
                RecipientTypeId = participantType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.ParticipantTypes.Add(personParticipantType);
            context.Genders.Add(gender);
            context.MoneyFlows.Add(moneyFlow);
            context.People.Add(sourcePerson);
            context.People.Add(recipientPerson);
            context.Participants.Add(sourceParticipant);
            context.Participants.Add(recipientParticipant);
            var results = MoneyFlowQueries.CreateIncomingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();

            Assert.AreEqual(recipientId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Participant.Id, dto.EntityTypeId);

            Assert.AreEqual(sourcePerson.FirstName + " " + sourcePerson.LastName, dto.SourceRecipientName);
            Assert.AreEqual(sourceId, dto.SourceRecipientEntityId);
            Assert.AreEqual(participantType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(participantType.TypeName, dto.SourceRecipientTypeName);
            Assert.AreEqual(personParticipantType.ParticipantTypeId, dto.ParticipantTypeId);
            Assert.AreEqual(personParticipantType.Name, dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateIncomingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_Program()
        {
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

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Programs.Add(sourceProgram);
            context.Programs.Add(recipientProgram);
            var results = MoneyFlowQueries.CreateIncomingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(recipientId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Program.Id, dto.EntityTypeId);

            Assert.AreEqual(sourceProgram.Name, dto.SourceRecipientName);
            Assert.AreEqual(sourceId, dto.SourceRecipientEntityId);
            Assert.AreEqual(programType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(programType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }

        [TestMethod]
        public void TestCreateIncomingMoneyFlowDTOsQuery_CheckSourceRecipientProperties_Project()
        {
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

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Projects.Add(recipientProject);
            var results = MoneyFlowQueries.CreateIncomingMoneyFlowDTOsQuery(context).ToList();
            Assert.AreEqual(1, results.Count);
            var dto = results.First();
            Assert.AreEqual(recipientId, dto.EntityId);
            Assert.AreEqual(MoneyFlowSourceRecipientType.Project.Id, dto.EntityTypeId);

            Assert.AreEqual(sourceProject.Name, dto.SourceRecipientName);
            Assert.AreEqual(sourceId, dto.SourceRecipientEntityId);
            Assert.AreEqual(projectType.MoneyFlowSourceRecipientTypeId, dto.SourceRecipientEntityTypeId);
            Assert.AreEqual(projectType.TypeName, dto.SourceRecipientTypeName);
            Assert.IsNull(dto.ParticipantTypeId);
            Assert.IsNull(dto.ParticipantTypeName);
        }
        #endregion

        [TestMethod]
        public void TestCreateGetMoneyFlowDTOsQuery_CheckUnion()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Program"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);

            var results = MoneyFlowQueries.CreateGetMoneyFlowDTOsQuery(context);
            Assert.AreEqual(2, results.Count());
            var projectResult = results.Where(x => x.EntityId == sourceId).FirstOrDefault();
            var programResult = results.Where(x => x.EntityId == recipientId).FirstOrDefault();
            Assert.IsNotNull(projectResult);
            Assert.IsNotNull(programResult);

            Assert.AreEqual(projectResult.SourceRecipientEntityTypeId, programType.MoneyFlowSourceRecipientTypeId);
            Assert.AreEqual(programResult.SourceRecipientEntityTypeId, projectType.MoneyFlowSourceRecipientTypeId);

            Assert.AreEqual(projectResult.MoneyFlowType, outgoing.MoneyFlowTypeName);
            Assert.AreEqual(programResult.MoneyFlowType, incoming.MoneyFlowTypeName);
        }

        [TestMethod]
        public void TestCreateGetMoneyFlowsDTOsByEntityType()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Program"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);

            var results = MoneyFlowQueries.CreateGetMoneyFlowsDTOsByEntityType(context, sourceProject.ProjectId, MoneyFlowSourceRecipientType.Project.Id);
            Assert.AreEqual(1, results.Count());
            var projectResult = results.Where(x => x.EntityId == sourceId).FirstOrDefault();
            Assert.IsNotNull(projectResult);
        }

        [TestMethod]
        public void TestCreateGetMoneyFlowsDTOsByEntityType_EntityTypeDoesNotExist()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Program"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);

            var results = MoneyFlowQueries.CreateGetMoneyFlowsDTOsByEntityType(context, sourceProject.ProjectId, MoneyFlowSourceRecipientType.Post.Id);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetMoneyFlowsDTOsByEntityType_EntityIdDoesNotExist()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Program"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);

            var results = MoneyFlowQueries.CreateGetMoneyFlowsDTOsByEntityType(context, -1, MoneyFlowSourceRecipientType.Project.Id);
            Assert.AreEqual(0, results.Count());
        }

        #region CreateGetMoneyFlowDTOsByProjectId
        [TestMethod]
        public void TestCreateGetMoneyFlowsDTOsByProjectId_Filter()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Program"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x=> x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<MoneyFlowDTO>(x => x.Id, ComparisonType.Equal, -1));


            var results = MoneyFlowQueries.CreateGetMoneyFlowDTOsByProjectId(context, sourceProject.ProjectId, queryOperator);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetMoneyFlowsDTOsByProjectId_Sorting()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Program"
            };

            var moneyFlow1 = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };


            var moneyFlow2 = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 11
            };
            context.MoneyFlows.Add(moneyFlow1);
            context.MoneyFlows.Add(moneyFlow2);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);

            var results = MoneyFlowQueries.CreateGetMoneyFlowDTOsByProjectId(context, sourceProject.ProjectId, queryOperator);
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(moneyFlow1.MoneyFlowId, results.First().Id);
            Assert.AreEqual(moneyFlow2.MoneyFlowId, results.Last().Id);
        }

        #endregion

        #region CreateGetMoneyFlowDTOsByProgramId
        [TestMethod]
        public void TestCreateGetMoneyFlowsDTOsByProgramId_Filter()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Program"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<MoneyFlowDTO>(x => x.Id, ComparisonType.Equal, -1));


            var results = MoneyFlowQueries.CreateGetMoneyFlowDTOsByProgramId(context, recipientProgram.ProgramId, queryOperator);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestCreateGetMoneyFlowsDTOsByProgramId_Sorting()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Program"
            };

            var moneyFlow1 = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };


            var moneyFlow2 = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 11
            };
            context.MoneyFlows.Add(moneyFlow1);
            context.MoneyFlows.Add(moneyFlow2);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);

            var results = MoneyFlowQueries.CreateGetMoneyFlowDTOsByProgramId(context, recipientProgram.ProgramId, queryOperator);
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(moneyFlow1.MoneyFlowId, results.First().Id);
            Assert.AreEqual(moneyFlow2.MoneyFlowId, results.Last().Id);
        }

        #endregion

    }
}
