using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class WllxbjController : Controller
    {
        // GET: Restaurant/Wllxbj
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu)
        {//行数 页码 search参数
            using (MainDb db = new MainDb())
            {
                var varList = db.zzdc_wllx.AsQueryable().ToList();
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit).OrderBy(p => p.rank);
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Insert()
        {
            return View("Insert");
        }
        public JsonResult Inserting(String wllxmc, String wllxms, String wllxxh)
        {
            using (MainDb db = new MainDb())
            {
                zzdc_wllx wllx = new zzdc_wllx();
                wllx.name = wllxmc;
                wllx.remark = wllxms;
                wllx.rank = Convert.ToInt32(wllxxh);
                db.zzdc_wllx.Add(wllx);
                int count = db.SaveChanges();
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }
        public ActionResult Update(int id)
        {
            using (MainDb db = new MainDb())
            {
                var wllx = db.zzdc_wllx.Where(p => p.id == id).FirstOrDefault();
                return View("Update", Json(new { wllx = wllx }));
            }
        }
        public JsonResult Updating(int wllxid, string wllxmc, string wllxms, string wllxxh)
        {
            using (MainDb db = new MainDb())
            {
                string sql = "update zzdc_wllx set name = '" + wllxmc + "',"
                                                + " remark = '" + wllxms + "',"
                                                + " rank = " + wllxxh
                                                + " where id = " + wllxid;
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }
        public JsonResult Delete(int id)
        {
            using (MainDb db = new MainDb())
            {
                string sql = string.Format(@"delete from zzdc_wllx where id in ('{0}')", string.Join("','", id));
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }

    }
}