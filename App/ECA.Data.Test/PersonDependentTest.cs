using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Data.Test
{
    [TestClass]
    public class ParticipantPersonTest
    {
        [TestMethod]
        public void TestGetDS2019FileName()
        {
            var instance = new ParticipantPerson
            {
                ParticipantId = Int32.MaxValue,
                SevisId = "N123456789"
            };
            Assert.AreEqual(string.Format(ParticipantPerson.DS2019_FILE_NAME_FORMAT_STRING, instance.ParticipantId, instance.SevisId), instance.GetDS2019FileName());
        }
    }
}
