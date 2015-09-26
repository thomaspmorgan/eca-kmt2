using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using ECA.Business.Search;
using System.Collections.Generic;
using ECA.Data;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class ProgramDocumentServiceTest
    {
        private string searchServiceName = "ecakmtsrch-dev";
        private string apikey = "494093D1A885C79D809ACAD6EB20F5AC";

        //[TestMethod]
        //public async Task Test()
        //{
        //    var context = new EcaContext(@"Data Source=(local);User Id=ECA;Password=wisconsin-89;Database=ECA_Local;Pooling=False");

        //    var indexService = new IndexService(
        //        new SearchServiceClient(searchServiceName, new SearchCredentials(apikey)),
        //        new List<IDocumentConfiguration>
        //        {
        //                new ProgramDTODocumentConfiguration()
        //        }
        //        );

        //    var service = new ProgramDocumentService(context, indexService, new TestIndexNotificationService());
        //    await service.ProcessAsync();

        //}
    }
}
