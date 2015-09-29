using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using System.Collections;
using System.Collections.Generic;

namespace ECA.Business.Search.Test
{
    public class ConfigurationTestClass : DocumentConfiguration<SimpleConfigurationTestClass, int>
    {
        public ConfigurationTestClass()
        {
        }
    }

    public class LookupTestClass
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class SimpleConfigurationTestClass
    {
        public SimpleConfigurationTestClass()
        {
            
        }

        public string OfficeSymbol { get; set; }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Themes
        {
            get; set;
        }

        public IEnumerable<string> Objectives
        {
            get; set;
        }

        public IEnumerable<string> Goals
        {
            get; set;
        }

        public IEnumerable<string> Foci
        {
            get; set;
        }

        public IEnumerable<string> PointsOfContact
        {
            get; set;
        }

        public DocumentType DocumentType { get; set; }
    }

    [TestClass]
    public class DocumentConfigurationTest
    {
        //[TestMethod]
        //public void TestConstructor()
        //{
        //    var configuration = new ConfigurationTestClass();
        //    Assert.IsNotNull(configuration.AdditionalFieldsDelegates);
        //    Assert.AreEqual(0, configuration.AdditionalFieldsDelegates.Count());
        //}

        [TestMethod]
        public void TestGetId()
        {
            var configuration = new ConfigurationTestClass();
            configuration.HasKey(x => x.Id);

            var instance = new SimpleConfigurationTestClass();
            instance.Id = 1;
            Assert.AreEqual(instance.Id, configuration.GetId(instance));
        }

        [TestMethod]
        public void TestGetId_HasKeyNotConfigured()
        {
            var configuration = new ConfigurationTestClass();
            var instance = new SimpleConfigurationTestClass();
            Action a = () => configuration.GetId(instance);
            var message = "The id has not been configured.  Use the HasKey method.";
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestHasKey()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.IdDelegate);

            configuration.HasKey(x => x.Id);

            Assert.IsNotNull(configuration.IdDelegate);
            var instance = new SimpleConfigurationTestClass();
            instance.Id = 1;
            Assert.AreEqual(instance.Id, configuration.IdDelegate(instance));
        }

        [TestMethod]
        public void TestIsDocumentType()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetDocumentType());

            var documentType = DocumentType.Program;
            configuration.IsDocumentType(documentType);

            Assert.IsNotNull(configuration.GetDocumentType());
            Assert.AreEqual(documentType, configuration.GetDocumentType());
        }

        [TestMethod]
        public void TestHasName()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.NameDelegate);
            configuration.HasName(x => x.Name);
            Assert.IsNotNull(configuration.NameDelegate);

            var instance = new SimpleConfigurationTestClass();
            instance.Name = "name";
            Assert.AreEqual(instance.Name, configuration.NameDelegate(instance));
        }

        [TestMethod]
        public void TestHasDescription()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.DescriptionDelegate);
            configuration.HasDescription(x => x.Description);
            Assert.IsNotNull(configuration.DescriptionDelegate);

            var instance = new SimpleConfigurationTestClass();
            instance.Description = "desc";
            Assert.AreEqual(instance.Description, configuration.DescriptionDelegate(instance));
        }

        [TestMethod]
        public void TestHasThemes()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.ThemesDelegate);
            configuration.HasThemes(x => x.Themes);
            Assert.IsNotNull(configuration.ThemesDelegate);

            var instance = new SimpleConfigurationTestClass();
            instance.Themes = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Themes, configuration.ThemesDelegate(instance)));
        }

        [TestMethod]
        public void TestHasGoals()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GoalsDelegate);
            configuration.HasGoals(x => x.Goals);
            Assert.IsNotNull(configuration.GoalsDelegate);

            var instance = new SimpleConfigurationTestClass();
            instance.Themes = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Goals, configuration.GoalsDelegate(instance)));
        }

        [TestMethod]
        public void TestHasFoci()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.FociDelegate);
            configuration.HasFoci(x => x.Foci);
            Assert.IsNotNull(configuration.FociDelegate);

            var instance = new SimpleConfigurationTestClass();
            instance.Themes = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Foci, configuration.FociDelegate(instance)));
        }

        [TestMethod]
        public void TestHasObjectives()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.ObjectivesDelegate);
            configuration.HasObjectives(x => x.Objectives);
            Assert.IsNotNull(configuration.ObjectivesDelegate);

            var instance = new SimpleConfigurationTestClass();
            instance.Themes = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Objectives, configuration.ObjectivesDelegate(instance)));
        }

        [TestMethod]
        public void TestHasOfficeSymbol()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.OfficeSymbolDelegate);
            configuration.HasOfficeSymbol(x => x.OfficeSymbol);
            Assert.IsNotNull(configuration.OfficeSymbolDelegate);

            var instance = new SimpleConfigurationTestClass();
            instance.OfficeSymbol = "symbol";
            Assert.IsTrue(Object.ReferenceEquals(instance.OfficeSymbol, configuration.OfficeSymbolDelegate(instance)));
        }

        [TestMethod]
        public void TestHasPointsOfContact()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.PointsOfContactDelegate);
            configuration.HasPointsOfContact(x => x.PointsOfContact);
            Assert.IsNotNull(configuration.PointsOfContactDelegate);

            var instance = new SimpleConfigurationTestClass();
            instance.PointsOfContact = new string[] { "value1", "value2" };
            Assert.IsTrue(Object.ReferenceEquals(instance.PointsOfContact, configuration.PointsOfContactDelegate(instance)));
        }

        [TestMethod]
        public void TestIsConfigurationForType()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsTrue(configuration.IsConfigurationForType(typeof(SimpleConfigurationTestClass)));
            Assert.IsFalse(configuration.IsConfigurationForType(typeof(string)));
        }

        [TestMethod]
        public void TestGetName()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetName(instance));

            configuration.HasName(x => x.Name);
            Assert.IsNull(configuration.GetName(instance));

            instance.Name = "name";
            Assert.AreEqual(instance.Name, configuration.GetName(instance));
        }

        [TestMethod]
        public void TestGetDescription()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetDescription(instance));

            configuration.HasDescription(x => x.Description);
            Assert.IsNull(configuration.GetDescription(instance));

            instance.Description = "desc";
            Assert.AreEqual(instance.Description, configuration.GetDescription(instance));
        }

        [TestMethod]
        public void TestGetOfficeSymbol()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetOfficeSymbol(instance));

            configuration.HasOfficeSymbol(x => x.OfficeSymbol);
            Assert.IsNull(configuration.GetOfficeSymbol(instance));

            instance.OfficeSymbol = "symbol";
            Assert.AreEqual(instance.OfficeSymbol, configuration.GetOfficeSymbol(instance));
        }

        [TestMethod]
        public void TestGetGoals()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetGoals(instance));

            configuration.HasGoals(x => x.Goals);
            Assert.IsNull(configuration.GetGoals(instance));

            instance.Goals = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Goals, configuration.GetGoals(instance)));
        }

        [TestMethod]
        public void TestGetObjectives()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetObjectives(instance));

            configuration.HasObjectives(x => x.Objectives);
            Assert.IsNull(configuration.GetObjectives(instance));

            instance.Objectives = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Objectives, configuration.GetObjectives(instance)));
        }

        [TestMethod]
        public void TestGetFoci()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetFoci(instance));

            configuration.HasFoci(x => x.Foci);
            Assert.IsNull(configuration.GetFoci(instance));

            instance.Foci = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Foci, configuration.GetFoci(instance)));
        }

        [TestMethod]
        public void TestGetThemes()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetThemes(instance));

            configuration.HasThemes(x => x.Themes);
            Assert.IsNull(configuration.GetThemes(instance));

            instance.Themes = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.Themes, configuration.GetThemes(instance)));
        }

        [TestMethod]
        public void TestGetPointsOfContact()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetPointsOfContact(instance));

            configuration.HasPointsOfContact(x => x.PointsOfContact);
            Assert.IsNull(configuration.GetPointsOfContact(instance));

            instance.PointsOfContact = new string[] { "value" };
            Assert.IsTrue(Object.ReferenceEquals(instance.PointsOfContact, configuration.GetPointsOfContact(instance)));
        }

    }
}
