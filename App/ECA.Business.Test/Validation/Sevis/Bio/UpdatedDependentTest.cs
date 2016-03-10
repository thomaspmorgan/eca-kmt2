using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using ECA.Business.Validation;
using ECA.Business.Sevis.Model;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class UpdatedDependentTest
    {
        [TestMethod]
        public void TestGetSevisExhangeVisitorDependentInstance()
        {
            var dependent = new UpdatedDependent
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
                Remarks = "remarks",
                IsRelationshipFieldSpecified = true,
                SevisId = "sevis id"
            };

            var instance = dependent.GetSevisExhangeVisitorDependentInstance();
            Assert.IsInstanceOfType(instance, typeof(SEVISEVBatchTypeExchangeVisitorDependentEdit));
            var sevisModel = (SEVISEVBatchTypeExchangeVisitorDependentEdit)instance;

            Assert.AreEqual(dependent.BirthCity, sevisModel.BirthCity);
            Assert.AreEqual(dependent.BirthCountryCode.GetBirthCntryCodeType(), sevisModel.BirthCountryCode);
            Assert.AreEqual(dependent.BirthDate, sevisModel.BirthDate);
            Assert.AreEqual(dependent.CitizenshipCountryCode.GetCountryCodeWithType(), sevisModel.CitizenshipCountryCode);
            Assert.AreEqual(dependent.EmailAddress, sevisModel.EmailAddress);
            Assert.AreEqual(dependent.Gender.GetEVGenderCodeType(), sevisModel.Gender);
            Assert.AreEqual(dependent.PermanentResidenceCountryCode.GetCountryCodeWithType(), sevisModel.PermanentResidenceCountryCode);
            Assert.AreEqual(dependent.Relationship.GetDependentCodeType(), sevisModel.Relationship);
            Assert.AreEqual(dependent.PrintForm, sevisModel.printForm);
            Assert.AreEqual(dependent.Remarks, sevisModel.Remarks);
            Assert.AreEqual(dependent.SevisId, sevisModel.dependentSevisID);

            Assert.IsTrue(sevisModel.RelationshipSpecified);            
            Assert.IsFalse(sevisModel.BirthCountryReasonSpecified);
        }
    }
}
