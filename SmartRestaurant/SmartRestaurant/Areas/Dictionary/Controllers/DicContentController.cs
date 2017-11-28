using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRestaurant.Model;
using System.Data.Entity.SqlServer;
using System.Net;
using System.IO;
using System.Text;

namespace SmartRestaurant.Areas.Dictionary.Controllers
{
    public class DicContentController : Controller
    {
        // GET: Dictionary/DicContent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            using (DicMainDb db = new DicMainDb())
            {
                int rows = Convert.ToInt32(Request["rows"]);
                int page = Convert.ToInt32(Request["page"]);
                string seach = Request["search"];
                string TableID = Request["TableID"]??"";


                var list = db.RcsDic.Join(db.RcsTable, p1 => p1.TableID, p2 => p2.ID, (p1, p2) => new
                {
                    ID=p1.ID,
                    Name=p1.Name,
                    TableName=p2.Name,
                    Type=p1.Type,
                    Value=p1.Value,
                    TableID=p2.ID
                });
                int count = 0;
                if(!string.IsNullOrEmpty(TableID))
                {
                    int tid = Convert.ToInt32(TableID);
                    list = list.Where(p => p.TableID == tid);
                }
                

                if (!string.IsNullOrEmpty(seach))
                {
                    list = list.Where(p => SqlFunctions.PatIndex("%"+seach+"%", p.Name)>0);
                }
                //取记录数
                count = list.Count();
                var result = list.OrderByDescending(p=>p.ID).Skip(rows*(page-1)).Take(rows).ToList();

                return Json(new
                {
                    total = count,
                    rows = result
                });

            }
        }


        public ActionResult Detail()
        {
            RcsDic model = null;
            if (Request["ID"] != null && !string.IsNullOrWhiteSpace(Request["ID"]))
            {
                using (DicMainDb maindb = new DicMainDb())
                {
                    int ID = Convert.ToInt32(Request["ID"]);
                    model = maindb.RcsDic.Where(p => p.ID == ID).FirstOrDefault();
                }
            }

            return View(model);
        }

        public ActionResult EditAciton(RcsDic t)
        {
            using (DicMainDb maindb = new DicMainDb())
            {
                if (t.ID == 0)
                {
                    maindb.RcsDic.Add(t);
                    var table = maindb.RcsTable.Where(p => p.ID == t.TableID).FirstOrDefault();
                    table.Count = table.Count + 1;
                }
                else
                {
                    maindb.RcsDic.Attach(t);
                    var entity = maindb.Entry(t);
                    entity.State = System.Data.Entity.EntityState.Modified;
                }
                maindb.SaveChanges();
            }
            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public ActionResult Delete()
        {
            using (DicMainDb db = new DicMainDb())
            {
                string[] ID = Request["ID"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string sql = string.Format(@"delete from RcsDic where ID in ('{0}')", string.Join("','", ID));

                int count = db.Database.ExecuteSqlCommand(sql);

                sql = @"update [dbo].[RcsTable] set Count=(
select count(0) from [dbo].[RcsDic] t1
where t1.TableID=[RcsTable].ID
)";
                db.Database.ExecuteSqlCommand(sql);
                if (count>0)
                {
                    return Json(new
                    {
                        success = true,
                        count = count
                    });
                }
            }
            

            return Json(new
            {
                success = false,
                count=0
            });
        }

        public ActionResult NameCheck(RcsDic t,string time)
        {
            JsonResult result = Json(new { success = false, time = time }, JsonRequestBehavior.AllowGet);
            using (DicMainDb db = new DicMainDb())
            {
                if (db.RcsDic.Where(p => p.ID != t.ID && p.Name == t.Name&&p.TableID==t.TableID).Count() == 0)
                {
                    result = Json(new { success = true, time = time }, JsonRequestBehavior.AllowGet);
                }
            }

            return result;
        }

        public ActionResult AutoAdd(RcsDic t,bool Sure)
        {
            using (DicMainDb db = new DicMainDb())
            {
                var list = db.RcsDic.Where(p => p.Name == t.Name).ToList();

                if(list.Count==0 || Sure)
                {
                    db.RcsDic.Add(t);
                    var table = db.RcsTable.Where(p => p.ID == t.TableID).FirstOrDefault();
                    table.Count = table.Count + 1;

                    db.SaveChanges();
                    return Json(new { success = true, Code = 0 });
                }
                else
                {
                    var table = (from row in list.AsEnumerable()
                                 where row.TableID == t.TableID
                                 select row).ToList();

                    if (table.Count == 0)
                    {
                        return Json(new { success = false, Code=1, Data = list.Select(p=>p.ID).ToArray() });
                    }
                }
                
            }
            return Json(new { success = false, Code=-1 });
        }

        public ActionResult Translate(string query,string time, string to="en",string from="auto")
        {
            /*static $Lang = Array (
            'auto' => '自动检测',
            'ara' => '阿拉伯语',
            'de' => '德语',
            'ru' => '俄语',
            'fra' => '法语',
            'kor' => '韩语',
            'nl' => '荷兰语',
            'pt' => '葡萄牙语',
            'jp' => '日语',
            'th' => '泰语',
            'wyw' => '文言文',
            'spa' => '西班牙语',
            'el' => '希腊语',
            'it' => '意大利语',
            'en' => '英语',
            'yue' => '粤语',
            'zh' => '中文' 
    );*/

            
            string url = $@"http://fanyi.baidu.com/v2transapi?from={from}&query={HttpUtility.UrlEncode(query)}&to={to}";
            HttpWebRequest request = WebRequest.Create(url) as System.Net.HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return Json(new { Data = retString, time= time },JsonRequestBehavior.AllowGet);
        }


        public ActionResult RepeatField(string IDs)
        {
            using (DicMainDb db = new DicMainDb())
            {

                string[] idarr = IDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                List<int> idlist = new List<int>();

                foreach(var id in idarr)
                {
                    idlist.Add(Int32.Parse(id));
                }

                var list = db.RcsDic.Where(p => idlist.Contains(p.ID)).ToList();

                ViewBag.List = list;

                int?[] tableid = list.Select(p => p.TableID).ToArray();

                var tablelist = db.RcsTable.Where(p => tableid.Contains(p.ID)).ToList();
                ViewBag.TableList = tablelist;

            }


            return View();
        }


        public ActionResult AutoMerge(string IDs,string Value)
        {
            using (DicMainDb db = new DicMainDb())
            {

                string[] idarr = IDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                List<int> idlist = new List<int>();

                foreach (var id in idarr)
                {
                    idlist.Add(Int32.Parse(id));
                }
                var list = db.RcsDic.Where(p => idlist.Contains(p.ID)).ToList();

                foreach(var t in list)
                {
                    db.RcsDic.Attach(t);
                    var entity = db.Entry(t);

                    entity.State = System.Data.Entity.EntityState.Deleted;
                }

                var defalutTable = db.RcsTable.Where(p => p.Name == "defaultdic").First();


                RcsDic newmodel = new RcsDic();

                newmodel.Name = list[0].Name;
                newmodel.Value = Value;
                newmodel.Type = 1;
                newmodel.TableID = defalutTable.ID;

                db.RcsDic.Add(newmodel);

                db.SaveChanges();

                string sql = @"update [dbo].[RcsTable] set Count=(
select count(0) from [dbo].[RcsDic] t1
where t1.TableID=[RcsTable].ID
)";
                db.Database.ExecuteSqlCommand(sql);
            }

            return Json(1);
        }

    }
}