using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ECA.Business.Validation;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class AddedDependentTest
    {

        [TestMethod]
        public void TestGetSevisExhangeVisitorDependentInstance()
        {
            var personId = 10;
            var participantId = 20;
            var dependent = new AddedDependent
            {
                BirthCity = "birth city",
                BirthCountryCode = "US",
                BirthCountryReason = "re",
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "UK",
                EmailAddress = "email@isp.com",
                FullName = new FullName
                {
                    FirstName = "first name",
                    LastName = "last name",
                    PassportName = "passport name",
                    PreferredName = "preferred name",
                    Suffix = FullNameValidator.SECOND_SUFFIX
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                PermanentResidenceCountryCode = "FR",
                PrintForm = true,
                Relationship = "01",
            };
            dependent.SetPersonId(personId);
            dependent.SetParticipantId(participantId);

            var instance = dependent.GetSevisExhangeVisitorDependentInstance();
            Assert.IsInstanceOfType(instance, typeof(SEVISEVBatchTypeExchangeVisitorDependentAdd));
            var sevisModel = (SEVISEVBatchTypeExchangeVisitorDependentAdd)instance;

            Assert.AreEqual(dependent.BirthCity, sevisModel.BirthCity);
            Assert.AreEqual(dependent.BirthCountryCode.GetBirthCntryCodeType(), sevisModel.BirthCountryCode);
            Assert.AreEqual(dependent.BirthDate, sevisModel.BirthDate);
            Assert.AreEqual(dependent.CitizenshipCountryCode.GetCountryCodeWithType(), sevisModel.CitizenshipCountryCode);
            Assert.AreEqual(dependent.EmailAddress, sevisModel.EmailAddress);
            Assert.AreEqual(dependent.Gender.GetEVGenderCodeType(), sevisModel.Gender);
            Assert.AreEqual(dependent.PermanentResidenceCountryCode.GetCountryCodeWithType(), sevisModel.PermanentResidenceCountryCode);
            Assert.AreEqual(dependent.Relationship.GetDependentCodeType(), sevisModel.Relationship);
            Assert.AreEqual(dependent.PrintForm, sevisModel.printForm);
            Assert.AreEqual(EVPrintReasonType.Item06, sevisModel.FormPurpose);
            Assert.IsFalse(sevisModel.BirthCountryReasonSpecified);
        }

        [TestMethod]
        public void TestGetEVPersonTypeDependent()
        {
            var personId = 10;
            var participantId = 20;
            var dependent = new AddedDependent
            {
                BirthCity = "birth city",
                BirthCountryCode = "US",
                BirthCountryReason = "re",
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "UK",
                EmailAddress = "email@isp.com",
                FullName = new FullName
                {
                    FirstName = "first name",
                    LastName = "last name",
                    PassportName = "passport name",
                    PreferredName = "preferred name",
                    Suffix = FullNameValidator.SECOND_SUFFIX
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                PermanentResidenceCountryCode = "FR",
                PrintForm = true,
                Relationship = "01",
            };
            dependent.SetPersonId(personId);
            dependent.SetParticipantId(participantId);

            var instance = dependent.GetEVPersonTypeDependent();
            Assert.AreEqual(participantId.ToString(), instance.UserDefinedA);
            Assert.AreEqual(personId.ToString(), instance.UserDefinedB);
            
            Assert.AreEqual(dependent.BirthCity, instance.BirthCity);
            Assert.AreEqual(dependent.BirthCountryCode.GetBirthCntryCodeType(), instance.BirthCountryCode);
            Assert.AreEqual(dependent.BirthDate, instance.BirthDate);
            Assert.AreEqual(dependent.CitizenshipCountryCode.GetCountryCodeWithType(), instance.CitizenshipCountryCode);
            Assert.AreEqual(dependent.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(dependent.Gender.GetEVGenderCodeType(), instance.Gender);
            Assert.AreEqual(dependent.PermanentResidenceCountryCode.GetCountryCodeWithType(), instance.PermanentResidenceCountryCode);
            Assert.AreEqual(dependent.Relationship.GetDependentCodeType(), instance.Relationship);
            Assert.AreEqual(dependent.UserDefinedA, instance.UserDefinedA);
            Assert.AreEqual(dependent.UserDefinedB, instance.UserDefinedB);
            Assert.IsFalse(instance.BirthCountryReasonSpecified);
        }
    }
}
