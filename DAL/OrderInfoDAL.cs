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
    /// <summary>
    /// 订单组织
    /// </summary>
    public class OrderInfoDAL
    {
        SqlHelp dbhelper = new SqlHelp();
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="docname">报刊名称</param>
        /// <param name="OrderNo">单位编号</param>
        /// <param name="unitname">单位名称</param>
        /// <param name="dt1">录入时间</param>
        /// <param name="dt2">录入时间</param>
        /// <param name="orgid">当前登录用户所属机构ID</param>
        /// <param name="chooseorg">界面上机构选择的</param>
        /// <param name="pagesize">每页显示数量</param>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        public DataTable GetOrderInfo(int id, string BKDH, string OrderNo, string unitname, string dt1, string dt2,
            int pagesize, int pageindex, string orgid, string chooseorg, string orderstate, string coststate) 
        {
            DataTable dt = new DataTable();
            string top = "";
            if (pagesize > 0)
            {
                top = "top "+ pagesize;
            }
            string sql = string.Format(@"select "+ top + @" * from (
select a.ID ,b.Name docname ,a.BKDH,c.UnitName,c.Name ToUser,a.OrderDate,a.OrderMonths,
a.OrderNum,CONVERT(varchar(100), a.Indate, 23) Indate,a.PosterID,d.Name as GetUser,e.NAME as InUser ,a.PersonID,a.NGUID,Cost.Money,Cost.MoneyPayed,Cost.ID as CostID,
ROW_NUMBER() over (order by a.ID) as rownumber,b.Price, case isnull(a.state,0) when 0 then '正常' when -1 then '退订' when 1 then '过期' end as OrderState,
case ISNULL(Cost.state,0) when 0 then '已缴清' when '1' then '未缴清' end as CostState
from [Order]  a 
inner join dbo.OrderPeople c on a.PersonID=c.ID
left join Cost on Cost.OrderID=a.ID
left join dbo.Doc b on a.BKDH=b.BKDH
left join dbo.USERS d on a.PosterID=d.ID
left join dbo.USERS e on a.userid=e.ID  
where 1=1 ");
            string all = "";
            string getfrompage = "";
            //这里加载当前登录人可以操作的用户
            if (!string.IsNullOrEmpty(orgid._ToStrTrim()))
            {
                OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
                string ids = orgInfoDAL.getChilds(orgid);
                all = ids.Substring(0, ids.Length - 1);
            }
            if (!string.IsNullOrEmpty(chooseorg._ToStrTrim()))
            {
                OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
                string ids = orgInfoDAL.getChilds(chooseorg);
                getfrompage = ids.Substring(0, ids.Length - 1);
                string[] str = getfrompage.Split(',').Intersect(all.Split(',')).ToArray();
                all = string.Join(",", str);
            }
            sql += " AND c.ORGID in (" + all + ")";
            if (!string.IsNullOrEmpty(OrderNo._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("OrderNo", OrderNo._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND c.OrderNo LIKE '%'+@OrderNo+'%'";
            }
            if (id > 0)
            {
                SqlParameter Para = new SqlParameter("ID", id._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND a.ID =@ID";
            }
            if (!string.IsNullOrEmpty(unitname._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("unitname", unitname._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND c.UnitName LIKE '%'+@unitname+'%'";
            }
            if (!string.IsNullOrEmpty(BKDH._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("BKDH", BKDH._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND a.BKDH=@BKDH";
            }
            if (!string.IsNullOrEmpty(orderstate._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("orderstate", orderstate._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND ISNULL(a.state,0)=@orderstate";
            }
            if (!string.IsNullOrEmpty(coststate._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("coststate", coststate._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND ISNULL(Cost.state,0)=@coststate";
            }
            if (!string.IsNullOrEmpty(dt1._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE1", dt1._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND CONVERT(varchar(100),a.INDATE, 23)  >= CONVERT(varchar(100),@INDATE1, 23)";
            }
            if (!string.IsNullOrEmpty(dt2._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE2", dt2._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND  CONVERT(varchar(100),a.INDATE, 23)  <= CONVERT(varchar(100),@INDATE2, 23) ";
            }
            dt = dbhelper.ExecuteSql(sql + string.Format(") t where rownumber > {0} ", (pageindex - 1) * pagesize));
            return dt;
        }


        public int GetCount(int id, string BKDH, string OrderNo, string unitname,
            string dt1, string dt2, string orgid, string chooseorg, string orderstate, string coststate) 
        {
            string sql = string.Format(@"
select count(1) as num
from [Order]  a 
inner join dbo.OrderPeople c on a.PersonID=c.ID
left join Cost on Cost.OrderID=a.ID
left join dbo.Doc b on a.BKDH=b.BKDH
left join dbo.USERS d on a.PosterID=d.ID
left join dbo.USERS e on a.userid=d.ID  
where 1=1 ");
            string all = "";
            string getfrompage = "";
            //这里加载当前登录人可以操作的用户
            if (!string.IsNullOrEmpty(orgid._ToStrTrim()))
            {
                OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
                string ids = orgInfoDAL.getChilds(orgid);
                all = ids.Substring(0, ids.Length - 1);
            }
            if (!string.IsNullOrEmpty(chooseorg._ToStrTrim()))
            {
                OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
                string ids = orgInfoDAL.getChilds(chooseorg);
                getfrompage = ids.Substring(0, ids.Length - 1);
                string[] str = getfrompage.Split(',').Intersect(all.Split(',')).ToArray();
                all = string.Join(",", str);
            }
            sql += " AND c.ORGID in (" + all + ")";
            if (!string.IsNullOrEmpty(OrderNo._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("OrderNo", OrderNo._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND c.OrderNo LIKE '%'+@OrderNo+'%'";
            }
            if (!string.IsNullOrEmpty(orderstate._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("orderstate", orderstate._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND a.state=@orderstate";
            }
            if (!string.IsNullOrEmpty(coststate._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("coststate", coststate._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND Cost.state=@coststate";
            }
            if (id > 0)
            {
                SqlParameter Para = new SqlParameter("ID", id._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND a.ID =@ID";
            }
            if (!string.IsNullOrEmpty(unitname._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("unitname", unitname._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND c.UnitName LIKE '%'+@unitname+'%'";
            }
            if (!string.IsNullOrEmpty(BKDH._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("BKDH", BKDH._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND a.BKDH=@BKDH";
            }
            if (!string.IsNullOrEmpty(dt1._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE1", dt1._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND CONVERT(varchar(100),a.INDATE, 23)  >= CONVERT(varchar(100),@INDATE1, 23)";
            }
            if (!string.IsNullOrEmpty(dt2._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("INDATE2", dt2._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND  CONVERT(varchar(100),a.INDATE, 23)  <= CONVERT(varchar(100),@INDATE2, 23) ";
            }
            int num = dbhelper.Count(sql);
            return num;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ordernum"></param>
        /// <param name="months"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string update(int ID, int ordernum, int months, string guid, string bkdh, int PersonID, int ModifyUser) 
        {
            string res = check(ID, guid);
            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }
            
            SqlConnection conn = new SqlConnection(dbhelper.SqlConnectionString);
            conn.Open();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlParameter Para = null;
                    string sql = @"update [order] set ordernum=@ordernum,ordermonths=@ordermonths,nguid=newid() ,
bkdh = @bkdh,PersonID = @PersonID,ModifyDate = GETDATE(),ModifyUser = @ModifyUser where id=@id";
                    Para = new SqlParameter("ordernum", ordernum);
                    dbhelper.SqlParameterList.Add(Para);
                    Para = new SqlParameter("ordermonths", months);
                    dbhelper.SqlParameterList.Add(Para);
                    Para = new SqlParameter("id", ID);
                    dbhelper.SqlParameterList.Add(Para);
                    Para = new SqlParameter("bkdh", bkdh);
                    dbhelper.SqlParameterList.Add(Para);
                    Para = new SqlParameter("PersonID", PersonID);
                    dbhelper.SqlParameterList.Add(Para);
                    Para = new SqlParameter("ModifyUser", ModifyUser);
                    dbhelper.SqlParameterList.Add(Para);
                    int num = dbhelper.ExecuteNonQuery(tran, sql);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    res = ex.Message;
                }
            }
            conn.Close();
            return res;
        }

        public string TD(string ids, int ModifyUser)
        {
            string res = "";
            SqlConnection conn = new SqlConnection(dbhelper.SqlConnectionString);
            conn.Open();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlParameter Para = null;
                    string[] pk = ids.Split(',');
                    foreach (string item in pk)
                    {
                        string sql = @"update [order] set state=-1,nguid=newid() ,
ModifyDate = GETDATE(),ModifyUser = @ModifyUser where id=@id and isnull(state,0)!=-1";
                        Para = new SqlParameter("id", item);
                        dbhelper.SqlParameterList.Add(Para);
                        Para = new SqlParameter("ModifyUser", ModifyUser);
                        dbhelper.SqlParameterList.Add(Para);
                        int num = dbhelper.ExecuteNonQuery(tran, sql);
                        if (num>0)
                        {
                            sql = " update cost set moneypayed=moneypayed*-1 where  orderid=@id ";
                            Para = new SqlParameter("id", item);
                            dbhelper.SqlParameterList.Add(Para);
                            num = dbhelper.ExecuteNonQuery(tran, sql);
                        }
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    res = ex.Message;
                }
            }
            conn.Close();
            return res;
        }
        private string check( int ID = 0, string guid = "")
        {
            string sql = @"";
            SqlParameter Para = null;
            if (!string.IsNullOrEmpty(guid))
            {
                sql = @" select 1 from [Order] where NGuid =@NGuid";
                Para = new SqlParameter("NGuid", guid._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                if (dbhelper.ExecuteSql(sql).Rows.Count == 0)
                {
                    return "数据已被更改,请重新加载!";
                }
            }
            return "";
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="BKDH">报刊代号</param>
        /// <param name="orderpeopleid">订户id</param>
        /// <param name="ordernum">订购数</param>
        /// <param name="ordermonths">订购多少月</param>
        /// <param name="orderDate">订购起始日期</param>
        /// <param name="inuser">录入员</param>
        /// <param name="posterid">投递员,相当于收订人</param>
        /// <returns></returns>
        public string Insert(string BKDH, int orderpeopleid, int ordernum, int ordermonths, string orderDate, string inuser, int posterid, SqlTransaction tran)
        {
            string res = "";
            try
            {
                string sql = @" insert into [Order](BKDH,personid,userid,ordernum,orderdate,posterid,ordermonths) 
values(@BKDH,@personid,@userid,@ordernum,@orderdate,@posterid,@ordermonths) ";
                SqlParameter Para = null;
                Para = new SqlParameter("BKDH", BKDH._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("personid", orderpeopleid._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("userid", inuser._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("ordernum", ordernum._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("orderdate", orderDate._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("posterid", posterid._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("ordermonths", ordermonths._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                int num = dbhelper.ExecuteInsert(tran,sql);
                if (num==0)
                {
                    res = "保存失败";
                }
                CostDAL dal = new CostDAL();
                DocDAL doc = new DocDAL();
                decimal money = doc.GetPrice(BKDH);
                dal.insert(tran, num, money * ordernum * ordermonths, 0, inuser._ToInt32());
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string DeleteByPK(string ids) 
        {
            string res = "";
            SqlConnection conn = new SqlConnection(dbhelper.SqlConnectionString);
            conn.Open();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlParameter Para = null;
                    string[] pk = ids.Split(',');
                    foreach (string item in pk)
                    {
                        string sql = "delete from  [order] where id=@id";
                        Para = new SqlParameter("id", item._ToInt32());
                        dbhelper.SqlParameterList.Add(Para);
                        int num = dbhelper.ExecuteNonQuery(tran, sql);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    res = ex.Message;
                }
            }
            conn.Close();
            return res;
        }
    }
}
