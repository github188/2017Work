using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRestaurant.Model;
using Newtonsoft.Json.Linq;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class HomeController : SmartRestaurant.Controllers.BaseController<sys_menu>
    {
        // GET: Restaurant/Home
        public ActionResult Index()
        {
            JObject json = new JObject();
            //Session["username"] = "system";
            json.Add("id", "0");
            json.Add("parentId", "0");
            json.Add("text", "主菜单");
            return View("index",getTreeMenu(json));
        }
        public JObject getTreeMenu(JObject jsonMenu) {
            using (MainDb db = new MainDb()) {
                int pid = Convert.ToInt32(jsonMenu["id"].ToString());
                var varList = db.sys_menu.Where(p => p.pid == pid);
                if (varList.Count() != 0) {//有子菜单
                    List<sys_menu> list = varList.ToList();
                    JArray jsonMenus = new JArray();
                    foreach (sys_menu menu in list) {
                        JObject json = new JObject();
                        json.Add("id", menu.id);
                        json.Add("parentId", menu.pid);
                        json.Add("text", menu.name);
                        if (menu.url != null && !"".Equals(menu.url))
                            json.Add("url", menu.url);
                        getTreeMenu(json);
                        jsonMenus.Add(json);
                    }
                    jsonMenu.Add("children", jsonMenus);
                    return jsonMenu;
                }else{//无子菜单
                    JObject json = new JObject();
                    jsonMenu.Add("children", json);
                    return jsonMenu;
                }
            }
        }

        public ActionResult test()
        {
            return Json(new {
                aa = "11",
                bbb = new int[1,2,3]
            },JsonRequestBehavior.AllowGet);
        }
        public ActionResult Welcome() {
            return View("Welcome");
        }
    }
}