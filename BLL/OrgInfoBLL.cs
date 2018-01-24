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

        public retValue GetOrgByParentID(int id)
        {
            retValue ret = new retValue();
            DataTable dt = dal.GetOrgByParentID(id);
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

        public retValue insert(string Name, string address, string OrgCode, int parentID = 0) 
        {
            retValue ret = new retValue();
            string res = dal.insert(Name, address, OrgCode, parentID);
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
