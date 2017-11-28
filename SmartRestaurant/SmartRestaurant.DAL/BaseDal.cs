using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SmartRestaurant.Model;
using System.Data.Entity;

namespace SmartRestaurant.DAL
{
    public class BaseDal<T> : IBaseDal<T> where T : class, new()
    {
        private DbContext dbcontext;

        public BaseDal(DbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        #region 增删改

        public void Add(T t)
        {
            dbcontext.Set<T>().Add(t);
        }

        /// <summary>
        /// 删除自己生成的对象需要调用此方法，如果是查询结果则不需要调用
        /// </summary>
        /// <param name="t"></param>
        public void Delete(T t)
        {
            dbcontext.Set<T>().Attach(t);
            var entity = dbcontext.Entry(t);
            dbcontext.Set<T>().Remove(t);
        }

        /// <summary>
        /// 自己生成的对象需要调用此方法，如果是查询结果则不需要调用
        /// </summary>
        /// <param name="t"></param>
        public void Update(T t)
        {
            dbcontext.Set<T>().Attach(t);
            var entity = dbcontext.Entry(t);
            entity.State = System.Data.Entity.EntityState.Modified;
        }

        #endregion



        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return dbcontext.Set<T>().Where(whereLambda);
        }

        /// <summary>
        /// 更加查询条件获取一页数据，惰性加载
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="pageSize">一页记录数</param>
        /// <param name="pageIndex">页码（第一页开始）</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="OrderByLambda">排序字段（必需）</param>
        /// <param name="WhereLambda">条件</param>
        /// <returns></returns>
        public IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda)
        {
            if(pageIndex<1)
            {
                pageIndex = 1;
            }
            if (isAsc)
            {
                return dbcontext.Set<T>().Where(WhereLambda).OrderBy(OrderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return dbcontext.Set<T>().Where(WhereLambda).OrderByDescending(OrderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }

        

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <returns>影响的记录数</returns>
        public int SaveChanges()
        {
            return dbcontext.SaveChanges();
        }

        
    }
}
