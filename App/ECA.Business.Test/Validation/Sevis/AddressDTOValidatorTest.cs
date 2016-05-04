using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Validation.Sevis
{
    [TestClass]
    public class AddressDTOValidatorTest
    {
        public AddressDTO GetValidUSAddress()
        {
            return new AddressDTO
            {
                Street1 = "address 1",
                Street2 = "address 2",
                City = "city",
                Division = "state",
                PostalCode = "12345",
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME
            };
        }

        [TestMethod]
        public void TestDefaultConstructor()
        {
            var validator = new AddressDTOValidator();
            Assert.IsNotNull(validator.AddressNameDelegate);
            Assert.AreEqual(AddressDTOValidator.C_STREET_ADDRESS, validator.AddressNameDelegate(null));
        }

        [TestMethod]
        public void TestConstructor_AddressNameDelegate()
        {
            Func<AddressDTO, object> d = (a) => "hello world";
            var validator = new AddressDTOValidator(d);
            Assert.AreEqual(d(null), validator.AddressNameDelegate(null));
        }

        [TestMethod]
        public void TestStreet1_Null()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Street1 = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(string.Format(AddressDTOValidator.ADDRESS_1_ERROR_MESSAGE, name, AddressDTOValidator.ADDRESS_MAX_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestStreet1_ExceedsMaxLength()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Street1 = new string('c', AddressDTOValidator.ADDRESS_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(string.Format(AddressDTOValidator.ADDRESS_1_ERROR_MESSAGE, name, AddressDTOValidator.ADDRESS_MAX_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestStreet2_Null()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Street2 = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestStreet2_ExceedsMaxLength()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Street2 = new String('c', AddressDTOValidator.ADDRESS_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(string.Format(AddressDTOValidator.ADDRESS_2_ERROR_MESSAGE, name, AddressDTOValidator.ADDRESS_MAX_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestCountry_IsNotUnitedStates()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Country = "hello world";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(string.Format(AddressDTOValidator.COUNTRY_ERROR_MESSAGE, name, LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestPostalCode_PostalCodeIsFiveDigits()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = "12345";
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestPostalCode_PostalCodeHasDashAndFourDigitsAfter()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = "12345-6789";
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestPostalCode_PostalCodeDoesNotHaveDash_HasFourDigitsAfter()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = "12345 6789";
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestPostalCode_Null()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(AddressDTOValidator.POSTAL_CODE_ERROR_MESSAGE, name, AddressDTOValidator.POSTAL_CODE_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestPostalCode_AllDigitsExceedsLength()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = new String('1', AddressDTOValidator.POSTAL_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(AddressDTOValidator.POSTAL_CODE_ERROR_MESSAGE, name, AddressDTOValidator.POSTAL_CODE_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestPostalCode_DoesNotContainDigits()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = new String('a', AddressDTOValidator.POSTAL_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(AddressDTOValidator.POSTAL_CODE_ERROR_MESSAGE, name, AddressDTOValidator.POSTAL_CODE_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestPostalCode_DoesNotHaveRequiredNumberOfDigits()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = new String('1', AddressDTOValidator.POSTAL_CODE_LENGTH - 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(AddressDTOValidator.POSTAL_CODE_ERROR_MESSAGE, name, AddressDTOValidator.POSTAL_CODE_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }


        [TestMethod]
        public void TestCity_ExceedsMaxLength()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.City = new string('c', AddressDTOValidator.CITY_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(string.Format(AddressDTOValidator.CITY_ERROR_MESSAGE, name, AddressDTOValidator.CITY_MAX_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestCity_IsEmpty()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.City = string.Empty;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestCity_IsNull()
        {
            var name = "address name";
            var validator = new AddressDTOValidator((a) => name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.City = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }
    }
}
