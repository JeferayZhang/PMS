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
    public class RoleDAL
    {
        SqlHelp dbhelper = new SqlHelp();

        #region 获取角色信息
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="RoleName">名称</param>
        /// <param name="State">状态(0有效,1无效)</param>
        /// <returns></returns>
        public DataTable GetRole(int RoleID,string RoleName, string State)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT *  FROM Role WHERE 1=1 ";
            if (!string.IsNullOrEmpty(RoleID._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("RoleID", RoleID._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND RoleID =@RoleID";
            }
            if (!string.IsNullOrEmpty(RoleName._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("RoleName", RoleName._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND RoleName LIKE '%@RoleName%'";
            }
            if (!string.IsNullOrEmpty(State._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("STATE", State._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND STATE =@STATE";
            }
            dt = dbhelper.ExecuteSql(sql + " ORDER BY RoleID");
            return dt;
        }
        #endregion

        #region 根据角色编号更新数据
        /// <summary>
        /// 根据角色编号更新数据
        /// </summary>
        /// <param name="RoleID">角色编号</param>
        /// <param name="RoleName">名称</param>
        /// <param name="Rights">权限</param>
        /// <param name="Remark">备注</param>
        /// <param name="State">状态(0有效,1无效)</param>
        /// <param name="guid">唯一标识,每次修改数据会同时修改此列</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string UpdateByPK(int RoleID,string RoleName, string State, string Rights,string Remark,string guid)
        {
            string res = "";
            try
            {
                if (!checkRole(guid))
                {
                    res = "数据已被修改,请刷新后尝试";
                }
                else
                {
                    string sql = @" UPDATE Role SET RoleName=@RoleName, State =@State ,Rights =@Rights, Remark =@Remark,NGuid =newid() WHERE RoleID = @RoleID ";
                    if (!string.IsNullOrEmpty(RoleName._ToStrTrim()))
                    {
                        SqlParameter Para = new SqlParameter("RoleName", RoleName._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(State._ToStrTrim()))
                    {
                        SqlParameter Para = new SqlParameter("State", State._ToInt32());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(Rights._ToStrTrim()))
                    {
                        SqlParameter Para = new SqlParameter("Rights", Rights._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(Remark._ToStrTrim()))
                    {
                        SqlParameter Para = new SqlParameter("Remark", Remark._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    SqlParameter Para2 = new SqlParameter("RoleID",  RoleID._ToInt32());
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

        #region 检测角色是否可以被修改或者删除
        /// <summary>
        /// 检测角色是否可以被修改或者删除
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool checkRole(string guid)
        {
            string sql = @" select 1 from Role where NGuid =@NGuid";
            SqlParameter Para = new SqlParameter("NGuid", guid._ToStrTrim());
            dbhelper.SqlParameterList.Add(Para);
            return dbhelper.ExecuteSql(sql).Rows.Count > 0;
        }
        #endregion

        #region 根据主键删除数据
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="RoleNos">角色编号,多个用英文逗号分隔</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string DeleteByPK(string RoleIDs)
        {
            string res = "";
            try
            {
                string[] list = RoleIDs.Split(',');
                int num = 0;
                foreach (string item in list)
                {
                    string sql = @" DELETE FROM  Role  WHERE RoleID = @RoleID ";
                    SqlParameter Para = new SqlParameter("RoleID", item._ToStrTrim());
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

        #region 插入角色
        /// <summary>
        /// 插入角色
        /// </summary>
        /// <param name="ROLENAME">名称</param>
        /// <param name="RIGHTS">菜单权限</param>
        /// <param name="REMARK">说明</param>
        /// <param name="STATE">状态</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string Insert(string ROLENAME, string RIGHTS, string REMARK, string STATE)
        {
            string res = "";
            try
            {
                string sql = @"INSERT INTO ROLE (ROLENAME, RIGHTS, REMARK, STATE) VALUES (@ROLENAME, @RIGHTS,  @REMARK,@STATE)";
                if (!string.IsNullOrEmpty(ROLENAME._ToStrTrim()))
                {
                    SqlParameter Para = new SqlParameter("ROLENAME", ROLENAME._ToStrTrim());
                    dbhelper.SqlParameterList.Add(Para);
                }
                if (!string.IsNullOrEmpty(RIGHTS._ToStrTrim()))
                {
                    SqlParameter Para = new SqlParameter("RIGHTS", RIGHTS._ToStrTrim());
                    dbhelper.SqlParameterList.Add(Para);
                }
                if (!string.IsNullOrEmpty(REMARK._ToStrTrim()))
                {
                    SqlParameter Para = new SqlParameter("REMARK", REMARK._ToStrTrim());
                    dbhelper.SqlParameterList.Add(Para);
                }
                if (!string.IsNullOrEmpty(STATE._ToStrTrim()))
                {
                    SqlParameter Para = new SqlParameter("STATE", STATE._ToInt32());
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
