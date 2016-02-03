using ECA.Business.Models.Fundings;
using ECA.Business.Service.Fundings;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
        public void TestDoValidateCreate()
        {
            var fullName = "full name";
            var position = "position";
            var likeEmailAddressCount = 0;
            Func<AdditionalPointOfContactValidationEntity> createEntity = () =>
            {
                return new AdditionalPointOfContactValidationEntity(
                    fullName: fullName,
                    position: position,
                    likeEmailAddressCount: likeEmailAddressCount
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());
        }

        #endregion

        #region Update
        [TestMethod]
        public void TestDoValidateUpdate()
        {
            var fullName = "full name";
            var position = "position";
            var likeEmailAddressCount = 0;
            Func<AdditionalPointOfContactValidationEntity> createEntity = () =>
            {
                return new AdditionalPointOfContactValidationEntity(
                    fullName: fullName,
                    position: position,
                    likeEmailAddressCount: likeEmailAddressCount
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());
        }
        #endregion
    }
}
