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
    public class SubscriberDAL
    {
        SqlHelp dbhelper = new SqlHelp();

        #region 获取订户
        /// <summary>
        /// 获取订户
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="OrderNo">公司代码,一般是纳税号</param>
        /// <param name="UnitName">单位名称</param>
        /// <param name="name">负责人名称</param>
        /// <param name="OrgID">所属网点</param>
        /// <param name="dt1">录入时间</param>
        /// <param name="dt2">录入时间</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetSubscriber(int ID, string OrderNo, string UnitName, string name, 
            string OrgID, string dt1, string dt2,string orgid, int pageLimit = 0, int pageIndex = 0)
        {
            DataTable dt = new DataTable();
            string sql = "";
            string top = "";
            if (pageLimit > 0)
            {
                top = " top "+ pageLimit;
            }
            sql = string.Format(@"SELECT "+ top + @" * from (select  OrderPeople.ID,OrderPeople.OrderNo,OrderPeople.UnitName,
OrderPeople.Name,OrderPeople.Phone,OrderPeople.Address,OrderPeople.MGuid,
ROW_NUMBER() over (order by OrderPeople.ID) as rownumber,
USERS.NAME InUser,CONVERT(varchar(100),OrderPeople.INDATE, 23) Indate,
Org.Name OrgName FROM  OrderPeople
LEFT JOIN USERS ON OrderPeople.InUser=USERS.ID
LEFT JOIN Org ON Org.OrgID=OrderPeople.OrgID WHERE 1=1 ");
            
            if (!string.IsNullOrEmpty(OrderNo._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("OrderNo", OrderNo._ToStrTrim().ToUpper());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND OrderPeople.OrderNo LIKE '%'+@OrderNo+'%'";
            }
            if (!string.IsNullOrEmpty(UnitName._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("UnitName", UnitName._ToStrTrim().ToUpper());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND OrderPeople.UnitName LIKE '%'+@UnitName+'%'";
            }
            if (!string.IsNullOrEmpty(name._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("name", name._ToStrTrim().ToUpper());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND OrderPeople.Name LIKE '%'+@name+'%'";
            }
            OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
            string all = orgInfoDAL.getChilds(orgid, OrgID._ToStr());
            sql += " AND OrderPeople.ORGID in (" + all + ")";
            if (ID > 0)
            {
                SqlParameter Para = new SqlParameter("ID", ID._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND OrderPeople.ID =@ID";
            }
           
            if (!string.IsNullOrEmpty(dt1._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE1", dt1._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND CONVERT(varchar(100),OrderPeople.INDATE, 23)  >= CONVERT(varchar(100),@INDATE1, 23)";
            }
            if (!string.IsNullOrEmpty(dt2._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE2", dt2._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND  CONVERT(varchar(100),OrderPeople.INDATE, 23)  <= CONVERT(varchar(100),@INDATE2, 23) ";
            }
            dt = dbhelper.ExecuteSql(sql + string.Format(") OrderPeople where rownumber > {0} ", (pageIndex - 1) * pageLimit));
            return dt;
        }

        public DataTable getbyNo(string OrderNo)
        {
            string sql = "select * from OrderPeople where 1=1 ";
            if (!string.IsNullOrEmpty(OrderNo._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("OrderNo", OrderNo._ToStrTrim().ToUpper());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND OrderPeople.OrderNo LIKE '%'+@OrderNo+'%'";
            }
            DataTable dt = dbhelper.ExecuteSql(sql);
            return dt;
        }

        

        public int GetCount(int ID, string OrderNo, string UnitName, string name,
            string OrgID, string dt1, string dt2,string orgid) 
        {
            string sql = "";
            sql = string.Format(@"select  count(1) as count FROM  OrderPeople
LEFT JOIN USERS ON OrderPeople.InUser=USERS.ID
LEFT JOIN Org ON Org.OrgID=OrderPeople.OrgID WHERE 1=1 ");
            if (!string.IsNullOrEmpty(OrgID) &&!string.IsNullOrEmpty(orgid))
            {
                OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
                string all = orgInfoDAL.getChilds(orgid, OrgID._ToStr());
                sql += " AND OrderPeople.ORGID in (" + all + ")";
            }
            
            if (!string.IsNullOrEmpty(OrderNo._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("OrderNo", OrderNo._ToStrTrim().ToUpper());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND OrderPeople.OrderNo LIKE '%'+@OrderNo+'%'";
            }
            if (!string.IsNullOrEmpty(UnitName._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("UnitName", UnitName._ToStrTrim().ToUpper());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND OrderPeople.UnitName LIKE '%'+@UnitName+'%'";
            }
            if (!string.IsNullOrEmpty(name._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("name", name._ToStrTrim().ToUpper());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND OrderPeople.Name LIKE '%'+@name+'%'";
            }
            if (ID > 0)
            {
                SqlParameter Para = new SqlParameter("ID", ID._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND OrderPeople.ID =@ID";
            }
            if (!string.IsNullOrEmpty(dt1._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE1", dt1._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND CONVERT(varchar(100),OrderPeople.INDATE, 23)  >= CONVERT(varchar(100),@INDATE1, 23)";
            }
            if (!string.IsNullOrEmpty(dt2._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE2", dt2._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND  CONVERT(varchar(100),OrderPeople.INDATE, 23)  <= CONVERT(varchar(100),@INDATE2, 23) ";
            }
            int count = dbhelper.Count(sql);
            return count;
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="OrderNo">公司编号</param>
        /// <param name="UnitName">单位名称</param>
        /// <param name="name">负责人名称</param>
        /// <param name="phone">联系方式</param>
        /// <param name="address">地址</param>
        /// <param name="OrgID">所属网点</param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string UpdateByPK(int ID, string OrderNo, string UnitName, string name, string phone,
            string address, string OrgID, string guid = "")
        {
            string res = "";
            try
            {
                res = check(OrderNo, ID, guid);
                if (!string.IsNullOrEmpty(res))
                {
                    return res;
                }
                string sql = @"update  OrderPeople set ";
                SqlParameter Para = null;
                if (!string.IsNullOrEmpty(OrderNo._ToStrTrim()))
                {
                    sql += " OrderNo=@OrderNo, ";
                    Para = new SqlParameter("OrderNo", OrderNo._ToStrTrim().ToUpper());
                    dbhelper.SqlParameterList.Add(Para);
                }
                if (!string.IsNullOrEmpty(UnitName._ToStrTrim()))
                {
                    sql += " UnitName=@UnitName, ";
                    Para = new SqlParameter("UnitName", UnitName._ToStrTrim().ToUpper());
                    dbhelper.SqlParameterList.Add(Para);
                }
                if (!string.IsNullOrEmpty(name._ToStrTrim()))
                {
                    sql += " name=@name, ";
                    Para = new SqlParameter("name", name._ToStrTrim().ToUpper());
                    dbhelper.SqlParameterList.Add(Para);
                }
                if (!string.IsNullOrEmpty(phone._ToStrTrim()))
                {
                    sql += " phone=@phone, ";
                    Para = new SqlParameter("phone", phone._ToStrTrim());
                    dbhelper.SqlParameterList.Add(Para);
                }
                if (!string.IsNullOrEmpty(address._ToStrTrim()))
                {
                    sql += " address=@address, ";
                    Para = new SqlParameter("address", address._ToStrTrim());
                    dbhelper.SqlParameterList.Add(Para);
                }
                if (OrgID._ToInt32() > 0)
                {
                    sql += " OrgID=@OrgID, ";
                    Para = new SqlParameter("OrgID", OrgID);
                    dbhelper.SqlParameterList.Add(Para);
                }
                sql += " MGuid=newid()  where ID=@ID ";
                Para = new SqlParameter("ID", ID);
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

        private string check(string OrderNo, int ID = 0, string guid = "")
        {
            string sql = @" select 1 from OrderPeople where OrderNo =@OrderNo ";
            SqlParameter Para = new SqlParameter("OrderNo", OrderNo._ToStrTrim().ToUpper());
            dbhelper.SqlParameterList.Add(Para);
            if (ID > 0)
            {
                Para = new SqlParameter("ID", ID._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += "  and ID<>@ID ";
            }
            if (dbhelper.ExecuteSql(sql).Rows.Count > 0)
            {
                return "单位编号已存在!";
            }
            if (!string.IsNullOrEmpty(guid))
            {
                sql = @" select 1 from OrderPeople where MGuid =@MGuid";
                Para = new SqlParameter("MGuid", guid._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                if (dbhelper.ExecuteSql(sql).Rows.Count == 0)
                {
                    return "数据已被更改,请重新加载!";
                }
            }
            return "";
        }

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="OrderNo">公司编号</param>
        /// <param name="UnitName">单位名称</param>
        /// <param name="name">负责人名称</param>
        /// <param name="phone">联系方式</param>
        /// <param name="address">地址</param>
        /// <param name="OrgID">所属网点</param>
        /// <param name="InUser">录入人</param>
        /// <returns></returns>
        public retValue Insert(string OrderNo, string UnitName, string name, string phone,
            string address, string OrgID, int InUser)
        {
            retValue ret = new retValue();
            string res = "";
            try
            {
                res = check(OrderNo);
                if (!string.IsNullOrEmpty(res))
                {
                    ret.result = false;
                    ret.reason = res;
                    return ret;
                }
                string sql = @" insert into OrderPeople(OrderNo,UnitName,name,phone,address,OrgID,InUser) 
values(@OrderNo,@UnitName,@name,@phone,@address,@OrgID,@InUser) ";
                SqlParameter Para = null;
                Para = new SqlParameter("OrderNo", OrderNo._ToStrTrim().ToUpper());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("UnitName", UnitName._ToStrTrim().ToUpper());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("name", name._ToStrTrim().ToUpper());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("phone", phone._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("address", address._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("OrgID", OrgID._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("InUser", InUser._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                int num = dbhelper.ExecuteInsert(sql);
                if (num == 0)
                {
                    ret.result = false;
                    res = "操作失败";
                }
                else
                {
                    ret.result = true;
                    ret.data = num;
                }
            }
            catch (Exception ex)
            {
                ret.result = false;
                res = ex.Message;
            }
            ret.reason = res;
            return ret;
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
                    string sql = @" DELETE FROM  OrderPeople  WHERE ID = @ID ";
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
    }
}
