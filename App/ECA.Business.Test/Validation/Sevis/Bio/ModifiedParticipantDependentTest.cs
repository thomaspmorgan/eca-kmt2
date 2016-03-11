using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Sevis.Model;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class ModifiedParticipantDependentTest
    {
        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorDependent()
        {
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
            var mailAddress = new AddressDTO
            {
                AddressId = 1,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME
            };
            var usAddress = new AddressDTO
            {
                AddressId = 2,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME
            };
            var printForm = true;
            var birthCountryReason = "reason";
            var relationship = DependentCodeType.Item01.ToString();

            var dependent = new AddedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReason: birthCountryReason,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                genderCode: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                positionCode: positionCode,
                relationship: relationship,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                participantId: participantId,
                personId: personId
                );

            var modifiedParticipantDependent = new ModifiedParticipantDependent();
            modifiedParticipantDependent.Dependent = dependent;
            modifiedParticipantDependent.UserDefinedA = "a";
            modifiedParticipantDependent.UserDefinedB = "b";

            var instance = modifiedParticipantDependent.GetSEVISEVBatchTypeExchangeVisitorDependent();
            Assert.IsNotNull(instance.Item);
            Assert.IsInstanceOfType(instance.Item, typeof(SEVISEVBatchTypeExchangeVisitorDependentAdd));
            Assert.AreEqual(modifiedParticipantDependent.UserDefinedA, instance.UserDefinedA);
            Assert.AreEqual(modifiedParticipantDependent.UserDefinedB, instance.UserDefinedB);
        }
    }
}
