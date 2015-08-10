using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    //public class PropertyOperatorTestClass
    //{
    //    public DateTime? NullableDate { get; set; }

    //    public string S { get; set; }

    //    public int Id { get; set; }

    //    public float F { get; set; }

    //    public double D { get; set; }

    //    public long L { get; set; }

    //    public decimal Dec { get; set; }

    //    public long? NullableL { get; set; }
    //    public int? NullableId { get; set; }
    //    public float? NullableF { get; set; }
    //    public double? NullableD { get; set; }
    //    public decimal? NullableDec { get; set; }

    //    public object O { get; set; }

    //    public string MethodName()
    //    {
    //        return null;
    //    }
    //}

    public class SimplePropertyOperator<T> : PropertyOperator<T> where T : class
    {
        public SimplePropertyOperator(string property) : base(property) { }

        public override System.Linq.Expressions.Expression<Func<T, bool>> ToWhereExpression()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class PropertyOperatorTest
    {
        [TestMethod]
        public void TestConstructor_NonNullableProperty()
        {
            var propertyName = "Id";
            var t = typeof(LinqFilterTestClass);
            var property = t.GetProperty(propertyName);
            var filter = new SimplePropertyOperator<LinqFilterTestClass>(propertyName);
            Assert.AreEqual(property, filter.PropertyInfo);
            Assert.IsFalse(filter.IsNullable);
        }

        [TestMethod]
        public void TestConstructor_NullableProperty()
        {
            var propertyName = "NullableId";
            var t = typeof(LinqFilterTestClass);
            var property = t.GetProperty(propertyName);
            var filter = new SimplePropertyOperator<LinqFilterTestClass>(propertyName);
            Assert.AreEqual(property, filter.PropertyInfo);
            Assert.IsTrue(filter.IsNullable);
        }

        [TestMethod]
        public void TestConstructor_PropertyNameCaseInsenstive()
        {
            var propertyName = "nullableid";
            var t = typeof(LinqFilterTestClass);
            var property = t.GetProperty("NullableId");
            var filter = new SimplePropertyOperator<LinqFilterTestClass>(propertyName);
            Assert.AreEqual(property, filter.PropertyInfo);
            Assert.IsTrue(filter.IsNullable);
        }

        [TestMethod]
        public void TestToString()
        {
            var propertyName = "S";
            var filter = new LikeFilter<LinqFilterTestClass>(propertyName, "hello");
            Assert.IsNotNull(filter.ToString());
        }


        [TestMethod]
        public void TestIsPropertyNumeric()
        {
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("Id").IsNumeric);
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("F").IsNumeric);
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("D").IsNumeric);
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("L").IsNumeric);
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("Id").IsNumeric);
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("Dec").IsNumeric);
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("NullableL").IsNumeric);
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("NullableId").IsNumeric);
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("NullableF").IsNumeric);
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("NullableD").IsNumeric);
            Assert.IsTrue(new SimplePropertyOperator<PropertyHelperTestClass>("NullableDec").IsNumeric);

            Assert.IsFalse(new SimplePropertyOperator<PropertyHelperTestClass>("S").IsNumeric);

        }
    }
}
