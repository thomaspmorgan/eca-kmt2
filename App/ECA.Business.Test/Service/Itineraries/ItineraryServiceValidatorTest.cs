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
    public class ItineraryServiceValidatorTest
    {
        private ItineraryServiceValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new ItineraryServiceValidator();
        }

        #region Create

        [TestMethod]
        public void TestDoValidateCreate_AddedEcaItineraryIsNull()
        {
            var creator = new User(1);
            var startDate = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now;
            AddedEcaItinerary model = null;
            Project project = null;
            Location arrivalLocation = null;
            Location departureLocation = null;
            var entity = new AddedEcaItineraryValidationEntity(model, project, arrivalLocation, departureLocation);
            Assert.AreEqual(0, validator.DoValidateCreate(entity).Count());
        }

        [TestMethod]
        public void TestDoValidateCreate_StartDateAfterEndDate()
        {
            var creator = new User(1);
            var startDate = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now;
            var projectId = 1;
            var name = "name";
            var arrivalLocationId = 2;
            var departureLocationId = 3;
            AddedEcaItinerary model = null;
            Project project = null;
            Location arrivalLocation = null;
            Location departureLocation = null;
            Func<AddedEcaItineraryValidationEntity> createEntity = () =>
            {
                project = new Project
                {
                    ProjectId = projectId
                };
                arrivalLocation = new Location
                {
                    LocationId = arrivalLocationId
                };
                departureLocation = new Location
                {
                    LocationId = departureLocationId
                };
                model = new AddedEcaItinerary(creator, startDate, endDate, name, projectId, arrivalLocationId, departureLocationId);
                return new AddedEcaItineraryValidationEntity(model, project, arrivalLocation, departureLocation);
            };
            Assert.AreEqual(0, validator.DoValidateCreate(createEntity()).Count());

            startDate = DateTimeOffset.Now.AddDays(1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryServiceValidator.START_DATE_AFTER_END_DATE_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<AddedEcaItinerary>(x => x.StartDate), validationErrors.First().Property);
        }
        #endregion

        #region Update
        [TestMethod]
        public void TestDoValidateUpdate_UpdatedItineraryIsNull()
        {
            var updator = new User(1);
            var startDate = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now;
            UpdatedEcaItinerary model = null;
            Location arrivalLocation = null;
            Location departureLocation = null;
            Itinerary itinerary = null;
            List<ItineraryStop> stops = new List<ItineraryStop>();
            var entity = new UpdatedEcaItineraryValidationEntity(model, itinerary, arrivalLocation, departureLocation, stops);

            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());
        }

        [TestMethod]
        public void TestDoValidateUpdate_StartDateAfterEndDate()
        {
            var id = 1;
            var updator = new User(1);
            var startDate = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now;
            var projectId = 1;
            var name = "name";
            var arrivalLocationId = 2;
            var departureLocationId = 3;
            UpdatedEcaItinerary model = null;
            Project project = null;
            Location arrivalLocation = null;
            Location departureLocation = null;
            Itinerary itinerary = null;
            List<ItineraryStop> stops = new List<ItineraryStop>();
            Func<UpdatedEcaItineraryValidationEntity> createEntity = () =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = id
                };
                project = new Project
                {
                    ProjectId = projectId
                };
                arrivalLocation = new Location
                {
                    LocationId = arrivalLocationId
                };
                departureLocation = new Location
                {
                    LocationId = departureLocationId
                };
                model = new UpdatedEcaItinerary(id, updator, startDate, endDate, name, projectId, arrivalLocationId, departureLocationId);
                return new UpdatedEcaItineraryValidationEntity(model, itinerary, arrivalLocation, departureLocation, stops);
            };
            Assert.AreEqual(0, validator.DoValidateUpdate(createEntity()).Count());

            startDate = DateTimeOffset.Now.AddDays(1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryServiceValidator.START_DATE_AFTER_END_DATE_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<AddedEcaItinerary>(x => x.StartDate), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_StartDateAfterMinItineraryStopDate()
        {
            var id = 1;
            var updator = new User(1);
            var startDate = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(1.0);
            var projectId = 1;
            var name = "name";
            var arrivalLocationId = 2;
            var departureLocationId = 3;
            UpdatedEcaItinerary model = null;
            Project project = null;
            Location arrivalLocation = null;
            Location departureLocation = null;
            Itinerary itinerary = null;
            List<ItineraryStop> stops = new List<ItineraryStop>();

            var itineraryStop = new ItineraryStop
            {
                DateArrive = DateTimeOffset.Now,
                DateLeave = DateTimeOffset.Now
            };

            stops.Add(itineraryStop);

            Func<UpdatedEcaItineraryValidationEntity> createEntity = () =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = id
                };
                project = new Project
                {
                    ProjectId = projectId
                };
                arrivalLocation = new Location
                {
                    LocationId = arrivalLocationId
                };
                departureLocation = new Location
                {
                    LocationId = departureLocationId
                };
                model = new UpdatedEcaItinerary(id, updator, startDate, endDate, name, projectId, arrivalLocationId, departureLocationId);
                return new UpdatedEcaItineraryValidationEntity(model, itinerary, arrivalLocation, departureLocation, stops);
            };
            Assert.AreEqual(0, validator.DoValidateUpdate(createEntity()).Count());

            startDate = DateTimeOffset.Now.AddDays(0.5);

            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryServiceValidator.START_DATE_BEFORE_ITINERARY_STOP_MIN_DATE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<AddedEcaItinerary>(x => x.StartDate), validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_EndDateAfterMaxItineraryStopDate()
        {
            var id = 1;
            var updator = new User(1);
            var startDate = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(1.0);
            var projectId = 1;
            var name = "name";
            var arrivalLocationId = 2;
            var departureLocationId = 3;
            UpdatedEcaItinerary model = null;
            Project project = null;
            Location arrivalLocation = null;
            Location departureLocation = null;
            Itinerary itinerary = null;
            List<ItineraryStop> stops = new List<ItineraryStop>();

            var itineraryStop = new ItineraryStop
            {
                DateArrive = DateTimeOffset.Now,
                DateLeave = DateTimeOffset.Now
            };

            stops.Add(itineraryStop);

            Func<UpdatedEcaItineraryValidationEntity> createEntity = () =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = id
                };
                project = new Project
                {
                    ProjectId = projectId
                };
                arrivalLocation = new Location
                {
                    LocationId = arrivalLocationId
                };
                departureLocation = new Location
                {
                    LocationId = departureLocationId
                };
                model = new UpdatedEcaItinerary(id, updator, startDate, endDate, name, projectId, arrivalLocationId, departureLocationId);
                return new UpdatedEcaItineraryValidationEntity(model, itinerary, arrivalLocation, departureLocation, stops);
            };
            Assert.AreEqual(0, validator.DoValidateUpdate(createEntity()).Count());

            endDate = DateTimeOffset.Now.AddDays(-1.0);

            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ItineraryServiceValidator.END_DATE_BEFORE_ITINERARY_STOP_MAX_DATE, validationErrors.First().ErrorMessage);
            Assert.AreEqual(PropertyHelper.GetPropertyName<AddedEcaItinerary>(x => x.EndDate), validationErrors.First().Property);
        }
        #endregion
    }
}
