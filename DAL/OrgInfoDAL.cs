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
        public int GetOrgByName(string OrgCode, int id = 0) 
        {
            DataTable dt=new DataTable();
            SqlParameter Para = null;
            Para = new SqlParameter("OrgCode", OrgCode._ToStrTrim().ToUpper());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("id", id);
            dbh.SqlParameterList.Add(Para);
            dt = dbh.ExecuteSql(" select orgid from org where OrgCode=@OrgCode and orgid<>@id");
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
            dt = dbh.ExecuteSql(@"  select org.* ,b.Name ParentName from org 
  left join Org b on Org.ParentID=b.OrgID  where org.orgID=@id");
            return dt;
        }

        /// <summary>
        /// 根据父节点ID查询子节点数据,筛选出用户所能看到的
        /// </summary>
        /// <param name="ParentID">父级ID</param>
        /// <param name="UserOrgID">用户所属机构ID</param>
        /// <returns></returns>
        public DataTable GetOrgByParentID(int ParentID, string UserOrgID = "")
        {
            DataTable dt = new DataTable();
            SqlParameter Para = null;
           
            //string all = getChilds(UserOrgID, "");
            Para = new SqlParameter("ParentID", ParentID._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            //dt = dbh.ExecuteSql(" select * from org where ParentID=@ParentID and orgid in (" + all + ")");
            dt = dbh.ExecuteSql(@" select org.* ,b.Name ParentName from org 
  left join Org b on Org.ParentID=b.OrgID  where org.ParentID=@ParentID");
            return dt;
        }

        public DataTable getCurr(int UserOrgID)
        {
            DataTable dt = new DataTable();
            SqlParameter Para = null;
            Para = new SqlParameter("orgid", UserOrgID._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            dt = dbh.ExecuteSql(@" select org.*,b.Name ParentName from org 
left join Org b on Org.ParentID=b.OrgID
 where org.orgid in (select ParentID from org where orgid=@orgid)");
            return dt;
        }

        public DataTable getAllOrgByUser(int UserOrgID,int userlevel)
        {
            DataTable dt =new DataTable();
            if (userlevel > 0)
            {
                for (int i = 0; i < userlevel; i++)
                {
                    DataTable nt = getCurr(UserOrgID);
                    if (nt.Rows.Count > 0)
                    {
                        dt = nt;
                        UserOrgID = nt.Rows[0]["orgid"]._ToInt32();
                    }
                }
            }
            else
            {
                dt = GetOrgByPK(UserOrgID);
            }
            return dt;
        }
        /// <summary>
        /// 初始化为0,
        /// </summary>
        public string childs = "0,";
        public string getChilds(string orgid)
        {
            if (string.IsNullOrEmpty(orgid))
            {
                return childs;
            }
            string sql = @"SELECT t.orgid,STUFF((SELECT ','+ltrim(org.orgID)  FROM org    
  WHERE parentid=t.orgid FOR XML PATH('')), 1, 1, '') AS ids
FROM org t  where t.orgid in ("+ orgid + @")
group by t.OrgID";
            DataTable dt = dbh.ExecuteSql(sql);
            if (dt.Rows.Count>0 && !string.IsNullOrEmpty(dt.Rows[0]["ids"]._ToStr()))
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(item["ids"]._ToStr()))
                    {
                        childs += item["ids"]._ToStr() + ",";
                    }
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
            string str1 = "";
            string str2 = "";
            if (!string.IsNullOrEmpty(userorg))
            {
                childs = userorg._ToInt32() + ",";
                str1 = getChilds(userorg);
                str1 = str1.Substring(0, str1.Length - 1);
            }
            if (!string.IsNullOrEmpty(chooseorg))
            {
                //这里是为了重置机构,免得重复
                childs = chooseorg + ",";
                str2 = getChilds(chooseorg);
                str2 = str2.Substring(0, str2.Length - 1);
                //取交集
                string[] str = str1.Split(',').Intersect(str2.Split(',')).ToArray();
                str1 = string.Join(",", str);
            }
            return str1;
        }
        public string insert(string Name,string address,string OrgCode,int parentID=0,int level=0) 
        {
            if (GetOrgByName(OrgCode)>0)
            {
                return "机构编号已经存在";
            }
            SqlParameter Para = null;
            Para = new SqlParameter("Name", Name._ToStrTrim().ToUpper());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("address", address._ToStrTrim().ToUpper());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("parentID", parentID._ToStrTrim());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("OrgCode", OrgCode._ToStrTrim().ToUpper());
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
            if (GetOrgByName(OrgCode, id) > 0)
            {
                return "机构编号已经存在";
            }
            SqlParameter Para = null;
            Para = new SqlParameter("Name", Name._ToStrTrim().ToUpper());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("address", address._ToStrTrim().ToUpper());
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("id", id);
            dbh.SqlParameterList.Add(Para);
            Para = new SqlParameter("OrgCode", OrgCode._ToStrTrim().ToUpper());
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

        public string check(int id,int userlevel) 
        {
            int level = 0;
            level = GetOrgByPK(id).Rows[0]["Level"]._ToInt32();
            if (level<userlevel)
            {
                return "您不能删除当前级别的机构";
            }
            else
            {
                return "";
            }
        }
        public string DeleteByPK(string ids, int userlevel)
        {
            string res = "";
            SqlConnection conn = new SqlConnection(dbh.SqlConnectionString);
            conn.Open();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlParameter Para = null;
                    string[] pk = ids.Split(',');
                    foreach (string item in pk)
                    {
                        res = check(item._ToInt32(), userlevel);
                        if (!string.IsNullOrEmpty(res))
                        {
                            tran.Rollback();
                            return res;
                        }
                        string sql = @" DELETE FROM  org  WHERE orgID = @ID ";
                        Para = new SqlParameter("ID", item._ToInt32());
                        dbh.SqlParameterList.Add(Para);
                        int num = dbh.ExecuteNonQuery(tran, sql);
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
