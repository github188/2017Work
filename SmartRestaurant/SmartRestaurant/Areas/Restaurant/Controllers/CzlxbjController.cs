using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class CzlxbjController : Controller
    {
        // GET: Restaurant/Czlxbj
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu)
        {//行数 页码 search参数
            using (MainDb db = new MainDb())
            {
                var varList = db.zzdc_czlx.AsQueryable().ToList();
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit).OrderBy(p => p.rank);
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Insert()
        {
            return View("Insert");
        }
        public JsonResult Inserting(String czlxmc, String czlxrs, String czlxms, String czlxxh)
        {
            using (MainDb db = new MainDb())
            {
                zzdc_czlx czlx = new zzdc_czlx();
                czlx.name = czlxmc;
                czlx.remark = czlxms;
                czlx.rank = Convert.ToInt32(czlxxh);
                czlx.rs = Convert.ToInt32(czlxrs);
                db.zzdc_czlx.Add(czlx);
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
                var czlx = db.zzdc_czlx.Where(p => p.id == id).FirstOrDefault();
                return View("Update", Json(new { czlx = czlx }));
            }
        }
        public JsonResult Updating(int czlxid, string czlxmc, String czlxrs, string czlxms, string czlxxh)
        {
            using (MainDb db = new MainDb())
            {
                string sql = "update zzdc_czlx set name = '" + czlxmc + "',"
                                                + " rs = " + czlxrs
                                                + " remark = '" + czlxms + "',"
                                                + " rank = " + czlxxh
                                                + " where id = " + czlxid;
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
                string sql = string.Format(@"delete from zzdc_czlx where id in ('{0}')", string.Join("','", id));
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }
    }
}