using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class CplxbjController : Controller
    {
        // GET: Restaurant/Cplxbj
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu)
        {//行数 页码 search参数
            using (MainDb db = new MainDb())
            {
                var varList = db.zzdc_cplx.AsQueryable().ToList();
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit).OrderBy(p=>p.rank);
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Insert()
        {
            return View("Insert");
        }
        public JsonResult Inserting(String cplxmc, String cplxms, String cplxxh)
        {
            using (MainDb db = new MainDb())
            {
                zzdc_cplx cplx = new zzdc_cplx();
                cplx.name = cplxmc;
                cplx.remark = cplxms;
                cplx.rank = Convert.ToInt32(cplxxh);
                db.zzdc_cplx.Add(cplx);
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
                var cplx = db.zzdc_cplx.Where(p => p.id == id).FirstOrDefault();
                return View("Update", Json(new { cplx = cplx }));
            }           
        }
        public JsonResult Updating(int cplxid, string cplxmc, string cplxms, string cplxxh)
        {
            using (MainDb db = new MainDb())
            {
                string sql = "update zzdc_cplx set name = '" + cplxmc + "',"
                                                + " remark = '" + cplxms + "',"
                                                + " rank = " + cplxxh
                                                + " where id = " + cplxid;
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }
        public JsonResult Delete(int id){
            using (MainDb db = new MainDb()){
                string sql = string.Format(@"delete from zzdc_cplx where id in ('{0}')", string.Join("','", id));
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }
    }
}