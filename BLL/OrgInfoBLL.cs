using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Common;

namespace BLL
{
    
    public class OrgInfoBLL
    {
        OrgInfoDAL dal = new OrgInfoDAL();
        public int GetIDByName(string OrgCode) 
        {
            return dal.GetOrgByName(OrgCode);
        }

        public retValue GetOrgByPK(int id) 
        {
            retValue ret = new retValue();
            DataTable dt = dal.GetOrgByPK(id);
            if (dt!=null&&dt.Rows.Count>0)
            {
                ret.result = true;
                ret.data = dt;
            }
            else
            {
                ret.result = false;
                ret.reason = "未查询到数据";
            }
            return ret;
        }

        /// <summary>
        /// 根据父节点ID查询子节点数据,筛选出用户所能看到的
        /// </summary>
        /// <param name="id">父级ID</param>
        /// <param name="UserOrgID">用户所属机构ID</param>
        /// <returns></returns>
        public retValue GetOrgByParentID(int id, int UserOrgID = 0, int userlevel = 0)
        {
            retValue ret = new retValue();
            DataTable dt = new DataTable();
            //初次加载省分
            if (id == 0)
            {
                if (userlevel != 0)
                {
                    //如果用户是市级别
                    if (userlevel == 1)
                    {
                        dt = dal.getAllOrgByUser(UserOrgID, 1);
                    }
                    //如果用户是县级别
                    if (userlevel == 2)
                    {
                        dt = dal.getAllOrgByUser(UserOrgID, 2);
                    }
                    //如果用户是网点级别
                    if (userlevel == 3)
                    {
                        dt = dal.getAllOrgByUser(UserOrgID, 3);
                    }
                }
                else
                {
                    if (id>0)
                    {
                        dt = dal.GetOrgByPK(id);
                    }
                    else
                    {
                        dt = dal.GetOrgByPK(UserOrgID);
                    }
                }
            }
            //如果不等于,那么表示选择了省份,需要加载用户所属市,县,网点
            else
            {
                //当前选择的机构的级别
                int level = dal.GetOrgByPK(id).Rows[0]["Level"]._ToInt32();
                //如果当前选择的机构级别大于用户级别
                if (level < userlevel)
                {
                    dt = dal.getAllOrgByUser(UserOrgID, userlevel - level - 1);
                }
                else
                {
                    dt = dal.GetOrgByParentID(id, UserOrgID._ToStr());
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.result = true;
                ret.data = dt;
            }
            else
            {
                ret.result = false;
                ret.reason = "未查询到数据";
            }
            return ret;
        }

        public retValue insert(string Name, string address, string OrgCode, int parentID = 0,int level=0) 
        {
            retValue ret = new retValue();
            string res = dal.insert(Name, address, OrgCode, parentID, level);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true;
                ret.data = "保存成功";
            }
            else
            {
                ret.result = false;
                ret.reason = res;
            }
            return ret;
        }

        public retValue update( int ID,string Name, string OrgCode, string address)
        {
            retValue ret = new retValue();
            string res = dal.update(ID, Name, OrgCode, address);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true;
                ret.data = "保存成功";
            }
            else
            {
                ret.result = false;
                ret.reason = res;
            }
            return ret;
        }

        public retValue DeleteByPK(string IDs)
        {
            retValue ret = new retValue();
            string res = dal.delete(IDs);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true; ret.data = "删除成功";
            }
            else
            {
                ret.result = false; ret.reason = res;
            }
            return ret;
        }
    }
}
