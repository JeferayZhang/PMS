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
    public class UserDAL
    {
        SqlHelp dbhelper = new SqlHelp();

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
        /// <returns></returns>
        public DataTable GetUser(string userNo, string userName, string sex, string userRole, string orgid,
            string IDCard, string userState, string userRegData_Begin, string userRegData_End,string userOrg)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT USERS.ID, USERS.USERNO, USERS.NAME, ISNULL(ROLE.ROLENAME, '未知角色') ROLE,
       USERS.PASSWORD,
       CASE USERS.SEX
          WHEN 0 THEN
           '女'
          ELSE
           '男'
        END AS SEX, ISNULL(ORG.NAME, '未知机构') ORGNAME,
       ISNULL(USERS.IDCARD, '') IDCARD, ISNULL(USERS.PHONENUMBER, '') PHONENUMBER,
       ISNULL(USERS.ADDRESS, '') ADDRESS, ISNULL(USERS.EMAIL, '') EMAIL,
       CASE USERS.STATE
          WHEN 0 THEN
           '有效'
          ELSE
           '无效'
        END AS STATE,B.NAME OPERATOR, 
        CONVERT(varchar(100), USERS.INDATE, 23) INDATE, USERS.MGUID
FROM USERS
LEFT JOIN ORG ON ORG.ORGID = USERS.ORGID
LEFT JOIN ROLE ON ROLE.ROLEID = USERS.ROLE 
LEFT JOIN USERS B ON B.ID=USERS.Operator WHERE 1=1 ";
            if (!string.IsNullOrEmpty(userNo._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("USERNO", userNo._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND USERS.USERNO LIKE @USERNO+'%'";
            }
            if (!string.IsNullOrEmpty(userName._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("NAME", userName._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND USERS.NAME LIKE '%'+@NAME+'%'";
            }
            if (!string.IsNullOrEmpty(sex._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("SEX", sex._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND USERS.SEX =@SEX";
            }
            if (!string.IsNullOrEmpty(userRole._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("ROLE", userRole._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND USERS.ROLE =@ROLE";
            }
            OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
            string all = orgInfoDAL.getChilds(userOrg, orgid);
            sql += " AND USERS.ORGID in (" + all + ")";
            if (!string.IsNullOrEmpty(IDCard._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("IDCARD", IDCard._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND USERS.IDCARD =@IDCARD";
            }
            if (!string.IsNullOrEmpty(userState._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("STATE", userState._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND USERS.STATE =@STATE";
            }
            if (!string.IsNullOrEmpty(userRegData_Begin._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE1", userRegData_Begin._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND CONVERT(varchar(100),USERS.INDATE, 23)  >= CONVERT(varchar(100),@INDATE1, 23)";
            }
            if (!string.IsNullOrEmpty(userRegData_End._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE2", userRegData_End._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND  CONVERT(varchar(100),USERS.INDATE, 23)  <= CONVERT(varchar(100),@INDATE2, 23) ";
            }
            dt = dbhelper.ExecuteSql(sql + " ORDER BY USERNO");
            return dt;
        }

        public DataTable GetPosters(string orgid,int role=3)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT USERS.ID, USERS.USERNO, USERS.NAME
FROM USERS WHERE 1=1 ";
            //这里加载当前登录人可以操作的用户
            if (!string.IsNullOrEmpty(orgid._ToStrTrim()))
            {
                OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
                string ids = orgInfoDAL.getChilds(orgid,"");
                sql += " AND USERS.ORGID in (" + ids + ") ";
            }
            sql += " AND USERS.ROLE="+ role + @" AND STATE=0";
            
            dt = dbhelper.ExecuteSql(sql + " ORDER BY USERNO");
            return dt;
        }

        public DataTable GetUserByPK(int ID) 
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT ID,USERNO, NAME, ROLE, PASSWORD, SEX, ORGID, IDCARD, PHONENUMBER,
       ADDRESS, EMAIL, STATE, OPERATOR, INDATE, MGUID FROM USERS WHERE 1=1 ";
            SqlParameter Para = new SqlParameter("ID", ID);
            dbhelper.SqlParameterList.Add(Para);
            sql += " AND ID =@ID";
            dt = dbhelper.ExecuteSql(sql + " ORDER BY USERNO");
            return dt;
        }
        #endregion

        #region 根据用户编号更新数据
        /// <summary>
        /// 根据用户编号更新数据
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <param name="userName">名称</param>
        /// <param name="sex">性别,1男0女</param>
        /// <param name="userRole">角色</param>
        /// <param name="userOrg">用户所属机构</param>
        /// <param name="IDCard">证件号</param>
        /// <param name="userState">状态(0有效,1无效)</param>
        /// <param name="guid">唯一标识,每次修改数据会同时修改此列</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public  string UpdateByPK(int ID,string userNo, string userName, string sex, string userRole, string userOrg, 
            string IDCard, string userState, string guid,string Address)
        {
            string res = "";
            try
            {
                res = checkUser(userNo, ID, guid);
                if (!string.IsNullOrEmpty(res))
                {
                    return res;
                }
                else
                {
                    string sql = @" UPDATE USERS SET   ";
                    if (!string.IsNullOrEmpty(userName._ToStrTrim()))
                    {
                        sql += " NAME=@NAME, ";
                        SqlParameter Para = new SqlParameter("NAME", userName._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(sex._ToStrTrim()))
                    {
                        sql += " SEX =@SEX ,";
                        SqlParameter Para = new SqlParameter("SEX", sex._ToInt32());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(userRole._ToStrTrim()))
                    {
                        sql += " ROLE =@ROLE, ";
                        SqlParameter Para = new SqlParameter("ROLE", userRole._ToInt32());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(userOrg._ToStrTrim()))
                    {
                        sql += " ORGID =@ORGID,";
                        SqlParameter Para = new SqlParameter("ORGID", userOrg._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(IDCard._ToStrTrim()))
                    {
                        sql += " IDCARD =@IDCARD,";
                        SqlParameter Para = new SqlParameter("IDCARD", IDCard._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(userState._ToStrTrim()))
                    {
                        sql += " STATE =@STATE,";
                        SqlParameter Para = new SqlParameter("STATE", userState._ToInt32());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(userNo._ToStrTrim()))
                    {
                        sql += " USERNO =@USERNO ,";
                        SqlParameter Para = new SqlParameter("USERNO", userNo._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (!string.IsNullOrEmpty(Address._ToStrTrim()))
                    {
                        sql += " Address =@Address ,";
                        SqlParameter Para = new SqlParameter("Address", Address._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    if (ID > 0)
                    {
                        SqlParameter Para = new SqlParameter("ID", ID);
                        dbhelper.SqlParameterList.Add(Para);
                    }
                    sql += "  MGuid =NEWID() WHERE ID = @ID  ";
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

        #region 检测用户是否可以被修改或者删除
        /// <summary>
        /// 检测用户是否可以被修改或者删除
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string checkUser(string userNo, int ID=0,string guid = "")
        {
            string sql = @" select 1 from users where USERNO =@USERNO ";
            SqlParameter Para = new SqlParameter("USERNO", userNo._ToStrTrim());
            dbhelper.SqlParameterList.Add(Para);
            if (ID > 0)
            {
                Para = new SqlParameter("ID", ID._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += "  and ID<>@ID ";
            }
            if (dbhelper.ExecuteSql(sql).Rows.Count > 0)
            {
                return "用户编号已存在!";
            }
            if (!string.IsNullOrEmpty(guid))
            {
                sql = @" select 1 from users where MGuid =@MGuid";
                Para = new SqlParameter("MGuid", guid._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                if (dbhelper.ExecuteSql(sql).Rows.Count == 0)
                {
                    return "数据已被更改,请重新加载!";
                }
            }
            return "";
        } 
        #endregion

        #region 根据主键删除数据
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="IDs">主键,多个用英文逗号分隔</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public string DeleteByPK(string IDs)
        {
            string res = "";
            try
            {
                string[] list = IDs.Split(',');
                int num = 0;
                foreach (string item in list)
                {
                    string sql = @" DELETE FROM  USERS  WHERE ID = @ID ";
                    SqlParameter Para = new SqlParameter("ID", item._ToStrTrim());
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
        public string Insert(string userNo, string userName, string sex, string userRole, string chooseOrg,
            string IDCard, string Password, string PhoneNumber, string Address, string Email, string OPERATOR,int userlevel)
        {
            string res = "";

            try
            {
                res = checkUser(userNo);
                if (!string.IsNullOrEmpty(res))
                {
                    return res;
                }
                OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
                int level = orgInfoDAL.GetOrgByPK(chooseOrg._ToInt32()).Rows[0]["Level"]._ToInt32();
                if (level < userlevel)
                {
                    res = "你不能添加当前机构下的用户";
                    return res;
                }
                string sql = @"INSERT INTO USERS(USERNO, NAME,  SEX,ROLE,ORGID, PASSWORD,  IDCARD, PHONENUMBER,
       ADDRESS, EMAIL, OPERATOR, INDATE) VALUES (@USERNO, @NAME,  @SEX,@ROLE,@ORGID, @PASSWORD,  @IDCARD, @PHONENUMBER,
       @ADDRESS, @EMAIL,  @OPERATOR,  GETDATE())";
                SqlParameter Para = new SqlParameter("USERNO", userNo._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);

                Para = new SqlParameter("NAME", userName._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);

                Para = new SqlParameter("SEX", sex._ToInt32());
                dbhelper.SqlParameterList.Add(Para);

                Para = new SqlParameter("ROLE", userRole._ToInt32());
                dbhelper.SqlParameterList.Add(Para);

                Para = new SqlParameter("ORGID", chooseOrg._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);

                Para = new SqlParameter("PASSWORD", Password._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);

                Para = new SqlParameter("IDCARD", IDCard._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);

                Para = new SqlParameter("PHONENUMBER", PhoneNumber._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);

                Para = new SqlParameter("ADDRESS", Address._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);

                Para = new SqlParameter("EMAIL", Email._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);

                Para = new SqlParameter("OPERATOR", OPERATOR._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
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

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <param name="password">密码</param>
        /// <returns>返回验证结果</returns>
        public retValue Login(string userNo, string password)
        {
            retValue ret = new retValue();
            try
            {
                SqlParameter Para = new SqlParameter("USERNO", userNo._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("PASSWORD", password._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                DataTable dt = dbhelper.ExecuteSql(@" SELECT USERS.*,Org.OrgCode OrgNo,Org.Name OrgName,Org.Level FROM USERS left join Org on USERS.OrgID=Org.OrgID
WHERE USERS.USERNO =@USERNO AND USERS.PASSWORD=@PASSWORD ");
                if (dt==null||dt.Rows.Count == 0)
                {
                    ret.result = false;
                    ret.reason = "账号或者密码不正确";
                }
                else
                {
                    if (dt.Rows[0]["State"]._ToInt32()!=0)
                    {
                        ret.result = false;
                        ret.reason = "账号不是有效状态,不能登录";
                    }
                    else
                    {
                        ret.result = true;
                        ret.data = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                ret.result = false;
                ret.reason = ex.Message;
            }
            return ret;
        } 
        #endregion
    }
}
