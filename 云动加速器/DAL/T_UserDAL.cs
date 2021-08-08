using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudsMove.DAL
{
    public class T_UserDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(T_User model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();

            if (model.ID != null)
            {
                strSql1.Append("ID,");
                strSql2.Append("'" + model.ID + "',");
            }
            if (model.EMail != null)
            {
                strSql1.Append("EMail,");
                strSql2.Append("'" + model.EMail + "',");
            }
            if (model.Pwd != null)
            {
                strSql1.Append("Pwd,");
                strSql2.Append("'" + model.Pwd + "',");
            }
            if (model.CreatTime != null)
            {
                strSql1.Append("CreatTime,");
                strSql2.Append("'" + model.CreatTime + "',");
            }
            if (model.BlockingTime != null)
            {
                strSql1.Append("BlockingTime,");
                strSql2.Append("'" + model.BlockingTime + "',");
            }
            if (model.State != null)
            {
                strSql1.Append("State,");
                strSql2.Append("'" + model.State + "',");
            }
            strSql1.Append("LoginCount,");
            strSql2.Append("'" + model.LoginCount + "',");

            strSql.Append("Insert Into T_User(");
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
        public bool Update(T_User model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            strSql.Append(" update T_User set ");
            if (model.EMail != null)
            {
                strSql1.Append("EMail='" + model.EMail + "',");
            }
            else
            {
                strSql1.Append("EMail=null,");
            }
            if (model.Pwd != null)
            {
                strSql1.Append("Pwd='" + model.Pwd + "',");
            }
            else
            {
                strSql1.Append("Pwd=null,");
            }
            if (model.CreatTime != null)
            {
                strSql1.Append("CreatTime='" + model.CreatTime + "',");
            }
            else
            {
                strSql1.Append("CreatTime=null,");
            }
            if (model.BlockingTime != null)
            {
                strSql1.Append("BlockingTime='" + model.BlockingTime + "',");
            }
            else
            {
                strSql1.Append("BlockingTime=null,");
            }
            if (model.State != null)
            {
                strSql1.Append("State='" + model.State + "',");
            }
            else
            {
                strSql1.Append("State=null,");
            }
            strSql1.Append("LoginCount='" + model.LoginCount + "',");
            strSql.Append(strSql1.ToString().TrimEnd(','));
            strSql.Append(" where ID='" + model.ID + "' ");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, strSql.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///删除一条数据
        ///</summary>
        public bool Delete(string id)
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.Append(" delete from T_User where ID='" + id + "'");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///得到一个对象实体
        ///</summary>
        public T_User Find(string mail)
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_User where EMail='" + mail + "' ");
            T_User model = null;
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                if (dr.Read())
                {
                    model = new T_User();

                    model.ID = dr["ID"].ToString();
                    if (dr["EMail"] != null)
                    {
                        model.EMail = dr["EMail"].ToString();
                    }
                    if (dr["Pwd"] != null)
                    {
                        model.Pwd = dr["Pwd"].ToString();
                    }
                    if (dr["CreatTime"] != null)
                    {
                        model.CreatTime = Convert.ToDateTime(dr["CreatTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (dr["BlockingTime"] != null)
                    {
                        model.BlockingTime = Convert.ToDateTime(dr["BlockingTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (dr["State"] != null)
                    {
                        model.State = dr["State"].ToString();
                    }
                    model.LoginCount = Convert.ToInt32(dr["LoginCount"]);
                }
            }
            return model;
        }

        public T_User Find(string mail,string pwd)
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_User where EMail='" + mail + "' and Pwd='"+pwd+"'");
            T_User model = null;
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                if (dr.Read())
                {
                    model = new T_User();

                    model.ID = dr["ID"].ToString();
                    if (dr["EMail"] != null)
                    {
                        model.EMail = dr["EMail"].ToString();
                    }
                    if (dr["Pwd"] != null)
                    {
                        model.Pwd = dr["Pwd"].ToString();
                    }
                    if (dr["CreatTime"] != null)
                    {
                        model.CreatTime = Convert.ToDateTime(dr["CreatTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (dr["BlockingTime"] != null)
                    {
                        model.BlockingTime = Convert.ToDateTime(dr["BlockingTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (dr["State"] != null)
                    {
                        model.State = dr["State"].ToString();
                    }
                    model.LoginCount = Convert.ToInt32(dr["LoginCount"]);
                }
            }
            return model;
        }



        /// <summary>
        /// 查找全部
        /// </summary>
        /// <returns></returns>
        public List<T_User> FindAll()
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_User");
            List<T_User> list = new List<T_User>();
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                while (dr.Read())
                {
                    T_User model = new T_User();
                    model.ID = dr["ID"].ToString();
                    if (dr["EMail"] != null)
                    {
                        model.EMail = dr["EMail"].ToString();
                    }
                    if (dr["Pwd"] != null)
                    {
                        model.Pwd = dr["Pwd"].ToString();
                    }
                    if (dr["CreatTime"] != null)
                    {
                        model.CreatTime = Convert.ToDateTime(dr["CreatTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (dr["BlockingTime"] != null)
                    {
                        model.BlockingTime = Convert.ToDateTime(dr["BlockingTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (dr["State"] != null)
                    {
                        model.State = dr["State"].ToString();
                    }
                    model.LoginCount = Convert.ToInt32(dr["LoginCount"]);
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
