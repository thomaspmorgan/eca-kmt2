using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Sevis.Model;
using ECA.Business.Validation;
using FluentAssertions;
using ECA.Business.Validation.Sevis.Exceptions;

namespace ECA.Business.Test.Validation
{
    [TestClass]
    public class SevisEnumHelperTest
    {

        [TestInitialize]
        public void Test()
        {
            
        }

        public string GetErrorMessage(Type t, string value)
        {
            return string.Format("The code type [{0}] could not be parsed from the given value [{1}].", t, value);
        }

        #region BirthCntryCodeType
        [TestMethod]
        public void TestGetBirthCntryCodeType()
        {
            var e = BirthCntryCodeType.AC;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetBirthCntryCodeType());
        }

        [TestMethod]
        public void TestGetBirthCntryCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetBirthCntryCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(BirthCntryCodeType), value));
        }
        #endregion

        #region EVGenderCodeType
        [TestMethod]
        public void TestGetEVGenderCodeType()
        {
            var e = EVGenderCodeType.F;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetEVGenderCodeType());
        }

        [TestMethod]
        public void TestGetEVGenderCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetEVGenderCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(EVGenderCodeType), value));
        }
        #endregion

        #region GenderCodeType
        [TestMethod]
        public void TestGetGenderCodeType()
        {
            var e = GenderCodeType.M;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetGenderCodeType());
        }

        [TestMethod]
        public void TestGetGenderCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetGenderCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(GenderCodeType), value));
        }
        #endregion

        #region EVCategoryCodeType
        [TestMethod]
        public void TestGetEVCategoryCodeType()
        {
            var e = EVCategoryCodeType.Item05;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetEVCategoryCodeType());
        }

        [TestMethod]
        public void TestGetEVCategoryCodeType_ValueDoesNotStartWithItem()
        {
            var e = EVCategoryCodeType.Item05;
            var value = e.ToString().Replace("Item", "");
            Assert.AreEqual(e, value.GetEVCategoryCodeType());
        }

        [TestMethod]
        public void TestGetEVCategoryCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetEVCategoryCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(EVCategoryCodeType), "Item" + value));
        }
        #endregion

        #region CntryCodeWithoutType
        [TestMethod]
        public void TestGetCountryCodeWithType()
        {
            var e = CntryCodeWithoutType.AE;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetCountryCodeWithType());
        }

        [TestMethod]
        public void TestGetCountryCodeWithType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetCountryCodeWithType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(CntryCodeWithoutType), value));
        }
        #endregion

        #region StateCodeType
        [TestMethod]
        public void TestGetStateCodeType()
        {
            var e = StateCodeType.AZ;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetStateCodeType());
        }

        [TestMethod]
        public void TestGetStateCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetStateCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(StateCodeType), value));
        }
        #endregion

        #region ProgSubjectCodeType
        [TestMethod]
        public void TestGetProgSubjectCodeType()
        {
            var e = ProgSubjectCodeType.Item010103;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetProgSubjectCodeType());
        }

        [TestMethod]
        public void TestGetProgSubjectCodeType_DoesNotContainItem()
        {
            var e = ProgSubjectCodeType.Item010103;
            var value = e.ToString().Replace("Item", "");
            Assert.AreEqual(e, value.GetProgSubjectCodeType());
        }

        [TestMethod]
        public void TestGetProgSubjectCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetProgSubjectCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(ProgSubjectCodeType), "Item" + value));
        }
        #endregion

        #region EVOccupationCategoryCodeType
        [TestMethod]
        public void TestGetEVOccupationCategoryCodeType()
        {
            var e = EVOccupationCategoryCodeType.Item04;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetEVOccupationCategoryCodeType());
        }

        [TestMethod]
        public void TestGetEVOccupationCategoryCodeType_DoesNotContainItem()
        {
            var e = EVOccupationCategoryCodeType.Item04;
            var value = e.ToString().Replace("Item", "");
            Assert.AreEqual(e, value.GetEVOccupationCategoryCodeType());
        }

        [TestMethod]
        public void TestGetEVOccupationCategoryCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetEVOccupationCategoryCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(EVOccupationCategoryCodeType), "Item" + value));
        }
        #endregion

        #region GovAgencyCodeType
        [TestMethod]
        public void TestGetGovAgencyCodeType()
        {
            var e = GovAgencyCodeType.DOD;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetGovAgencyCodeType());
        }

        [TestMethod]
        public void TestGetGovAgencyCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetGovAgencyCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(GovAgencyCodeType), value));
        }
        #endregion

        #region InternationalOrgCodeType
        [TestMethod]
        public void TestGetInternationalOrgCodeType()
        {
            var e = InternationalOrgCodeType.ECOSOC;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetInternationalOrgCodeType());
        }

        [TestMethod]
        public void TestGetInternationalOrgCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetInternationalOrgCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(InternationalOrgCodeType), value));
        }
        #endregion

        #region NameSuffixCodeType
        [TestMethod]
        public void TestGetNameSuffixCodeType()
        {
            var e = NameSuffixCodeType.III;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetNameSuffixCodeType());
        }

        [TestMethod]
        public void TestGetNameSuffixCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetNameSuffixCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(NameSuffixCodeType), value));
        }
        #endregion

        #region DependentCodeType
        [TestMethod]
        public void TestGetDependentCodeType()
        {
            var e = DependentCodeType.Item02;
            var value = e.ToString();
            Assert.AreEqual(e, value.GetDependentCodeType());
        }

        [TestMethod]
        public void TestGetDependentCodeType_DoesNotContainItem()
        {
            var e = DependentCodeType.Item02;
            var value = e.ToString().Replace("Item", "");
            Assert.AreEqual(e, value.GetDependentCodeType());
        }

        [TestMethod]
        public void TestGetDependentCodeType_StringNotSupported()
        {
            var value = "hello world";
            Action a = () => value.GetDependentCodeType();
            a.ShouldThrow<CodeTypeConversionException>().WithMessage(GetErrorMessage(typeof(DependentCodeType), "Item" + value));
        }
        #endregion
    }
}
