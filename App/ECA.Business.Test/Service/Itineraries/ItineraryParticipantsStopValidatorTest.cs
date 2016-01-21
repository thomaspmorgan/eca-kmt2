using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using ECA.Business.Service;
using ECA.Data;
using ECA.Core.DynamicLinq;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryParticipantsStopValidatorTest
    {
        private ItineraryParticipantsStopValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new ItineraryParticipantsStopValidator();
        }

        #region Create

        [TestMethod]
        public void TestDoValidateCreate_HasNotAllowedParticipantIds()
        {
            var notAllowedParticipantIds = new List<int>();
            Func<ItineraryStopParticipantsValidationEntity> createEntity = () =>
            {
                return new ItineraryStopParticipantsValidationEntity(notAllowedParticipantsByParticipantId: notAllowedParticipantIds);                
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());

            notAllowedParticipantIds.Add(1);

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryParticipantsStopValidator.ALL_PARTICIPANTS_OF_ITINERARY_STOP_MUST_PARTICIPANT_IN_ITINERARY_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<ItineraryStopParticipants>(x => x.ParticipantIds), validationErrors.First().Property);
        }
        #endregion

        #region Update
        [TestMethod]
        public void TestDoValidateUpdate_HasNotAllowedParticipantIds()
        {
            var notAllowedParticipantIds = new List<int>();
            Func<ItineraryStopParticipantsValidationEntity> createEntity = () =>
            {
                return new ItineraryStopParticipantsValidationEntity(notAllowedParticipantsByParticipantId: notAllowedParticipantIds);
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());

            notAllowedParticipantIds.Add(1);

            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryParticipantsStopValidator.ALL_PARTICIPANTS_OF_ITINERARY_STOP_MUST_PARTICIPANT_IN_ITINERARY_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<ItineraryStopParticipants>(x => x.ParticipantIds), validationErrors.First().Property);
        }
        #endregion
    }
}
