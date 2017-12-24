using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SmartRestaurant.Areas.Restaurant.Controllers
{
    public class CpbjController : Controller
    {
        // GET: Restaurant/Cpbj
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Update(int id)
        {
            using (MainDb db = new MainDb())
            {
                var cplx = db.zzdc_cplx.AsQueryable().ToList();
                var cpdw = db.sys_dict.Where(p => p.pid == 1).ToList();//id=1 为 菜品单位      
                var cpxx = db.zzdc_cpxx.Where(p => p.id == id).ToList();    
                return View("Update", Json(new { cplx = cplx, cpdw = cpdw ,cpxx = cpxx}));
            }
        }
        public ActionResult Insert() {
            using (MainDb db = new MainDb())
            {
                var cplx = db.zzdc_cplx.AsQueryable().ToList();
                var cpdw = db.sys_dict.Where(p => p.pid == 1).ToList();//id=1 为 菜品单位          
                return View("Insert", Json(new { cplx = cplx , cpdw = cpdw }));
            }
        }
        public JsonResult getList() {
            using (MainDb db = new MainDb()) {
                var varList = db.zzdc_cpxx.Join(db.zzdc_cplx, p1 => p1.cplx_id, p2 => p2.id, (p1, p2)=>new {
                    id = p1.id,
                    name = p1.name,
                    price = p1.price,
                    img = p1.img,
                    remark = p1.remark,
                    type = p2.name,
                    zd_id = p1.zd_id
                }).Join(db.sys_dict, p1 => p1.zd_id, p2 => p2.id, (p1, p2)=> new {
                    id = p1.id,//id
                    name = p1.name,//名称
                    price = p1.price,//价格
                    img = p1.img,//图片
                    remark = p1.remark,//简介
                    type = p1.type,//菜品类型
                    unit = p2.name//菜品单位
                }).ToList();
                // var total = varList.Count;
                //var rows = varList.Skip(1).Take(3);
                return Json(new { rows = varList});
            }
        }
        public JsonResult Inserting(string cpmc,int cplx,int cpdw,string cpjg,string cpms,string cpxh)
        {
            using (MainDb db = new MainDb())
            {
                HttpFileCollectionBase files =  Request.Files;
                string upload = "";
                string fileName = "";
                if (files.Count>0)
                {
                    HttpPostedFileBase file =files[0];
                    try {
                        string Name = file.FileName;
                        string[] NameArray = Name.Split(new char[] { '.' });
                        fileName = DateTime.Now.ToString("yyyyMMddHHmmss") +"."+ NameArray[NameArray.Length - 1];
                        file.SaveAs(Server.MapPath("/Content/restaurant/img/upload") 
                            + "/" + fileName);
                        upload = "success";
                    }
                    catch(IOException e) {
                        Console.WriteLine("IOException source: {0}", e.Source);
                        upload = "fail";
                    }
                }
                zzdc_cpxx cpxx = new zzdc_cpxx();
                cpxx.name = cpmc;
                if (upload.Equals("success"))
                    cpxx.img = "/Content/restaurant/img/upload/" + fileName;
                else
                    cpxx.img = "/Content/restaurant/img/miss";
                cpxx.cplx_id = cplx;
                cpxx.zd_id = cpdw;
                cpxx.price = Convert.ToSingle(cpjg);
                cpxx.sfsj = 0;
                cpxx.remark = cpms;
                cpxx.rank = Convert.ToInt32(cpxh);
                db.zzdc_cpxx.Add(cpxx);
                int count = db.SaveChanges();
                if (count > 0)
                    return Json(new { result = "success" ,upload = upload});
                else
                    return Json(new { result = "fail",upload = upload });
            }
        }
    }
}