using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CloudsMove.DAL
{
    public partial class T_GameDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(T_Game model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();

            strSql1.Append("SerialCode,");
            strSql2.Append("'" + Guid.NewGuid().ToString("N") + "',");
            if (model.GameName != null)
            {
                strSql1.Append("GameName,");
                strSql2.Append("'" + model.GameName + "',");
            }
            if (model.GameRouteUrl != null)
            {
                strSql1.Append("GameRouteUrl,");
                strSql2.Append("'" + model.GameRouteUrl + "',");
            }
            if (model.GameImageUrl != null)
            {
                strSql1.Append("GameImageUrl,");
                strSql2.Append("'" + model.GameImageUrl + "',");
            }
            if (model.LineType != null)
            {
                strSql1.Append("LineType,");
                strSql2.Append("'" + model.LineType + "',");
            }
            if (model.GameType != null)
            {
                strSql1.Append("GameType,");
                strSql2.Append("'" + model.GameType + "',");
            }
            
            strSql.Append("Insert Into T_Game(");
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
        public bool Update(T_Game model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            strSql.Append(" update T_Game set ");
            //strSql1.Append("id='" + model.id + "',");
            if (model.GameName != null)
            {
                strSql1.Append("GameName='" + model.GameName + "',");
            }
            else
            {
                strSql1.Append("GameName=null,");
            }
            if (model.GameRouteUrl != null)
            {
                strSql1.Append("GameRouteUrl='" + model.GameRouteUrl + "',");
            }
            else
            {
                strSql1.Append("GameRouteUrl=null,");
            }
            if (model.GameImageUrl != null)
            {
                strSql1.Append("GameImageUrl='" + model.GameImageUrl + "',");
            }
            else
            {
                strSql1.Append("GameImageUrl=null,");
            }
            if (model.GameType != null)
            {
                strSql1.Append("GameType='" + model.GameType + "',");
            }
            else
            {
                strSql1.Append("GameType=null,");
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
            sql_cmd.Append(" delete from T_Game where SerialCode='" + serialCode + "'");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///得到一个对象实体
        ///</summary>
        public T_Game Find(string serialCode)
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_Game where SerialCode='" + serialCode + "' ");
            T_Game model = new T_Game();
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                if (dr.Read())
                {
                    model.SerialCode = dr["SerialCode"].ToString();
                    if (dr["GameName"] != null)
                    {
                        model.GameName = dr["GameName"].ToString();
                    }
                    if (dr["GameRouteUrl"] != null)
                    {
                        model.GameRouteUrl = dr["GameRouteUrl"].ToString();
                    }
                    if (dr["GameImageUrl"] != null)
                    {
                        model.GameImageUrl = dr["GameImageUrl"].ToString();
                    }
                    if (dr["GameType"] != null)
                    {
                        model.GameType = dr["GameType"].ToString();
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
        public List<T_Game> FindAll()
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_Game");
            List<T_Game> list = new List<T_Game>();
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                while (dr.Read())
                {
                    T_Game model = new T_Game();
                    model.SerialCode = dr["SerialCode"].ToString();
                    if (dr["GameName"] != null)
                    {
                        model.GameName = dr["GameName"].ToString();
                    }
                    if (dr["GameRouteUrl"] != null)
                    {
                        model.GameRouteUrl = dr["GameRouteUrl"].ToString();
                    }
                    if (dr["GameImageUrl"] != null)
                    {
                        model.GameImageUrl = dr["GameImageUrl"].ToString();
                    }
                    if (dr["GameType"] != null)
                    {
                        model.GameType = dr["GameType"].ToString();
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
