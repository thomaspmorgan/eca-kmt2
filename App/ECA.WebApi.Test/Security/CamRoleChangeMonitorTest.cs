using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using System.Reflection;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class CamRoleChangeMonitorTest
    {
        [TestMethod]
        public void TestUniqueId()
        {
            var monitor1 = new CamRoleChangeMonitor();
            var monitor2 = new CamRoleChangeMonitor();
            Assert.AreNotEqual(Guid.Empty, monitor1.UniqueId);
            Assert.AreNotEqual(Guid.Empty, monitor2.UniqueId);
            Assert.AreNotEqual(monitor1.UniqueId, monitor2.UniqueId);
        }

        [TestMethod]
        public void TestChanged_HasDelegate()
        {
            var monitor = new CamRoleChangeMonitor();
            var eventRaised = false;
            
            CamRoleChangeMonitor.RoleChanged += delegate (object sender, CamRoleChangeMonitorEventArgs args)
            {
                eventRaised = true;
                Assert.IsNull(sender);
                Assert.IsNotNull(args);
            };

            Assert.IsFalse(eventRaised);
            CamRoleChangeMonitor.Changed();
            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void TestChanged_DoesNotHaveDelegate()
        { 
            Action a = () => CamRoleChangeMonitor.Changed();
            a.ShouldNotThrow();
        }
    }
}
