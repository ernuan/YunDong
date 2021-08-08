using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudsMove.DAL
{
    public class T_CDKeyDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(T_CDKey model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();

            if (model.SerialCode != null)
            {
                strSql1.Append("SerialCode,");
                strSql2.Append("'" + model.SerialCode + "',");
            }
            if (model.Length != null)
            {
                strSql1.Append("Length,");
                strSql2.Append("'" + model.Length + "',");
            }
            if (model.State != null)
            {
                strSql1.Append("State,");
                strSql2.Append("'" + model.State + "',");
            }
            if (model.UserEmail != null)
            {
                strSql1.Append("UserEmail,");
                strSql2.Append("'" + model.UserEmail + "',");
            }
            if (model.UseTime != null)
            {
                strSql1.Append("UseTime,");
                strSql2.Append("'" + model.UseTime + "',");
            }
            strSql.Append("Insert Into T_CDKey(");
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
        public bool Update(T_CDKey model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            strSql.Append(" update T_CDKey set ");
            if (model.Length != null)
            {
                strSql1.Append("Length='" + model.Length + "',");
            }
            else
            {
                strSql1.Append("Length=null,");
            }
            if (model.State != null)
            {
                strSql1.Append("State='" + model.State + "',");
            }
            else
            {
                strSql1.Append("State=null,");
            }
            if (model.UserEmail != null)
            {
                strSql1.Append("UserEmail='" + model.UserEmail + "',");
            }
            else
            {
                strSql1.Append("UserEmail=null,");
            }
            if (model.UseTime != null)
            {
                strSql1.Append("UseTime='" + model.UseTime + "',");
            }
            else
            {
                strSql1.Append("UseTime=null,");
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
            sql_cmd.Append(" delete from T_CDKey where SerialCode='" + id + "'");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///得到一个对象实体
        ///</summary>
        public T_CDKey Find(string id)
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_CDKey where SerialCode='" + id + "' ");
            T_CDKey model = null;
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                if (dr.Read())
                {
                    model = new T_CDKey();
                    model.SerialCode = dr["SerialCode"].ToString();
                    if (dr["Length"] != null)
                    {
                        model.Length = dr["Length"].ToString();
                    }
                    if (dr["State"] != null)
                    {
                        model.State = dr["State"].ToString();
                    }
                    if (dr["UserEmail"] != null)
                    {
                        model.UserEmail = dr["UserEmail"].ToString();
                    }
                    if (dr["UseTime"] != null)
                    {
                        model.UseTime = dr["UseTime"].ToString();
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 查找全部
        /// </summary>
        /// <returns></returns>
        public List<T_CDKey> FindAll()
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_CDKey");
            List<T_CDKey> list = new List<T_CDKey>();
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                while (dr.Read())
                {
                    T_CDKey model = new T_CDKey();
                    model.SerialCode = dr["SerialCode"].ToString();
                    if (dr["Length"] != null)
                    {
                        model.Length = dr["Length"].ToString();
                    }
                    if (dr["State"] != null)
                    {
                        model.State = dr["State"].ToString();
                    }
                    if (dr["UserEmail"] != null)
                    {
                        model.UserEmail = dr["UserEmail"].ToString();
                    }
                    if (dr["UseTime"] != null)
                    {
                        model.UseTime = dr["UseTime"].ToString();
                    }
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
