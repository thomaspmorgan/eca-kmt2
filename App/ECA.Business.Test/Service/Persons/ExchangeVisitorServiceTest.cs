using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Service;
using ECA.Business.Service.Persons;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ExchangeVisitorServiceTest
    {
        private TestEcaContext context;
        private ExchangeVisitorService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ExchangeVisitorService(context);
        }

        [TestMethod]
        public void TestGetCreateExchangeVisitor_CheckProperties()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate
            };
            var position = new Position
            {
                PositionId = 30,
                PositionCode = "posCode"
            };
            var category = new ProgramCategory
            {
                ProgramCategoryId = 20,
                ProgramCategoryCode = "catCode"
            };
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                Position = position,
                PositionId = position.PositionId,
                ProgramCategory = category,
                ProgramCategoryId = category.ProgramCategoryId
            };

            var instance = service.GetCreateExchangeVisitor(participant, user, project, visitor);
            Assert.AreEqual(participant.ParticipantId.ToString(), instance.requestID);
            Assert.AreEqual(user.Id.ToString(), instance.userID);
            Assert.AreEqual(project.StartDate.UtcDateTime, instance.PrgStartDate);
            Assert.AreEqual(project.EndDate.Value.UtcDateTime, instance.PrgEndDate.Value);
            Assert.AreEqual(ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE, instance.OccupationCategoryCode);
            Assert.AreEqual(position.PositionCode, instance.PositionCode);
            Assert.AreEqual(category.ProgramCategoryCode, instance.CategoryCode);
        }

        [TestMethod]
        public void TestGetCreateExchangeVisitor_ProjectEndDateAndPositionAndCategoryAreNull()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
            };
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
            };
            var instance = service.GetCreateExchangeVisitor(participant, user, project, visitor);
            Assert.IsNull(instance.PositionCode);
            Assert.IsNull(instance.CategoryCode);
            Assert.IsFalse(instance.PrgEndDate.HasValue);
        }
    }
}
