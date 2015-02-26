using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Core.DynamicLinq.Test
{

    public class PropertyOperatorTestClass
    {
        public DateTime? NullableDate { get; set; }

        public string S { get; set; }

        public int Id { get; set; }

        public float F { get; set; }

        public double D { get; set; }

        public long L { get; set; }

        public long? NullableL { get; set; }
        public int? NullableId { get; set; }
        public float? NullableF { get; set; }
        public double? NullableD { get; set; }

        public object O { get; set; }

        public string MethodName()
        {
            return null;
        }
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

        [TestMethod]
        public void TestIsPropertyNumeric()
        {
            Assert.IsTrue(new PropertyOperator<PropertyOperatorTestClass>("Id").IsNumeric);
            Assert.IsTrue(new PropertyOperator<PropertyOperatorTestClass>("F").IsNumeric);
            Assert.IsTrue(new PropertyOperator<PropertyOperatorTestClass>("D").IsNumeric);
            Assert.IsTrue(new PropertyOperator<PropertyOperatorTestClass>("L").IsNumeric);
            Assert.IsTrue(new PropertyOperator<PropertyOperatorTestClass>("Id").IsNumeric);
            Assert.IsTrue(new PropertyOperator<PropertyOperatorTestClass>("NullableL").IsNumeric);
            Assert.IsTrue(new PropertyOperator<PropertyOperatorTestClass>("NullableId").IsNumeric);
            Assert.IsTrue(new PropertyOperator<PropertyOperatorTestClass>("NullableF").IsNumeric);
            Assert.IsTrue(new PropertyOperator<PropertyOperatorTestClass>("NullableD").IsNumeric);

            Assert.IsFalse(new PropertyOperator<PropertyOperatorTestClass>("S").IsNumeric);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetPropertyName_ComplexObject()
        {
            var propertyName = PropertyOperator<PropertyOperatorTestClass>.GetPropertyName(x => x.MethodName());
        }
    }
}
