using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class WlxxbjController : Controller
    {
        // GET: Restaurant/Wlxxbj
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu)
        {//行数 页码 search参数
            using (MainDb db = new MainDb())
            {
                var varList = db.zzdc_wlxx.Join(db.zzdc_wllx, p1 => p1.wllx_id, p2 => p2.id, (p1, p2) => new {
                    id = p1.id,
                    name = p1.name,
                    type = p2.name,
                    kcsl = p1.kcsl,
                    dw = p1.zd_id,
                    remark = p1.remark,
                    rank = p1.rank
                }).Join(db.sys_dict,p1=>p1.dw,p2=>p2.id,(p1,p2)=>new
                {
                    id = p1.id,
                    name = p1.name,
                    type = p1.type,
                    kcsl = p1.kcsl,
                    dw = p2.name,
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
                var varListwl = db.zzdc_wllx.AsQueryable().ToList();
                var varListzd = db.sys_dict.Where(p=>p.pid==5).ToList();
                return View("Insert", Json(new { wllx = varListwl , zd = varListzd }));
            }
        }
        public JsonResult Inserting(String wlxxname, String wllx, String zd, String wlxxms, String wlxxxh)
        {
            using (MainDb db = new MainDb())
            {
                zzdc_wlxx wlxx = new zzdc_wlxx();
                wlxx.name = wlxxname;
                wlxx.wllx_id = Convert.ToInt32(wllx);
                wlxx.kcsl = 0;
                wlxx.zd_id = Convert.ToInt32(zd);
                wlxx.remark = wlxxms;
                wlxx.rank = Convert.ToInt32(wlxxxh);
                db.zzdc_wlxx.Add(wlxx);
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
                var wlxx = db.zzdc_wlxx.Where(p => p.id == id).FirstOrDefault();
                var varList = db.zzdc_wllx.AsQueryable().ToList();
                var varListzd = db.sys_dict.Where(p => p.pid == 5).ToList();
                return View("Update", Json(new { wlxx = wlxx, wllx = varList ,zd = varListzd}));
            }
        }
        public JsonResult Updating(int wlxxid, String wlxxname, String wllx, String zd, String wlxxms, String wlxxxh)
        {
            using (MainDb db = new MainDb())
            {
                string sql = "update zzdc_wlxx set name = '" + wlxxname + "',"
                                                + " wllx_id = " + wllx + ","
                                                + " zd_id = " + zd + ","
                                                //+ " status = " + wlxxstatus + ","
                                                + " remark = '" + wlxxms + "',"
                                                + " rank = " + wlxxxh
                                                + " where id = " + wlxxid;
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
                string sql = string.Format(@"delete from zzdc_wlxx where id in ('{0}')", string.Join("','", id));
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }


    }
}