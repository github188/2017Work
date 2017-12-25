using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class CpsjController : Controller
    {
        // GET: Restaurant/Cpsj
        public ActionResult Index()
        {
            using (MainDb db = new MainDb())
            {
                var cplx = db.zzdc_cplx.AsQueryable().ToList();
                return View("index", Json(new { cplx = cplx }));
            }
        }
        public JsonResult getList(int limit, int offset, string search, int type)
        {
            using (MainDb db = new MainDb())
            {
                string cpmc = "";
                if (search != null && !"".Equals(search))
                    cpmc = search;
                var tList = db.zzdc_cpxx.Where(p => SqlFunctions.PatIndex("%" + cpmc + "%", p.name) > 0);
                if (type != 0)
                    tList = tList.Where(p => p.cplx_id == type);
                var varList = tList.Join(db.zzdc_cplx, p1 => p1.cplx_id, p2 => p2.id, (p1, p2) => new {
                    id = p1.id,
                    name = p1.name,
                    price = p1.price,
                    img = p1.img,
                    remark = p1.remark,
                    type = p2.name,
                    zd_id = p1.zd_id,
                    sfsj = p1.sfsj//是否上架
                }).Join(db.sys_dict, p1 => p1.zd_id, p2 => p2.id, (p1, p2) => new {
                    id = p1.id,//id
                    name = p1.name,//名称
                    price = p1.price,//价格
                    img = p1.img,//图片
                    remark = p1.remark,//简介
                    type = p1.type,//菜品类型
                    unit = p2.name,//菜品单位
                    sfsj = p1.sfsj//是否上架
                }).ToList();
                // var total = varList.Count;
                //var rows = varList.Skip(1).Take(3);
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit);
                return Json(new { total = total, rows = rows });
            }
        }
    }
}