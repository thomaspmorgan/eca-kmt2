using System.Linq;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;

namespace ECA.Business.Test.Queries.Persons
{
    [TestClass]
    public class ParticipantExchangeVisitorQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetParticipantExchangeVisitorsDTOQuery_CheckProperties()
        {
            var project = new Project
            {
                ProjectId = 100
            };
            var participant = new Participant
            {
                ParticipantId = 2,
                ProjectId = project.ProjectId,
                Project = project
            };
            var fieldOfStudy = new FieldOfStudy
            {
                FieldOfStudyId = 3,
                Description = "field of study",
            };
            var programCategory = new ProgramCategory
            {
                ProgramCategoryId = 3,
                Description = "program category",
                ProgramCategoryCode = "pccode"
            };
            var position = new Position
            {
                PositionId = 4,
                Description = "position",
                PositionCode = "positioncode"
            };
            var usGovAgency1 = new USGovernmentAgency
            {
                AgencyCode = "usgovcode1",
                AgencyId = 5,
                Description = "us gov agency1",
            };
            var usGovAgency2 = new USGovernmentAgency
            {
                AgencyCode = "usgovcode2",
                AgencyId = 6,
                Description = "us gov agency2",
            };

            var internationalOrg1 = new InternationalOrganization
            {
                Description = "int org 1",
                OrganizationCode = "int org 1 code",
                OrganizationId = 7,
            };
            var internationalOrg2 = new InternationalOrganization
            {
                Description = "int org 2",
                OrganizationCode = "int org 2 code",
                OrganizationId = 8,
            };
            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                FieldOfStudy = fieldOfStudy,
                FieldOfStudyId = fieldOfStudy.FieldOfStudyId,
                FundingGovtAgency1 = 2.0m,
                FundingGovtAgency2 = 3.0m,
                FundingIntlOrg1 = 4.0m,
                FundingIntlOrg2 = 5.0m,
                FundingOther = 6.0m,
                FundingPersonal = 7.0m,
                FundingSponsor = 8.0m,
                FundingTotal = 9.0m,
                FundingVisBNC = 10.0m,
                FundingVisGovt = 11.0m,
                GovtAgency1 = usGovAgency1,
                GovtAgency1Id = usGovAgency1.AgencyId,
                GovtAgency1OtherName = "other name",
                GovtAgency2 = usGovAgency2,
                GovtAgency2Id = usGovAgency2.AgencyId,
                GovtAgency2OtherName = "other other name",
                IntlOrg1 = internationalOrg1,
                IntlOrg1Id = internationalOrg1.OrganizationId,
                IntlOrg1OtherName = "other other other name",
                IntlOrg2 = internationalOrg2,
                IntlOrg2Id = internationalOrg2.OrganizationId,
                IntlOrg2OtherName = "other name 2",
                OtherName = "other name again",
                ProgramCategory = programCategory,
                ProgramCategoryId = programCategory.ProgramCategoryId,
                Position = position,
                PositionId = position.PositionId
            };
            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.FieldOfStudies.Add(fieldOfStudy);
            context.ProgramCategories.Add(programCategory);
            context.Positions.Add(position);
            context.ParticipantExchangeVisitors.Add(visitor);
            context.USGovernmentAgencies.Add(usGovAgency1);
            context.USGovernmentAgencies.Add(usGovAgency2);
            context.InternationalOrganizations.Add(internationalOrg1);
            context.InternationalOrganizations.Add(internationalOrg2);

            Action<ParticipantExchangeVisitorDTO> tester = (result) =>
            {
                Assert.AreEqual(project.ProjectId, result.ProjectId);
                Assert.AreEqual(participant.ParticipantId, result.ParticipantId);
                Assert.AreEqual(visitor.FundingGovtAgency1, result.FundingGovtAgency1);
                Assert.AreEqual(visitor.FundingGovtAgency2, result.FundingGovtAgency2);
                Assert.AreEqual(visitor.FundingIntlOrg1, result.FundingIntlOrg1);
                Assert.AreEqual(visitor.FundingIntlOrg2, result.FundingIntlOrg2);
                Assert.AreEqual(visitor.FundingOther, result.FundingOther);
                Assert.AreEqual(visitor.FundingPersonal, result.FundingPersonal);
                Assert.AreEqual(visitor.FundingSponsor, result.FundingSponsor);
                Assert.AreEqual(visitor.FundingTotal, result.FundingTotal);
                Assert.AreEqual(visitor.FundingVisBNC, result.FundingVisBNC);
                Assert.AreEqual(visitor.FundingVisGovt, result.FundingVisGovt);                
                Assert.AreEqual(visitor.GovtAgency1OtherName, result.GovtAgency1OtherName);                
                Assert.AreEqual(visitor.GovtAgency2OtherName, result.GovtAgency2OtherName);                
                Assert.AreEqual(visitor.IntlOrg1OtherName, result.IntlOrg1OtherName);                
                Assert.AreEqual(visitor.IntlOrg2OtherName, result.IntlOrg2OtherName);
                Assert.AreEqual(visitor.OtherName, result.OtherName);

                Assert.AreEqual(visitor.FieldOfStudyId, result.FieldOfStudyId);
                Assert.AreEqual(fieldOfStudy.Description, result.FieldOfStudy);

                Assert.AreEqual(visitor.GovtAgency1Id, result.GovtAgency1Id);
                Assert.AreEqual(usGovAgency1.Description, result.GovtAgency1Name);

                Assert.AreEqual(visitor.GovtAgency2Id, result.GovtAgency2Id);
                Assert.AreEqual(usGovAgency2.Description, result.GovtAgency2Name);

                Assert.AreEqual(visitor.ProgramCategoryId, result.ProgramCategoryId);
                Assert.AreEqual(programCategory.Description, result.ProgramCategory);
                Assert.AreEqual(programCategory.ProgramCategoryCode, result.ProgramCategoryCode);

                Assert.AreEqual(visitor.IntlOrg1Id, result.IntlOrg1Id);
                Assert.AreEqual(internationalOrg1.Description, result.IntlOrg1Name);

                Assert.AreEqual(visitor.IntlOrg2Id, result.IntlOrg2Id);
                Assert.AreEqual(internationalOrg2.Description, result.IntlOrg2Name);                

                Assert.AreEqual(visitor.PositionId, result.PositionId);
                Assert.AreEqual(visitor.Position.Description, result.Position);
                Assert.AreEqual(position.PositionCode, result.PositionCode);
            };

            var queryResult = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorsDTOQuery(context).ToList();
            Assert.AreEqual(1, queryResult.Count());
            tester(queryResult.First());
        }

        [TestMethod]
        public void TestCreateGetParticipantExchangeVisitorsDTOQuery_AllValuesNull()
        {
            var project = new Project
            {
                ProjectId = 100
            };
            var participant = new Participant
            {
                ParticipantId = 2,
                ProjectId = project.ProjectId,
                Project = project
            };
            
            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(visitor);
            Action<ParticipantExchangeVisitorDTO> tester = (result) =>
            {
                Assert.AreEqual(project.ProjectId, result.ProjectId);
                Assert.AreEqual(participant.ParticipantId, result.ParticipantId);
                Assert.AreEqual(0, result.FundingGovtAgency1);
                Assert.AreEqual(0, result.FundingGovtAgency2);
                Assert.AreEqual(0, result.FundingIntlOrg1);
                Assert.AreEqual(0, result.FundingIntlOrg2);
                Assert.AreEqual(0, result.FundingOther);
                Assert.AreEqual(0, result.FundingPersonal);
                Assert.AreEqual(0, result.FundingSponsor);
                Assert.AreEqual(0, result.FundingTotal);
                Assert.AreEqual(0, result.FundingVisBNC);
                Assert.AreEqual(0, result.FundingVisGovt);
                Assert.AreEqual(string.Empty, result.GovtAgency1OtherName);
                Assert.AreEqual(string.Empty, result.GovtAgency2OtherName);
                Assert.AreEqual(string.Empty, result.IntlOrg1OtherName);
                Assert.AreEqual(string.Empty, result.IntlOrg2OtherName);
                Assert.AreEqual(string.Empty, result.OtherName);

                Assert.AreEqual(visitor.FieldOfStudyId, result.FieldOfStudyId);

                Assert.AreEqual(0, result.GovtAgency1Id);
                Assert.AreEqual(string.Empty, result.GovtAgency1Name);

                Assert.AreEqual(0, result.GovtAgency2Id);
                Assert.AreEqual(string.Empty, result.GovtAgency2Name);

                Assert.IsFalse(result.ProgramCategoryId.HasValue);
                Assert.AreEqual(string.Empty, result.ProgramCategory);
                Assert.AreEqual(string.Empty, result.ProgramCategoryCode);

                Assert.AreEqual(0, result.IntlOrg1Id);
                Assert.AreEqual(string.Empty, result.IntlOrg1Name);

                Assert.AreEqual(0, result.IntlOrg2Id);
                Assert.AreEqual(string.Empty, result.IntlOrg2Name);

                Assert.IsFalse(result.PositionId.HasValue);
                Assert.AreEqual(string.Empty, result.Position);
                Assert.AreEqual(string.Empty, result.PositionCode);
            };
            var queryResult = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorsDTOQuery(context).ToList();
            Assert.AreEqual(1, queryResult.Count());
            tester(queryResult.First());
        }

        [TestMethod]
        public void TestCreateGetParticipantExchangeVisitorDTOByIdQuery()
        {
            var project = new Project
            {
                ProjectId = 100
            };
            var participant = new Participant
            {
                ParticipantId = 2,
                ProjectId = project.ProjectId,
                Project = project
            };

            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(visitor);
            Action<ParticipantExchangeVisitorDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
                Assert.AreEqual(project.ProjectId, result.ProjectId);
                Assert.AreEqual(participant.ParticipantId, result.ParticipantId);
            };
            var queryResult = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(context, project.ProjectId, participant.ParticipantId).ToList();
            Assert.AreEqual(1, queryResult.Count());
            tester(queryResult.First());
        }

        [TestMethod]
        public void TestCreateGetParticipantExchangeVisitorDTOByIdQuery_InvalidParticipantId()
        {
            var project = new Project
            {
                ProjectId = 100
            };
            var participant = new Participant
            {
                ParticipantId = 2,
                ProjectId = project.ProjectId,
                Project = project
            };

            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(visitor);
            var queryResult = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(context, project.ProjectId, participant.ParticipantId).ToList();
            Assert.AreEqual(1, queryResult.Count());

            queryResult = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(context, project.ProjectId, participant.ParticipantId + 1).ToList();
            Assert.AreEqual(0, queryResult.Count());
        }

        [TestMethod]
        public void TestCreateGetParticipantExchangeVisitorDTOByIdQuery_InvalidProjectId()
        {
            var project = new Project
            {
                ProjectId = 100
            };
            var participant = new Participant
            {
                ParticipantId = 2,
                ProjectId = project.ProjectId,
                Project = project
            };

            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(visitor);
            var queryResult = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(context, project.ProjectId, participant.ParticipantId).ToList();
            Assert.AreEqual(1, queryResult.Count());

            queryResult = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(context, project.ProjectId + 1, participant.ParticipantId).ToList();
            Assert.AreEqual(0, queryResult.Count());
        }
    }
}
