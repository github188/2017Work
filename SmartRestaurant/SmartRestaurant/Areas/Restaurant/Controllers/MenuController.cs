using Newtonsoft.Json.Linq;
using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class MenuController : SmartRestaurant.Controllers.BaseController<sys_menu>
    {
        public ActionResult Index(){
            return View();
        }
        public ActionResult Insert(){
            return View("Insert",getSelect());
        }
        public JsonResult Inserting(int sjcd,String cdmc,String cdurl,String cdms,String cdxh) {
            using (MainDb db = new MainDb())
            {
                sys_menu menu = new sys_menu();
                menu.pid = sjcd;
                menu.name = cdmc;
                menu.url = cdurl;
                menu.remark = cdms;
                menu.rank = Convert.ToInt32(cdxh);
                db.sys_menu.Add(menu);
                int count = db.SaveChanges();
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }
        public ActionResult Update(int id) {
            using (MainDb db = new MainDb()){
                var menu = db.sys_menu.Where(p => p.id == id).FirstOrDefault();
                return View("Update",Json(new { menu = menu }));
            }
        }

        public JsonResult Updating(int cdid, int sjcd, String cdmc, String cdurl, String cdms, String cdxh) {
            using (MainDb db = new MainDb()){
                string sql = "update sys_menu set name = '" + cdmc + "',"
                                                + " pid = '" + sjcd + "',"
                                                + " url = '" + cdurl + "',"
                                                + " remark = '" + cdms + "',"
                                                + " rank = " + cdxh 
                                                + " where id = " + cdid;
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
            }
        }
        public JsonResult Delete(int id) {
            using (MainDb db = new MainDb()) {
                if (isHaveSon(id) > 0) {
                    return Json(new { result = "notallow" });
                } else
                {
                    string sql = string.Format(@"delete from sys_menu where id in ('{0}')", string.Join("','", id));
                    int count = db.Database.ExecuteSqlCommand(sql);
                    if (count > 0)
                        return Json(new { result = "success" });
                    else
                        return Json(new { result = "fail" });
                }
            }
        }
        public JsonResult getList(int limit, int offset, string departmentname, string statu) {//行数 页码 search参数
            using (MainDb db = new MainDb()){

                var varList = (from row in db.sys_menu.Where(p => p.pid == 0) select new {
                    id = row.id,
                    name = row.name,
                    url = row.url,
                    remark = row.remark,
                    pname = "主菜单",
                    isHaveSon = 0
                }).ToList();
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit);
                return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);

                //var varList = (from row in db.sys_menu.AsQueryable()
                //               select new
                //               {
                //                   name = row.name,
                //                   url = row.url,
                //               }).ToList();
                //var total = varList.Count;
                //var rows = varList.Skip(offset).Take(limit).ToList();
                //return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);


                //System.Data.DataSet ds = SqlHelper.GetDataSet(@"select * from aa ;select * from aa ;
                //    ");
                //ds.Tables[0].Rows[0]["column1"];

                //var list = db.sys_menu.Join(db.sys_menu, p1 => p1.id, p2 => p2.pid, (p1, p2) => new
                //{
                //    Name = p2.Name,
                //    pName = p1.,
                //    Value = p1.Value,
                //    TableID = p2.ID
                //});          
            }
        }
        public JsonResult getSonList(int intParentID)//获取子表
        {
            //int pid = id;
            //int pid = Convert.ToInt32(Request["strParentID"]);
            using (MainDb db = new MainDb())
            {
                var varList = db.sys_menu.Where(p => p.pid == intParentID).Join(db.sys_menu, p1 => p1.pid, p2 => p2.id,(p1, p2)=> new {
                    id = p1.id,
                    name = p1.name,
                    url = p1.url,
                    remark = p1.remark,
                    pname = p2.name
                }).ToList();
                var total = varList.Count;
                return Json(new {  total = total, rows = varList }, JsonRequestBehavior.AllowGet);
            }
        }
        public int isHaveSon(int id) {//判断是否有子节点
            using (MainDb db = new MainDb())
            {
                var sonList = db.sys_menu.Where(p => p.pid == id).ToList();
                if (sonList.Count > 0)
                    return 1;
                else
                    return 0;
            }
       } 

        public JArray getSelect() {//获取下拉菜单
            JObject json = new JObject();
            json.Add("text", "主菜单");
            json.Add("value", "0");
            JArray jsons= new JArray();
            jsons.Add(json);
            getSelectList(0, jsons, "");
            return jsons;
        }
        public JArray getSelectList(int pid,JArray jsons,String str)//获取下拉菜单的子菜单
        {
            using (MainDb db = new MainDb())
            {
                var varList = db.sys_menu.Where(p => p.pid == pid);
                if (varList.Count() != 0)
                {//有子菜单
                    List<sys_menu> list = varList.ToList();
                    foreach (sys_menu menu in list)
                    {
                        JObject json = new JObject();
                        json.Add("text", str+ "&nbsp&nbsp&nbsp&nbsp" + menu.name);
                        json.Add("value", menu.id);
                        jsons.Add(json);
                        getSelectList(menu.id,jsons, str+ "&nbsp&nbsp&nbsp&nbsp");
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