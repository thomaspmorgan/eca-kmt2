using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.Data;
using System.Collections.Generic;
using ECA.Business.Service.Persons;
using FluentAssertions;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class NewPersonTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var projectId = 1;
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var countriesOfCitizenship = new List<int> { 1 };
            var personTypeId = PersonType.Participant.Id;

            var instance = new NewPerson(
                user, 
                projectId, 
                participantTypeId, 
                firstName, 
                lastName,
                gender, 
                dateOfBirth, 
                isDateOfBirthUnknown, 
                isDateOfBirthEstimated, 
                isPlaceOfBirthUnknown, 
                cityOfBirth,
                personTypeId,
                countriesOfCitizenship);
            Assert.IsTrue(Object.ReferenceEquals(countriesOfCitizenship, instance.CountriesOfCitizenship));
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));

            Assert.AreEqual(firstName, instance.FirstName);
            Assert.AreEqual(lastName, instance.LastName);
            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(participantTypeId, instance.ParticipantTypeId);
            Assert.AreEqual(gender, instance.Gender);
            Assert.AreEqual(dateOfBirth, instance.DateOfBirth);
            Assert.AreEqual(isDateOfBirthEstimated, instance.IsDateOfBirthEstimated);
            Assert.AreEqual(isDateOfBirthUnknown, instance.IsDateOfBirthUnknown);
            Assert.AreEqual(isPlaceOfBirthUnknown, instance.IsPlaceOfBirthUnknown);
            Assert.AreEqual(cityOfBirth, instance.CityOfBirth);
            Assert.AreEqual(personTypeId, instance.PersonTypeId);
            
        }

        [TestMethod]
        public void TestConstructor_CheckIsDateOfBirthUnknown()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var projectId = 1;
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = false;
            var isPlaceOfBirthUnknown = false;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int> { 1 };

            var instance = new NewPerson(user,
                projectId,
                participantTypeId,
                firstName,
                lastName,
                gender,
                dateOfBirth,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown,
                cityOfBirth,
                personTypeId,
                countriesOfCitizenship);
            Assert.AreEqual(isDateOfBirthUnknown, instance.IsDateOfBirthUnknown);

        }

        [TestMethod]
        public void TestConstructor_CheckIsDateOfBirthEstimated()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var projectId = 1;
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = false;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = false;
            var cityOfBirth = 5;
            var countriesOfCitizenship = new List<int> { 1 };
            var personTypeId = PersonType.Participant.Id;

            var instance = new NewPerson(user,
                projectId,
                participantTypeId,
                firstName,
                lastName,
                gender,
                dateOfBirth,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown,
                cityOfBirth,
                personTypeId,
                countriesOfCitizenship);
            Assert.AreEqual(isDateOfBirthEstimated, instance.IsDateOfBirthEstimated);

        }

        [TestMethod]
        public void TestConstructor_CheckIsPlaceOfBirthUnknown()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var projectId = 1;
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = false;
            var isDateOfBirthEstimated = false;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int> { 1 };

            var instance = new NewPerson(user,
                projectId,
                participantTypeId,
                firstName,
                lastName,
                gender,
                dateOfBirth,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown,
                cityOfBirth,
                personTypeId,
                countriesOfCitizenship);
            Assert.AreEqual(isPlaceOfBirthUnknown, instance.IsPlaceOfBirthUnknown);

        }

        [TestMethod]
        public void TestConstructor_PersonTypeIdNotSupported()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var projectId = 1;
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = false;
            var isDateOfBirthEstimated = false;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = 0;
            var countriesOfCitizenship = new List<int> { 1 };

            Action a = () => new NewPerson(user,
                projectId,
                participantTypeId,
                firstName,
                lastName,
                gender,
                dateOfBirth,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown,
                cityOfBirth,
                personTypeId,
                countriesOfCitizenship);
            a.ShouldThrow<UnknownStaticLookupException>().WithMessage(String.Format("The person type id [{0}] is not recognized.", personTypeId));

        }
    }
}
