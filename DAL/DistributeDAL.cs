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
        /// <param name="type">如果为0,则表示分发行为;如果为1,则表示查看日志</param>
        /// <returns></returns>
        public DataTable GetTable(string BKDH, string chooseorg, string userorg, string Group_Type, string dt1, int type = 0)
        {
            DataTable dt = new DataTable();
            OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
            string all = orgInfoDAL.getChilds(userorg, chooseorg);
            string wheresql = " AND OrderPeople.ORGID in (" + all + @") ";
            SqlParameter Para = null;
            if (!string.IsNullOrEmpty(BKDH))
            {
                wheresql += " AND t.BKDH=@BKDH";
                Para = new SqlParameter("BKDH", BKDH);
                dbhelper.SqlParameterList.Add(Para);
            }
            string logsql = "";
            if (type == 1)
            {
                logsql += " and log.type=1 ";
            }
            else
            {
                //分发行为,要查出已经被市公司分发过了,但是没有被县公司分发的记录
                logsql += " and log.type=0 ";
                wheresql += @" and not exists(
select 1 from log where log.orderid=t.id and 
CONVERT(varchar(100), log.date, 23)=CONVERT(varchar(100), @INDATE1, 23) and log.type=1 ) ";
            }
            string sql = @"select  a.BKDH,a.DocName,a.OrgName,SUM(a.OrderNum) OrderNum,a.ParentID,ids=STUFF((SELECT ','+ltrim([order].ID)  FROM [order]    
  WHERE BKDH=a.BKDH FOR XML PATH('')), 1, 1, '') from (
select a.ID,a.BKDH,Doc.Name DocName,a.PersonID,a.UnitName,a.OrderNum, Org.ParentID,Org.Name OrgName from (
select a.ID,a.BKDH,a.PersonID,a.UnitName,a.OrderNum, Org.ParentID,Org.Name from (
select t.ID ,t.BKDH,t.PersonID,OrderPeople.UnitName,Org.ParentID,org.Name,t.OrderNum from [Order] t  
left join OrderPeople on t.PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID 
where   dateadd(MONTH,t.OrderMonths,t.OrderDate)>CONVERT(varchar(100), @INDATE1, 23)
and not exists(
select 1 from log where log.orderid=t.id and 
CONVERT(varchar(100), log.date, 23)=CONVERT(varchar(100), @INDATE1, 23) " + logsql + @"
) " + wheresql + @" 
) a
left join Org on a.ParentID=Org.OrgID) a 
left join Org on a.ParentID=Org.OrgID 
left join Doc on Doc.BKDH=a.BKDH) a 
GROUP BY a.BKDH,a.DocName,a.OrgName,a.ParentID";
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
            dt = dbhelper.ExecuteSql(sql);
            return dt;
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
        public DataTable GetTable2(string BKDH, string chooseorg, string userorg, string Group_Type, string dt1,int type = 0)
        {
            DataTable dt = new DataTable();
            OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
            string all = orgInfoDAL.getChilds(userorg, chooseorg);
            string wheresql = " AND OrderPeople.ORGID in (" + all + @") ";
            SqlParameter Para = null;
            
            if (!string.IsNullOrEmpty(BKDH))
            {
                wheresql += " AND t.BKDH=@BKDH";
                Para = new SqlParameter("BKDH", BKDH);
                dbhelper.SqlParameterList.Add(Para);
            }
            string logsql = "";
            if (type == 1)
            {
                logsql += " and log.type=1 ";
            }
            else
            {
                //分发行为,要查出已经被市公司分发过了,但是没有被县公司分发的记录
                logsql += " and log.type=0 ";
                wheresql += @" and not exists(
select 1 from log where log.orderid=t.id and 
CONVERT(varchar(100), log.date, 23)=CONVERT(varchar(100), @INDATE1, 23) and log.type=1 ) ";
            }
            string sql = @"select  a.BKDH,a.DocName,a.OrgName ,SUM(a.OrderNum) OrderNum,ids=STUFF((SELECT ','+ltrim([order].ID)  FROM [order]    
  WHERE PersonID=a.PersonID and BKDH=a.BKDH FOR XML PATH('')), 1, 1, '') from (
select t.ID ,t.BKDH,Doc.Name DocName,Org.Name OrgName ,t.OrderNum,t.PersonID from [Order] t  
left join OrderPeople on t.PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID 
left join Doc on Doc.BKDH=t.BKDH
where   dateadd(MONTH,t.OrderMonths,t.OrderDate)>CONVERT(varchar(100), @INDATE1, 23)
and exists(
select 1 from log where log.orderid=t.id and 
CONVERT(varchar(100), log.date, 23)=CONVERT(varchar(100), @INDATE1, 23) "+ logsql + @"
)" + wheresql + @" 
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
            
            dt = dbhelper.ExecuteSql(sql);
            return dt;
        }


        /// <summary>
        /// 插入分发记录,理论上每条订购记录一天只能分发一次
        /// </summary>
        /// <param name="orderids">订购流水号</param>
        /// <param name="nianjuanqi">报纸年卷期</param>
        /// <returns></returns>
        public string insertLog(string orderids,string nianjuanqi, int UserID)
        {
            string res = "";
            SqlConnection conn = new SqlConnection(dbhelper.SqlConnectionString);
            conn.Open();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlParameter Para = null;
                    string[] pk = orderids.Split(',');
                    foreach (string item in pk)
                    {
                        string sql = @"INSERT INTO LOG(ORDERID,DATE,NIANJUANQI,UserID) VALUES(@ORDERID,getdate(),@NIANJUANQI,@UserID)";
                        Para = new SqlParameter("ORDERID", item._ToInt32());
                        dbhelper.SqlParameterList.Add(Para);
                        Para = new SqlParameter("NIANJUANQI", nianjuanqi._ToStrTrim());
                        dbhelper.SqlParameterList.Add(Para);
                        Para = new SqlParameter("UserID", UserID);
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
        public DataTable getlog1(string dt1, string dt2, string userid, int limit = 0, int index = 0)
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
            string sql = @"select  ROW_NUMBER() over (order by a.BKDH) as rownumber,a.BKDH,a.DocName,a.OrgName,SUM(a.OrderNum) OrderNum,a.ParentID,ids=STUFF((SELECT ','+ltrim([order].ID)  FROM [order]    
  WHERE BKDH=a.BKDH FOR XML PATH('')), 1, 1, ''),CONVERT(varchar(100),a.Date, 23) as Date,NianJuanQi,a.NAME from (
select a.ID,a.BKDH,Doc.Name DocName,a.PersonID,a.UnitName,a.OrderNum, Org.ParentID,Org.Name OrgName,a.Date,NianJuanQi,a.NAME from (
select a.ID,a.BKDH,a.PersonID,a.UnitName,a.OrderNum, Org.ParentID,Org.Name OrgName,a.Date,NianJuanQi,a.NAME from (
select t.ID ,t.BKDH,t.PersonID,OrderPeople.UnitName,Org.ParentID,org.Name OrgName,t.OrderNum,log.Date ,NianJuanQi,users.NAME from [Order] t  
left join OrderPeople on t.PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID 
left join log on log.orderid=t.id 
left join users on users.id=log.userid where 1=1 " + logsql + @"
) a
left join Org on a.ParentID=Org.OrgID) a 
left join Org on a.ParentID=Org.OrgID 
left join Doc on Doc.BKDH=a.BKDH) a 
GROUP BY a.BKDH,a.DocName,a.OrgName,a.ParentID,a.Date,a.NAME,NianJuanQi";
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
        public DataTable getlog2(string dt1, string dt2, string userid, int limit = 0, int index = 0)
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
            string sql = @"select  ROW_NUMBER() over (order by a.BKDH) as rownumber,a.BKDH,a.DocName,a.OrgName ,SUM(a.OrderNum) OrderNum,ids=STUFF((SELECT ','+ltrim([order].ID)  FROM [order]    
  WHERE PersonID=a.PersonID and BKDH=a.BKDH FOR XML PATH('')), 1, 1, ''),CONVERT(varchar(100),a.Date, 23) as Date,a.NAME,NianJuanQi from (
select t.ID ,t.BKDH,Doc.Name DocName,Org.Name OrgName ,t.OrderNum,t.PersonID,log.Date,users.NAME,NianJuanQi from [Order] t  
left join OrderPeople on t.PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID 
left join Doc on Doc.BKDH=t.BKDH
left join Log on log.OrderID=t.ID
left join users on users.id=log.userid where log.type=1 " + logsql + @"
) a GROUP BY a.BKDH,a.DocName,a.OrgName ,a.PersonID,a.Date,a.NAME,NianJuanQi ;";
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
