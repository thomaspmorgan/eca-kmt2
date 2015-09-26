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
            this.Lookups = new List<LookupTestClass>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string AdditionalField1 { get; set; }

        public string AdditionalField2 { get; set; }

        public string Description { get; set; }

        public IEnumerable<LookupTestClass> Lookups { get; set; }

        public DocumentType DocumentType { get; set; }
    }

    [TestClass]
    public class DocumentConfigurationTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNotNull(configuration.AdditionalFieldsDelegates);
            Assert.AreEqual(0, configuration.AdditionalFieldsDelegates.Count());
        }

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
        public void TestHasTitle()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.TitleDelegate);
            configuration.HasTitle(x => x.Title);
            Assert.IsNotNull(configuration.TitleDelegate);

            var instance = new SimpleConfigurationTestClass();
            instance.Title = "title";
            Assert.AreEqual(instance.Title, configuration.TitleDelegate(instance));
        }

        [TestMethod]
        public void TestHasSubtitle()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.SubtitleDelegate);
            configuration.HasSubtitle(x => x.Subtitle);
            Assert.IsNotNull(configuration.SubtitleDelegate);

            var instance = new SimpleConfigurationTestClass();
            instance.Subtitle = "subtitle";
            Assert.AreEqual(instance.Subtitle, configuration.SubtitleDelegate(instance));
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
        public void TestHasAdditionalField_OneField_DefaultPropertyName()
        {
            var configuration = new ConfigurationTestClass();
            Assert.AreEqual(0, configuration.AdditionalFieldsDelegates.Count);
            configuration.HasAdditionalField(x => x.AdditionalField1);
            Assert.AreEqual(1, configuration.AdditionalFieldsDelegates.Count);

            var instance = new SimpleConfigurationTestClass();
            instance.AdditionalField1 = "addfield1";

            var dictionary = configuration.GetAdditionalFields(instance);
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey("AdditionalField1"));
            Assert.AreEqual(instance.AdditionalField1, dictionary.Values.First());
        }

        [TestMethod]
        public void TestHasAdditionalField_OneField_DifferentPropertyName()
        {
            var fieldName = "MyField";
            var configuration = new ConfigurationTestClass();
            Assert.AreEqual(0, configuration.AdditionalFieldsDelegates.Count);
            configuration.HasAdditionalField(fieldName, x => x.AdditionalField1);
            Assert.AreEqual(1, configuration.AdditionalFieldsDelegates.Count);

            var instance = new SimpleConfigurationTestClass();
            instance.AdditionalField1 = "addfield1";

            var dictionary = configuration.GetAdditionalFields(instance);
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey(fieldName));
            Assert.AreEqual(instance.AdditionalField1, dictionary.Values.First());
        }

        [TestMethod]
        public void TestHasAdditionalField_TwoFields()
        {
            var configuration = new ConfigurationTestClass();
            Assert.AreEqual(0, configuration.AdditionalFieldsDelegates.Count);
            configuration.HasAdditionalField(x => x.AdditionalField1);
            configuration.HasAdditionalField(x => x.AdditionalField2);
            Assert.AreEqual(2, configuration.AdditionalFieldsDelegates.Count);

            var instance = new SimpleConfigurationTestClass();
            instance.AdditionalField1 = "addfield1";
            instance.AdditionalField2 = "addfield2";

            var dictionary = configuration.GetAdditionalFields(instance);
            Assert.AreEqual(2, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey("AdditionalField1"));
            Assert.IsTrue(dictionary.ContainsKey("AdditionalField2"));
            Assert.AreEqual(instance.AdditionalField1, dictionary["AdditionalField1"]);
            Assert.AreEqual(instance.AdditionalField2, dictionary["AdditionalField2"]);
        }

        [TestMethod]
        public void TestHasAdditionalField_IEnumerableProperty()
        {
            var configuration = new ConfigurationTestClass();
            Assert.AreEqual(0, configuration.AdditionalFieldsDelegates.Count);
            configuration.HasAdditionalField(x => x.Lookups, y => String.Join(", ", y.Lookups.Select(z => z.Value).ToList()));
            Assert.AreEqual(1, configuration.AdditionalFieldsDelegates.Count);

            var instance = new SimpleConfigurationTestClass();
            var lookup = new LookupTestClass
            {
                Id = 1,
                Value = "value"
            };
            instance.Lookups = new List<LookupTestClass> { lookup };

            var dictionary = configuration.GetAdditionalFields(instance);
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey("Lookups"));
            Assert.AreEqual("value", dictionary["Lookups"]);
        }

        [TestMethod]
        public void TestHasAdditionalField_IEnumerableProperty_DifferentPropertyName()
        {
            var fieldName = "X";
            var configuration = new ConfigurationTestClass();
            Assert.AreEqual(0, configuration.AdditionalFieldsDelegates.Count);
            configuration.HasAdditionalField(fieldName, y => String.Join(", ", y.Lookups.Select(z => z.Value).ToList()));
            Assert.AreEqual(1, configuration.AdditionalFieldsDelegates.Count);

            var instance = new SimpleConfigurationTestClass();
            var lookup = new LookupTestClass
            {
                Id = 1,
                Value = "value"
            };
            instance.Lookups = new List<LookupTestClass> { lookup };

            var dictionary = configuration.GetAdditionalFields(instance);
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey(fieldName));
            Assert.AreEqual("value", dictionary[fieldName]);
        }

        [TestMethod]
        public void TestIsConfigurationForType()
        {
            var configuration = new ConfigurationTestClass();
            Assert.IsTrue(configuration.IsConfigurationForType(typeof(SimpleConfigurationTestClass)));
            Assert.IsFalse(configuration.IsConfigurationForType(typeof(string)));
        }

        [TestMethod]
        public void TestGetTitle()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetTitle(instance));

            configuration.HasTitle(x => x.Title);
            Assert.IsNull(configuration.GetTitle(instance));

            instance.Title = "title";
            Assert.AreEqual(instance.Title, configuration.GetTitle(instance));
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
        public void TestSubtitle()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.IsNull(configuration.GetSubtitle(instance));

            configuration.HasSubtitle(x => x.Subtitle);
            Assert.IsNull(configuration.GetSubtitle(instance));

            instance.Subtitle = "sub";
            Assert.AreEqual(instance.Subtitle, configuration.GetSubtitle(instance));
        }

        [TestMethod]
        public void TestGetAdditionalFields_ZeroAdditionalField()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();

            Assert.AreEqual(0, configuration.GetAdditionalFields(instance).Count);
        }

        [TestMethod]
        public void TestGetAdditionalFields_OneAdditionalField_ValueIsNull()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();

            configuration.HasAdditionalField(x => x.AdditionalField1);
            instance.AdditionalField1 = null;
            Assert.AreEqual(0, configuration.GetAdditionalFields(instance).Count);
        }

        [TestMethod]
        public void TestGetAdditionalFields_OneAdditionalField_ValueIsNotNull()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();

            configuration.HasAdditionalField(x => x.AdditionalField1);
            instance.AdditionalField1 = "additionalfield1";

            var values = configuration.GetAdditionalFields(instance);
            Assert.AreEqual(1, values.Count);
            Assert.IsTrue(values.ContainsKey("AdditionalField1"));
            Assert.AreEqual(instance.AdditionalField1, values.First().Value);
        }

        [TestMethod]
        public void TestGetAdditionalFieldNames_ZeroFields()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            Assert.AreEqual(0, configuration.GetAdditionalFieldNames().Count);
        }

        [TestMethod]
        public void TestGetAdditionalFieldNames_MultipleFields()
        {
            var instance = new SimpleConfigurationTestClass();
            var configuration = new ConfigurationTestClass();
            configuration.HasAdditionalField(x => x.AdditionalField1);
            configuration.HasAdditionalField(x => x.AdditionalField2);

            var fields = configuration.GetAdditionalFieldNames();
            Assert.AreEqual(2, fields.Count);
            Assert.IsTrue(fields.Contains("AdditionalField1"));
            Assert.IsTrue(fields.Contains("AdditionalField2"));
        }
    }
}
