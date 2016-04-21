using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Data.Test
{
    [TestClass]
    public class PersonDependentTest
    {
        [TestMethod]
        public void TestGetDS2019FileName()
        {
            var instance = new PersonDependent
            {
                DependentId = Int32.MaxValue,
                SevisId = "N123456789"
            };
            Assert.AreEqual(string.Format(PersonDependent.DS2019_FILE_NAME_FORMAT_STRING, instance.DependentId, instance.SevisId), instance.GetDS2019FileName());
        }
    }
}
