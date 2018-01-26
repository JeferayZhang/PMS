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
        /// 根据机构统计
        /// </summary>
        /// <param name="chooseorg">所选机构ID</param>
        /// <returns></returns>
        public DataTable GetTableByOrg(string chooseorg, string userorg)
        {
            DataTable dt = new DataTable();
            string sql = @" SELECT t.BKDH ,org.Name OrgName,Doc.Name DocName ,
STUFF((SELECT ','+ltrim([order].ID)  FROM [order]    
  WHERE bkdh=t.bkdh FOR XML PATH('')), 1, 1, '') AS ids,
  SUM(t.OrderNum)OrderNum
FROM [order] t  
left join OrderPeople on t.PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID
left join Doc on Doc.bkdh=t.bkdh
where   dateadd(MONTH,t.OrderMonths,t.Indate)>GETDATE()
and not exists(select 1 from log where log.orderid=t.id and CONVERT(varchar(100), log.date, 23)=CONVERT(varchar(100), getdate(), 23))
";
            OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
            string all = orgInfoDAL.getChilds(userorg, chooseorg);
            sql += " AND OrderPeople.ORGID in (" + all + ") GROUP BY t.BKDH ,org.Name,Doc.Name";
            dt = dbhelper.ExecuteSql(sql);
            return dt;
        }

        /// <summary>
        /// 根据报纸统计
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public DataTable GetTableByBK(string BKDH,string userorg,string chooseorg)
        {
            DataTable dt = new DataTable();
            string sql = @" SELECT t.BKDH ,Doc.Name DocName,org.Name OrgName ,
STUFF((SELECT ','+ltrim([order].ID)  FROM [order]    
  WHERE bkdh=t.bkdh FOR XML PATH('')), 1, 1, '') AS ids,
  SUM(t.OrderNum)OrderNum
FROM [order] t  
left join OrderPeople on t.PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID
left join Doc on Doc.bkdh=t.bkdh
where   dateadd(MONTH,t.OrderMonths,t.Indate)>GETDATE()
and t.BKDH=@BKDH
and not exists(select 1 from log where log.orderid=t.id and CONVERT(varchar(100), log.date, 23)=CONVERT(varchar(100), getdate(), 23))";
            OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
            string all = orgInfoDAL.getChilds(userorg, chooseorg);
            sql += " AND OrderPeople.ORGID in (" + all + ") GROUP BY t.BKDH ,org.Name,Doc.Name";
            SqlParameter Para = null;
            Para = new SqlParameter("BKDH", BKDH);
            dbhelper.SqlParameterList.Add(Para);
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
    }
}
