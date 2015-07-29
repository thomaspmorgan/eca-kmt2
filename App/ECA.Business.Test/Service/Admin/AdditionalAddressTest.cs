using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ECA.Business.Service;
using ECA.Business.Service.Admin;

namespace ECA.Business.Test.Service.Admin
{
    public class AdditionalAddressTestClass : AdditionalAddress<AddressableTestClass>
    {
        private int testId;

        public AdditionalAddressTestClass(
            User creator,
            int addressTypeId,
            bool isPrimary,
            string street1,
            string street2,
            string street3,
            string postalCode,
            string locationName,
            int countryId,
            int cityId,
            int divisionId,
            int testId
            )
            : base(creator, addressTypeId, isPrimary, street1, street2, street3, postalCode, locationName, countryId, cityId, divisionId)
        {
            this.testId = testId;
        }


        public override int GetAddressableEntityId()
        {
            return testId;
        }

        public override IQueryable<Address> CreateGetAddressesQuery(EcaContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class AddressableTestClass : IAddressable
    {
        public AddressableTestClass()
        {
            this.Addresses = new List<Address>();
        }

        public int Id { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public int GetId()
        {
            return Id;
        }
    }

    [TestClass]
    public class AdditionalAddressTest
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
            var testId = 5;
            var instance = new AdditionalAddressTestClass(
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
                testId
                );
            Assert.IsNotNull(instance.Create);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Create.User));
            //Assert base constructor is called
            Assert.IsNotNull(instance.Street1);
        }

        [TestMethod]
        public void TestGetLocation()
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
            var testId = 5;
            var instance = new AdditionalAddressTestClass(
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
                testId
                );

            var location = instance.GetLocation();
            Assert.IsNotNull(location);
            Assert.AreEqual(LocationType.Address.Id, location.LocationTypeId);
            Assert.AreEqual(cityId, location.CityId);
            Assert.AreEqual(countryId, location.CountryId);
            Assert.AreEqual(divisionId, location.DivisionId);
            Assert.AreEqual(locationName, location.LocationName);
            Assert.AreEqual(postalCode, location.PostalCode);
            Assert.AreEqual(street1, location.Street1);
            Assert.AreEqual(street2, location.Street2);
            Assert.AreEqual(street3, location.Street3);
            Assert.AreEqual(0, location.LocationId);
            Assert.AreEqual(1, location.History.CreatedBy);
            Assert.AreEqual(1, location.History.RevisedBy);
            DateTimeOffset.Now.Should().BeCloseTo(location.History.CreatedOn, 2000);
            DateTimeOffset.Now.Should().BeCloseTo(location.History.RevisedOn, 2000);
        }

        [TestMethod]
        public void TestGetAddress()
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
            var testId = 5;
            var instance = new AdditionalAddressTestClass(
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
                testId
                );

            var address = instance.GetAddress();
            Assert.IsNotNull(address);
            Assert.IsNotNull(address.Location);
            Assert.AreEqual(addressTypeId, address.AddressTypeId);
            Assert.AreEqual(isPrimary, address.IsPrimary);
            Assert.AreEqual(1, address.History.CreatedBy);
            Assert.AreEqual(1, address.History.RevisedBy);
            DateTimeOffset.Now.Should().BeCloseTo(address.History.CreatedOn, 2000);
            DateTimeOffset.Now.Should().BeCloseTo(address.History.RevisedOn, 2000);
        }

        [TestMethod]
        public void TestAddAddress()
        {
            var addressable = new AddressableTestClass();
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
            var testId = 5;
            var instance = new AdditionalAddressTestClass(
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
                testId
                );

            Assert.AreEqual(0, addressable.Addresses.Count);
            instance.AddAddress(addressable);

            Assert.AreEqual(1, addressable.Addresses.Count);
            var address = addressable.Addresses.First();
            Assert.IsNotNull(address.Location);
            var location = address.Location;

            Assert.AreEqual(LocationType.Address.Id, location.LocationTypeId);
            Assert.AreEqual(cityId, location.CityId);
            Assert.AreEqual(countryId, location.CountryId);
            Assert.AreEqual(divisionId, location.DivisionId);
            Assert.AreEqual(locationName, location.LocationName);
            Assert.AreEqual(postalCode, location.PostalCode);
            Assert.AreEqual(street1, location.Street1);
            Assert.AreEqual(street2, location.Street2);
            Assert.AreEqual(street3, location.Street3);
            Assert.AreEqual(0, location.LocationId);
            Assert.AreEqual(addressTypeId, address.AddressTypeId);
            Assert.AreEqual(isPrimary, address.IsPrimary);
            Assert.AreEqual(1, address.History.CreatedBy);
            Assert.AreEqual(1, address.History.RevisedBy);
            DateTimeOffset.Now.Should().BeCloseTo(address.History.CreatedOn, 2000);
            DateTimeOffset.Now.Should().BeCloseTo(address.History.RevisedOn, 2000);
        }
    }
}
