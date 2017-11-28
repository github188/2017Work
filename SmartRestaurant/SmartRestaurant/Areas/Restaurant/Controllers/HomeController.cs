using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRestaurant.Model;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class HomeController : SmartRestaurant.Controllers.BaseController<sys_dict>
    {
        // GET: Restaurant/Home
        public ActionResult Index()
        {
            
            return View();
        }
    }
}