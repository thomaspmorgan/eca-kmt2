using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Core.DynamicLinq.Test
{
    public class PropertyHelperTestClass
    {
        public DateTime? NullableDate { get; set; }

        public string S { get; set; }

        public int Id { get; set; }

        public float F { get; set; }

        public double D { get; set; }

        public long L { get; set; }

        public decimal Dec { get; set; }

        public long? NullableL { get; set; }
        public int? NullableId { get; set; }
        public float? NullableF { get; set; }
        public double? NullableD { get; set; }
        public decimal? NullableDec { get; set; }

        public object O { get; set; }

        public object GetObject()
        {
            return null;
        }

        public string GetString()
        {
            return String.Empty;
        }
    }


    [TestClass]
    public class PropertyHelperTest
    {
        [TestMethod]
        public void TestGetPropertyName_NullableDateProperty()
        {
            var propertyName = PropertyHelper.GetPropertyName<PropertyHelperTestClass>(x => x.NullableDate);
            Assert.AreEqual("NullableDate", propertyName);
        }

        [TestMethod]
        public void TestGetPropertyName_StringProperty()
        {
            var propertyName = PropertyHelper.GetPropertyName<PropertyHelperTestClass>(x => x.S);
            Assert.AreEqual("S", propertyName);
        }

        [TestMethod]
        public void TestGetPropertyName_IntProperty()
        {
            var propertyName = PropertyHelper.GetPropertyName<PropertyHelperTestClass>(x => x.Id);
            Assert.AreEqual("Id", propertyName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetPropertyName_MethodReturnsAnObject()
        {
            var propertyName = PropertyHelper.GetPropertyName<PropertyHelperTestClass>(x => x.GetObject());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetPropertyName_MethodReturnsAString()
        {
            var propertyName = PropertyHelper.GetPropertyName<PropertyHelperTestClass>(x => x.GetString());
        }
    }
}
