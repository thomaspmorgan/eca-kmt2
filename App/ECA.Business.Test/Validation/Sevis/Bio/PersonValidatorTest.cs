using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ECA.Business.Test.Validation.Sevis.Bio
{

    [TestClass]
    public class PersonValidatorTest
    {
        //public Business.Validation.Sevis.Bio.Person GetValidPerson()
        //{
        //    return new Business.Validation.Sevis.Bio.Person
        //    {
        //        BirthCity = "birth city",
        //        BirthCountryCode = "US",
        //        BirthCountryReason = "re",
        //        BirthDate = DateTime.Now,
        //        CitizenshipCountryCode = "UK",
        //        EmailAddress = "email@isp.com",
        //        FullName = new FullName
        //        {
        //            FirstName = "first name",
        //            LastName = "last name",
        //            PassportName = "passport name",
        //            PreferredName = "preferred name",
        //            Suffix = FullNameValidator.SECOND_SUFFIX
        //        },
        //        Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
        //        PermanentResidenceCountryCode = "FR",
        //        PhoneNumber = "123-456-7890"
        //    };
        //}

        #region PhoneNumber

        [TestMethod]
        public void TestFullName_ShouldRunFullNameValidator()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;

            var personId = 100;
            var participantId = 200;
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            var positionCode = "120";
            var printForm = true;
            var birthCountryReason = "reason";
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                var person = new Business.Validation.Sevis.Bio.Person
                    (birthCity: birthCity,
                    fullName: fullName,
                    birthCountryCode: birthCountryCode,
                    birthCountryReason: birthCountryReason,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: email,
                    genderCode: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: phone,
                    remarks: remarks,
                    positionCode: positionCode,

                    programCategoryCode: programCataegoryCode,
                    subjectField: subjectField,
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: printForm,
                    personId: personId,
                    participantId: participantId
                    );
                return person;
            };



            var validator = new PersonValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            phone = null;
            instance = createEntity();

            result = validator.Validate(instance);
            Assert.Fail("come back to this validation test.");
            //Assert.IsFalse(result.IsValid);
            //Assert.AreEqual(1, result.Errors.Count);
            //Assert.AreEqual(BiographyTestValidator.FULL_NAME_NULL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion
    }
}
