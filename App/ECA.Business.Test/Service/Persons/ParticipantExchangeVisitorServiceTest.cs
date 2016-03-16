using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using ECA.Business.Service;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.Exceptions;
using ECA.Business.Queries.Models.Persons;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantExchangeVisitorServiceTest
    {
        private TestEcaContext context;
        private ParticipantExchangeVisitorService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ParticipantExchangeVisitorService(context);
        }

        #region Get
        [TestMethod]
        public async Task TestGetParticipantExchangeVisitorById()
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
            var dto = service.GetParticipantExchangeVisitorById(project.ProjectId, participant.ParticipantId);
            var dtoAsync = await service.GetParticipantExchangeVisitorByIdAsync(project.ProjectId, participant.ParticipantId);
            tester(dto);
            tester(dtoAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantExchangeVisitorById_ParticipantDoesNotBelongToProject()
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
            var dto = service.GetParticipantExchangeVisitorById(project.ProjectId, participant.ParticipantId);
            var dtoAsync = await service.GetParticipantExchangeVisitorByIdAsync(project.ProjectId, participant.ParticipantId);
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dtoAsync);

            dto = service.GetParticipantExchangeVisitorById(project.ProjectId + 1, participant.ParticipantId);
            dtoAsync = await service.GetParticipantExchangeVisitorByIdAsync(project.ProjectId + 1, participant.ParticipantId);
            Assert.IsNull(dto);
            Assert.IsNull(dtoAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantExchangeVisitorById_ParticipantDoesNotExist()
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
            var dto = service.GetParticipantExchangeVisitorById(project.ProjectId, participant.ParticipantId);
            var dtoAsync = await service.GetParticipantExchangeVisitorByIdAsync(project.ProjectId, participant.ParticipantId);
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dtoAsync);

            dto = service.GetParticipantExchangeVisitorById(project.ProjectId, participant.ParticipantId + 1);
            dtoAsync = await service.GetParticipantExchangeVisitorByIdAsync(project.ProjectId, participant.ParticipantId + 1);
            Assert.IsNull(dto);
            Assert.IsNull(dtoAsync);
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdate_ParticipantDoesNotBelongToProject()
        {
            var user = new User(1);
            int projectId = 1;
            int participantId = 2;
            int? fieldOfStudyId = null;
            int? positionId = null;
            int? programCategoryId = null;
            decimal? fundingSponsor = null;
            decimal? fundingPersonal = null;
            decimal? fundingVisGovt = null;
            decimal? fundingVisBNC = null;
            decimal? fundingGovtAgency1 = null;
            int? govtAgency1Id = null;
            string govtAgency1OtherName = null;
            decimal? fundingGovtAgency2 = null;
            int? govtAgency2Id = null;
            string govtAgency2OtherName = null;
            decimal? fundingIntlOrg1 = null;
            int? intlOrg1Id = null;
            string intlOrg1OtherName = null;
            decimal? fundingIntlOrg2 = null;
            int? intlOrg2Id = null;
            string intlOrg2OtherName = null;
            decimal? fundingOther = null;
            string otherName = null;
            decimal? fundingTotal = null;

            Participant participant = new Participant
            {
                ParticipantId = participantId
            };
            ParticipantExchangeVisitor visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                Participant = participant
            };
            Project otherProject = new Project
            {
                ProjectId = projectId + 1
            };
            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(visitor);
            context.Projects.Add(otherProject);
            var model = new UpdatedParticipantExchangeVisitor(
                updater: user,
                projectId: projectId,
                participantId: participantId,
                fieldOfStudyId: fieldOfStudyId,
                positionId: positionId,
                programCategoryId: programCategoryId,
                fundingSponsor: fundingSponsor,
                fundingPersonal: fundingPersonal,
                fundingVisGovt: fundingVisGovt,
                fundingVisBNC: fundingVisBNC,
                fundingGovtAgency1: fundingGovtAgency1,
                govtAgency1Id: govtAgency1Id,
                govtAgency1OtherName: govtAgency1OtherName,
                fundingGovtAgency2: fundingGovtAgency2,
                govtAgency2Id: govtAgency2Id,
                govtAgency2OtherName: govtAgency2OtherName,
                fundingIntlOrg1: fundingIntlOrg1,
                intlOrg1Id: intlOrg1Id,
                intlOrg1OtherName: intlOrg1OtherName,
                fundingIntlOrg2: fundingIntlOrg2,
                intlOrg2Id: intlOrg2Id,
                intlOrg2OtherName: intlOrg2OtherName,
                fundingOther: fundingOther,
                otherName: otherName,
                fundingTotal: fundingTotal
                );

            var message = String.Format("The user with id [{0}] attempted to delete a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        user.Id,
                        participant.ParticipantId,
                        projectId);

            Action a = () => service.Update(model);
            Func<Task> f = async () => await service.UpdateAsync(model);
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        #endregion
    }
}
