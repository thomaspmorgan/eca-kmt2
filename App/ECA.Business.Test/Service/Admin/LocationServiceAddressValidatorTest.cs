using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class LocationServiceAddressValidatorTest
    {
        #region DoValidateCreate
    
        
        [TestMethod]
        public void TestValidateCreate_InvalidAddressTypeId()
        {
            var addressTypeId = AddressType.Home.Id;
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId);

            var validator = new LocationServiceAddressValidator();
            Assert.AreEqual(0, validator.ValidateCreate(entity).Count());

            addressTypeId = 0;
            entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId);
            var results = validator.DoValidateCreate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("AddressTypeId", results.First().Property);
            Assert.AreEqual(LocationServiceAddressValidator.INVALID_ADDRESS_TYPE_MESSAGE, results.First().ErrorMessage);
        }

        #endregion

        #region DoValidateUpdate
     

        [TestMethod]
        public void TestValidateUpdate_InvalidAddressTypeId()
        {
            var addressTypeId = AddressType.Home.Id;
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId);

            var validator = new LocationServiceAddressValidator();
            Assert.AreEqual(0, validator.ValidateCreate(entity).Count());

            addressTypeId = 0;
            entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId);
            var results = validator.DoValidateUpdate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("AddressTypeId", results.First().Property);
            Assert.AreEqual(LocationServiceAddressValidator.INVALID_ADDRESS_TYPE_MESSAGE, results.First().ErrorMessage);
        }
        #endregion
    }
}
