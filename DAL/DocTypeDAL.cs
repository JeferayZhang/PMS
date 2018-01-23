using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Data.SqlClient;


namespace DAL
{
    public class DocTypeDAL
    {
        SqlHelp dbhelper = new SqlHelp();

        #region 获取类型信息
        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="TypeName">名称</param>
        /// <param name="TypeID">类型ID</param>
        /// <returns></returns>
        public DataTable GetDocType(string TypeID ,string TypeName)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT *  FROM DocType WHERE 1=1 ";
            if (!string.IsNullOrEmpty(TypeID._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("TypeID", TypeID._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND TypeID=@TypeID";
            }
            if (!string.IsNullOrEmpty(TypeName._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("TypeName", TypeName._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND TypeName LIKE '%@TypeName%'";
            }
            dt = dbhelper.ExecuteSql(sql + " ORDER BY TypeID");
            return dt;
        }
        #endregion

        #region 根据类型编号更新数据
        /// <summary>
        /// 根据类型编号更新数据
        /// </summary>
        /// <param name="TypeID">类型编号</param>
        /// <param name="TypeName">名称</param>
        /// <param name="TypeDesc">备注</param>
        /// <param name="guid">唯一标识,每次修改数据会同时修改此列</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string UpdateByPK(int TypeID, string TypeName, string TypeDesc, string guid)
        {
            string res = "";
            try
            {
                if (!checkDocType(guid))
                {
                    res = "数据已被修改,请刷新后尝试";
                }
                else
                {
                    string sql = @" UPDATE DocType SET TypeName=@TypeName,  TypeDesc =@TypeDesc,NGuid =newid() WHERE TypeID = @TypeID ";
                    if (!string.IsNullOrEmpty(TypeName._ToStrTrim()))
                    {
                        SqlParameter Para = new SqlParameter("TypeName", TypeName._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(TypeDesc._ToStrTrim()))
                    {
                        SqlParameter Para = new SqlParameter("TypeDesc", TypeDesc._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    SqlParameter Para2 = new SqlParameter("TypeID", TypeID._ToInt32());
                    dbhelper.SqlParameterList.Add(Para2);
                    int num = dbhelper.ExecuteNonQuery(sql);
                    if (num == 0)
                    {
                        res = "操作失败";
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        #endregion

        #region 检测类型是否可以被修改或者删除
        /// <summary>
        /// 检测类型是否可以被修改或者删除
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool checkDocType(string guid)
        {
            string sql = @" select 1 from DocType where NGuid =@NGuid";
            SqlParameter Para = new SqlParameter("NGuid", guid._ToStrTrim());
            dbhelper.SqlParameterList.Add(Para);
            return dbhelper.ExecuteSql(sql).Rows.Count > 0;
        }
        #endregion

        #region 根据主键删除数据
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="TypeIDs">类型编号,多个用英文逗号分隔</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string DeleteByPK(string TypeIDs)
        {
            string res = "";
            try
            {
                string[] list = TypeIDs.Split(',');
                int num = 0;
                foreach (string item in list)
                {
                    string sql = @" DELETE FROM  DocType  WHERE TypeID = @TypeID ";
                    SqlParameter Para = new SqlParameter("TypeID", item._ToStrTrim());
                    dbhelper.SqlParameterList.Add(Para);
                    num += dbhelper.ExecuteNonQuery(sql);
                }
                if (num == 0)
                {
                    res = "操作失败";
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        #endregion

        #region 插入类型
        /// <summary>
        /// 插入类型
        /// </summary>
        /// <param name="TypeName">名称</param>
        /// <param name="TypeDesc">说明</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string Insert(string TypeName, string TypeDesc)
        {
            string res = "";
            try
            {
                string sql = @"INSERT INTO DocType (TypeName, TypeDesc) VALUES (@TypeName,  @TypeDesc)";
                if (!string.IsNullOrEmpty(TypeName._ToStrTrim()))
                {
                    SqlParameter Para = new SqlParameter("TypeName", TypeName._ToStrTrim());
                    dbhelper.SqlParameterList.Add(Para);
                }
                if (!string.IsNullOrEmpty(TypeDesc._ToStrTrim()))
                {
                    SqlParameter Para = new SqlParameter("TypeDesc", TypeDesc._ToStrTrim());
                    dbhelper.SqlParameterList.Add(Para);
                }
                int num = dbhelper.ExecuteNonQuery(sql);
                if (num == 0)
                {
                    res = "操作失败";
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        #endregion
    }
}
