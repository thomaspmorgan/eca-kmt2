using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using ECA.Business.Service;
using ECA.Data;
using System.Collections.Generic;
using ECA.Core.DynamicLinq;
using ECA.Business.Queries.Models.Itineraries;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class EcaItineraryGroupValidatorTest
    {
        private EcaItineraryGroupValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new EcaItineraryGroupValidator();
        }


        #region Create
        [TestMethod]
        public void TestDoValidateCreate_NoParticipants()
        {
            var participantType = new ParticipantType
            {
                IsPerson = true,
                ParticipantTypeId = 1,
                Name = "type"
            };
            var participants = new List<Participant>
            {
                new Participant
                {
                    ParticipantId = 1,
                    ParticipantType = participantType,
                    ParticipantTypeId = participantType.ParticipantTypeId
                }
            };
            AddedEcaItineraryGroupValidationEntity model = null;
            var existingItineraries = new List<ItineraryGroupDTO>();
            Func<AddedEcaItineraryGroupValidationEntity> createEntity = () =>
            {

                model = new AddedEcaItineraryGroupValidationEntity(participants, existingItineraries);
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());

            participants.Clear();

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateCreate_ParticipantsAreNotPeople()
        {
            var participantType = new ParticipantType
            {
                IsPerson = true,
                ParticipantTypeId= 1,
                Name = "type"
            };
            var participants = new List<Participant>
            {
                new Participant
                {
                    ParticipantId = 1,
                    ParticipantType = participantType,
                    ParticipantTypeId = participantType.ParticipantTypeId
                }
            };
            var existingItineraries = new List<ItineraryGroupDTO>();
            AddedEcaItineraryGroupValidationEntity model = null;
            Func<AddedEcaItineraryGroupValidationEntity> createEntity = () =>
            {

                model = new AddedEcaItineraryGroupValidationEntity(participants, existingItineraries);
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());

            participantType.IsPerson = false;

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(EcaItineraryGroupValidator.ALL_PARTICIPANTS_MUST_BE_A_PERSON_PARTICIPANT_TYPE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<AddedEcaItineraryGroup>(x => x.ParticipantIds), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_ItineraryGroupsAlreadyExist()
        {
            var participantType = new ParticipantType
            {
                IsPerson = true,
                ParticipantTypeId = 1,
                Name = "type"
            };
            var participants = new List<Participant>
            {
                new Participant
                {
                    ParticipantId = 1,
                    ParticipantType = participantType,
                    ParticipantTypeId = participantType.ParticipantTypeId
                }
            };
            List<ItineraryGroupDTO> existingItineraries = null;
            AddedEcaItineraryGroupValidationEntity model = null;
            Func<AddedEcaItineraryGroupValidationEntity> createEntity = () =>
            {

                model = new AddedEcaItineraryGroupValidationEntity(participants, existingItineraries);
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());

            existingItineraries = new List<ItineraryGroupDTO>();
            existingItineraries.Add(new ItineraryGroupDTO
            {
                
            });

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(EcaItineraryGroupValidator.ITINERARY_GROUP_ALREADY_EXISTS, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<AddedEcaItineraryGroup>(x => x.Name), validationErrors.First().Property);
        }

        #endregion
    }
}
