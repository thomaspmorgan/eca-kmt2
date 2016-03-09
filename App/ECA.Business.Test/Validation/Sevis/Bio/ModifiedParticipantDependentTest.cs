using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Sevis.Model;
using ECA.Data;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class ModifiedParticipantDependentTest
    {
        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorDependent()
        {
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
                UserDefinedA = "defined a",
                UserDefinedB = "defined b"
            };

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

        [TestMethod]
        public void TestGetSevisEvBatchTypeExchangeVisitorUpdateComponent()
        {
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
                UserDefinedA = "defined a",
                UserDefinedB = "defined b"
            };

            var modifiedParticipantDependent = new ModifiedParticipantDependent();
            modifiedParticipantDependent.Dependent = dependent;
            modifiedParticipantDependent.UserDefinedA = "a";
            modifiedParticipantDependent.UserDefinedB = "b";

            var instance = modifiedParticipantDependent.GetSevisEvBatchTypeExchangeVisitorUpdateComponent();
            Assert.IsNotNull(instance);
            Assert.IsInstanceOfType(instance, typeof(SEVISEVBatchTypeExchangeVisitorDependent));
        }
    }
}
