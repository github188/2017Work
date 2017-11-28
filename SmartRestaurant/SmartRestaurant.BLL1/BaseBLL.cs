using SmartRestaurant.DAL;
using SmartRestaurant.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartRestaurant.BLL
{
    public class BaseBLL<T> : IBaseBLL<T> where T : class, new()
    {
        private DbContext dbcontext;
        private BaseDal<T> dal;

        public BaseBLL(DbContext dbcontext)
        {
            this.dbcontext = dbcontext;
            dal = new BaseDal<T>(dbcontext);
        }

        #region 增删改

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Add(T t)
        {
            dal.Add(t);
            return dal.SaveChanges() > 0;
        }
        /// <summary>
        /// 应用与非连接的对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Delete(T t)
        {
            dal.Delete(t);
            return dal.SaveChanges() > 0;
        }
        /// <summary>
        /// 应用与非连接的对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Update(T t)
        {
            dal.Update(t);
            return dal.SaveChanges() > 0;
        }

        #endregion

        /// <summary>
        /// 获取符合条件的可查找集合
        /// </summary>
        /// <param name="whereLambda">条件</param>
        /// <returns></returns>
        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return dal.GetModels(whereLambda);
        }

        /// <summary>
        /// 获取符合条件的一页数据的可查找集合
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="pageSize">数据数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="OrderByLambda">排序字段</param>
        /// <param name="WhereLambda">条件字段</param>
        /// <returns></returns>
        public IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda)
        {
            return dal.GetModelsByPage(pageSize, pageIndex, isAsc, OrderByLambda, WhereLambda);
        }

        
    }
}
