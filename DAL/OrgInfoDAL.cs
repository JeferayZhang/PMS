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
        public string childs = "0,";
        public string getChilds(string orgid)
        {
            string sql = @"SELECT t.orgid,STUFF((SELECT ','+ltrim(org.orgID)  FROM org    
  WHERE parentid=t.orgid FOR XML PATH('')), 1, 1, '') AS ids
FROM org t  where t.orgid in ("+ orgid + @")
group by t.OrgID";
            DataTable dt = dbh.ExecuteSql(sql);
            if (dt.Rows.Count>0 && !string.IsNullOrEmpty(dt.Rows[0]["ids"]._ToStr()))
            {
                foreach (DataRow item in dt.Rows)
                {
                    childs += item["ids"]._ToStr() + ",";
                    getChilds(item["ids"]._ToStr());
                }
                
            }
            return childs;
        }

        /// <summary>
        /// 获取用户和用户界面上选择的机构的交集,即可操作的机构ID
        /// </summary>
        /// <param name="userorg">用户所属机构</param>
        /// <param name="chooseorg">选择的机构</param>
        /// <returns></returns>
        public string getChilds(string userorg,string chooseorg)
        {
            string str1 = getChilds(userorg);
            str1= str1.Substring(0, str1.Length - 1);
            childs = "0,";
            if (string.IsNullOrEmpty(chooseorg))
            {
                return str1;
            }
            string str2 = getChilds(chooseorg);
            str2 = str2.Substring(0, str2.Length - 1);
            string[] str = str1.Split(',').Intersect(str2.Split(',')).ToArray();
            string str3 = string.Join(",", str);
            return str3;
        }
        public string insert(string Name,string address,string OrgCode,int parentID=0,int level=0) 
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
            Para = new SqlParameter("level", level._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            string sql = @" insert into org(name,address,parentid,OrgCode,level) values(@Name,@address,@parentID,@OrgCode,@level)";
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
