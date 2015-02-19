using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Core.DynamicLinq.Test
{
    public class PropertyOperatorTestClass
    {
        public DateTime? NullableDate { get; set; }

        public string S { get; set; }

        public Int32 Id { get; set; }
    }


    [TestClass]
    public class PropertyOperatorTest
    {
        [TestMethod]
        public void TestGetPropertyName_NullableDateProperty()
        {
            var propertyName = PropertyOperator<PropertyOperatorTestClass>.GetPropertyName(x => x.NullableDate);
            Assert.AreEqual("NullableDate", propertyName);
        }

        [TestMethod]
        public void TestGetPropertyName_StringProperty()
        {
            var propertyName = PropertyOperator<PropertyOperatorTestClass>.GetPropertyName(x => x.S);
            Assert.AreEqual("S", propertyName);
        }

        [TestMethod]
        public void TestGetPropertyName_IntProperty()
        {
            var propertyName = PropertyOperator<PropertyOperatorTestClass>.GetPropertyName(x => x.Id);
            Assert.AreEqual("Id", propertyName);
        }
    }
}
