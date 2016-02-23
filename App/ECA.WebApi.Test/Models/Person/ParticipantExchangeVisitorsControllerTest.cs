using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ECA.Business.Service.Persons;
using ECA.WebApi.Security;
using ECA.WebApi.Controllers.Persons;

namespace ECA.WebApi.Test.Models.Person
{
    [TestClass]
    public class ParticipantExchangeVisitorsControllerTest
    {
        private Mock<IParticipantExchangeVisitorService> service;
        private Mock<IParticipantService> participantService;
        private Mock<IUserProvider> userProvider;
        private ParticipantExchangeVisitorsController controller;

        [TestInitialize]
        public void TestInit()
        {
            service = new Mock<IParticipantExchangeVisitorService>();
            participantService = new Mock<IParticipantService>();
            userProvider = new Mock<IUserProvider>();

            controller = new ParticipantExchangeVisitorsController(service.Object, participantService.Object, userProvider.Object);
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
