using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class JsglController : Controller
    {
        // GET: Restaurant/Jsgl
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu)
        {//行数 页码 search参数
            using (MainDb db = new MainDb())
            {
                var varList = db.sys_role.AsQueryable().ToList();
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit).OrderBy(p => p.rank);
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Insert()
        {
            return View("Insert");
        }
        public JsonResult Inserting(String jsglmc, String jsglms, String jsglxh)
        {
            using (MainDb db = new MainDb())
            {
                sys_role jsgl = new sys_role();
                jsgl.name = jsglmc;
                jsgl.remark = jsglms;
                jsgl.rank = Convert.ToInt32(jsglxh);
                db.sys_role.Add(jsgl);
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
                var jsgl = db.sys_role.Where(p => p.id == id).FirstOrDefault();
                return View("Update", Json(new { jsgl = jsgl }));
            }
        }
        public JsonResult Updating(int jsglid, string jsglmc, string jsglms, string jsglxh)
        {
            using (MainDb db = new MainDb())
            {
                string sql = "update sys_role set name = '" + jsglmc + "',"
                                                + " remark = '" + jsglms + "',"
                                                + " rank = " + jsglxh
                                                + " where id = " + jsglid;
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
                string sql = string.Format(@"delete from sys_role where id in ('{0}')", string.Join("','", id));
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }

    }
}