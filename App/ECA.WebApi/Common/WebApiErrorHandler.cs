using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ECA.WebApi.Common
{
    public class WebApiErrorHandler : ExceptionFilterAttribute
    {
        // add reference to static logger
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Debug.WriteLine(actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }
    }
}