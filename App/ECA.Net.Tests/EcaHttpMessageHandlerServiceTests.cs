using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ECA.Net.Tests
{
    [TestClass()]
    public class EcaHttpMessageHandlerServiceTests
    {
        [TestMethod()]
        public void GetHttpMessageHandlerTest()
        {
            string thumbprint = "f6d68b2d9b7018ed94f78f18cb6c020e3aed28c7";
            var service = new EcaHttpMessageHandlerService();
            var messageHandler = service.GetHttpMessageHandler(thumbprint);
            Assert.IsNotNull(messageHandler);
        }
    }
}