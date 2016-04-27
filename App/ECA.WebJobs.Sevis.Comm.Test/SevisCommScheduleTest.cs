using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Settings;
using System.Collections.Specialized;
using System.Configuration;

namespace ECA.WebJobs.Sevis.Comm.Test
{
    [TestClass]
    public class SevisCommScheduleTest
    {
        [TestMethod]
        public void TestGetNextOccurrence()
        {
            var appSettings = new NameValueCollection();
            var connectionStrings = new ConnectionStringSettingsCollection();
            var settings = new AppSettings(appSettings, connectionStrings);
            var expression = "0/20 * * * * *";
            appSettings.Add(AppSettings.SEVIS_COMM_CRON_SCHEDULE_KEY, expression);

            var now = new DateTime(2016, 1, 1, 12, 0, 0);
            var expected = new DateTime(2016, 1, 1, 12, 0, 20);
            var instance = new SevisCommSchedule(settings);
            var actual = instance.GetNextOccurrence(now);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(Object.ReferenceEquals(settings, instance.AppSettings));
        }

        [TestMethod]
        public void TestConstructor_AppSettings()
        {
            var appSettings = new NameValueCollection();
            var connectionStrings = new ConnectionStringSettingsCollection();
            var settings = new AppSettings(appSettings, connectionStrings);
            var instance = new SevisCommSchedule(settings);
            Assert.IsTrue(Object.ReferenceEquals(settings, instance.AppSettings));
        }

        [TestMethod]
        public void TestDefaultConstructor()
        {
            var instance = new SevisCommSchedule();
            Assert.IsNotNull(instance.AppSettings);
        }
    }
}
