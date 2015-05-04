using ECA.Core.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Test.Data
{
    public class TestConcurrentClass : IConcurrent
    {

        public byte[] RowVersion { get; set; }
    }

    [TestClass]
    public class IConcurrentExtensionsTest
    {
        [TestMethod]
        public void TestGetRowVersionAsString()
        {
            var rowVersion = new byte[1] { (byte)1 };
            var instance = new TestConcurrentClass
            {
                RowVersion = rowVersion
            };

            var rowVersionAsString = instance.GetRowVersionAsString();
            Assert.IsNotNull(rowVersionAsString);
            Assert.AreEqual(Convert.ToBase64String(rowVersion), rowVersionAsString);
        }

        [TestMethod]
        public void TestSetRowVersion()
        {
            var rowVersion = new byte[1] { (byte)1 };
            var rowVersionAsString = Convert.ToBase64String(rowVersion);
            var instance = new TestConcurrentClass();
            instance.SetRowVersion(rowVersionAsString);
            Assert.IsNotNull(instance.RowVersion);
            CollectionAssert.AreEqual(rowVersion, instance.RowVersion);
        }
    }
}
