using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using ECA.Core.DynamicLinq;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryParticipantsValidatorTest
    {
        private ItineraryParticipantsValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new ItineraryParticipantsValidator();
        }

        #region DoValidateCreate

        [TestMethod]
        public void TestDoValidateCreate_HasOrphanedParticipantIds()
        {
            List<int> orphanedParticipantIds = new List<int>();
            List<int> nonPersonParticipantIds = new List<int>();
            Func<ItineraryParticipantsValidationEntity> createEntity = () =>
            {

                return new ItineraryParticipantsValidationEntity(
                    orphanedParticipantsByParticipantId: orphanedParticipantIds,
                    nonPersonParticipantsByParticipantIds: nonPersonParticipantIds
                    );
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            orphanedParticipantIds.Add(1);

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryParticipantsValidator.ITINERARY_STOP_PARTICIPANT_ORPHANED_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<ItineraryParticipants>(x => x.ParticipantIds), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_HasNonPersonParticipants()
        {
            List<int> orphanedParticipantIds = new List<int>();
            List<int> nonPersonParticipantIds = new List<int>();
            Func<ItineraryParticipantsValidationEntity> createEntity = () =>
            {

                return new ItineraryParticipantsValidationEntity(
                    orphanedParticipantsByParticipantId: orphanedParticipantIds,
                    nonPersonParticipantsByParticipantIds: nonPersonParticipantIds
                    );
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            nonPersonParticipantIds.Add(1);

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryParticipantsValidator.PARTICIPANTS_MUST_BE_PEOPLE_PARTICIPANT_TYPES_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<ItineraryParticipants>(x => x.ParticipantIds), validationErrors.First().Property);
        }

        #endregion

        #region DoValidateUpdate

        [TestMethod]
        public void TestDoValidateUpdate_HasOrphanedParticipantIds()
        {
            List<int> orphanedParticipantIds = new List<int>();
            List<int> nonPersonParticipantIds = new List<int>();
            Func<ItineraryParticipantsValidationEntity> createEntity = () =>
            {

                return new ItineraryParticipantsValidationEntity(
                    orphanedParticipantsByParticipantId: orphanedParticipantIds,
                    nonPersonParticipantsByParticipantIds: nonPersonParticipantIds
                    );
            };
            Assert.AreEqual(0, validator.DoValidateUpdate(createEntity()).Count());
            orphanedParticipantIds.Add(1);

            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryParticipantsValidator.ITINERARY_STOP_PARTICIPANT_ORPHANED_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<ItineraryParticipants>(x => x.ParticipantIds), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_HasNonPersonParticipants()
        {
            List<int> orphanedParticipantIds = new List<int>();
            List<int> nonPersonParticipantIds = new List<int>();
            Func<ItineraryParticipantsValidationEntity> createEntity = () =>
            {

                return new ItineraryParticipantsValidationEntity(
                    orphanedParticipantsByParticipantId: orphanedParticipantIds,
                    nonPersonParticipantsByParticipantIds: nonPersonParticipantIds
                    );
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            nonPersonParticipantIds.Add(1);

            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryParticipantsValidator.PARTICIPANTS_MUST_BE_PEOPLE_PARTICIPANT_TYPES_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<ItineraryParticipants>(x => x.ParticipantIds), validationErrors.First().Property);
        }

        #endregion
    }
}
