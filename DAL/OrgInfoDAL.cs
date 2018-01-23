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
    public class OrgInfoDAL
    {
        SqlHelp dbh = new SqlHelp();
        /// <summary>
        /// 根据名称获取ID
        /// </summary>
        /// <param name="orgname">此名称应该唯一</param>
        /// <returns></returns>
        public int GetOrgByName(string orgname) 
        {
            DataTable dt=new DataTable();
            SqlParameter Para = null;
            Para = new SqlParameter("orgname", orgname._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            dt = dbh.ExecuteSql(" select orgid from org where name=@orgname");
            if (dt!=null &&dt.Rows.Count>0)
            {
                return dt.Rows[0][0]._ToInt32();
            }
            return 0;
        }

        public DataTable GetOrgByPK(int id) 
        {
            DataTable dt = new DataTable();
            SqlParameter Para = null;
            Para = new SqlParameter("id", id._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            dt = dbh.ExecuteSql(" select * from org where orgID=@id");
            return dt;
        }

        public DataTable GetOrgByParentID(int ParentID)
        {
            DataTable dt = new DataTable();
            SqlParameter Para = null;
            Para = new SqlParameter("ParentID", ParentID._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            dt = dbh.ExecuteSql(" select * from org where ParentID=@ParentID");
            return dt;
        }

        public int insert(string Name,string address,int parentID=0) 
        {
            SqlParameter Para = null;
            Para = new SqlParameter("Name", Name._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("address", address._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("parentID", parentID._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            string sql = @" insert into org(name,address,parentid) values(@Name,@address,@parentID)";
            return dbh.ExecuteInsert(sql);
        }

        public int update(int id, string Name, string address)
        {
            SqlParameter Para = null;
            Para = new SqlParameter("Name", Name._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("address", address._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("id", id);
            dbh.SqlParameterList.Add(Para);
            string sql = @" update org set name=@Name,address=@address where orgID=@id";
            return dbh.ExecuteNonQuery(sql);
        }

        public string delete(string IDs)
        {
            string res = "";
            try
            {
                string[] list = IDs.Split(',');
                int num = 0;
                foreach (string item in list)
                {
                    string sql = @" DELETE FROM  org  WHERE orgID = @ID ";
                    SqlParameter Para = new SqlParameter("ID", item._ToStrTrim());
                    dbh.SqlParameterList.Add(Para);
                    num += dbh.ExecuteNonQuery(sql);
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
    }
}
