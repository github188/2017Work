using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRestaurant.Model;
using Newtonsoft.Json;

namespace SmartRestaurant.Areas.Mobiel.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Mobiel/Home
        public ActionResult Index()
        {
            using(MainDb db=new MainDb())
            {
                int dinlingTableId = Int32.Parse(Request["dinlingTableId"]??"1");

                var orderList= db.zzdc_ddxx.Where(p => p.czxx_id == dinlingTableId && p.zd_id < 2).ToList();

                if(orderList.Count>0)
                {
                    OrderID = orderList[0].id;
                }
                else
                {
                    zzdc_ddxx newOrder = new zzdc_ddxx();
                    newOrder.cjsj = DateTime.Now;
                    newOrder.cjr = "test";
                    newOrder.remark = "";
                    newOrder.yxfk = 0;
                    newOrder.zd_id = 0;
                    newOrder.czxx_id = dinlingTableId;

                    db.zzdc_ddxx.Add(newOrder);
                    int count = db.SaveChanges();

                    newOrder = db.zzdc_ddxx.Where(p => p.czxx_id == dinlingTableId && p.zd_id < 2).First();

                    OrderID = newOrder.id;

                    //增加订单
                }

                var typeList = db.zzdc_cplx.Take(10).ToList();

                var idlist = typeList.Select(p1 => p1.id).ToList();
                var itemList = db.zzdc_cpxx.Where(p => idlist.Contains(p.cplx_id)).Take(100).ToList();

                var result = new List<Menu>();
                
                foreach(var type in typeList)
                {
                    var tmp = new List<MenuItem>();
                    foreach(var item in itemList.Where(p=>p.cplx_id==type.id))
                    {
                        tmp.Add(new MenuItem
                        {
                            id=item.id,
                            name=item.name,
                            summary=item.remark,
                            imgUrl=item.img,
                            price=item.price,
                            count=0
                        });
                    }
                    result.Add(new Menu
                    {
                        tagID=type.id,
                        tagName=type.name,
                        items=tmp
                    });
                }
                return View(result);
            }
        }
        
        public ActionResult ConfrimSubmit()
        {
            string datastr = Request["data"];

            List<MenuItem> datalist = JsonConvert.DeserializeObject<List<MenuItem>>(datastr);

            return View(datalist);
        }

        public ActionResult HasCheckItem()
        {
            using (MainDb db = new MainDb())
            {
                var result = db.zzdc_ddxq.Where(p => p.ddxx_id == OrderID).Join(db.zzdc_cpxx, p1 => p1.cpxx_id, p2 => p2.id, (p1, p2) => new MenuItem
                {
                    count = (int)p1.cpsl,
                    id = p2.id,
                    name = p2.name,
                    summary = p2.remark,
                    price = p2.price,
                    imgUrl = p2.img
                }).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Order()
        {
            string datastr = Request["data"];
            List<MenuItem> datalist = JsonConvert.DeserializeObject<List<MenuItem>>(datastr);

            try
            {
                using (MainDb db = new MainDb())
                {
                    double total = 0;

                    var order = db.zzdc_ddxx.Where(p => p.id == OrderID).First();
                    var detailList = new List<zzdc_ddxq>();
                    foreach (var item in datalist)
                    {
                        var tmp = new zzdc_ddxq();
                        tmp.remark = "";
                        tmp.ddxx_id = OrderID;
                        tmp.cpxx_id = item.id;
                        tmp.zd_id = 0;
                        tmp.cpsl = item.count;
                        db.zzdc_ddxq.Add(tmp);
                        total += (item.count * item.price);
                    }
                    order.yxfk += total;
                    db.SaveChanges();
                    return Json(new
                    {
                        IsSuccess = true
                    });
                }
            }
            catch(Exception e)
            {
                return Json(new
                {
                    IsSuccess = false
                });
            }

            
        }
        

        public ActionResult CheckOutOrder()
        {
            using (MainDb db = new MainDb())
            {
                var model=db.zzdc_ddxq.Where(p=>p.ddxx_id==OrderID).Join(db.zzdc_cpxx, p1 => p1.cpxx_id, p2 => p2.id, (p1, p2) => new MenuItem
                {
                    count = (int)p1.cpsl,
                    id = p2.id,
                    name = p2.name,
                    summary = p2.remark,
                    price = p2.price,
                    imgUrl = p2.img
                }).ToList();

                return View(model);
            }

        }

        public ActionResult SureCheckOutOrder()
        {
            using (MainDb db = new MainDb())
            {
                var order = db.zzdc_ddxx.Where(p => p.id == OrderID).FirstOrDefault();

                if(order!=null)
                {
                    order.zd_id = 2;
                    db.SaveChanges();
                    return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }

    public class Menu
    {
        public string tagName;
        public int tagID;
        public List<MenuItem> items;

    }
    public class MenuItem
    {
        public int id = 0;
        public string name=string.Empty;
        public string summary=string.Empty;
        public string imgUrl=string.Empty;
        public double price=0;
        public int count=0;
    }

}