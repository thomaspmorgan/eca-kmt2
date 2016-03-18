using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis.ErrorPaths;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class DependentValidatorTest
    {
        [TestMethod]
        public void TestRelationship_Null()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "street1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "street2";
            usAddress.PostalCode = "22222";

            string birthCity = "birth city";
            string birthCountryCode = "US";
            string birthCountryReason = "re";
            DateTime birthDate = DateTime.Now;
            string citizenshipCountryCode = "UK";
            string emailAddress = "email@isp.com";

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            string gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            string permanentResidenceCountryCode = "FR";
            string phoneNumber = "18505551212";
            string relationship = "relations";
            Func<AddedDependent> createEntity = () =>
            {
                return new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReason: birthCountryReason,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: emailAddress,
                    gender: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: phoneNumber,
                    relationship: relationship,
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: true,
                    personId: 10,
                    participantId: 20);
            };

            var instance = createEntity();
            var validator = new DependentValidator();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            relationship = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual(String.Format(DependentValidator.DEPENDENT_RELATIONSHIP_REQUIRED, fullName.FirstName, fullName.LastName), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(DependentErrorPath));
        }
        
    }
}
