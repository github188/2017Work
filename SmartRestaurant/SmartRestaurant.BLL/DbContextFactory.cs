using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmartRestaurant.Model
{
    public static class DbContextFactory
    {
        /// <summary>
        /// 获取当前请求上下文关联的DbContext 多线程和异步处理等无请求上下文时请勿调用此方法
        /// </summary>
        /// <returns>与当前请求上下文关联的DbContext</returns>
        public static DbContext GetDbContext()
        {
            DbContext dbcontext = HttpContext.Current.Items["DbContext"] as DbContext;
            if (dbcontext == null)
            {
                dbcontext = new MainDb();
                HttpContext.Current.Items["DbContext"] = dbcontext;
            }
            return dbcontext;
        }
    }
}
