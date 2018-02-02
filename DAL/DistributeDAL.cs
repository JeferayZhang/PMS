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
    /// 报纸分发
    /// </summary>
    public class DistributeDAL
    {
        SqlHelp dbhelper = new SqlHelp();

        
        /// <summary>
        /// 统计-市-县级分发
        /// </summary>
        /// <param name="BKDH">报刊代号</param>
        /// <param name="chooseorg">所选机构ID</param>
        /// <param name="userorg">用户所属机构ID</param>
        /// <param name="Group_Type">排序方式:1根据报刊名称排序.2根据机构排序</param>
        /// <param name="dt1">分发日期</param>
        /// <returns></returns>
        public DataTable GetTable(string BKDH, string chooseorg, string userorg, string Group_Type, string dt1,int limit,int page)
        {
            DataTable dt = new DataTable();
            OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
            string all = orgInfoDAL.getChilds(userorg, chooseorg);
            string wheresql = " AND OrderPeople.ORGID in (" + all + @") ";
            SqlParameter Para = null;
            if (!string.IsNullOrEmpty(BKDH))
            {
                wheresql += " AND upper(t.BKDH)=@BKDH";
                Para = new SqlParameter("BKDH", BKDH.ToUpper());
                dbhelper.SqlParameterList.Add(Para);
            }
            string sql = @"select ROW_NUMBER() over (order by a.DocName) as rownumber, a.DocName,a.OrgName,SUM(a.OrderNum) OrderNum,ids=STUFF((SELECT ','+ltrim(b.ID)  FROM (select a.ID,a.BKDH,a.OrderNum,Org.OrgID, Org.Name OrgName from (
select a.ID,a.BKDH,a.PersonID,a.UnitName,a.OrderNum, Org.ParentID,Org.Name from (
select t.ID ,t.BKDH,t.PersonID,OrderPeople.UnitName,Org.ParentID,org.Name,t.OrderNum from [Order] t  
left join OrderPeople on t.PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID 
where   dateadd(MONTH,t.OrderMonths,t.OrderDate)>CONVERT(varchar(100), @INDATE1, 23) 
and CONVERT(varchar(100),t.OrderDate, 23)<=CONVERT(varchar(100),getdate(), 23)  
" + wheresql + @" 
) a
left join Org on a.ParentID=Org.OrgID) a 
left join Org on a.ParentID=Org.OrgID 
left join Doc on Doc.BKDH=a.BKDH) b  
  WHERE b.OrgID=a.OrgID and b.BKDH=a.BKDH  FOR XML PATH('')), 1, 1, '') from (
select a.ID,Doc.Name DocName,a.BKDH,a.OrderNum,Org.OrgID,Org.Name OrgName from (
select a.ID,a.BKDH,a.PersonID,a.UnitName,a.OrderNum, Org.ParentID,Org.Name from (
select t.ID ,t.BKDH,t.PersonID,OrderPeople.UnitName,Org.ParentID,org.Name,t.OrderNum from [Order] t  
left join OrderPeople on t.PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID 
where   dateadd(MONTH,t.OrderMonths,t.OrderDate)>CONVERT(varchar(100), @INDATE1, 23) and CONVERT(varchar(100),t.OrderDate, 23)<=
CONVERT(varchar(100),getdate(), 23) " + wheresql + @" 
) a
left join Org on a.ParentID=Org.OrgID) a 
left join Org on a.ParentID=Org.OrgID 
left join Doc on Doc.BKDH=a.BKDH) a 
GROUP BY a.DocName,a.OrgName,a.BKDH,a.OrgID ";
            if (Group_Type=="1")
            {
                sql += " order by a.DocName";
            }
            if (Group_Type == "2")
            {
                sql += " order by a.OrgName";
            }
            if (!string.IsNullOrEmpty(dt1._ToStrTrim()))
            {
                Para = new SqlParameter("INDATE1", dt1._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
            }
            else
            {
                Para = new SqlParameter("INDATE1", System.DateTime.Now.ToShortDateString());
                dbhelper.SqlParameterList.Add(Para);
            }
            string top = "";
            if (limit>0)
            {
                top = " top "+limit;
            }
            dt = dbhelper.ExecuteSql("select "+top+" * from ("+sql+ ") tttt where tttt.rownumber>"+ limit * (page-1));
            return dt;
        }

        public int count(string ids,int type,string nianjuanqi) 
        {
            string sql = string.Format(@"select count(1) from log where log.orderid in ({0}) and type={1} and nianjuanqi='{2}'", ids, type, nianjuanqi);
            int cou = dbhelper.Count(sql);
            return cou;
        } 
        /// <summary>
        /// 统计-县-段道级分发
        /// </summary>
        /// <param name="BKDH">报刊代号</param>
        /// <param name="chooseorg">所选机构ID</param>
        /// <param name="userorg">用户所属机构ID</param>
        /// <param name="Group_Type">排序方式:1根据报刊名称排序.2根据机构排序</param>
        /// <param name="dt1">分发日期</param>
        /// <param name="type">如果为0,则表示分发行为;如果为1,则表示查看日志</param>
        /// <returns></returns>
        public DataTable GetTable2(string BKDH, string chooseorg, string userorg, string Group_Type, string dt1, int limit, int page, int type = 0)
        {
            DataTable dt = new DataTable();
            OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
            string all = orgInfoDAL.getChilds(userorg, chooseorg);
            string wheresql = " AND OrderPeople.ORGID in (" + all + @") ";
            SqlParameter Para = null;
            
            if (!string.IsNullOrEmpty(BKDH))
            {
                wheresql += " AND upper(t.BKDH)=@BKDH";
                Para = new SqlParameter("BKDH", BKDH.ToUpper());
                dbhelper.SqlParameterList.Add(Para);
            }
            string logsql = "";
            logsql += " and log.type=0 ";
            string sql = @"select  ROW_NUMBER() over (order by a.DocName) as rownumber,a.BKDH,a.DocName,a.OrgName ,SUM(a.OrderNum) OrderNum,ids=STUFF((SELECT ','+ltrim([order].ID)  FROM [order]    
  WHERE PersonID=a.PersonID and BKDH=a.BKDH FOR XML PATH('')), 1, 1, '') from (
select t.ID ,t.BKDH,Doc.Name DocName,Org.Name OrgName ,t.OrderNum,t.PersonID from [Order] t  
left join OrderPeople on t.PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID 
left join Doc on Doc.BKDH=t.BKDH
where   dateadd(MONTH,t.OrderMonths,t.OrderDate)>CONVERT(varchar(100), @INDATE1, 23)
and exists(
select 1 from log where log.orderid=t.id and 
CONVERT(varchar(100), log.date, 23)=CONVERT(varchar(100), @INDATE1, 23) " + logsql + @"
) 
) a
GROUP BY a.BKDH,a.DocName,a.OrgName ,a.PersonID ";
            if (Group_Type == "1")
            {
                sql += " order by a.DocName";
            }
            if (Group_Type == "2")
            {
                sql += " order by a.OrgName";
            }
            if (!string.IsNullOrEmpty(dt1._ToStrTrim()))
            {
                Para = new SqlParameter("INDATE1", dt1._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
            }
            else
            {
                Para = new SqlParameter("INDATE1", System.DateTime.Now.ToShortDateString());
                dbhelper.SqlParameterList.Add(Para);
            }
            string top = "";
            if (limit > 0)
            {
                top = " top " + limit;
            }
            dt = dbhelper.ExecuteSql("select " + top + " * from (" + sql + ") tttt where tttt.rownumber>" + limit * (page - 1));
            return dt;
        }


        /// <summary>
        /// 插入分发记录,理论上每条订购记录一天只能分发一次
        /// </summary>
        /// <param name="orderids">订购流水号</param>
        /// <param name="nianjuanqi">报纸年卷期</param>
        /// <returns></returns>
        public string insertLog(string orderids,string nianjuanqi, int UserID,int type=0)
        {
            string res = "";
            SqlConnection conn = new SqlConnection(dbhelper.SqlConnectionString);
            conn.Open();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlParameter Para = null;
                    string[] pk = orderids.Split(',').Distinct().ToArray();
                    foreach (string item in pk)
                    {
                        string sql = @"INSERT INTO LOG(ORDERID,DATE,NIANJUANQI,UserID,Type) VALUES(@ORDERID,getdate(),@NIANJUANQI,@UserID,@Type)";
                        Para = new SqlParameter("ORDERID", item._ToInt32());
                        dbhelper.SqlParameterList.Add(Para);
                        Para = new SqlParameter("NIANJUANQI", nianjuanqi._ToStrTrim().ToUpper());
                        dbhelper.SqlParameterList.Add(Para);
                        Para = new SqlParameter("UserID", UserID);
                        dbhelper.SqlParameterList.Add(Para);
                        Para = new SqlParameter("Type", type);
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

        /// <summary>
        /// 统计-市-县级分发历史
        /// </summary>
        /// <param name="dt1">分发日期</param>
        /// <param name="dt2">分发日期</param>
        /// <param name="userid">分发员</param>
        /// <returns></returns>
        public DataTable getlog1(string dt1, string dt2, int userorg, string userid, int limit = 0, int index = 0)
        {
            DataTable dt = new DataTable();
            //OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
            //string all = orgInfoDAL.getChilds(userorg, chooseorg);
            SqlParameter Para = null;
            string logsql = "";
            if (!string.IsNullOrEmpty(dt1._ToStrTrim()))
            {
                Para = new SqlParameter("INDATE1", dt1._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                logsql += " AND CONVERT(varchar(100),log.date, 23)  >= CONVERT(varchar(100),@INDATE1, 23)";
            }
            if (!string.IsNullOrEmpty(dt2._ToStrTrim()))
            {
                Para = new SqlParameter("INDATE2", dt2._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                logsql += " AND  CONVERT(varchar(100),log.date, 23)  <= CONVERT(varchar(100),@INDATE2, 23) ";
            }
            if (!string.IsNullOrEmpty(userid._ToStrTrim()))
            {
                Para = new SqlParameter("userid", userid._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                logsql += " AND  log.userid=@userid ";
            }
            OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
            string all = orgInfoDAL.getChilds(userorg._ToStr(), "");
            logsql += " AND e.ORGID in (" + all + ")";
            string sql = @"select  ROW_NUMBER() over (order by log.ID) as rownumber, log.ID,log.nianjuanqi,CONVERT(VARCHAR, log.Date,120) Date,c.name as ffname, f.Name as OrgName,
e.UnitName UnitName,d.Name, 
case Log.type when 0 then '市级分发' else '县级分发' end as type,b.BKDH,b.OrderNum from log  
left join [Order] b on Log.OrderID=b.ID
left join USERS c on c.ID=Log.UserID
left join Doc d on d.BKDH=b.BKDH
left join OrderPeople e on b.PersonID=e.ID
left join Org f on f.OrgID=e.OrgID where  Log.TYPE=0 AND   1=1 " + logsql;
            string top = "";
            if (limit > 0)
            {
                top = " top "+ limit;
            }
            dt = dbhelper.ExecuteSql(" select " + top + " a.* from (" + sql + " )a where a.rownumber>" + limit * index);
            return dt;
        }


        /// <summary>
        /// 统计-县-段道级分发历史
        /// </summary>
        /// <param name="dt1">分发日期</param>
        /// <param name="dt2">分发日期</param>
        /// <param name="userid">分发员</param>
        /// <returns></returns>
        public DataTable getlog2(string dt1, string dt2,int userorg, string userid, int limit = 0, int index = 0)
        {
            DataTable dt = new DataTable();
            SqlParameter Para = null;
            string logsql = "";
            if (!string.IsNullOrEmpty(dt1._ToStrTrim()))
            {
                Para = new SqlParameter("INDATE1", dt1._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                logsql += " AND CONVERT(varchar(100),log.date, 23)  >= CONVERT(varchar(100),@INDATE1, 23)";
            }
            if (!string.IsNullOrEmpty(dt2._ToStrTrim()))
            {
                Para = new SqlParameter("INDATE2", dt2._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                logsql += " AND  CONVERT(varchar(100),log.date, 23)  <= CONVERT(varchar(100),@INDATE2, 23) ";
            }
            if (!string.IsNullOrEmpty(userid._ToStrTrim()))
            {
                Para = new SqlParameter("userid", userid._ToDateTime());
                dbhelper.SqlParameterList.Add(Para);
                logsql += " AND  log.userid=@userid ";
            }
            OrgInfoDAL _org = new OrgInfoDAL();
            string all = _org.getChilds(userorg._ToStr(), "");
            logsql += " AND e.ORGID in (" + all + ")";
            string sql = @"select  ROW_NUMBER() over (order by log.ID) as rownumber, log.ID,log.nianjuanqi,log.Date,c.name as ffname, f.Name as OrgName,
e.UnitName UnitName,d.Name, 
case Log.type when 0 then '市级分发' else '县级分发' end as type,b.BKDH,b.OrderNum from log  
left join [Order] b on Log.OrderID=b.ID
left join USERS c on c.ID=Log.UserID
left join Doc d on d.BKDH=b.BKDH
left join OrderPeople e on b.PersonID=e.ID
left join Org f on f.OrgID=e.OrgID where Log.TYPE=1 AND  1=1 " + logsql;
            string top = "";
            if (limit > 0)
            {
                top = " top " + limit;
            }
            dt = dbhelper.ExecuteSql(" select " + top + " a.* from (" + sql + " )a where a.rownumber>" + limit * index);
            return dt;
        }
    }
}
