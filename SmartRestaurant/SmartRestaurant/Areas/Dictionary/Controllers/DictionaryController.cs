using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRestaurant.Model;
using System.Data.Entity.SqlServer;
using System.Text;

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

        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="type">0为中文1为英文默认为英文</param>
        /// <returns></returns>
        public ActionResult  OutPutJs(int type=1)
        {
            using (DicMainDb db = new DicMainDb())
            {
                var tablelist = db.RcsTable.OrderBy(p=>p.ID).ToList();

                int[] tableid = tablelist.Select(p => p.ID).ToArray();

                var diclist = db.RcsDic.Where(p => tableid.Contains(p.TableID.Value)&&p.Type==1).ToList();

                StringBuilder result = new StringBuilder();

                result.Append("var dictionary = {\r\n");
                if(type!=0)
                {
                    foreach (var table in tablelist)
                    {
                        var tabledic = diclist.Where(p => p.TableID == table.ID).ToList();

                        result.Append("    " + table.Name + ": {\r\n");

                        foreach (var dic in tabledic)
                        {
                            result.Append("    " + dic.Name + ": \"" + dic.Value + "\",\r\n");

                        }

                        if (tabledic.Count > 0)
                        {
                            result.Remove(result.Length - 3, 3);
                            result.Append("\r\n");
                        }

                        result.Append("    },\r\n");

                    }
                }
                else
                {
                    foreach (var table in tablelist)
                    {
                        var tabledic = diclist.Where(p => p.TableID == table.ID).ToList();

                        result.Append("    " + table.Name + ": {\r\n");

                        foreach (var dic in tabledic)
                        {
                            result.Append("    " + dic.Name + ": \"" + dic.Name + "\",\r\n");

                        }

                        if (tabledic.Count > 0)
                        {
                            result.Remove(result.Length - 3, 3);
                            result.Append("\r\n");
                        }

                        result.Append("    },\r\n");

                    }
                }

                result.Append("}");


                return File(Encoding.UTF8.GetBytes(result.ToString()), "application/octet-stream", type==1?"en-us.js": "zh-cn.js");

            }


        }
    }
}