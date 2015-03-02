using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ECA.WebApi.Test.Controllers
{
    public static class ControllerHelper
    {
        public static void InitializeController<T>(T controller) where T : ApiController
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }
    }
}
