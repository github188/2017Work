using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SmartRestaurant
{
    public class SqlHelper
    {
        public static string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["DicDbCon"].ToString();
        private static SqlConnection Open()
        {
            SqlConnection conn = new SqlConnection(ConnStr);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// 执行带参数的sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>受影响的行数</returns>
        public static int ExecSql(string sql, params SqlParameter[] parameters)
        {
            try
            {

                SqlConnection conn = Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
                int result = cmd.ExecuteNonQuery();
                conn.Dispose();
                conn.Close();
                return result;
            }
            catch (Exception ex)
            {
                //TxtLogHelper.Log(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 执行简单的sql语句
        /// </summary>
        /// <param name="sql">受影响的行数</param>
        /// <returns></returns>
        public static int ExtSql(string sql)
        {

            SqlConnection conn = Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            int result = cmd.ExecuteNonQuery();
            conn.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// 查找第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>第一行第一列</returns>
        public static object ScalSql(string sql)
        {
            try
            {
                SqlConnection conn = Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();
                conn.Dispose();
                conn.Close();
                return result;
            }
            catch (Exception ex)
            {
                //TxtLogHelper.Log(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 获取datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, params SqlParameter[] parameters)
        {
            try
            {
                SqlConnection conn = Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(sql, conn);
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Dispose();
                conn.Close();
                return dt;

            }
            catch (SqlException e)
            {
                return new DataTable();
            }

        }

        public static DataSet GetDataSet(string sql)
        {
            try
            {
                SqlConnection conn = Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
                sda.Fill(ds);
                conn.Dispose();
                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                //TxtLogHelper.Log(ex.Message);
                return null;
            }
        }
    }
}