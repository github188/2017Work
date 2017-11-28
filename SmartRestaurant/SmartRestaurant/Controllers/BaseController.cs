using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRestaurant.BLL;
using System.Web.Routing;

namespace SmartRestaurant.Controllers
{
    //基类控制器
    public abstract class BaseController<T> : Controller where T :class,new ()
    {
        private BaseBLL<T> bll;

        public BaseBLL<T> Bll
        {
            get
            {
                return bll;
            }

            set
            {
                bll = value;
            }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Bll = new BaseBLL<T>(GetDbContext());
        }


        /// <summary>
        /// 获取当前请求上下文关联的DbContext 多线程和异步处理等无请求上下文时请勿调用此方法
        /// </summary>
        /// <returns>与当前请求上下文关联的DbContext</returns>
        public  DbContext GetDbContext()
        {
            DbContext dbcontext = HttpContext.Items["DbContext"] as DbContext;
            if (dbcontext == null)
            {
                dbcontext = new MainDb();
                HttpContext.Items["DbContext"] = dbcontext;
            }
            return dbcontext;
        }



    }
}