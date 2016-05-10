using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ContactServiceValidatorTest
    {
        private ContactServiceValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new ContactServiceValidator();
        }
        #region Create
        
        [TestMethod]
        public void TestDoValidateCreate_MoreThanOnePrimaryEmail()
        {
            var fullName = "full name";
            var position = "position";
            var numberOfPrimaryEmailAddresses = 1;
            var numberOfPrimaryPhoneNumbers = 1;
            Func<AdditionalPointOfContactValidationEntity> createEntity = () =>
            {
                return new AdditionalPointOfContactValidationEntity(
                    fullName: fullName,
                    position: position,
                    numberOfPrimaryEmailAddresses: numberOfPrimaryEmailAddresses,
                    numberOfPrimaryPhoneNumbers: numberOfPrimaryPhoneNumbers
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());
            numberOfPrimaryEmailAddresses = 2;

            var entity = createEntity();
            var results = validator.DoValidateCreate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(PropertyHelper.GetPropertyName<AdditionalPointOfContact>(x => x.EmailAddresses), results.First().Property);
            Assert.AreEqual(ContactServiceValidator.MORE_THAN_ONE_PRIMARY_EMAIL_ADDRESS_ERROR, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_MoreThanOnePrimaryPhoneNumber()
        {
            var fullName = "full name";
            var position = "position";
            var numberOfPrimaryEmailAddresses = 1;
            var numberOfPrimaryPhoneNumbers = 1;
            Func<AdditionalPointOfContactValidationEntity> createEntity = () =>
            {
                return new AdditionalPointOfContactValidationEntity(
                    fullName: fullName,
                    position: position,
                    numberOfPrimaryEmailAddresses: numberOfPrimaryEmailAddresses,
                    numberOfPrimaryPhoneNumbers: numberOfPrimaryPhoneNumbers
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());
            numberOfPrimaryPhoneNumbers = 2;

            var entity = createEntity();
            var results = validator.DoValidateCreate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(PropertyHelper.GetPropertyName<AdditionalPointOfContact>(x => x.PhoneNumbers), results.First().Property);
            Assert.AreEqual(ContactServiceValidator.MORE_THAN_ONE_PRIMARY_PHONE_NUMBER_ERROR, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_FullNameIsNull()
        {
            string fullName = "full name";
            var position = "position";
            var numberOfPrimaryEmailAddresses = 1;
            var numberOfPrimaryPhoneNumbers = 1;
            Func<AdditionalPointOfContactValidationEntity> createEntity = () =>
            {
                return new AdditionalPointOfContactValidationEntity(
                    fullName: fullName,
                    position: position,
                    numberOfPrimaryEmailAddresses: numberOfPrimaryEmailAddresses,
                    numberOfPrimaryPhoneNumbers: numberOfPrimaryPhoneNumbers
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());
            fullName = null;

            var entity = createEntity();
            var results = validator.DoValidateCreate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(PropertyHelper.GetPropertyName<AdditionalPointOfContact>(x => x.FullName), results.First().Property);
            Assert.AreEqual(ContactServiceValidator.POINT_OF_CONTACT_MUST_HAVE_A_FULL_NAME_VALUE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_FullNameIsEmpty()
        {
            string fullName = "full name";
            var position = "position";
            var numberOfPrimaryEmailAddresses = 1;
            var numberOfPrimaryPhoneNumbers = 1;
            Func<AdditionalPointOfContactValidationEntity> createEntity = () =>
            {
                return new AdditionalPointOfContactValidationEntity(
                    fullName: fullName,
                    position: position,
                    numberOfPrimaryEmailAddresses: numberOfPrimaryEmailAddresses,
                    numberOfPrimaryPhoneNumbers: numberOfPrimaryPhoneNumbers
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());
            fullName = String.Empty;

            var entity = createEntity();
            var results = validator.DoValidateCreate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(PropertyHelper.GetPropertyName<AdditionalPointOfContact>(x => x.FullName), results.First().Property);
            Assert.AreEqual(ContactServiceValidator.POINT_OF_CONTACT_MUST_HAVE_A_FULL_NAME_VALUE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_FullNameIsWhitespace()
        {
            string fullName = "full name";
            var position = "position";
            var numberOfPrimaryEmailAddresses = 1;
            var numberOfPrimaryPhoneNumbers = 1;
            Func<AdditionalPointOfContactValidationEntity> createEntity = () =>
            {
                return new AdditionalPointOfContactValidationEntity(
                    fullName: fullName,
                    position: position,
                    numberOfPrimaryEmailAddresses: numberOfPrimaryEmailAddresses,
                    numberOfPrimaryPhoneNumbers: numberOfPrimaryPhoneNumbers
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());
            fullName = " ";

            var entity = createEntity();
            var results = validator.DoValidateCreate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(PropertyHelper.GetPropertyName<AdditionalPointOfContact>(x => x.FullName), results.First().Property);
            Assert.AreEqual(ContactServiceValidator.POINT_OF_CONTACT_MUST_HAVE_A_FULL_NAME_VALUE, results.First().ErrorMessage);
        }
        #endregion

        #region Update
        [TestMethod]
        public void TestDoValidateUpdate()
        {
            var fullName = "full name";
            var position = "position";
            var numberOfPrimaryEmailAddresses = 0;
            var numberOfPrimaryPhoneNumbers = 0;
            Func<AdditionalPointOfContactValidationEntity> createEntity = () =>
            {
                return new AdditionalPointOfContactValidationEntity(
                    fullName: fullName,
                    position: position,
                    numberOfPrimaryEmailAddresses: numberOfPrimaryEmailAddresses,
                    numberOfPrimaryPhoneNumbers: numberOfPrimaryPhoneNumbers
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());
        }
        #endregion
    }
}
