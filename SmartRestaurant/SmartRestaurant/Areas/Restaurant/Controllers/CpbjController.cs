using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
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
            using (MainDb db = new MainDb())
            {
                var cplx = db.zzdc_cplx.AsQueryable().ToList();
                return View("index", Json(new { cplx = cplx }));
            }
        }
        public JsonResult Delete(int id) {
            using (MainDb db = new MainDb())
            {
                var cpxx = db.zzdc_cpxx.Where(p => p.id == id).FirstOrDefault();
                if (!"/Content/restaurant/img/miss.jpg".Equals(cpxx.img))//若图片为miss则不删除
                {
                    var fullPath = Server.MapPath(cpxx.img);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        ViewBag.deleteSuccess = "true";
                    }
                }

                string sql = string.Format(@"delete from zzdc_cpxx where id in ('{0}')", string.Join("','", id));
                int count = db.Database.ExecuteSqlCommand(sql);
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
                var cplx = db.zzdc_cplx.AsQueryable().ToList();
                var cpdw = db.sys_dict.Where(p => p.pid == 1).ToList();//id=1 为 菜品单位      
                var cpxx = db.zzdc_cpxx.Where(p => p.id == id).FirstOrDefault();    
                return View("Update", Json(new { cplx = cplx, cpdw = cpdw ,cpxx = cpxx}));
            }
        }
        public JsonResult Updating(int cpid,string cpmc, int cplx, int cpdw, string cpjg, string cpms, string cpxh)
        {
            using (MainDb db = new MainDb())
            {
                HttpFileCollectionBase files = Request.Files;
                string upload = "";
                string fileName = "";
                if (files.Count > 0)
                {
                    HttpPostedFileBase file = files[0];
                    if (file.FileName != null && !"".Equals(file.FileName))//表示有文件
                    {
                        try
                        {
                            zzdc_cpxx cpxx = db.zzdc_cpxx.Where(p => p.id == cpid).FirstOrDefault();
                            if ("/Content/restaurant/img/miss.jpg".Equals(cpxx.img))//若图片miss
                            {
                                string Name = file.FileName;
                                string[] NameArray = Name.Split(new char[] { '.' });
                                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + NameArray[NameArray.Length - 1];
                            }
                            else//置换图片
                            {
                                string[] NameArray = cpxx.img.Split(new char[] { '/' });
                                fileName = NameArray[NameArray.Length - 1];
                            }
                            file.SaveAs(Server.MapPath("/Content/restaurant/img/upload")
                                    + "/" + fileName);
                            upload = "success";
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine("IOException source: {0}", e.Source);
                            upload = "fail";
                        }
                    }
                }
                string sql;
                if (upload.Equals("success")) {
                    sql = "update zzdc_cpxx set img = '/Content/restaurant/img/upload/" + fileName + "',"
                                            + " name = '" + cpms + "',"
                                            + " cplx_id = " + cplx + ","
                                            + " zd_id = " + cpdw + ","
                                            + " price = " + cpjg + ","
                                            + " remark = '" + cpms + "',"
                                            + " rank = " + cpxh
                                            + " where id = " + cpid;
                }
                else
                {
                    sql = "update zzdc_cpxx set name = '" + cpmc + "',"
                                            + " cplx_id = " + cplx + ","
                                            + " zd_id = " + cpdw + ","
                                            + " price = " + cpjg + ","
                                            + " remark = '" + cpms + "',"
                                            + " rank = " + cpxh
                                            + " where id = " + cpid;
                }
                int count = db.Database.ExecuteSqlCommand(sql);
                if (count > 0)
                    return Json(new { result = "success" });
                else
                    return Json(new { result = "fail" });
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
        public JsonResult getList(int limit,int offset,string search,int type) {
            using (MainDb db = new MainDb()) {
                string cpmc = "";       
                if (search != null && !"".Equals(search))
                    cpmc = search;
                var tList = db.zzdc_cpxx.Where(p => SqlFunctions.PatIndex("%" + cpmc + "%", p.name) > 0);
                if(type!=0)
                    tList = tList.Where(p=>p.cplx_id == type);
                var varList = tList.Join(db.zzdc_cplx, p1 => p1.cplx_id, p2 => p2.id, (p1, p2)=>new {
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
                var total = varList.Count;
                var rows = varList.Skip(offset).Take(limit);
                return Json(new { total = total ,rows = rows });
            }
        }
        public JsonResult Inserting(string cpmc,int cplx,int cpdw,string cpjg,string cpms,string cpxh)
        {
            using (MainDb db = new MainDb())
            {
                HttpFileCollectionBase files =  Request.Files;
                string upload = "";
                string fileName = "";
                if (files.Count>0)//
                {
                    HttpPostedFileBase file =files[0];
                    if (file.FileName != null && !"".Equals(file.FileName))//表示有文件
                        {
                            try
                            {
                                string Name = file.FileName;
                                string[] NameArray = Name.Split(new char[] { '.' });
                                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + NameArray[NameArray.Length - 1];
                                file.SaveAs(Server.MapPath("/Content/restaurant/img/upload")
                                    + "/" + fileName);
                                upload = "success";
                            }
                            catch (IOException e)
                            {
                                Console.WriteLine("IOException source: {0}", e.Source);
                                upload = "fail";
                            }
                        }
                }
                zzdc_cpxx cpxx = new zzdc_cpxx();
                cpxx.name = cpmc;
                if (upload.Equals("success"))
                    cpxx.img = "/Content/restaurant/img/upload/" + fileName;
                else
                    cpxx.img = "/Content/restaurant/img/miss.jpg";
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