using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Common;

namespace BLL
{
    public class RoleBLL
    {
        RoleDAL dal = new RoleDAL();

        #region 获取角色信息
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="RoleName">名称</param>
        /// <param name="State">状态(0有效,1无效)</param>
        /// <returns></returns>
        public DataTable GetRole(int RoleID,string RoleName, string State)
        {
            return dal.GetRole(RoleID, RoleName, State);
        }
        #endregion

        #region 根据用户编号更新数据
        /// <summary>
        /// 根据用户编号更新数据
        /// </summary>
        /// <param name="RoleID">用户编号</param>
        /// <param name="RoleName">名称</param>
        /// <param name="Rights">权限</param>
        /// <param name="Remark">备注</param>
        /// <param name="State">状态(0有效,1无效)</param>
        /// <param name="guid">唯一标识,每次修改数据会同时修改此列</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string UpdateByPK(int RoleID, string RoleName, string State, string Rights, string Remark, string guid)
        {
            return dal.UpdateByPK(RoleID, RoleName, State, Rights, Remark, guid);
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
            return dal.DeleteByPK(RoleIDs);
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
            return dal.Insert(ROLENAME, RIGHTS, REMARK, STATE);
        }
        #endregion
    }
}
