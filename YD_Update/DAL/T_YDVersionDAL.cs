using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudsMove.DAL
{
    public class T_YDVersionDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(T_YDVersion model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();

            if (model.ID != null)
            {
                strSql1.Append("ID,");
                strSql2.Append("'" + model.ID + "',");
            }
            if (model.Version != null)
            {
                strSql1.Append("Version,");
                strSql2.Append("'" + model.Version + "',");
            }
            if (model.DownLoadUrl != null)
            {
                strSql1.Append("DownLoadUrl,");
                strSql2.Append("'" + model.DownLoadUrl + "',");
            }
            strSql.Append("Insert Into T_YDVersion(");
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
        public bool Update(T_YDVersion model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            strSql.Append(" update T_YDVersion set ");
            if (model.Version != null)
            {
                strSql1.Append("Version='" + model.Version + "',");
            }
            else
            {
                strSql1.Append("Version=null,");
            }
            if (model.DownLoadUrl != null)
            {
                strSql1.Append("DownLoadUrl='" + model.DownLoadUrl + "',");
            }
            else
            {
                strSql1.Append("DownLoadUrl=null,");
            }
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
            sql_cmd.Append(" delete from T_YDVersion where ID='" + id + "'");
            int rows = SqlHelper.ExecuteNonQuery(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString());
            if (rows > 0) { return true; } else { return false; }
        }

        ///<summary>
        ///得到一个对象实体
        ///</summary>
        public T_YDVersion Find(string id)
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_YDVersion where ID='" + id + "' ");
            T_YDVersion model = new T_YDVersion();
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                if (dr.Read())
                {
                    model.ID = dr["ID"].ToString();
                    if (dr["Version"] != null)
                    {
                        model.Version = dr["Version"].ToString();
                    }
                    if (dr["DownLoadUrl"] != null)
                    {
                        model.DownLoadUrl = dr["DownLoadUrl"].ToString();
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 查找全部
        /// </summary>
        /// <returns></returns>
        public List<T_YDVersion> FindAll()
        {
            StringBuilder sql_cmd = new StringBuilder();
            sql_cmd.AppendLine(" select *  from T_YDVersion");
            List<T_YDVersion> list = new List<T_YDVersion>();
            using (IDataReader dr = SqlHelper.ExecuteReader(SqlHelper.connectstring, CommandType.Text, sql_cmd.ToString()))
            {
                while (dr.Read())
                {
                    T_YDVersion model = new T_YDVersion();
                    model.ID = dr["ID"].ToString();
                    if (dr["Version"] != null)
                    {
                        model.Version = dr["Version"].ToString();
                    }
                    if (dr["DownLoadUrl"] != null)
                    {
                        model.DownLoadUrl = dr["DownLoadUrl"].ToString();
                    }
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
