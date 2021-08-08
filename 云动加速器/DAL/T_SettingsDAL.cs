using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CloudsMove.DAL
{
    public partial class T_SettingsDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(T_Settings model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();

            strSql1.Append("SerialCode,");
            strSql2.Append("'" + "YunDong" + "',");
            if (model.LoginViewImg != null)
            {
                strSql1.Append("LoginViewImg,");
                strSql2.Append("'" + model.LoginViewImg + "',");
            }
            if (model.UserRegBlockTime != null)
            {
                strSql1.Append("UserRegBlockTime,");
                strSql2.Append("'" + model.UserRegBlockTime + "',");
            }
            if (model.QQHeadApi != null)
            {
                strSql1.Append("QQHeadApi,");
                strSql2.Append("'" + model.QQHeadApi + "',");
            }
            
            strSql.Append("Insert Into T_Settings(");
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
        public bool Update(T_Settings model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            strSql.Append(" update T_Settings set ");
            //strSql1.Append("id='" + model.id + "',");
            if (model.LoginViewImg != null)
            {
                strSql1.Append("LoginViewImg='" + model.LoginViewImg + "',");
            }
            else
            {
                strSql1.Append("LoginViewImg=null,");
            }
            if (model.UserRegBlockTime != null)
            {
                strSql1.Append("UserRegBlockTime='" + model.UserRegBlockTime + "',");
            }
            else
            {
                strSql1.Append("UserRegBlockTime=null,");
            }
            if (model.QQHeadApi != null)
            {
                strSql1.Append("QQHeadApi='" + model.QQHeadApi + "',");
            }
            else
            {
                strSql1.Append("QQHeadApi=null,");
            }
            
            
            strSql.Append(strSql1.ToString().TrimEnd(','));
            strSql.Append(" where SerialCode='" + model.SerialCode + "' ");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, strSql.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///得到一个对象实体
        ///</summary>
        public T_Settings Find()
        {
            string serialCode = "YunDong";
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_Settings where SerialCode='" + serialCode + "' ");
            T_Settings model = null;
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                if (dr.Read())
                {
                    model = new T_Settings();
                    model.SerialCode = dr["SerialCode"].ToString();
                    if (dr["UserRegBlockTime"] != null)
                    {
                        model.UserRegBlockTime =Convert.ToDateTime(dr["UserRegBlockTime"]);
                    }
                    if (dr["LoginViewImg"] != null)
                    {
                        model.LoginViewImg = dr["LoginViewImg"].ToString();
                    }
                    if (dr["QQHeadApi"] != null)
                    {
                        model.QQHeadApi = dr["QQHeadApi"].ToString();
                    }
                }
            }
            return model;
        }
    }
}
