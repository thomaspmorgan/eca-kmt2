using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using ECA.Data;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantPersonServiceValidatorTest
    {
        private ParticipantPersonServiceValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new ParticipantPersonServiceValidator();
        }

        [TestMethod]
        public void TestDoValidateCreate()
        {
            Func<object> createEntity = () =>
            {
                return new object();
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());
        }

        [TestMethod]
        public void TestDoValidateUpdate_ParticipantTypeIsNotPerson()
        {
            var participantType = new ParticipantType
            {
                IsPerson = true
            };

            Func<UpdatedParticipantPersonValidationEntity> createEntity = () =>
            {
                return new UpdatedParticipantPersonValidationEntity(
                    participantType: participantType
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            participantType.IsPerson = false;
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ParticipantPersonServiceValidator.PARTICIPANT_TYPE_IS_NOT_A_PERSON_PARTICIPANT_TYPE_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("ParticipantTypeId", validationErrors.First().Property);
        }
    }
}
