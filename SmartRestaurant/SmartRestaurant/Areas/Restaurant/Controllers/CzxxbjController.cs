using Newtonsoft.Json.Linq;
using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class CzxxbjController : Controller
    {
        // GET: Restaurant/Czxxbj
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu)
        {//行数 页码 search参数
            using (MainDb db = new MainDb())
            {
                var varList = db.zzdc_czxx.Join(db.zzdc_czlx,p1=>p1.czlx_id,p2=>p2.id,(p1,p2)=>new {
                    id = p1.id,
                    zh = p1.zh,
                    type = p2.name,
                    status = p1.status,
                    remark = p1.remark,
                    rank = p1.rank              
                }).ToList();
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit).OrderBy(p => p.rank);
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Insert()
        {
            using (MainDb db = new MainDb())
            {
                var varList = db.zzdc_czlx.AsQueryable().ToList();
                return View("Insert", Json(new { czlx = varList }));
            }
        }
        public JsonResult Inserting(String czxxzh, String czlx, String czxxms, String czxxxh)
        {
            using (MainDb db = new MainDb())
            {
                zzdc_czxx czxx = new zzdc_czxx();
                czxx.zh = czxxzh;
                czxx.czlx_id = Convert.ToInt32(czlx);
                czxx.status = 0;
                czxx.remark = czxxms;
                czxx.rank = Convert.ToInt32(czxxxh);
                db.zzdc_czxx.Add(czxx);
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
                var czxx = db.zzdc_czxx.Where(p => p.id == id).FirstOrDefault();
                var varList = db.zzdc_czlx.AsQueryable().ToList();
                return View("Update", Json(new { czxx = czxx, czlx = varList }));
            }
        }
        public JsonResult Updating(int czxxid, String czxxzh, String czlx, String czxxms, String czxxxh)
        {
            using (MainDb db = new MainDb())
            {
                string sql = "update zzdc_czxx set zh = '" + czxxzh + "',"
                                                + " czlx_id = " + czlx + ","
                                                //+ " status = " + czxxstatus + ","
                                                + " remark = '" + czxxms + "',"
                                                + " rank = " + czxxxh
                                                + " where id = " + czxxid;
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
                string sql = string.Format(@"delete from zzdc_czxx where id in ('{0}')", string.Join("','", id));
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }
        
    }
}