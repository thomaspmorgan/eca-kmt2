using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class DocumentConfigurationTest
    {
        private TestDocumentConfiguration configuration;

        [TestInitialize]
        public void TestInit()
        {
            configuration = new TestDocumentConfiguration(false);
        }


        [TestMethod]
        public void TestGetId()
        {
            configuration.HasKey(x => x.Id);

            var instance = new TestDocument();
            instance.Id = 1;
            Assert.AreEqual(instance.Id, configuration.GetId(instance));
        }

        [TestMethod]
        public void TestGetId_HasKeyNotConfigured()
        {
            var instance = new TestDocument();
            Action a = () => configuration.GetId(instance);
            var message = "The id has not been configured.  Use the HasKey method.";
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestHasKey()
        {
            Assert.IsNull(configuration.IdDelegate);

            configuration.HasKey(x => x.Id);

            Assert.IsNotNull(configuration.IdDelegate);
            var instance = new TestDocument();
            instance.Id = 1;
            Assert.AreEqual(instance.Id, configuration.IdDelegate(instance));
        }

        [TestMethod]
        public void TestIsDocumentType()
        {
            var documentTypeId = Guid.NewGuid();
            var documentTypeName = "hello";
            configuration.IsDocumentType(documentTypeId, documentTypeName);

            Assert.AreEqual(documentTypeId, configuration.GetDocumentTypeId());
            Assert.AreEqual(documentTypeName, configuration.GetDocumentTypeName());
        }

        [TestMethod]
        public void TestHasStartDate()
        {
            Assert.IsNull(configuration.StartDateDelegate);
            configuration.HasStartDate(x => x.StartDate);
            Assert.IsNotNull(configuration.StartDateDelegate);

            var instance = new TestDocument();
            instance.StartDate = DateTimeOffset.Now;
            Assert.AreEqual(instance.StartDate, configuration.StartDateDelegate(instance));
        }

        [TestMethod]
        public void TestHasEndDate()
        {
            Assert.IsNull(configuration.EndDateDelegate);
            configuration.HasEndDate(x => x.EndDate);
            Assert.IsNotNull(configuration.EndDateDelegate);

            var instance = new TestDocument();
            instance.EndDate = DateTimeOffset.Now;
            Assert.AreEqual(instance.EndDate, configuration.EndDateDelegate(instance));
        }

        [TestMethod]
        public void TestHasName()
        {
            Assert.IsNull(configuration.NameDelegate);
            configuration.HasName(x => x.Name);
            Assert.IsNotNull(configuration.NameDelegate);

            var instance = new TestDocument();
            instance.Name = "name";
            Assert.AreEqual(instance.Name, configuration.NameDelegate(instance));
        }

        [TestMethod]
        public void TestHasStatus()
        {
            Assert.IsNull(configuration.StatusDelegate);
            configuration.HasStatus(x => x.Status);
            Assert.IsNotNull(configuration.StatusDelegate);

            var instance = new TestDocument();
            instance.Status = "status";
            Assert.AreEqual(instance.Status, configuration.StatusDelegate(instance));
        }

        [TestMethod]
        public void TestHasDescription()
        {
            Assert.IsNull(configuration.DescriptionDelegate);
            configuration.HasDescription(x => x.Description);
            Assert.IsNotNull(configuration.DescriptionDelegate);

            var instance = new TestDocument();
            instance.Description = "desc";
            Assert.AreEqual(instance.Description, configuration.DescriptionDelegate(instance));
        }

        [TestMethod]
        public void TestHasCountries()
        {
            Assert.IsNull(configuration.CountriesDelegate);
            configuration.HasCountries(x => x.Countries);
            Assert.IsNotNull(configuration.CountriesDelegate);

            var instance = new TestDocument();
            instance.Countries = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Countries, configuration.CountriesDelegate(instance)));
        }


        [TestMethod]
        public void TestHasAddresses()
        {
            Assert.IsNull(configuration.AddressesDelegate);
            configuration.HasAddresses(x => x.Addresses);
            Assert.IsNotNull(configuration.AddressesDelegate);

            var instance = new TestDocument();
            instance.Addresses = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Addresses, configuration.AddressesDelegate(instance)));
        }

        [TestMethod]
        public void TestHasPhoneNumbers()
        {
            Assert.IsNull(configuration.PhoneNumbersDelegate);
            configuration.HasPhoneNumbers(x => x.PhoneNumbers);
            Assert.IsNotNull(configuration.PhoneNumbersDelegate);

            var instance = new TestDocument();
            instance.PhoneNumbers = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.PhoneNumbers, configuration.PhoneNumbersDelegate(instance)));
        }

        [TestMethod]
        public void TestHasRegions()
        {
            Assert.IsNull(configuration.RegionsDelegate);
            configuration.HasRegions(x => x.Regions);
            Assert.IsNotNull(configuration.RegionsDelegate);

            var instance = new TestDocument();
            instance.Regions = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Regions, configuration.RegionsDelegate(instance)));
        }

        [TestMethod]
        public void TestHasLocations()
        {
            Assert.IsNull(configuration.LocationsDelegate);
            configuration.HasLocations(x => x.Locations);
            Assert.IsNotNull(configuration.LocationsDelegate);

            var instance = new TestDocument();
            instance.Locations = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Locations, configuration.LocationsDelegate(instance)));
        }

        [TestMethod]
        public void TestHasWebsites()
        {
            Assert.IsNull(configuration.WebsitesDelegate);
            configuration.HasWebsites(x => x.Websites);
            Assert.IsNotNull(configuration.WebsitesDelegate);

            var instance = new TestDocument();
            instance.Websites = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Websites, configuration.WebsitesDelegate(instance)));
        }

        [TestMethod]
        public void TestHasThemes()
        {
            Assert.IsNull(configuration.ThemesDelegate);
            configuration.HasThemes(x => x.Themes);
            Assert.IsNotNull(configuration.ThemesDelegate);

            var instance = new TestDocument();
            instance.Themes = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Themes, configuration.ThemesDelegate(instance)));
        }

        [TestMethod]
        public void TestHasGoals()
        {
            Assert.IsNull(configuration.GoalsDelegate);
            configuration.HasGoals(x => x.Goals);
            Assert.IsNotNull(configuration.GoalsDelegate);

            var instance = new TestDocument();
            instance.Themes = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Goals, configuration.GoalsDelegate(instance)));
        }

        [TestMethod]
        public void TestHasFoci()
        {
            Assert.IsNull(configuration.FociDelegate);
            configuration.HasFoci(x => x.Foci);
            Assert.IsNotNull(configuration.FociDelegate);

            var instance = new TestDocument();
            instance.Themes = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Foci, configuration.FociDelegate(instance)));
        }

        [TestMethod]
        public void TestHasObjectives()
        {
            Assert.IsNull(configuration.ObjectivesDelegate);
            configuration.HasObjectives(x => x.Objectives);
            Assert.IsNotNull(configuration.ObjectivesDelegate);

            var instance = new TestDocument();
            instance.Themes = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Objectives, configuration.ObjectivesDelegate(instance)));
        }

        [TestMethod]
        public void TestHasOfficeSymbol()
        {
            Assert.IsNull(configuration.OfficeSymbolDelegate);
            configuration.HasOfficeSymbol(x => x.OfficeSymbol);
            Assert.IsNotNull(configuration.OfficeSymbolDelegate);

            var instance = new TestDocument();
            instance.OfficeSymbol = "symbol";
            Assert.IsTrue(Object.ReferenceEquals(instance.OfficeSymbol, configuration.OfficeSymbolDelegate(instance)));
        }

        [TestMethod]
        public void TestHasPointsOfContact()
        {
            Assert.IsNull(configuration.PointsOfContactDelegate);
            configuration.HasPointsOfContact(x => x.PointsOfContact);
            Assert.IsNotNull(configuration.PointsOfContactDelegate);

            var instance = new TestDocument();
            instance.PointsOfContact = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.PointsOfContact, configuration.PointsOfContactDelegate(instance)));
        }

        [TestMethod]
        public void TestIsConfigurationForType()
        {
            Assert.IsTrue(configuration.IsConfigurationForType(typeof(TestDocument)));
            Assert.IsFalse(configuration.IsConfigurationForType(typeof(string)));
        }

        [TestMethod]
        public void TestGetName()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetName(instance));

            configuration.HasName(x => x.Name);
            Assert.IsNull(configuration.GetName(instance));

            instance.Name = "name";
            Assert.AreEqual(instance.Name, configuration.GetName(instance));
        }

        [TestMethod]
        public void TestGetDescription()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetDescription(instance));

            configuration.HasDescription(x => x.Description);
            Assert.IsNull(configuration.GetDescription(instance));

            instance.Description = "desc";
            Assert.AreEqual(instance.Description, configuration.GetDescription(instance));
        }

        [TestMethod]
        public void TestGetOfficeSymbol()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetOfficeSymbol(instance));

            configuration.HasOfficeSymbol(x => x.OfficeSymbol);
            Assert.IsNull(configuration.GetOfficeSymbol(instance));

            instance.OfficeSymbol = "symbol";
            Assert.AreEqual(instance.OfficeSymbol, configuration.GetOfficeSymbol(instance));
        }

        [TestMethod]
        public void TestGetStatus()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetStatus(instance));

            configuration.HasStatus(x => x.Status);
            Assert.IsNull(configuration.GetStatus(instance));

            instance.Status = "status";
            Assert.AreEqual(instance.Status, configuration.GetStatus(instance));
        }

        [TestMethod]
        public void TestGetGoals()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetGoals(instance));

            configuration.HasGoals(x => x.Goals);
            Assert.IsNull(configuration.GetGoals(instance));

            instance.Goals = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Goals, configuration.GetGoals(instance)));
        }

        [TestMethod]
        public void TestGetAddresses()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetAddresses(instance));

            configuration.HasAddresses(x => x.Addresses);
            Assert.IsNull(configuration.GetAddresses(instance));

            instance.Addresses = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Addresses, configuration.GetAddresses(instance)));
        }

        [TestMethod]
        public void TestGetPhoneNumbers()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetPhoneNumbers(instance));

            configuration.HasPhoneNumbers(x => x.PhoneNumbers);
            Assert.IsNull(configuration.GetPhoneNumbers(instance));

            instance.Addresses = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.PhoneNumbers, configuration.GetPhoneNumbers(instance)));
        }

        [TestMethod]
        public void TestGetObjectives()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetObjectives(instance));

            configuration.HasObjectives(x => x.Objectives);
            Assert.IsNull(configuration.GetObjectives(instance));

            instance.Objectives = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Objectives, configuration.GetObjectives(instance)));
        }

        [TestMethod]
        public void TestGetFoci()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetFoci(instance));

            configuration.HasFoci(x => x.Foci);
            Assert.IsNull(configuration.GetFoci(instance));

            instance.Foci = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Foci, configuration.GetFoci(instance)));
        }

        [TestMethod]
        public void TestGetThemes()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetThemes(instance));

            configuration.HasThemes(x => x.Themes);
            Assert.IsNull(configuration.GetThemes(instance));

            instance.Themes = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Themes, configuration.GetThemes(instance)));
        }

        [TestMethod]
        public void TestGetPointsOfContact()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetPointsOfContact(instance));

            configuration.HasPointsOfContact(x => x.PointsOfContact);
            Assert.IsNull(configuration.GetPointsOfContact(instance));

            instance.PointsOfContact = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.PointsOfContact, configuration.GetPointsOfContact(instance)));
        }

        [TestMethod]
        public void TestGetRegions()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetRegions(instance));

            configuration.HasRegions(x => x.Regions);
            Assert.IsNull(configuration.GetRegions(instance));

            instance.Regions = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Regions, configuration.GetRegions(instance)));
        }

        [TestMethod]
        public void TestGetCountries()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetCountries(instance));

            configuration.HasCountries(x => x.Countries);
            Assert.IsNull(configuration.GetCountries(instance));

            instance.Countries = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Countries, configuration.GetCountries(instance)));
        }

        [TestMethod]
        public void TestGetLocations()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetLocations(instance));

            configuration.HasLocations(x => x.Locations);
            Assert.IsNull(configuration.GetLocations(instance));

            instance.Locations = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Locations, configuration.GetLocations(instance)));
        }

        [TestMethod]
        public void TestGetWebsites()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetWebsites(instance));

            configuration.HasWebsites(x => x.Websites);
            Assert.IsNull(configuration.GetWebsites(instance));

            instance.Websites = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Websites, configuration.GetWebsites(instance)));
        }


        [TestMethod]
        public void TestGetStartDate()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetStartDate(instance));

            configuration.HasStartDate(x => x.StartDate);
            Assert.IsNull(configuration.GetStartDate(instance));

            instance.StartDate = null;
            Assert.IsNull(configuration.GetStartDate(instance));

            var now = DateTimeOffset.Now;
            instance.StartDate = now;
            Assert.AreEqual(now, configuration.GetStartDate(instance));
        }

        [TestMethod]
        public void TestGetEndDate()
        {
            var instance = new TestDocument();
            Assert.IsNull(configuration.GetEndDate(instance));

            configuration.HasEndDate(x => x.EndDate);
            Assert.IsNull(configuration.GetEndDate(instance));

            instance.EndDate = null;
            Assert.IsNull(configuration.GetEndDate(instance));

            var now = DateTimeOffset.Now;
            instance.EndDate = now;
            Assert.AreEqual(now, configuration.GetEndDate(instance));
        }

        [TestMethod]
        public void TestGetDocumentKeyType()
        {
            var instance = new TestDocument();
            configuration.HasKey(x => x.Id);
            Assert.AreEqual(DocumentKeyType.Int, configuration.GetDocumentKeyType(instance));
        }

        [TestMethod]
        public void TestGetDocumentKeyType_KeyHasNotBeenConfigured()
        {
            var instance = new TestDocument();
            Action a = () => configuration.GetDocumentKeyType(instance);
            a.ShouldThrow<NotSupportedException>().WithMessage("The document key type can not be determined because the Key has not been configured on this type.");
        }
    }
}
