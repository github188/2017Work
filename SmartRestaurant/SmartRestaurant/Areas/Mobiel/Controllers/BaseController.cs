using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Mobiel.Controllers
{
    public class BaseController : Controller
    {
        //// GET: Mobiel/Base
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public int OrderID
        {
            get {
                AssertOrder();
                return Int32.Parse(Session["OrderID"].ToString());
                }
            set
            {
                Session["OrderID"] = value;
            }
        }

        public bool AssertOrder()
        {
            bool result = true;
            if (Session["OrderID"]==null)
            {
                Response.Redirect("~/Mobiel/Home/Index");
                result = false;
            }

            return result;
        }
    }
}