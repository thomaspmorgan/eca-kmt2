﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECA.WebApp.Controllers
{
    public class HomeController : Controller
    {
            public ActionResult Index()
            {
                return Redirect(Url.Content("~/index.html"));
                //return View();
            }
    }
}