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
    public class UserBLL
    {
        UserDAL dal = new UserDAL();

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <param name="userName">名称</param>
        /// <param name="sex">性别,1男0女</param>
        /// <param name="userRole">角色</param>
        /// <param name="userOrg">用户所属机构</param>
        /// <param name="IDCard">证件号</param>
        /// <param name="userState">状态(0有效,1无效)</param>
        /// <param name="userRegData_Begin">用户注册时间范围,起始值</param>
        /// <param name="userRegData_End">用户注册时间范围,截止值</param>
        /// <returns>data为DataTable</returns>
        public PageModel GetUser(string userNo, string userName, string sex, string userRole, string userOrg, string IDCard, string userState,
            string userRegData_Begin, string userRegData_End,string orgid,int limit ,int page)
        {
            PageModel ret = new PageModel();
            DataTable dt = dal.GetUser(userNo, userName, sex, userRole, userOrg, IDCard, userState,
                userRegData_Begin, userRegData_End, orgid, limit, page);
            if (dt!=null && dt.Rows.Count>0)
            {
                ret.code = 0;
                ret.data = dt;
                ret.count= dal.GetUserCount(userNo, userName, sex, userRole, userOrg, IDCard, userState,
                userRegData_Begin, userRegData_End, orgid);
            }
            else
            {
                ret.code = 0;
                ret.msg = "未能查询到数据";
                ret.count = 0;
            }
            return ret;
        }
        public DataTable GetPosters(string orgid, int role =3) 
        {
            return dal.GetPosters(orgid, role);
        }
        /// <summary>
        /// 根据主键获取用户信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>data为DataTable</returns>
        public retValue GetUserByPK(int ID)
        {
            retValue ret = new retValue();
            DataTable dt = dal.GetUserByPK(ID);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.result = true;
                ret.data = dt;
            }
            else
            {
                ret.result = false;
                ret.reason = "未能查询到数据";
            }
            return ret;
        } 
        #endregion

        #region 根据用户编号更新数据
        /// <summary>
        /// 根据用户编号更新数据
        /// </summary>
        /// <param name="ID">流水号</param>
        /// <param name="userNo">用户编号</param>
        /// <param name="userName">名称</param>
        /// <param name="sex">性别,1男0女</param>
        /// <param name="userRole">角色</param>
        /// <param name="userOrg">用户所属机构</param>
        /// <param name="IDCard">证件号</param>
        /// <param name="userState">状态(0有效,1无效)</param>
        /// <param name="guid">唯一标识,每次修改数据会同时修改此列</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public retValue UpdateByPK(int ID,string userNo, string userName, string sex, string userRole, string userOrg, string IDCard, 
            string userState, string guid,string Email, string PhoneNumber,string Password="") 
        {
            retValue ret = new retValue();
            string res = dal.UpdateByPK(ID,userNo, userName, sex, userRole, userOrg, IDCard, userState, guid, Email, PhoneNumber, Password);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true; ret.data = "保存成功";
            }
            else
            {
                ret.result = false; ret.reason = res;
            }
            
            return ret;
        }
        #endregion

        #region 根据主键删除数据
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="UserNos">用户编号,多个用英文逗号分隔</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public retValue DeleteByPK(string UserNos)
        {
            retValue ret = new retValue();
            string res = dal.DeleteByPK(UserNos);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true; ret.data = "删除成功";
            }
            else
            {
                ret.result = false; ret.reason = res;
            }
            return ret;
        }
        #endregion

        #region 插入用户
        /// <summary>
        /// 插入用户
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <param name="userName">名称</param>
        /// <param name="sex">性别,1男0女</param>
        /// <param name="userRole">角色</param>
        /// <param name="userOrg">用户所属机构</param>
        /// <param name="IDCard">证件号</param>
        /// <param name="Password">密码</param>
        /// <param name="PhoneNumber">电话</param>
        /// <param name="Address">地址</param>
        /// <param name="Email">邮箱</param>
        /// <param name="OPERATOR">操作者</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public retValue Insert(string userNo, string userName, string sex, string userRole, string chooseOrg,
            string IDCard, string Password, string PhoneNumber, string Address, string Email, string OPERATOR,int userlevel)
        {
            retValue ret = new retValue();
            string res = dal.Insert(userNo, userName, sex, userRole, chooseOrg,
             IDCard, Password, PhoneNumber, Address, Email, OPERATOR, userlevel);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true; ret.data = "保存成功";
            }
            else
            {
                ret.result = false; ret.reason = res;
            }
            return ret;
        }
        #endregion

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="UserNo">用户编号</param>
        /// <param name="password">密码</param>
        /// <returns>返回验证结果</returns>
        public retValue Login(string UserNo, string password)
        {
            retValue ret = new retValue();
            ret = dal.Login(UserNo, password);
            return ret;
        } 
        #endregion
    }
}
