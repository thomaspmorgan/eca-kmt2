using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Business.Service;
using ECA.Data;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class AdditionalPersonAddressTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var userId = 1;
            var user = new User(userId);
            var addressTypeId = AddressType.Business.Id;
            var isPrimary = true;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = 2;
            var cityId = 3;
            var divisionId = 4;
            var personId = 5;

            var instance = new AdditionalPersonAddress(
                user,
                addressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId,
                personId
                );
            Assert.AreEqual(personId, instance.GetAddressableEntityId());
            //ensure base constructor called
            Assert.AreEqual(street1, instance.Street1);
        }

        [TestMethod]
        public void TestCreateGetAddressesQuery()
        {
            var userId = 1;
            var user = new User(userId);
            var addressTypeId = AddressType.Business.Id;
            var isPrimary = true;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = 2;
            var cityId = 3;
            var divisionId = 4;
            var personId = 5;

            var person = new Person
            {
                PersonId = personId
            };
            var address = new Address
            {
                PersonId = person.PersonId
            };
            var context = new TestEcaContext();
            context.Addresses.Add(address);

            var instance = new AdditionalPersonAddress(
                user,
                addressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId,
                personId
                );

            var testAddresses = instance.CreateGetAddressesQuery(context).ToList();
            Assert.AreEqual(1, testAddresses.Count);
            Assert.IsTrue(Object.ReferenceEquals(address, testAddresses.First()));
        }
    }
}
