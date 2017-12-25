using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class YhglController : Controller
    {
        // GET: Restaurant/Yhgl
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu)
        {//行数 页码 search参数
            using (MainDb db = new MainDb())
            {
                var varList = db.sys_user.AsQueryable().ToList();
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit).OrderBy(p => p.rank);
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Insert()
        {
            return View("Insert");
        }
        public JsonResult Inserting(String yhgluname, String yhglpasswd, String yhglmc, String yhglms, String yhglxh)
        {
            using (MainDb db = new MainDb())
            {
                sys_user yhgl = new sys_user();
                yhgl.username = yhgluname;
                yhgl.password = yhglpasswd;
                yhgl.name = yhglmc;
                yhgl.remark = yhglms;
                yhgl.rank = Convert.ToInt32(yhglxh);
                db.sys_user.Add(yhgl);
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
                var yhgl = db.sys_user.Where(p => p.id == id).FirstOrDefault();
                return View("Update", Json(new { yhgl = yhgl }));
            }
        }
        public JsonResult Updating(int yhglid, String yhgluname, String yhglpasswd, string yhglmc, string yhglms, string yhglxh)
        {
            using (MainDb db = new MainDb())
            {
                string sql = "update sys_user set username = '" + yhgluname + "',"
                                                + " password = '" + yhglpasswd + "',"
                                                + " name = '" + yhglms + "',"
                                                + " remark = '" + yhglms + "',"
                                                + " rank = " + yhglxh
                                                + " where id = " + yhglid;
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
                string sql = string.Format(@"delete from sys_user where id in ('{0}')", string.Join("','", id));
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }

    }
}