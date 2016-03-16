using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class ProgramCategoryCodeErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new ProgramCategoryCodeErrorPath();
            var lookup = SevisErrorType.ProgramCategoryCode;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
