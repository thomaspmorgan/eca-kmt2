using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Sevis.Model;
using ECA.Business.Service;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Sevis
{
    public partial class SevisBatchProcessingServiceTest
    {
        [TestMethod]
        public async Task TestProcessTransactionLog_TransactionLogHasNewParticipantAndNewDependent()
        {
            var expectedParticipantSevisId = "N0000158857";
            var expectedDependentSevisId = "N0000158274";
            var transactionXml = ECA.Business.Test.Properties.Resources.TransactionLogWithNewParticipantAndNewDependent;
            var batchXml = ECA.Business.Test.Properties.Resources.NewParticipantWithNewDependentBatchXml;
            var user = new User(1);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var otherUser = new User(user.Id + 1);
            var batchId = "----kynEn47azQ";
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            PersonDependent personDependent = null;
            Data.Person person = null;
            SevisBatchProcessing batch = null;
            ExchangeVisitorHistory history = null;
            var personId = 63280;
            var participantId = 59079;
            context.SetupActions.Add(() =>
            {
                batch = new SevisBatchProcessing
                {
                    BatchId = batchId,
                    Id = 1,
                    SendString = batchXml
                };
                participant = new Participant
                {
                    ParticipantId = participantId
                };
                participantPerson = new ParticipantPerson
                {
                    ParticipantId = participant.ParticipantId,
                    Participant = participant,
                    SevisBatchResult = "sevis batch result",
                };
                participantPerson.History.CreatedBy = otherUser.Id;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedBy = otherUser.Id;
                participantPerson.History.RevisedOn = yesterday;

                participant.ParticipantPerson = participantPerson;
                person = new Data.Person
                {
                    PersonId = personId
                };
                participant.Person = person;
                participant.PersonId = person.PersonId;

                personDependent = new PersonDependent
                {
                    DependentId = 6,
                    Person = person,
                    PersonId = person.PersonId
                };
                person.Family.Add(personDependent);
                history = new ExchangeVisitorHistory
                {
                    ParticipantId = participantId
                };
                context.ExchangeVisitorHistories.Add(history);
                context.PersonDependents.Add(personDependent);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                context.SevisBatchProcessings.Add(batch);
            });

            Action tester = () =>
            {
                Assert.AreEqual(expectedParticipantSevisId, participantPerson.SevisId);
                Assert.AreEqual(expectedDependentSevisId, personDependent.SevisId);
            };
            context.Revert();
            service.ProcessTransactionLog(user, batchId, transactionXml, fileProvider.Object);
            tester();

            context.Revert();
            await service.ProcessTransactionLogAsync(user, batchId, transactionXml, fileProvider.Object);
            tester();
        }
    }
}
