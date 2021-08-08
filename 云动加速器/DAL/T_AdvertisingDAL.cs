using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudsMove.DAL
{
    public class T_AdvertisingDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(T_Advertising model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();

            if (model.SerialCode != null)
            {
                strSql1.Append("SerialCode,");
                strSql2.Append("'" + model.SerialCode + "',");
            }
            if (model.ImageUrl != null)
            {
                strSql1.Append("ImageUrl,");
                strSql2.Append("'" + model.ImageUrl + "',");
            }
            if (model.ImageLink != null)
            {
                strSql1.Append("ImageLink,");
                strSql2.Append("'" + model.ImageLink + "',");
            }
            strSql.Append("Insert Into T_Advertising(");
            strSql.Append(strSql1.ToString().TrimEnd(','));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().TrimEnd(','));
            strSql.Append(")");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, strSql.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///更新一条数据
        ///</summary>
        public bool Update(T_Advertising model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            strSql.Append(" update T_Advertising set ");
            if (model.ImageUrl != null)
            {
                strSql1.Append("ImageUrl='" + model.ImageUrl + "',");
            }
            else
            {
                strSql1.Append("ImageUrl=null,");
            }
            if (model.ImageLink != null)
            {
                strSql1.Append("ImageLink='" + model.ImageLink + "',");
            }
            else
            {
                strSql1.Append("ImageLink=null,");
            }
            strSql.Append(strSql1.ToString().TrimEnd(','));
            strSql.Append(" where SerialCode='" + model.SerialCode + "' ");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, strSql.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///删除一条数据
        ///</summary>
        public bool Delete(string id)
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.Append(" delete from T_Advertising where SerialCode='" + id + "'");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///得到一个对象实体
        ///</summary>
        public T_Advertising Find(string id)
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_Advertising where SerialCode='" + id + "' ");
            T_Advertising model = new T_Advertising();
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                if (dr.Read())
                {
                    model.SerialCode = dr["SerialCode"].ToString();
                    if (dr["ImageUrl"] != null)
                    {
                        model.ImageUrl = dr["ImageUrl"].ToString();
                    }
                    if (dr["ImageLink"] != null)
                    {
                        model.ImageLink = dr["ImageLink"].ToString();
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 查找全部
        /// </summary>
        /// <returns></returns>
        public List<T_Advertising> FindAll()
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_Advertising");
            List<T_Advertising> list = new List<T_Advertising>();
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                while (dr.Read())
                {
                    T_Advertising model = new T_Advertising();
                    model.SerialCode = dr["SerialCode"].ToString();
                    if (dr["ImageUrl"] != null)
                    {
                        model.ImageUrl = dr["ImageUrl"].ToString();
                    }
                    if (dr["ImageLink"] != null)
                    {
                        model.ImageLink = dr["ImageLink"].ToString();
                    }
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
