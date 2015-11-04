using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using System.Threading.Tasks;
using ECA.Data;
using System.Linq;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantPersonSevisServiceTest
    {
        private TestEcaContext context;
        private ParticipantPersonSevisService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ParticipantPersonSevisService(context);
        }

        [TestMethod]
        public async Task TestSendToSevis()
        {
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id,
                AddedOn = DateTimeOffset.Now
            };

            var status2 = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                AddedOn = DateTimeOffset.Now
            };

            context.ParticipantPersonSevisCommStatuses.Add(status);
            context.ParticipantPersonSevisCommStatuses.Add(status2);

            var response = await service.SendToSevis(new int[] { status.ParticipantId });

            Assert.AreEqual(1, response.Length);
            Assert.AreEqual(status.ParticipantId, response[0]);

            var newStatus = context.ParticipantPersonSevisCommStatuses.Where(p => p.ParticipantId == status.ParticipantId)
                .OrderByDescending(o => o.AddedOn)
                .FirstOrDefault();

            Assert.AreEqual(SevisCommStatus.QueuedToSubmit.Id, newStatus.SevisCommStatusId);
        }

        [TestMethod]
        public async Task TestSendToSevis_EmptyArray()
        {
            var response = await service.SendToSevis(new int[] {});
            Assert.AreEqual(0, response.Length);
        }

        [TestMethod]
        public async Task TestSendToSevis_Null()
        {
            var response = await service.SendToSevis(null);
            Assert.AreEqual(0, response.Length);
        }

        [TestMethod]
        public async Task TestSendToSevis_IncorrectStatus()
        {
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id,
                AddedOn = DateTimeOffset.Now
            };

            context.ParticipantPersonSevisCommStatuses.Add(status);

            var response = await service.SendToSevis(new int[] { status.ParticipantId });

            Assert.AreEqual(0, response.Length);
        }
    }
}
