using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CloudsMove.DAL
{
    public partial class T_EMailManageMentDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(T_EMailManageMent model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();

            strSql1.Append("SerialCode,");
            strSql2.Append("'" + "YunDong" + "',");
            if (model.EMailSendMode != null)
            {
                strSql1.Append("EMailSendMode,");
                strSql2.Append("'" + model.EMailSendMode + "',");
            }
            if (model.EmailAccount != null)
            {
                strSql1.Append("EmailAccount,");
                strSql2.Append("'" + model.EmailAccount + "',");
            }
            if (model.EmailPwd != null)
            {
                strSql1.Append("EmailPwd,");
                strSql2.Append("'" + model.EmailPwd + "',");
            }
            if (model.EmailTemplate != null)
            {
                strSql1.Append("EmailTemplate,");
                strSql2.Append("'" + model.EmailTemplate + "',");
            }
            
            strSql.Append("Insert Into T_EMailManageMent(");
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
        public bool Update(T_EMailManageMent model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            strSql.Append(" update T_EMailManageMent set ");

            if (model.EMailSendMode != null)
            {
                strSql1.Append("EMailSendMode='" + model.EMailSendMode + "',");
            }
            else
            {
                strSql1.Append("EMailSendMode=null,");
            }
            if (model.EmailAccount != null)
            {
                strSql1.Append("EmailAccount='" + model.EmailAccount + "',");
            }
            else
            {
                strSql1.Append("EmailAccount=null,");
            }
            if (model.EmailPwd != null)
            {
                strSql1.Append("EmailPwd='" + model.EmailPwd + "',");
            }
            else
            {
                strSql1.Append("EmailPwd=null,");
            }
            if (model.EmailTemplate != null)
            {
                strSql1.Append("EmailTemplate='" + model.EmailTemplate + "',");
            }
            else
            {
                strSql1.Append("EmailTemplate=null,");
            }
            
            
            strSql.Append(strSql1.ToString().TrimEnd(','));
            strSql.Append(" where SerialCode='" + model.SerialCode + "' ");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, strSql.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///得到一个对象实体
        ///</summary>
        public T_EMailManageMent Find()
        {
            string serialCode = "YunDong";
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_EMailManageMent where SerialCode='" + serialCode + "' ");
            T_EMailManageMent model = null;
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                if (dr.Read())
                {
                    model = new T_EMailManageMent();
                    model.SerialCode = dr["SerialCode"].ToString();
                    if (dr["EMailSendMode"] != null)
                    {
                        model.EMailSendMode = dr["EMailSendMode"].ToString();
                    }
                    if (dr["EmailAccount"] != null)
                    {
                        model.EmailAccount = dr["EmailAccount"].ToString();
                    }
                    if (dr["EmailTemplate"] != null)
                    {
                        model.EmailTemplate = dr["EmailTemplate"].ToString();
                    }
                    if (dr["EmailPwd"] != null)
                    {
                        model.EmailPwd = dr["EmailPwd"].ToString();
                    }
                }
            }
            return model;
        }
    }
}
