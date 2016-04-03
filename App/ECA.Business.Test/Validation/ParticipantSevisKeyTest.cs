using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Test.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Bio;
using FluentAssertions;
using ECA.Business.Sevis.Model.TransLog;

namespace ECA.Business.Test.Validation
{
    [TestClass]
    public class ParticipantSevisKeyTest
    {
        [TestMethod]
        public void TestConstructor_Dependent()
        {
            var testDependent = new TestDependent();
            Assert.AreEqual(10, testDependent.PersonId);
            Assert.AreEqual(20, testDependent.ParticipantId);

            var key = new ParticipantSevisKey(testDependent);
            Assert.AreEqual(testDependent.PersonId, key.PersonId);
            Assert.AreEqual(testDependent.ParticipantId, key.ParticipantId);
        }

        [TestMethod]
        public void TestConstructor_Person()
        {
            var participantId = 10;
            var personId = 20;
            var testPerson = new Person(
                fullName: null,
                birthCity: null,
                birthCountryCode: null,
                birthDate: null,
                citizenshipCountryCode: null,
                emailAddress: null,
                gender: null,
                permanentResidenceCountryCode: null,
                phoneNumber: null,
                remarks: null,
                positionCode: null,
                programCategoryCode: null,
                subjectField: null,
                mailAddress: null,
                usAddress: null,
                printForm: true,
                personId: personId,
                participantId: participantId);

            var key = new ParticipantSevisKey(testPerson);
            Assert.AreEqual(testPerson.PersonId, key.PersonId);
            Assert.AreEqual(testPerson.ParticipantId, key.ParticipantId);
        }

        [TestMethod]
        public void TestConstructor_UserDefinedAAndUserDefinedB()
        {
            var participantId = 10;
            var personId = 20;

            var userDefinedA = participantId.ToString();
            var userDefinedB = "B" + personId.ToString();

            var key = new ParticipantSevisKey(userDefinedA, userDefinedB);
            Assert.AreEqual(personId, key.PersonId);
            Assert.AreEqual(participantId, key.ParticipantId);
        }

        [TestMethod]
        public void TestConstructor_TransactionLogTypeBatchDetailProcessRecordDependent()
        {
            var participantId = 10;
            var personId = 20;

            var userDefinedA = participantId.ToString();
            var userDefinedB = "B" + personId.ToString();

            var dependent = new TransactionLogTypeBatchDetailProcessRecordDependent
            {
                UserDefinedA = userDefinedA,
                UserDefinedB = userDefinedB
            };

            var key = new ParticipantSevisKey(dependent);
            Assert.AreEqual(personId, key.PersonId);
            Assert.AreEqual(participantId, key.ParticipantId);
        }

        [TestMethod]
        public void TestConstructor_TransactionLogTypeBatchDetailProcessRecord()
        {
            var participantId = 10;
            var personId = 20;

            var userDefinedA = participantId.ToString();
            var userDefinedB = "B" + personId.ToString();

            var dependent = new TransactionLogTypeBatchDetailProcessRecord
            {
                UserDefinedA = userDefinedA,
                UserDefinedB = userDefinedB
            };

            var key = new ParticipantSevisKey(dependent);
            Assert.AreEqual(personId, key.PersonId);
            Assert.AreEqual(participantId, key.ParticipantId);
        }

        [TestMethod]
        public void TestConstructor_InvalidUserDefinedAField()
        {
            var personId = 20;

            var userDefinedA = "hello world";
            var userDefinedB = "B" + personId.ToString();

            Action a = () => new ParticipantSevisKey(userDefinedA, userDefinedB);
            var message = "The user defined a field which represents the participant id is not valid.";
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestConstructor_EmptyUserDefinedAField()
        {
            var personId = 20;

            var userDefinedA = String.Empty;
            var userDefinedB = "B" + personId.ToString();

            Action a = () => new ParticipantSevisKey(userDefinedA, userDefinedB);
            var message = "The user defined a field which represents the participant id is not valid.";
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestConstructor_InvalidUserDefinedBField()
        {
            var participantId = 10;

            var userDefinedA = participantId.ToString();
            var userDefinedB = "hello world";

            Action a = () => new ParticipantSevisKey(userDefinedA, userDefinedB);
            var message = "The user defined b field which represents the person id is not valid.";
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestConstructor_EmptyUserDefinedBField()
        {
            var participantId = 10;

            var userDefinedA = participantId.ToString();
            var userDefinedB = String.Empty;

            Action a = () => new ParticipantSevisKey(userDefinedA, userDefinedB);
            var message = "The user defined b field which represents the person id is not valid.";
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestConstructor_UserDefinedBFieldExceedsMaxIdLength()
        {
            var participantId = 10;
            var personId = 99999999999999;

            var userDefinedA = participantId.ToString();
            var userDefinedB = "B" + personId.ToString();

            Action a = () => new ParticipantSevisKey(userDefinedA, userDefinedB);
            var message = "The user defined b field which represents the person id is not valid.";
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }
    }
}
