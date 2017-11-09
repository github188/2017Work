using System.Web.Mvc;
using SmartRestaurant.BLL;
using System.Linq;

namespace SmartRestaurant.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (var db = new MainDb())
            {
                //取id=1的列
                var model = db.zzdc_czlx.Where(p => p.id == 1).FirstOrDefault();

                
            }

                return View();
        }
    }
}