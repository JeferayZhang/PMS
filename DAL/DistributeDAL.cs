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
        /// <param name="orgid"></param>
        /// <returns></returns>
        public DataTable GetTableByOrg(string orgid)
        {
            DataTable dt = new DataTable();
            string sql = @" select sum([Order].OrderNum) as allnums,org.Name OrgName,Doc.Name DocName 
from [Order]  left join OrderPeople on [Order].PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID
left join Doc on Doc.bkdh=[Order].bkdh
where   dateadd(MONTH,[Order].OrderMonths,[Order].Indate)>GETDATE()
and Org.ParentID=@orgid and not exists(select 1 from log where log.orderid=[Order].id and CONVERT(varchar(100), log.date, 23)=CONVERT(varchar(100), getdate(), 23))
group by org.Name,Doc.Name ";
            SqlParameter Para = null;
            Para = new SqlParameter("orgid", orgid._ToInt32());
            dbhelper.SqlParameterList.Add(Para);
            dt = dbhelper.ExecuteSql(sql);
            return dt;
        }

        /// <summary>
        /// 根据报纸统计
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public DataTable GetTableByBK(string BKDH)
        {
            DataTable dt = new DataTable();
            string sql = @" select sum([Order].OrderNum) as allnums,org.Name OrgName,Doc.Name DocName 
from [Order]  left join OrderPeople on [Order].PersonID=OrderPeople.ID 
left join Org on org.OrgID=OrderPeople.OrgID
left join Doc on Doc.bkdh=[Order].bkdh
where   dateadd(MONTH,[Order].OrderMonths,[Order].Indate)>GETDATE()
and [Order].BKDH=@BKDH and not exists(select 1 from log where log.orderid=[Order].id and CONVERT(varchar(100), log.date, 23)=CONVERT(varchar(100), getdate(), 23))
group by org.Name,Doc.Name ";
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
        public string insertLog(string orderids,string nianjuanqi)
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
                        string sql = @"INSERT INTO LOG(ORDERID,DATE,NIANJUANQI) VALUES(@ORDERID,getdate(),@NIANJUANQI)";
                        Para = new SqlParameter("ORDERID", item._ToInt32());
                        dbhelper.SqlParameterList.Add(Para);
                        Para = new SqlParameter("NIANJUANQI", nianjuanqi._ToStrTrim());
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
