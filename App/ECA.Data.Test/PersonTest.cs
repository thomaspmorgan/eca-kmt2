using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Data.Test
{
    [TestClass]
    public class PersonTest
    {
        
        [TestMethod]
        public void TestGetId()
        {
            var person = new Person();
            person.PersonId = 1;
            Assert.AreEqual(person.PersonId, person.GetId());
        }
    }
}
