using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using ECA.Core.DynamicLinq;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryStopValidatorTest
    {
        private ItineraryStopServiceValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new ItineraryStopServiceValidator();
        }

        #region Create
        [TestMethod]
        public void TestDoValidateCreate_ItineraryStopArrivalDateIsBeforeItineraryStartDate()
        {
            var timezoneId = "timezone";
            var itineraryStartDate = DateTimeOffset.UtcNow.AddDays(-100.0);
            var itineraryEndDate = DateTimeOffset.UtcNow.AddDays(100.0);
            var stopArrivalDate = DateTimeOffset.UtcNow.AddDays(-10.0);
            var stopDepartureDate = DateTimeOffset.UtcNow.AddDays(10.0);

            EcaItineraryStopValidationEntity model = null;
            Func<EcaItineraryStopValidationEntity> createEntity = () =>
            {

                model = new EcaItineraryStopValidationEntity(
                    itineraryStartDate: itineraryStartDate,
                    itineraryEndDate: itineraryEndDate,
                    itineraryStopArrivalDate: stopArrivalDate,
                    itineraryStopDepartureDate: stopDepartureDate,
                    timezoneId: timezoneId
                    );
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            stopArrivalDate = itineraryStartDate.AddDays(-1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryStopServiceValidator.ITINERARY_STOP_ARRIVAL_DATE_IS_NOT_WITHIN_ITINERARY_DATES, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<EcaItineraryStop>(x => x.ArrivalDate), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_ItineraryStopArrivalDateIsAfterItineraryEndDate()
        {
            var timezoneId = "timezone";
            var itineraryStartDate = DateTimeOffset.UtcNow.AddDays(-100.0);
            var itineraryEndDate = DateTimeOffset.UtcNow.AddDays(100.0);
            var stopArrivalDate = DateTimeOffset.UtcNow.AddDays(-10.0);
            var stopDepartureDate = DateTimeOffset.UtcNow.AddDays(10.0);

            EcaItineraryStopValidationEntity model = null;
            Func<EcaItineraryStopValidationEntity> createEntity = () =>
            {

                model = new EcaItineraryStopValidationEntity(
                    itineraryStartDate: itineraryStartDate,
                    itineraryEndDate: itineraryEndDate,
                    itineraryStopArrivalDate: stopArrivalDate,
                    itineraryStopDepartureDate: stopDepartureDate,
                    timezoneId: timezoneId
                    );
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            stopArrivalDate = itineraryEndDate.AddDays(1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryStopServiceValidator.ITINERARY_STOP_ARRIVAL_DATE_IS_NOT_WITHIN_ITINERARY_DATES, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<EcaItineraryStop>(x => x.ArrivalDate), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_ItineraryStopDepartureDateIsBeforeItineraryStartDate()
        {
            var timezoneId = "timezone";
            var itineraryStartDate = DateTimeOffset.UtcNow.AddDays(-100.0);
            var itineraryEndDate = DateTimeOffset.UtcNow.AddDays(100.0);
            var stopArrivalDate = DateTimeOffset.UtcNow.AddDays(-10.0);
            var stopDepartureDate = DateTimeOffset.UtcNow.AddDays(10.0);

            EcaItineraryStopValidationEntity model = null;
            Func<EcaItineraryStopValidationEntity> createEntity = () =>
            {

                model = new EcaItineraryStopValidationEntity(
                    itineraryStartDate: itineraryStartDate,
                    itineraryEndDate: itineraryEndDate,
                    itineraryStopArrivalDate: stopArrivalDate,
                    itineraryStopDepartureDate: stopDepartureDate,
                    timezoneId: timezoneId
                    );
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            stopDepartureDate = itineraryStartDate.AddDays(-1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryStopServiceValidator.ITINERARY_STOP_DEPARTURE_DATE_IS_NOT_WITHIN_ITINERARY_DATES, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<EcaItineraryStop>(x => x.DepartureDate), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_ItineraryStopDepartureDateIsAfterItineraryEndDate()
        {
            var timezoneId = "timezone";
            var itineraryStartDate = DateTimeOffset.UtcNow.AddDays(-100.0);
            var itineraryEndDate = DateTimeOffset.UtcNow.AddDays(100.0);
            var stopArrivalDate = DateTimeOffset.UtcNow.AddDays(-10.0);
            var stopDepartureDate = DateTimeOffset.UtcNow.AddDays(10.0);

            EcaItineraryStopValidationEntity model = null;
            Func<EcaItineraryStopValidationEntity> createEntity = () =>
            {

                model = new EcaItineraryStopValidationEntity(
                    itineraryStartDate: itineraryStartDate,
                    itineraryEndDate: itineraryEndDate,
                    itineraryStopArrivalDate: stopArrivalDate,
                    itineraryStopDepartureDate: stopDepartureDate,
                    timezoneId: timezoneId
                    );
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            stopDepartureDate = itineraryEndDate.AddDays(1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryStopServiceValidator.ITINERARY_STOP_DEPARTURE_DATE_IS_NOT_WITHIN_ITINERARY_DATES, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<EcaItineraryStop>(x => x.DepartureDate), validationErrors.First().Property);
        }

        #endregion

        #region Update
        [TestMethod]
        public void TestDoValidateUpdate_ItineraryStopArrivalDateIsBeforeItineraryStartDate()
        {
            var timezoneId = "timezone";
            var itineraryStartDate = DateTimeOffset.UtcNow.AddDays(-100.0);
            var itineraryEndDate = DateTimeOffset.UtcNow.AddDays(100.0);
            var stopArrivalDate = DateTimeOffset.UtcNow.AddDays(-10.0);
            var stopDepartureDate = DateTimeOffset.UtcNow.AddDays(10.0);

            EcaItineraryStopValidationEntity model = null;
            Func<EcaItineraryStopValidationEntity> createEntity = () =>
            {

                model = new EcaItineraryStopValidationEntity(
                    itineraryStartDate: itineraryStartDate,
                    itineraryEndDate: itineraryEndDate,
                    itineraryStopArrivalDate: stopArrivalDate,
                    itineraryStopDepartureDate: stopDepartureDate,
                    timezoneId: timezoneId
                    );
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            stopArrivalDate = itineraryStartDate.AddDays(-1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryStopServiceValidator.ITINERARY_STOP_ARRIVAL_DATE_IS_NOT_WITHIN_ITINERARY_DATES, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<EcaItineraryStop>(x => x.ArrivalDate), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ItineraryStopArrivalDateIsAfterItineraryEndDate()
        {
            var timezoneId = "timezone";
            var itineraryStartDate = DateTimeOffset.UtcNow.AddDays(-100.0);
            var itineraryEndDate = DateTimeOffset.UtcNow.AddDays(100.0);
            var stopArrivalDate = DateTimeOffset.UtcNow.AddDays(-10.0);
            var stopDepartureDate = DateTimeOffset.UtcNow.AddDays(10.0);

            EcaItineraryStopValidationEntity model = null;
            Func<EcaItineraryStopValidationEntity> createEntity = () =>
            {

                model = new EcaItineraryStopValidationEntity(
                    itineraryStartDate: itineraryStartDate,
                    itineraryEndDate: itineraryEndDate,
                    itineraryStopArrivalDate: stopArrivalDate,
                    itineraryStopDepartureDate: stopDepartureDate,
                    timezoneId: timezoneId
                    );
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            stopArrivalDate = itineraryEndDate.AddDays(1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryStopServiceValidator.ITINERARY_STOP_ARRIVAL_DATE_IS_NOT_WITHIN_ITINERARY_DATES, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<EcaItineraryStop>(x => x.ArrivalDate), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ItineraryStopDepartureDateIsBeforeItineraryStartDate()
        {
            var timezoneId = "timezone";
            var itineraryStartDate = DateTimeOffset.UtcNow.AddDays(-100.0);
            var itineraryEndDate = DateTimeOffset.UtcNow.AddDays(100.0);
            var stopArrivalDate = DateTimeOffset.UtcNow.AddDays(-10.0);
            var stopDepartureDate = DateTimeOffset.UtcNow.AddDays(10.0);

            EcaItineraryStopValidationEntity model = null;
            Func<EcaItineraryStopValidationEntity> createEntity = () =>
            {

                model = new EcaItineraryStopValidationEntity(
                    itineraryStartDate: itineraryStartDate,
                    itineraryEndDate: itineraryEndDate,
                    itineraryStopArrivalDate: stopArrivalDate,
                    itineraryStopDepartureDate: stopDepartureDate,
                    timezoneId: timezoneId
                    );
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            stopDepartureDate = itineraryStartDate.AddDays(-1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryStopServiceValidator.ITINERARY_STOP_DEPARTURE_DATE_IS_NOT_WITHIN_ITINERARY_DATES, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<EcaItineraryStop>(x => x.DepartureDate), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ItineraryStopDepartureDateIsAfterItineraryEndDate()
        {
            var timezoneId = "timezone";
            var itineraryStartDate = DateTimeOffset.UtcNow.AddDays(-100.0);
            var itineraryEndDate = DateTimeOffset.UtcNow.AddDays(100.0);
            var stopArrivalDate = DateTimeOffset.UtcNow.AddDays(-10.0);
            var stopDepartureDate = DateTimeOffset.UtcNow.AddDays(10.0);

            EcaItineraryStopValidationEntity model = null;
            Func<EcaItineraryStopValidationEntity> createEntity = () =>
            {

                model = new EcaItineraryStopValidationEntity(
                    itineraryStartDate: itineraryStartDate,
                    itineraryEndDate: itineraryEndDate,
                    itineraryStopArrivalDate: stopArrivalDate,
                    itineraryStopDepartureDate: stopDepartureDate,
                    timezoneId: timezoneId
                    );
                return model;
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());
            stopDepartureDate = itineraryEndDate.AddDays(1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryStopServiceValidator.ITINERARY_STOP_DEPARTURE_DATE_IS_NOT_WITHIN_ITINERARY_DATES, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<EcaItineraryStop>(x => x.DepartureDate), validationErrors.First().Property);
        }

        #endregion

    }
}
