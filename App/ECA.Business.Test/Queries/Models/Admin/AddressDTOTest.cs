using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using FluentAssertions;
using ECA.Business.Validation;

namespace ECA.Business.Test.Queries.Model.Admin
{
    [TestClass]
    public class AddressDTOTest
    {
        [TestMethod]
        public void TestToString_HasStreetsCityDivisionCountryPostalCode()
        {
            var dto = new AddressDTO();
            dto.Street1 = "1";
            dto.Street2 = "2";
            dto.Street3 = "3";
            dto.City = "city";
            dto.Division = "division";
            dto.Country = "country";
            dto.PostalCode = "12345";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.Street1));
            Assert.IsTrue(s.Contains(dto.Street2));
            Assert.IsTrue(s.Contains(dto.Street3));
            Assert.IsTrue(s.Contains(dto.City));
            Assert.IsTrue(s.Contains(dto.Division));
            Assert.IsTrue(s.Contains(dto.Country));
            Assert.IsTrue(s.Contains(dto.PostalCode));
        }

        [TestMethod]
        public void TestToString_DoesNotContainStreets()
        {
            var dto = new AddressDTO();
            dto.City = "city";
            dto.Division = "division";
            dto.Country = "country";
            dto.PostalCode = "12345";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.City));
            Assert.IsTrue(s.Contains(dto.Division));
            Assert.IsTrue(s.Contains(dto.Country));
            Assert.IsTrue(s.Contains(dto.PostalCode));
        }

        [TestMethod]
        public void TestToString_OnlyHasCity()
        {
            var dto = new AddressDTO();
            dto.City = "city";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.City));
        }

        [TestMethod]
        public void TestToString_OnlyHasCountry()
        {
            var dto = new AddressDTO();
            dto.Country = "Country";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.Country));
        }

        [TestMethod]
        public void TestToString_OnlyHasDivision()
        {
            var dto = new AddressDTO();
            dto.Division = "Division";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.Division));
        }

        [TestMethod]
        public void TestToString_OnlyHasPostCode()
        {
            var dto = new AddressDTO();
            dto.PostalCode = "PostalCode";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.PostalCode));
        }

        [TestMethod]
        public void TestGetUSAddress()
        {
            var dto = new AddressDTO();
            dto.Street1 = "1";
            dto.Street2 = "2";
            dto.Street3 = "3";
            dto.City = "city";
            dto.Division = "Florida";
            dto.DivisionIso = "FL";
            dto.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            dto.PostalCode = "12345";

            var instance = dto.GetUSAddress();
            Assert.AreEqual(dto.Street1, instance.Address1);
            Assert.AreEqual(dto.Street2, instance.Address2);
            Assert.AreEqual(dto.City, instance.City);
            Assert.AreEqual(dto.DivisionIso, instance.State);
            Assert.AreEqual(dto.PostalCode, instance.PostalCode);
            Assert.IsNull(instance.Explanation);
            Assert.IsNull(instance.ExplanationCode);
        }

        [TestMethod]
        public void TestGetUSAddress_CountryIsNotUnitedStates()
        {
            var dto = new AddressDTO();
            dto.Street1 = "1";
            dto.Street2 = "2";
            dto.Street3 = "3";
            dto.City = "city";
            dto.Division = "FL";
            dto.Country = "country";
            dto.PostalCode = "12345";
            dto.AddressId = 10;

            Action a = () => dto.GetUSAddress();
            a.ShouldThrow<NotSupportedException>().WithMessage(String.Format("The address with id [{0}] is not in the united states.", dto.AddressId));
        }

        [TestMethod]
        public void TestGetUSAddress_CountryIsNull()
        {
            var dto = new AddressDTO();
            dto.Street1 = "1";
            dto.Street2 = "2";
            dto.Street3 = "3";
            dto.City = "city";
            dto.Division = "FL";
            dto.Country = null;
            dto.PostalCode = "12345";
            dto.AddressId = 10;

            Action a = () => dto.GetUSAddress();
            a.ShouldThrow<NotSupportedException>().WithMessage(String.Format("The address with id [{0}] is not in the united states.", dto.AddressId));
        }

        [TestMethod]
        public void TestGetUSAddress_CountryIsWhitespace()
        {
            var dto = new AddressDTO();
            dto.Street1 = "1";
            dto.Street2 = "2";
            dto.Street3 = "3";
            dto.City = "city";
            dto.Division = "FL";
            dto.Country = " ";
            dto.PostalCode = "12345";
            dto.AddressId = 10;

            Action a = () => dto.GetUSAddress();
            a.ShouldThrow<NotSupportedException>().WithMessage(String.Format("The address with id [{0}] is not in the united states.", dto.AddressId));
        }

        [TestMethod]
        public void TestGetUSAddress_CountryIsEmpty()
        {
            var dto = new AddressDTO();
            dto.Street1 = "1";
            dto.Street2 = "2";
            dto.Street3 = "3";
            dto.City = "city";
            dto.Division = "FL";
            dto.Country = String.Empty;
            dto.PostalCode = "12345";
            dto.AddressId = 10;

            Action a = () => dto.GetUSAddress();
            a.ShouldThrow<NotSupportedException>().WithMessage(String.Format("The address with id [{0}] is not in the united states.", dto.AddressId));
        }
    }
}
