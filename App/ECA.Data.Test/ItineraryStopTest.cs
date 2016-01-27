using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ECA.Core.Data;

namespace ECA.Data.Test
{
    [TestClass]
    public class ItineraryStopTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }
        [TestMethod]
        public void TestGetId()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            Assert.AreEqual(project.ProjectId, project.GetId());
        }        

        [TestMethod]
        public void TestItineraryStopName_Unique()
        {
            
            var existingStop = new ItineraryStop
            {
                Name = "  HELLO  ",
                ItineraryId = 1,
                ItineraryStopId = 2
            };
            context.ItineraryStops.Add(existingStop);

            var testItineraryStop = new ItineraryStop
            {
                Name = "  hello ",
                ItineraryId = existingStop.ItineraryId
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(testItineraryStop, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(testItineraryStop, vc, results);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());

            var expectedErrorMessage = String.Format("The itinerary stop with the name [{0}] already exists.",
                        testItineraryStop.Name);
            Assert.AreEqual(expectedErrorMessage, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestItineraryStopName_Unique_DifferentItinerary()
        {

            var existingStop = new ItineraryStop
            {
                Name = "  HELLO  ",
                ItineraryId = 1,
                ItineraryStopId = 2
            };
            context.ItineraryStops.Add(existingStop);

            var testItineraryStop = new ItineraryStop
            {
                Name = "  hello ",
                ItineraryId = existingStop.ItineraryId
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(testItineraryStop, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(testItineraryStop, vc, results);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());

            var expectedErrorMessage = String.Format("The itinerary stop with the name [{0}] already exists.",
                        testItineraryStop.Name);
            Assert.AreEqual(expectedErrorMessage, results.First().ErrorMessage);

            testItineraryStop.ItineraryId = 10;

            results = new List<ValidationResult>();
            actual = Validator.TryValidateObject(testItineraryStop, vc, results);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestNameMaxLength()
        {
            var stop = new ItineraryStop
            {
                Name = new string('a', ItineraryStop.NAME_MAX_LENGTH),
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(stop, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(stop, vc, results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            stop.Name = new string('a', ItineraryStop.NAME_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(stop, vc, results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());
        }
    }
}
