using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class DocumentSaveActionConfigurationTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new DocumentSaveActionConfiguration<SimpleEntity>();
            Assert.IsNotNull(instance.CreatedExclusionRules);
            Assert.IsNotNull(instance.ModifiedExclusionRules);
            Assert.IsNotNull(instance.DeletedExclusionRules);
        }
    }
}
