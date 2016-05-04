using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Settings;
using System.Collections.Specialized;
using System.Configuration;
using System.Collections.Generic;

namespace ECA.WebJobs.Sevis.Comm.Test
{
    [TestClass]
    public class SevisCommScheduleTest
    {
        [TestMethod]
        public void TestGetNextOccurrence_EveryFiveMinutes()
        {
            var appSettings = new NameValueCollection();
            var connectionStrings = new ConnectionStringSettingsCollection();
            var settings = new AppSettings(appSettings, connectionStrings);
            var expression = "0 */5 * * * *"; //0 0/15 0,1,2,3,4,5,6,18,19,20,21,22,23 * * *
            appSettings.Add(AppSettings.SEVIS_COMM_CRON_SCHEDULE_KEY, expression);

            var now = new DateTime(2016, 1, 1, 12, 0, 0);

            var expected1 = new DateTime(2016, 1, 1, 12, 5, 0);
            var expected2 = new DateTime(2016, 1, 1, 12, 10, 0);
            var expected3 = new DateTime(2016, 1, 1, 12, 15, 0);
            var expected4 = new DateTime(2016, 1, 1, 12, 20, 0);
            var expected5 = new DateTime(2016, 1, 1, 12, 25, 0);
            var expected6 = new DateTime(2016, 1, 1, 12, 30, 0);
            var expected7 = new DateTime(2016, 1, 1, 12, 35, 0);
            var expected8 = new DateTime(2016, 1, 1, 12, 40, 0);
            var expected9 = new DateTime(2016, 1, 1, 12, 45, 0);
            var expected10 = new DateTime(2016, 1, 1, 12, 50, 0);
            var expected11 = new DateTime(2016, 1, 1, 12, 55, 0);

            var expectedDates = new List<DateTime>();
            expectedDates.Add(expected1);
            expectedDates.Add(expected2);
            expectedDates.Add(expected3);
            expectedDates.Add(expected4);
            expectedDates.Add(expected5);
            expectedDates.Add(expected6);
            expectedDates.Add(expected7);
            expectedDates.Add(expected8);
            expectedDates.Add(expected9);
            expectedDates.Add(expected10);
            expectedDates.Add(expected11);

            var instance = new SevisCommSchedule(settings);
            var actual = instance.GetNextOccurrence(now);
            var all = instance.GetNextOccurrences(11, now).ToList();
            CollectionAssert.AreEqual(expectedDates, all);
        }

        [TestMethod]
        public void TestGetNextOccurrence_EveryFifteenMinutesOffBusinessHours()
        {
            var appSettings = new NameValueCollection();
            var connectionStrings = new ConnectionStringSettingsCollection();
            var settings = new AppSettings(appSettings, connectionStrings);
            var expression = "0 0/15 0,1,2,3,4,5,6,18,19,20,21,22,23 * * *";
            appSettings.Add(AppSettings.SEVIS_COMM_CRON_SCHEDULE_KEY, expression);

            var now = new DateTime(2016, 1, 1, 12, 0, 0);

            var expected1 = new DateTime(2016, 1, 1, 18, 0, 0);
            var expected2 = new DateTime(2016, 1, 1, 18, 15, 0);
            var expected3 = new DateTime(2016, 1, 1, 18, 30, 0);
            var expected4 = new DateTime(2016, 1, 1, 18, 45, 0);
            var expected5 = new DateTime(2016, 1, 1, 19, 0, 0);
            var expected6 = new DateTime(2016, 1, 1, 19, 15, 0);
            var expected7 = new DateTime(2016, 1, 1, 19, 30, 0);
            var expected8 = new DateTime(2016, 1, 1, 19, 45, 0);
            var expected9 = new DateTime(2016, 1, 1, 20, 0, 0);
            var expected10 = new DateTime(2016, 1, 1, 20, 15, 0);
            var expected11 = new DateTime(2016, 1, 1, 20, 30, 0);

            var expectedDates = new List<DateTime>();
            expectedDates.Add(expected1);
            expectedDates.Add(expected2);
            expectedDates.Add(expected3);
            expectedDates.Add(expected4);
            expectedDates.Add(expected5);
            expectedDates.Add(expected6);
            expectedDates.Add(expected7);
            expectedDates.Add(expected8);
            expectedDates.Add(expected9);
            expectedDates.Add(expected10);
            expectedDates.Add(expected11);

            var instance = new SevisCommSchedule(settings);
            var actual = instance.GetNextOccurrence(now);
            var all = instance.GetNextOccurrences(11, now).ToList();
            CollectionAssert.AreEqual(expectedDates, all);
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
