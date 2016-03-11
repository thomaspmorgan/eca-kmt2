using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis;
using ECA.Business.Queries.Models.Persons;
using ECA.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Validation;

namespace ECA.Business.Test.Validation.Sevis
{
    [TestClass]
    public class NewExchangeVisitorTest
    {

        //[TestMethod]
        //public void TestGetSEVISBatchTypeExchangeVisitor_CheckEVPersonTypeBiographical()
        //{
        //    var birthCountryCode = "FR";
        //    var citizenshipCountryCode = "CN";
        //    var mailAddress = new AddressDTO
        //    {
        //        AddressId = 10,
        //        Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME,
        //    };
        //    var usAddress = new AddressDTO
        //    {
        //        AddressId = 20,
        //        Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME,
        //    };
        //    var residenceCountryCode = "MX";
        //    var positionCode = "100";
        //    var dto = new BiographicalDTO
        //    {
        //        BirthCity = "birth city",
        //        BirthCountryCode = birthCountryCode,
        //        BirthDate = DateTime.Now,
        //        CitizenshipCountryCode = citizenshipCountryCode,
        //        EmailAddress = "somone@isp.com",
        //        FullName = new FullNameDTO
        //        {
        //            FirstName = "first name",
        //            LastName = "last name",
        //            PassportName = "passport",
        //            PreferredName = "preferred",
        //            Suffix = "Jr."
        //        },
        //        Gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE,
        //        MailAddress = mailAddress,
        //        NumberOfCitizenships = 1,
        //        PermanentResidenceCountryCode = residenceCountryCode,
        //        PersonId = 20,
        //        PhoneNumber = "123-456-7890",
        //        PositionCode = positionCode,
        //    };

        //    var programCategory = new ProgramCategory();
        //    programCategory.ProgramCategoryCode = "1D";

        //    var instance = new NewExchangeVisitor();
        //    instance.SetPerson(dto);
        //    instance.SetCategoryCode(programCategory);

        //    var sevisModel = instance.GetSEVISBatchTypeExchangeVisitor();
        //    Assert.IsNotNull(sevisModel);
        //    Assert.IsNotNull(sevisModel.Biographical);

        //    Assert.AreEqual(dto.BirthCity, sevisModel.Biographical.BirthCity);
        //    Assert.AreEqual(dto.BirthCountryCode.GetBirthCntryCodeType(), sevisModel.Biographical.BirthCountryCode);
        //    Assert.IsFalse(sevisModel.Biographical.BirthCountryReasonSpecified);
        //    Assert.AreEqual(dto.BirthDate.Value, sevisModel.Biographical.BirthDate);
        //    Assert.AreEqual(dto.CitizenshipCountryCode.GetCountryCodeWithType(), sevisModel.Biographical.CitizenshipCountryCode);
        //    Assert.AreEqual(dto.EmailAddress, sevisModel.Biographical.EmailAddress);            
        //    Assert.AreEqual(dto.Gender.GetEVGenderCodeType(), sevisModel.Biographical.Gender);
        //    Assert.AreEqual(dto.PermanentResidenceCountryCode.GetCountryCodeWithType(), sevisModel.Biographical.PermanentResidenceCountryCode);
        //    Assert.AreEqual(dto.PhoneNumber, sevisModel.Biographical.PhoneNumber);

        //    Assert.IsNotNull(sevisModel.Biographical.FullName);
        //    Assert.AreEqual(dto.FullName.FirstName, sevisModel.Biographical.FullName.FirstName);
        //    Assert.AreEqual(dto.FullName.LastName, sevisModel.Biographical.FullName.LastName);
        //    Assert.AreEqual(dto.FullName.PassportName, sevisModel.Biographical.FullName.PassportName);
        //    Assert.AreEqual(dto.FullName.PreferredName, sevisModel.Biographical.FullName.PreferredName);
        //    Assert.AreEqual(dto.FullName.Suffix.GetNameSuffixCodeType(), sevisModel.Biographical.FullName.Suffix);
        //}

        //[TestMethod]
        //public void TestGetSEVISBatchTypeExchangeVisitor_CheckCategoryCode()
        //{
        //    var birthCountryCode = "FR";
        //    var citizenshipCountryCode = "CN";
        //    var mailAddress = new AddressDTO
        //    {
        //        AddressId = 10,
        //        Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME,
        //    };
        //    var usAddress = new AddressDTO
        //    {
        //        AddressId = 20,
        //        Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME,
        //    };
        //    var residenceCountryCode = "MX";
        //    var positionCode = "100";
        //    var dto = new BiographicalDTO
        //    {
        //        BirthCity = "birth city",
        //        BirthCountryCode = birthCountryCode,
        //        BirthDate = DateTime.Now,
        //        CitizenshipCountryCode = citizenshipCountryCode,
        //        EmailAddress = "somone@isp.com",
        //        FullName = new FullNameDTO
        //        {
        //            FirstName = "first name",
        //            LastName = "last name",
        //            PassportName = "passport",
        //            PreferredName = "preferred",
        //            Suffix = "Jr."
        //        },
        //        Gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE,
        //        MailAddress = mailAddress,
        //        NumberOfCitizenships = 1,
        //        PermanentResidenceCountryCode = residenceCountryCode,
        //        PersonId = 20,
        //        PhoneNumber = "123-456-7890",
        //        PositionCode = positionCode,
        //    };
        //    var programCategory = new ProgramCategory();
        //    programCategory.ProgramCategoryCode = "1D";

        //    var instance = new NewExchangeVisitor();
        //    instance.SetPerson(dto);
        //    instance.SetCategoryCode(programCategory);

        //    var sevisModel = instance.GetSEVISBatchTypeExchangeVisitor();
        //    Assert.AreEqual(programCategory.ProgramCategoryCode.GetEVCategoryCodeType(), sevisModel.CategoryCode);
        //}

        //[TestMethod]
        //public void TestGetSEVISBatchTypeExchangeVisitor_CheckCategoryCode_CheckDependents()
        //{
        //    Assert.Fail("here");
        //}
    }
}
