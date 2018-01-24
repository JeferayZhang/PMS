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
    public class CostDAL
    {
        SqlHelp dbhelper = new SqlHelp();
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id">缴费单据流水号</param>
        /// <param name="state">0:未缴清,1:已缴清</param>
        /// <param name="OrderNo">单位编号</param>
        /// <param name="unitname">单位名称</param>
        /// <param name="dt1">录入时间</param>
        /// <param name="dt2">录入时间</param>
        /// <param name="pagesize">每页显示数量</param>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        public DataTable GetCostRecords(int id, string state,int orderid, string OrderNo, string unitname, int pagesize, int pageindex)
        {
            DataTable dt = new DataTable();
            string sql = string.Format(@"select top {0} * from (
select a.id,a.orderid,c.UnitName,c.OrderNo,a.money,
a.moneypayed,CONVERT(varchar(100), a.updatetime, 23)updatetime,d.NAME updateuser ,
b.indate,b.ordermonths,
ROW_NUMBER() over (order by a.ID) as rownumber ,
case a.state when 1 then '未缴清' when 0 then'已缴清' else '异常状态' end as state from cost a
left join [Order] b on a.orderid=b.ID
left join OrderPeople c on b.PersonID=c.ID
left join USERS d on a.updateuser=d.ID ", pagesize);
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
            if (orderid > 0)
            {
                SqlParameter Para = new SqlParameter("orderid", orderid._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND b.ID =@orderid";
            }
            if (!string.IsNullOrEmpty(state._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("state", state._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND  state=@state";
            }
            if (!string.IsNullOrEmpty(unitname._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("unitname", unitname._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND c.UnitName LIKE '%'+@unitname+'%'";
            }
            dt = dbhelper.ExecuteSql(sql + string.Format(") t where rownumber > {0} ", (pageindex - 1) * pagesize));
            return dt;
        }


        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="OrderNo"></param>
        /// <param name="unitname"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public int GetCount(int id, int orderid,string OrderNo, string unitname)
        {
            string sql = string.Format(@"
select count(1) as num from cost a
left join [Order] b on a.orderid=b.ID
left join OrderPeople c on b.PersonID=c.ID
left join USERS d on a.updateuser=d.ID  ");
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
            if (orderid > 0)
            {
                SqlParameter Para = new SqlParameter("orderid", orderid._ToInt32());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND b.ID =@orderid";
            }
            if (!string.IsNullOrEmpty(unitname._ToStrTrim()))
            {
                SqlParameter Para = new SqlParameter("unitname", unitname._ToStrTrim());
                dbhelper.SqlParameterList.Add(Para);
                sql += " AND c.UnitName LIKE '%'+@unitname+'%'";
            }

            int num = dbhelper.Count(sql);
            return num;
        }

        /// <summary>
        /// 更新已交费用
        /// </summary>
        /// <param name="id">记录流水号</param>
        /// <param name="moneypayed">金额</param>
        /// <param name="userid">操作员</param>
        /// <returns></returns>
        public string update(int id , decimal money, decimal moneypayed,int userid) 
        {
            string res = "";
            string sql = @"update cost set money=@money, moneypayed=moneypayed+@moneypayed,updatetime=getdate(), updateuser=@updateuser where id=@id";
            SqlConnection conn = new SqlConnection(dbhelper.SqlConnectionString);
            conn.Open();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlParameter Para = null;
                    Para = new SqlParameter("money", money);
                    dbhelper.SqlParameterList.Add(Para);
                    Para = new SqlParameter("moneypayed", moneypayed);
                    dbhelper.SqlParameterList.Add(Para);
                    Para = new SqlParameter("updateuser", userid);
                    dbhelper.SqlParameterList.Add(Para);
                    Para = new SqlParameter("id", id);
                    dbhelper.SqlParameterList.Add(Para);
                    int num = dbhelper.ExecuteNonQuery(tran, sql);
                    sql = "update cost set state=0 where id=@id and money=moneypayed";
                    Para = new SqlParameter("id", id._ToInt32());
                    dbhelper.SqlParameterList.Add(Para);
                    num = dbhelper.ExecuteNonQuery(tran, sql);
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

        public string UpdateStateByPK(string ids)
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
                        string sql = "update cost set state=0 where id=@id";
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

        public int insert(SqlTransaction tran,int orderid,decimal money ,decimal moneypayed, int userid) 
        {
            int num = 0;
            string sql = @"insert into cost(orderid,money,moneypayed,updateuser,state) values(@orderid,@money,@moneypayed,@updateuser,1)";
            try
            {
                SqlParameter Para = null;
                Para = new SqlParameter("orderid", orderid);
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("money", money);
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("moneypayed", moneypayed);
                dbhelper.SqlParameterList.Add(Para);
                Para = new SqlParameter("updateuser", userid);
                dbhelper.SqlParameterList.Add(Para);
                num = dbhelper.ExecuteNonQuery(tran, sql);
            }
            catch (Exception ex)
            {
                num = 0;
            }
            return num;
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
                        string sql = "delete from  cost where id=@id";
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
