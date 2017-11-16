using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRestaurant.BLL;
using System.Data.Entity.SqlServer;

namespace SmartRestaurant.Areas.Dictionary.Controllers
{
    public class DictionaryController : Controller
    {
        // GET: Dictionary/Dictionary
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditAciton(RcsTable t)
        {
            using(DicMainDb maindb=new DicMainDb())
            {
                if(t.ID==0)
                {
                    maindb.RcsTable.Add(t);
                }
                else
                {
                    maindb.RcsTable.Attach(t);
                    var entity = maindb.Entry(t);
                    entity.State = System.Data.Entity.EntityState.Modified;
                }
                maindb.SaveChanges();
            }
            return Json(new {
                success=true
            });
        }

        [HttpPost]
        public ActionResult Delete()
        {
            string[] ID = Request["ID"].Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);

            string sql = string.Format(@"
delete from RcsDic where TableID in ('{0}');
delete from RcsTable where ID in ('{0}')", string.Join("','", ID));

            SqlHelper.ExtSql(sql);

            return Json(new
            {
                success = true
            });
        }

        public ActionResult Detail()
        {
            RcsTable model = null;
            if(Request["ID"]!=null&&!string.IsNullOrWhiteSpace(Request["ID"]))
            {
                using (DicMainDb maindb = new DicMainDb())
                {
                    int ID = Convert.ToInt32(Request["ID"]);
                    model = maindb.RcsTable.Where(p=>p.ID== ID).FirstOrDefault();
                }
            }

            return View("Detail","", model);
        }

        public ActionResult List()
        {
            
            using (DicMainDb maindb = new DicMainDb())
            {
                int rows = Convert.ToInt32(Request["rows"]);
                int page= Convert.ToInt32(Request["page"]);
                string seach = Request["search"];
                List<RcsTable> list = null;

                var tlist = maindb.RcsTable.AsQueryable();

                if (!string.IsNullOrEmpty(seach))
                {
                    tlist=tlist.Where(p => SqlFunctions.PatIndex("%" + seach + "%", p.Name) > 0);
                }
                
                int count = tlist.Count();

                list = tlist.OrderBy(p => p.ID).Skip((page - 1) * rows).Take(rows).ToList();

                var result = new
                {
                    total = count,
                    rows = list
                };


                return Json(result);

            }
        }

        public ActionResult NameCheck(RcsTable t,string time)
        {
            JsonResult result = Json(new { success = false, time = time }, JsonRequestBehavior.AllowGet);
            using (DicMainDb db = new DicMainDb())
            {
                if(db.RcsTable.Where(p => p.ID != t.ID && p.Name == t.Name).Count()==0)
                {
                    result = Json(new { success = true, time = time }, JsonRequestBehavior.AllowGet);
                }
            }

            return result;
        }

    }
}