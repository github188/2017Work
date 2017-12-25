using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class XsjhController : Controller
    {
        // GET: Restaurant/Xsjh
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu)
        {//行数 页码 search参数
            using (MainDb db = new MainDb())
            {
                var varList = db.zzdc_xsxx.AsQueryable().ToList();
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit);
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}