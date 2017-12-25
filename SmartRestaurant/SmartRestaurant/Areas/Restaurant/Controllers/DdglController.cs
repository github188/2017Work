using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class DdglController : Controller
    {
        // GET: Restaurant/Ddgl
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu)
        {//行数 页码 search参数
            using (MainDb db = new MainDb())
            {
                var varList = db.zzdc_ddxx.AsQueryable().ToList();
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit).OrderBy(p => p.id);
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Insert()
        {
            return View("Insert");
        }
        public JsonResult Inserting(string ddxxczxx_id, string ddxxcjsj, string ddxxcjr, string ddxxremark, string ddxxjdsj, string ddxxjdr, string ddxxjd_remark,
                                    string ddxxfksj, string ddxxfkr, string ddxxyxfk, string ddxxsjfk, string ddxxzd_id, string ddxxpj)
        {
            using (MainDb db = new MainDb())
            {

                zzdc_ddxx ddxx = new zzdc_ddxx();
                ddxx.czxx_id = Convert.ToInt32(ddxxczxx_id);
                //ddxx.cjsj = ddxxcjsj;
                ddxx.cjr = ddxxcjr;
                ddxx.remark = ddxxremark;
                //ddxx.jdsj = ddxxjdsj;
                ddxx.jdr = ddxxjdr;
                ddxx.jd_remark = ddxxjd_remark;
                //ddxx.fksj = ddxxfksj;
                ddxx.fkr = ddxxfkr;
                ddxx.yxfk = Convert.ToInt32(ddxxyxfk);
                ddxx.sjfk = Convert.ToInt32(ddxxsjfk);
                ddxx.zd_id = Convert.ToInt32(ddxxzd_id);
                ddxx.pj = ddxxpj;
                db.zzdc_ddxx.Add(ddxx);
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
                var ddxx = db.zzdc_ddxx.Where(p => p.id == id).FirstOrDefault();
                return View("Update", Json(new { ddxx = ddxx }));
            }
        }
        public JsonResult Updating(int ddxxid, string ddxxmc, string ddxxms, string ddxxxh)
        {
            using (MainDb db = new MainDb())
            {
                string sql = "update zzdc_ddxx set name = '" + ddxxmc + "',"
                                                + " remark = '" + ddxxms + "',"
                                                + " rank = " + ddxxxh
                                                + " where id = " + ddxxid;
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
                string sql = string.Format(@"delete from zzdc_ddxx where id in ('{0}')", string.Join("','", id));
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }

    }
}