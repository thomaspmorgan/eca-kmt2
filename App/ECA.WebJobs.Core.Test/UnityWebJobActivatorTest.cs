using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using ECA.Core.Settings;

namespace ECA.WebJobs.Core.Test
{
    [TestClass]
    public class UnityWebJobActivatorTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<object>();

            var activator = new UnityWebJobActivator(unityContainer);
            Assert.IsNotNull(activator.CreateInstance<object>());
        }
    }
}
