using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class DependentErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var personDependentId = 10;
            var instance = new DependentErrorPath(personDependentId);
            var lookup = SevisErrorType.Dependent;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
            Assert.AreEqual(personDependentId, instance.PersonDependentId);
        }
    }
}
