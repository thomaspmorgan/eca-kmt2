using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Models.Admin;
using ECA.Data;
using ECA.Business.Validation.Sevis.Bio;

namespace ECA.Business.Test.Queries.Model.ExchangeVisitor
{
    [TestClass]
    public class DependentBiographicalDTOTest
    {
        [TestMethod]
        public void TestGetDependent_HasSevisId()
        {
            var personId = 1500;
            var participantId = 2;
            var fullName = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };
            var mailAddress = new AddressDTO();
            mailAddress.AddressId = 50;

            var residenceAddress = new AddressDTO();
            residenceAddress.AddressId = 60;

            var usAddress = new AddressDTO();
            usAddress.AddressId = 70;

            var model = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@ispl.com",
                FullName = fullName,
                Gender = "male",
                PermanentResidenceCountryCode = "perm resident country code",
                PersonId = personId,
                PhoneNumber = "123-456-7890",
                PositionCode = "position code",
                EmailAddressId = 3,
                GenderId = 4,
                MailAddress = mailAddress,
                NumberOfCitizenships = 2000,
                PermanentResidenceAddressId = residenceAddress.AddressId,
                PhoneNumberId = 5,
                USAddress = usAddress,
                PersonTypeId = PersonType.Child.Id,
                Relationship = "child",
                SevisId = "sevisId",
                ParticipantId = participantId
            };

            var dependent = model.GetDependent();
            Assert.IsNotNull(dependent);
            Assert.IsInstanceOfType(dependent, typeof(UpdatedDependent));
        }

        [TestMethod]
        public void TestGetDependent_NullSevisId()
        {
            var personId = 1500;
            var participantId = 2;
            var fullName = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };
            var mailAddress = new AddressDTO();
            mailAddress.AddressId = 50;

            var residenceAddress = new AddressDTO();
            residenceAddress.AddressId = 60;

            var usAddress = new AddressDTO();
            usAddress.AddressId = 70;

            var model = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@ispl.com",
                FullName = fullName,
                Gender = "male",
                PermanentResidenceCountryCode = "perm resident country code",
                PersonId = personId,
                PhoneNumber = "123-456-7890",
                PositionCode = "position code",
                EmailAddressId = 3,
                GenderId = 4,
                MailAddress = mailAddress,
                NumberOfCitizenships = 2000,
                PermanentResidenceAddressId = residenceAddress.AddressId,
                PhoneNumberId = 5,
                USAddress = usAddress,
                PersonTypeId = PersonType.Child.Id,
                Relationship = "child",
                SevisId = null,
                ParticipantId = participantId
            };

            var dependent = model.GetDependent();
            Assert.IsNotNull(dependent);
            Assert.IsInstanceOfType(dependent, typeof(AddedDependent));
        }

        [TestMethod]
        public void TestGetDependent_WhitespaceSevisId()
        {
            var personId = 1500;
            var participantId = 2;
            var fullName = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };
            var mailAddress = new AddressDTO();
            mailAddress.AddressId = 50;

            var residenceAddress = new AddressDTO();
            residenceAddress.AddressId = 60;

            var usAddress = new AddressDTO();
            usAddress.AddressId = 70;

            var model = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@ispl.com",
                FullName = fullName,
                Gender = "male",
                PermanentResidenceCountryCode = "perm resident country code",
                PersonId = personId,
                PhoneNumber = "123-456-7890",
                PositionCode = "position code",
                EmailAddressId = 3,
                GenderId = 4,
                MailAddress = mailAddress,
                NumberOfCitizenships = 2000,
                PermanentResidenceAddressId = residenceAddress.AddressId,
                PhoneNumberId = 5,
                USAddress = usAddress,
                PersonTypeId = PersonType.Child.Id,
                Relationship = "child",
                SevisId = " ",
                ParticipantId = participantId
            };

            var dependent = model.GetDependent();
            Assert.IsNotNull(dependent);
            Assert.IsInstanceOfType(dependent, typeof(AddedDependent));
        }

        [TestMethod]
        public void TestGetDependent_EmptySevisId()
        {
            var personId = 1500;
            var participantId = 2;
            var fullName = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };
            var mailAddress = new AddressDTO();
            mailAddress.AddressId = 50;

            var residenceAddress = new AddressDTO();
            residenceAddress.AddressId = 60;

            var usAddress = new AddressDTO();
            usAddress.AddressId = 70;

            var model = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@ispl.com",
                FullName = fullName,
                Gender = "male",
                PermanentResidenceCountryCode = "perm resident country code",
                PersonId = personId,
                PhoneNumber = "123-456-7890",
                PositionCode = "position code",
                EmailAddressId = 3,
                GenderId = 4,
                MailAddress = mailAddress,
                NumberOfCitizenships = 2000,
                PermanentResidenceAddressId = residenceAddress.AddressId,
                PhoneNumberId = 5,
                USAddress = usAddress,
                PersonTypeId = PersonType.Child.Id,
                Relationship = "child",
                SevisId = String.Empty,
                ParticipantId = participantId
            };

            var dependent = model.GetDependent();
            Assert.IsNotNull(dependent);
            Assert.IsInstanceOfType(dependent, typeof(AddedDependent));
        }

        [TestMethod]
        public void TestGetAddedDependent()
        {
            var personId = 1500;
            var participantId = 2;
            var fullName = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };
            var mailAddress = new AddressDTO();
            mailAddress.AddressId = 50;

            var residenceAddress = new AddressDTO();
            residenceAddress.AddressId = 60;

            var usAddress = new AddressDTO();
            usAddress.AddressId = 70;

            var model = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@ispl.com",
                FullName = fullName,
                Gender = "male",
                PermanentResidenceCountryCode = "perm resident country code",
                PersonId = personId,
                PhoneNumber = "123-456-7890",
                PositionCode = "position code",
                EmailAddressId = 3,
                GenderId = 4,
                MailAddress = mailAddress,
                NumberOfCitizenships = 2000,
                PermanentResidenceAddressId = residenceAddress.AddressId,
                PhoneNumberId = 5,
                USAddress = usAddress,
                PersonTypeId = PersonType.Child.Id,
                Relationship = "child",
                SevisId = "sevisId",
                ParticipantId = participantId
            };

            var person = model.GetAddedDependent();
            Assert.IsNotNull(person);
            Assert.AreEqual(model.BirthCity, person.BirthCity);
            Assert.AreEqual(model.BirthCountryCode, person.BirthCountryCode);            
            Assert.AreEqual(model.BirthDate, person.BirthDate);
            Assert.AreEqual(model.CitizenshipCountryCode, person.CitizenshipCountryCode);
            Assert.AreEqual(model.EmailAddress, person.EmailAddress);
            Assert.AreEqual(model.Gender, person.Gender);
            Assert.AreEqual(model.PermanentResidenceCountryCode, person.PermanentResidenceCountryCode);
            Assert.AreEqual(model.Relationship, person.Relationship);

            Assert.AreEqual(participantId, person.GetParticipantId());
            Assert.AreEqual(personId, person.GetPersonId());

            Assert.AreEqual(fullName.FirstName, person.FullName.FirstName);
            Assert.AreEqual(fullName.LastName, person.FullName.LastName);
            Assert.AreEqual(fullName.PassportName, person.FullName.PassportName);
            Assert.AreEqual(fullName.PreferredName, person.FullName.PreferredName);
            Assert.AreEqual(fullName.Suffix, person.FullName.Suffix);

            Assert.IsNull(person.BirthCountryReason);
        }

        [TestMethod]
        public void TestGetUpdatedDependent()
        {
            var personId = 1500;
            var participantId = 2;
            var fullName = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };
            var mailAddress = new AddressDTO();
            mailAddress.AddressId = 50;

            var residenceAddress = new AddressDTO();
            residenceAddress.AddressId = 60;

            var usAddress = new AddressDTO();
            usAddress.AddressId = 70;

            var model = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@ispl.com",
                FullName = fullName,
                Gender = "male",
                PermanentResidenceCountryCode = "perm resident country code",
                PersonId = personId,
                ParticipantId = participantId,
                PhoneNumber = "123-456-7890",
                PositionCode = "position code",
                EmailAddressId = 3,
                GenderId = 4,
                MailAddress = mailAddress,
                NumberOfCitizenships = 2000,
                PermanentResidenceAddressId = residenceAddress.AddressId,
                PhoneNumberId = 5,
                USAddress = usAddress,
                PersonTypeId = PersonType.Child.Id,
                Relationship = "child",
                SevisId = "sevisId",
            };

            var person = model.GetUpdatedDependent();
            Assert.IsNotNull(person);
            Assert.AreEqual(model.BirthCity, person.BirthCity);
            Assert.AreEqual(model.BirthCountryCode, person.BirthCountryCode);
            Assert.AreEqual(model.BirthDate, person.BirthDate);
            Assert.AreEqual(model.CitizenshipCountryCode, person.CitizenshipCountryCode);
            Assert.AreEqual(model.EmailAddress, person.EmailAddress);
            Assert.AreEqual(model.Gender, person.Gender);
            Assert.AreEqual(model.PermanentResidenceCountryCode, person.PermanentResidenceCountryCode);
            Assert.AreEqual(model.Relationship, person.Relationship);
            Assert.AreEqual(model.SevisId, person.SevisId);

            Assert.IsTrue(person.IsRelationshipFieldSpecified);
            Assert.IsTrue(person.PrintForm);
            Assert.IsNull(person.Remarks);

            Assert.AreEqual(participantId, person.GetParticipantId());
            Assert.AreEqual(personId, person.GetPersonId());

            Assert.AreEqual(fullName.FirstName, person.FullName.FirstName);
            Assert.AreEqual(fullName.LastName, person.FullName.LastName);
            Assert.AreEqual(fullName.PassportName, person.FullName.PassportName);
            Assert.AreEqual(fullName.PreferredName, person.FullName.PreferredName);
            Assert.AreEqual(fullName.Suffix, person.FullName.Suffix);

            Assert.IsNull(person.BirthCountryReason);
        }
    }
}
