using Newtonsoft.Json.Linq;
using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class ZdglController : Controller
    {
        // GET: Restaurant/Zdglontroller
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Insert() {
            return View("Insert",getSelect());
        }
        public JsonResult Delete(int id)
        {
            using (MainDb db = new MainDb())
            {
                if (isHaveSon(id) > 0)
                {
                    return Json(new { result = "notallow" });
                }
                else
                {
                    string sql = string.Format(@"delete from sys_dict where id in ('{0}')", string.Join("','", id));
                    int count = db.Database.ExecuteSqlCommand(sql);
                    if (count > 0)
                        return Json(new { result = "success" });
                    else
                        return Json(new { result = "fail" });
                }
            }
        }
        public int isHaveSon(int id)
        {//判断是否有子节点
            using (MainDb db = new MainDb())
            {
                var sonList = db.sys_dict.Where(p => p.pid == id).ToList();
                if (sonList.Count > 0)
                    return 1;
                else
                    return 0;
            }
        }
        public ActionResult Update(int id)
        {
            using (MainDb db = new MainDb())
            {
                var dict = db.sys_dict.Where(p => p.id == id).FirstOrDefault();
                return View("Update", Json(new { dict = dict }));
            }
        }
        public JsonResult Updating(int zdid, int sjzd, String zdmc, String zddz, String zdms, String zdxh)
        {
            using (MainDb db = new MainDb())
            {
                string sql = "update sys_dict set name = '" + zdmc + "',"
                                                + " pid = '" + sjzd + "',"
                                                + " value = '" + zddz + "',"
                                                + " remark = '" + zdms + "',"
                                                + " rank = " + zdxh
                                                + " where id = " + zdid;
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }
        public JsonResult Inserting(int sjzd, String zdmc, String zddz, String zdms, String zdxh)
        {
            using (MainDb db = new MainDb())
            {
                sys_dict dict = new sys_dict();
                dict.pid = sjzd;
                dict.name = zdmc;
                dict.value = zddz;
                dict.remark = zdms;
                dict.rank = Convert.ToInt32(zdxh);
                db.sys_dict.Add(dict);
                int count = db.SaveChanges();
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu)
        {//行数 页码 search参数
            using (MainDb db = new MainDb())
            {
                var varList = (from row in db.sys_dict.Where(p => p.pid == 0)
                               select new
                               {
                                   id = row.id,
                                   name = row.name,
                                   value = row.value,
                                   rank = row.rank,                           
                                   remark = row.remark,
                                   pname = "字典"
                               }).ToList();
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit);
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);       
            }
        }
        public JsonResult getSonList(int intParentID)//获取子表
        {
            using (MainDb db = new MainDb())
            {
                var varList = db.sys_dict.Where(p => p.pid == intParentID).Join(db.sys_dict, p1 => p1.pid, p2 => p2.id, (p1, p2) => new {
                    id = p1.id,
                    name = p1.name,
                    value = p1.value,
                    rank = p1.rank,
                    remark = p1.remark,
                    pname = p2.name
                }).ToList();
                var total = varList.Count;
                return Json(new { total = total, rows = varList }, JsonRequestBehavior.AllowGet);
            }
        }
        public JArray getSelect()
        {//获取下拉菜单
            JObject json = new JObject();
            json.Add("text", "字典");
            json.Add("value", "0");
            JArray jsons = new JArray();
            jsons.Add(json);
            getSelectList(0, jsons, "");
            return jsons;
        }
        public JArray getSelectList(int pid, JArray jsons, String str)//获取下拉菜单的子菜单
        {
            using (MainDb db = new MainDb())
            {
                var varList = db.sys_dict.Where(p => p.pid == pid);
                if (varList.Count() != 0)
                {//有子菜单
                    List<sys_dict> list = varList.ToList();
                    foreach (sys_dict dict in list)
                    {
                        JObject json = new JObject();
                        json.Add("text", str + "&nbsp&nbsp&nbsp&nbsp" + dict.name);
                        json.Add("value", dict.id);
                        jsons.Add(json);
                        getSelectList(dict.id, jsons, str + "&nbsp&nbsp&nbsp&nbsp");
                    }
                    return jsons;
                }
                else
                {//无子菜单
                    return null;
                }
            }
        }
    }
}