using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Queries.Models.Persons;
using ECA.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Validation.Sevis.Bio;

namespace ECA.Business.Test.Queries.Models.Persons.ExchangeVisitor
{
    [TestClass]
    public class DependentBiographicalDTOTest
    {
        [TestMethod]
        public void TestGetDependent_HasSevisId()
        {
            var mailAddress = new AddressDTO();
            var usAddress = new AddressDTO();
            var remarks = "remarks";
            var dto = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReasonId = 1,
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "somone@isp.com",
                EmailAddressId = 1,
                FullName = new FullNameDTO
                {
                    FirstName = "first name",
                    LastName = "last Name",
                    PassportName = "passport",
                    PreferredName = "preferred",
                    Suffix = "jr"
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                GenderId = 2,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                ParticipantId = 10,
                PermanentResidenceAddressId = 20,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 30,
                DependentTypeId = DependentType.Child.Id,
                PhoneNumber = "123-456-7890",
                PhoneNumberId = 50,
                Relationship = "relationship", 
                SevisId = "sevisId"
            };
            var instance = dto.GetDependent(usAddress, remarks);
            Assert.IsInstanceOfType(instance, typeof(UpdatedDependent));
        }

        [TestMethod]
        public void TestGetDependent_NullSevisId()
        {
            var mailAddress = new AddressDTO();
            var usAddress = new AddressDTO();
            var remarks = "remarks";
            var dto = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReasonId = 1,
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "somone@isp.com",
                EmailAddressId = 1,
                FullName = new FullNameDTO
                {
                    FirstName = "first name",
                    LastName = "last Name",
                    PassportName = "passport",
                    PreferredName = "preferred",
                    Suffix = "jr"
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                GenderId = 2,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                ParticipantId = 10,
                PermanentResidenceAddressId = 20,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 30,
                DependentTypeId = DependentType.Child.Id,
                PhoneNumber = "123-456-7890",
                PhoneNumberId = 50,
                Relationship = "relationship",
                SevisId = null
            };
            var instance = dto.GetDependent(usAddress, remarks);
            Assert.IsInstanceOfType(instance, typeof(AddedDependent));
        }

        [TestMethod]
        public void TestGetDependent_EmptySevisId()
        {
            var mailAddress = new AddressDTO();
            var usAddress = new AddressDTO();
            var remarks = "remarks";
            var dto = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReasonId = 1,
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "somone@isp.com",
                EmailAddressId = 1,
                FullName = new FullNameDTO
                {
                    FirstName = "first name",
                    LastName = "last Name",
                    PassportName = "passport",
                    PreferredName = "preferred",
                    Suffix = "jr"
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                GenderId = 2,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                ParticipantId = 10,
                PermanentResidenceAddressId = 20,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 30,
                DependentTypeId = DependentType.Child.Id,
                PhoneNumber = "123-456-7890",
                PhoneNumberId = 50,
                Relationship = "relationship",
                SevisId = string.Empty,
            };
            var instance = dto.GetDependent(usAddress, remarks);
            Assert.IsInstanceOfType(instance, typeof(AddedDependent));
        }

        [TestMethod]
        public void TestGetDependent_WhitespaceSevisId()
        {
            var mailAddress = new AddressDTO();
            var usAddress = new AddressDTO();
            var remarks = "remarks";
            var dto = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReasonId = 1,
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "somone@isp.com",
                EmailAddressId = 1,
                FullName = new FullNameDTO
                {
                    FirstName = "first name",
                    LastName = "last Name",
                    PassportName = "passport",
                    PreferredName = "preferred",
                    Suffix = "jr"
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                GenderId = 2,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                ParticipantId = 10,
                PermanentResidenceAddressId = 20,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 30,
                DependentTypeId = DependentType.Child.Id,
                PhoneNumber = "123-456-7890",
                PhoneNumberId = 50,
                Relationship = "relationship",
                SevisId = " "
            };
            var instance = dto.GetDependent(usAddress, remarks);
            Assert.IsInstanceOfType(instance, typeof(AddedDependent));
        }

        [TestMethod]
        public void TestGetAddedDependent()
        {
            var mailAddress = new AddressDTO();
            var usAddress = new AddressDTO();
            var dto = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReasonId = 155,
                BirthCountryReasonCode = "birth country reason code",
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "somone@isp.com",
                EmailAddressId = 1,
                FullName = new FullNameDTO
                {
                    FirstName = "first name",
                    LastName = "last Name",
                    PassportName = "passport",
                    PreferredName = "preferred",
                    Suffix = "jr"
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                GenderId = 2,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                ParticipantId = 10,
                PermanentResidenceAddressId = 20,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 30,
                DependentTypeId = DependentType.Child.Id,
                PhoneNumber = "123-456-7890",
                PhoneNumberId = 50,
                Relationship = "relationship",
                SevisId = "sevisId",
                IsDeleted = true,
                IsTravelingWithParticipant = true
            };
            var instance = dto.GetAddedDependent(usAddress);
            Assert.IsTrue(object.ReferenceEquals(mailAddress, instance.MailAddress));
            Assert.IsTrue(object.ReferenceEquals(usAddress, instance.USAddress));
            Assert.AreEqual(dto.BirthCity, instance.BirthCity);
            Assert.AreEqual(dto.BirthCountryCode, instance.BirthCountryCode);
            Assert.AreEqual(dto.BirthCountryReasonCode, instance.BirthCountryReasonCode);
            Assert.AreEqual(dto.BirthDate, instance.BirthDate);
            Assert.AreEqual(dto.CitizenshipCountryCode, instance.CitizenshipCountryCode);
            Assert.AreEqual(dto.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(dto.Gender, instance.Gender);
            Assert.AreEqual(dto.PermanentResidenceCountryCode, instance.PermanentResidenceCountryCode);
            Assert.AreEqual(dto.PhoneNumber, instance.PhoneNumber);
            Assert.IsTrue(instance.PrintForm);
            Assert.AreEqual(dto.Relationship, instance.Relationship);
            Assert.AreEqual(dto.IsTravelingWithParticipant, instance.IsTravelingWithParticipant);

            Assert.AreEqual(dto.FullName.FirstName, instance.FullName.FirstName);
            Assert.AreEqual(dto.FullName.LastName, instance.FullName.LastName);
            Assert.AreEqual(dto.FullName.PassportName, instance.FullName.PassportName);
            Assert.AreEqual(dto.FullName.PreferredName, instance.FullName.PreferredName);
            Assert.AreEqual(dto.FullName.Suffix, instance.FullName.Suffix);
        }

        [TestMethod]
        public void TestGetAddedDependent_FullNameIsNull()
        {
            var mailAddress = new AddressDTO();
            var usAddress = new AddressDTO();
            var remarks = "remarks";
            var dto = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReasonId = 1,
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "somone@isp.com",
                EmailAddressId = 1,
                FullName = null,
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                GenderId = 2,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                ParticipantId = 10,
                PermanentResidenceAddressId = 20,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 30,
                DependentTypeId = DependentType.Child.Id,
                PhoneNumber = "123-456-7890",
                PhoneNumberId = 50,
                Relationship = "relationship",
                SevisId = "sevisId"
            };
            var instance = dto.GetAddedDependent(usAddress);
            Assert.IsNull(instance.FullName);
        }

        [TestMethod]
        public void TestGetUpdatedDependent()
        {
            var mailAddress = new AddressDTO();
            var usAddress = new AddressDTO();
            var remarks = "remarks";
            var dto = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReasonId = 155,
                BirthCountryReasonCode = "birth country reason code",
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "somone@isp.com",
                EmailAddressId = 1,
                FullName = new FullNameDTO
                {
                    FirstName = "first name",
                    LastName = "last Name",
                    PassportName = "passport",
                    PreferredName = "preferred",
                    Suffix = "jr"
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                GenderId = 2,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                ParticipantId = 10,
                PermanentResidenceAddressId = 20,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 30,
                DependentTypeId = DependentType.Child.Id,
                PhoneNumber = "123-456-7890",
                PhoneNumberId = 50,
                Relationship = "relationship",
                SevisId = "sevisId"
            };
            var instance = dto.GetUpdatedDependent(usAddress, remarks);
            Assert.IsTrue(object.ReferenceEquals(mailAddress, instance.MailAddress));
            Assert.IsTrue(object.ReferenceEquals(usAddress, instance.USAddress));
            Assert.AreEqual(dto.BirthCity, instance.BirthCity);
            Assert.AreEqual(dto.BirthCountryCode, instance.BirthCountryCode);
            Assert.AreEqual(dto.BirthCountryReasonCode, instance.BirthCountryReasonCode);
            Assert.AreEqual(dto.BirthDate, instance.BirthDate);
            Assert.AreEqual(dto.CitizenshipCountryCode, instance.CitizenshipCountryCode);
            Assert.AreEqual(dto.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(dto.Gender, instance.Gender);
            Assert.AreEqual(dto.PermanentResidenceCountryCode, instance.PermanentResidenceCountryCode);
            Assert.AreEqual(dto.PhoneNumber, instance.PhoneNumber);
            Assert.IsTrue(instance.PrintForm);
            Assert.AreEqual(dto.Relationship, instance.Relationship);
            Assert.AreEqual(dto.SevisId, instance.SevisId);
            Assert.AreEqual(remarks, instance.Remarks);
            Assert.AreEqual(dto.IsDeleted, instance.IsDeleted);

            Assert.AreEqual(dto.FullName.FirstName, instance.FullName.FirstName);
            Assert.AreEqual(dto.FullName.LastName, instance.FullName.LastName);
            Assert.AreEqual(dto.FullName.PassportName, instance.FullName.PassportName);
            Assert.AreEqual(dto.FullName.PreferredName, instance.FullName.PreferredName);
            Assert.AreEqual(dto.FullName.Suffix, instance.FullName.Suffix);
        }

        [TestMethod]
        public void TestGetUpdatedDependent_FullNameIsNull()
        {
            var mailAddress = new AddressDTO();
            var usAddress = new AddressDTO();
            var remarks = "remarks";
            var dto = new DependentBiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReasonId = 1,
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "somone@isp.com",
                EmailAddressId = 1,
                FullName = null,
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                GenderId = 2,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                ParticipantId = 10,
                PermanentResidenceAddressId = 20,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 30,
                DependentTypeId = DependentType.Child.Id,
                PhoneNumber = "123-456-7890",
                PhoneNumberId = 50,
                Relationship = "relationship",
                SevisId = "sevisId"
            };
            var instance = dto.GetUpdatedDependent(usAddress, remarks);
            Assert.IsNull(instance.FullName);
        }
    }
}

