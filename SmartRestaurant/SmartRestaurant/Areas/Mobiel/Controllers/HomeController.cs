﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Mobiel.Controllers
{
    public class HomeController : Controller
    {
        // GET: Mobiel/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}