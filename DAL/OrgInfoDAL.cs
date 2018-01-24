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
        /// 根据机构编号获取ID
        /// </summary>
        /// <param name="orgname">机构编号唯一</param>
        /// <returns></returns>
        public int GetOrgByName( string OrgCode) 
        {
            DataTable dt=new DataTable();
            SqlParameter Para = null;
            Para = new SqlParameter("OrgCode", OrgCode._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            dt = dbh.ExecuteSql(" select orgid from org where OrgCode=@OrgCode");
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

        public string insert(string Name,string address,string OrgCode,int parentID=0) 
        {
            if (GetOrgByName(OrgCode)>0)
            {
                return "机构编号已经存在";
            }
            SqlParameter Para = null;
            Para = new SqlParameter("Name", Name._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("address", address._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("parentID", parentID._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("OrgCode", OrgCode._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            string sql = @" insert into org(name,address,parentid,OrgCode) values(@Name,@address,@parentID,@OrgCode)";
            if (dbh.ExecuteInsert(sql) > 0)
            {
                return "";
            }
            else
            {
                return "新增失败";
            }
        }

        public string update(int id, string Name, string OrgCode, string address)
        {
            if (GetOrgByName(OrgCode) > 0)
            {
                return "机构编号已经存在";
            }
            SqlParameter Para = null;
            Para = new SqlParameter("Name", Name._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("address", address._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("id", id);
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("OrgCode", OrgCode._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            string sql = @" update org set name=@Name,address=@address,OrgCode=@OrgCode where orgID=@id";
            if (dbh.ExecuteNonQuery(sql)>0)
            {
                return "";
            }
            else
            {
                return "更新失败";
            }
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
