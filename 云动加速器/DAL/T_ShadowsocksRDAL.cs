using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CloudsMove.DAL
{
    public class T_ShadowsocksRDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(T_ShadowsocksR model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            //strSql1.Append("id,");
            //strSql2.Append("'" + model.id + "',");
            strSql1.Append("SerialCode,");
            strSql2.Append("'" + Guid.NewGuid().ToString("N") + "',");
            if (model.Remark != null)
            {
                strSql1.Append("Remark,");
                strSql2.Append("'" + model.Remark + "',");
            }
            if (model.HostName != null)
            {
                strSql1.Append("HostName,");
                strSql2.Append("'" + model.HostName + "',");
            }
            if (model.Port != null)
            {
                strSql1.Append("Port,");
                strSql2.Append("'" + model.Port + "',");
            }
            if (model.Method != null)
            {
                strSql1.Append("Method,");
                strSql2.Append("'" + model.Method + "',");
            }
            if (model.Password != null)
            {
                strSql1.Append("Password,");
                strSql2.Append("'" + model.Password + "',");
            }
            if (model.Protocol != null)
            {
                strSql1.Append("Protocol,");
                strSql2.Append("'" + model.Protocol + "',");
            }
            if (model.ProtocolParam != null)
            {
                strSql1.Append("ProtocolParam,");
                strSql2.Append("'" + model.ProtocolParam + "',");
            }
            if (model.OBFS != null)
            {
                strSql1.Append("OBFS,");
                strSql2.Append("'" + model.OBFS + "',");
            }
            if (model.OBFSParam != null)
            {
                strSql1.Append("OBFSParam,");
                strSql2.Append("'" + model.OBFSParam + "',");
            }
            if (model.LineType != null)
            {
                strSql1.Append("LineType,");
                strSql2.Append("'" + model.LineType + "',");
            }

            strSql.Append("Insert Into T_ShadowsocksR(");
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
        public bool Update(T_ShadowsocksR model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            strSql.Append(" update T_ShadowsocksR set ");
            //strSql1.Append("id='" + model.id + "',");
            if (model.Remark != null)
            {
                strSql1.Append("Remark='" + model.Remark + "',");
            }
            else
            {
                strSql1.Append("Remark=null,");
            }
            if (model.HostName != null)
            {
                strSql1.Append("HostName='" + model.HostName + "',");
            }
            else
            {
                strSql1.Append("HostName=null,");
            }
            if (model.Port != null)
            {
                strSql1.Append("Port='" + model.Port + "',");
            }
            else
            {
                strSql1.Append("Port=null,");
            }
            if (model.Method != null)
            {
                strSql1.Append("Method='" + model.Method + "',");
            }
            else
            {
                strSql1.Append("Method=null,");
            }
            if (model.Password != null)
            {
                strSql1.Append("Password='" + model.Password + "',");
            }
            else
            {
                strSql1.Append("Password=null,");
            }
            if (model.Protocol != null)
            {
                strSql1.Append("Protocol='" + model.Protocol + "',");
            }
            else
            {
                strSql1.Append("Protocol=null,");
            }
            if (model.ProtocolParam != null)
            {
                strSql1.Append("ProtocolParam='" + model.ProtocolParam + "',");
            }
            else
            {
                strSql1.Append("ProtocolParam=null,");
            }
            if (model.OBFS != null)
            {
                strSql1.Append("OBFS='" + model.OBFS + "',");
            }
            else
            {
                strSql1.Append("OBFS=null,");
            }
            if (model.OBFSParam != null)
            {
                strSql1.Append("OBFSParam='" + model.OBFSParam + "',");
            }
            else
            {
                strSql1.Append("OBFSParam=null,");
            }
            if (model.LineType != null)
            {
                strSql1.Append("LineType='" + model.LineType + "',");
            }
            else
            {
                strSql1.Append("LineType=null,");
            }

            strSql.Append(strSql1.ToString().TrimEnd(','));
            strSql.Append(" where SerialCode='" + model.SerialCode + "' ");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, strSql.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///删除一条数据
        ///</summary>
        public bool Delete(string serialCode)
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.Append(" delete from T_ShadowsocksR where SerialCode='" + serialCode + "'");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///得到一个对象实体
        ///</summary>
        public T_ShadowsocksR Find(string serialCode)
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_ShadowsocksR where SerialCode='" + serialCode + "' and LineState='启用'");
            T_ShadowsocksR model = new T_ShadowsocksR();
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                if (dr.Read())
                {
                    model.SerialCode = dr["SerialCode"].ToString();
                    if (dr["Remark"] != null)
                    {
                        model.Remark = dr["Remark"].ToString();
                    }
                    if (dr["HostName"] != null)
                    {
                        model.HostName = dr["HostName"].ToString();
                    }
                    if (dr["Port"] != null)
                    {
                        model.Port = dr["Port"].ToString();
                    }
                    if (dr["Method"] != null)
                    {
                        model.Method = dr["Method"].ToString();
                    }
                    if (dr["Password"] != null)
                    {
                        model.Password = dr["Password"].ToString();
                    }
                    if (dr["Protocol"] != null)
                    {
                        model.Protocol = dr["Protocol"].ToString();
                    }
                    if (dr["ProtocolParam"] != null)
                    {
                        model.ProtocolParam = dr["ProtocolParam"].ToString();
                    }
                    if (dr["OBFS"] != null)
                    {
                        model.OBFS = dr["OBFS"].ToString();
                    }
                    if (dr["OBFSParam"] != null)
                    {
                        model.OBFSParam = dr["OBFSParam"].ToString();
                    }
                    if (dr["LineType"] != null)
                    {
                        model.LineType = dr["LineType"].ToString();
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 查找全部
        /// </summary>
        /// <returns></returns>
        public List<T_ShadowsocksR> FindAll()
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_ShadowsocksR where LineState='启用'");
            List<T_ShadowsocksR> list = new List<T_ShadowsocksR>();
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                while (dr.Read())
                {
                    T_ShadowsocksR model = new T_ShadowsocksR();
                    model.SerialCode = dr["SerialCode"].ToString();
                    if (dr["Remark"] != null)
                    {
                        model.Remark = dr["Remark"].ToString();
                    }
                    if (dr["HostName"] != null)
                    {
                        model.HostName = dr["HostName"].ToString();
                    }
                    if (dr["Port"] != null)
                    {
                        model.Port = dr["Port"].ToString();
                    }
                    if (dr["Method"] != null)
                    {
                        model.Method = dr["Method"].ToString();
                    }
                    if (dr["Password"] != null)
                    {
                        model.Password = dr["Password"].ToString();
                    }
                    if (dr["Protocol"] != null)
                    {
                        model.Protocol = dr["Protocol"].ToString();
                    }
                    if (dr["ProtocolParam"] != null)
                    {
                        model.ProtocolParam = dr["ProtocolParam"].ToString();
                    }
                    if (dr["OBFS"] != null)
                    {
                        model.OBFS = dr["OBFS"].ToString();
                    }
                    if (dr["OBFSParam"] != null)
                    {
                        model.OBFSParam = dr["OBFSParam"].ToString();
                    }
                    if (dr["LineType"] != null)
                    {
                        model.LineType = dr["LineType"].ToString();
                    }
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
