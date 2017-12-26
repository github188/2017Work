using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class LoginController : Controller
    {
        // GET: Restaurant/Login
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Logining(string username,string password)
        {
            using (MainDb db = new MainDb())
            {
                sys_user user = db.sys_user.Where(p => p.username == username && p.password == password).FirstOrDefault();
                if (user != null)
                {
                    Session["username"] = user.username;
                    Session["password"] = user.password;
                    Session["name"] = user.name;
                    //try
                    //{
                    //    if (System.Web.HttpContext.Current.Session["username"] != null)
                    //    {
                    //        string loginname = System.Web.HttpContext.Current.Session["username"].ToString();
                    //        string loginname1 = System.Web.HttpContext.Current.Session["username1"].ToString();
                    //    }
                    //}
                    //catch
                    //{
                    //}
                    return Json(new { result = "success" });
                }
                else
                    return Json(new { result = "fail" });
            }
        }
    }
}