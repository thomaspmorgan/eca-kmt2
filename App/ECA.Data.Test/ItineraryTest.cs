using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ECA.Core.Data;

namespace ECA.Data.Test
{
    [TestClass]
    public class ItineraryTest
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
        public void TestItineraryName_UniqueName()
        {
            
            var existingItinerary = new Itinerary
            {
                Name = "  HELLO  ",
                ItineraryId = 1,
            };
            context.Itineraries.Add(existingItinerary);

            var testItinerary = new Itinerary
            {
                Name = "  hello ",
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(testItinerary, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(testItinerary, vc, results);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());

            var expectedErrorMessage = String.Format("The itinerary with the name [{0}] already exists.",
                        testItinerary.Name);
            Assert.AreEqual(expectedErrorMessage, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestItineraryName_UniqueName_DifferentProject()
        {

            var existingItinerary = new Itinerary
            {
                Name = "  HELLO  ",
                ItineraryId = 1,
            };
            context.Itineraries.Add(existingItinerary);

            var testItinerary = new Itinerary
            {
                Name = "  hello ",
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(testItinerary, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(testItinerary, vc, results);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());

            testItinerary.ProjectId = 10;
            actual = Validator.TryValidateObject(testItinerary, vc, results);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestNameMaxLength()
        {
            var itinerary = new Itinerary
            {
                Name = new string('a', Itinerary.NAME_MAX_LENGTH),
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(itinerary, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(itinerary, vc, results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            itinerary.Name = new string('a', Itinerary.NAME_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(itinerary, vc, results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());
        }
    }
}
