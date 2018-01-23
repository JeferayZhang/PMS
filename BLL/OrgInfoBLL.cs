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
        public int GetIDByName(string name) 
        {
            return dal.GetOrgByName(name);
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

        public retValue insert(string Name, string address, int parentID = 0) 
        {
            retValue ret = new retValue();
            int res = dal.insert(Name, address, parentID);
            if (res > 0)
            {
                ret.result = true;
                ret.data = "保存成功";
            }
            else
            {
                ret.result = false;
                ret.reason = "保存失败";
            }
            return ret;
        }

        public retValue update( int ID,string Name, string address)
        {
            retValue ret = new retValue();
            int res = dal.update(ID, Name, address);
            if (res > 0)
            {
                ret.result = true;
                ret.data = "保存成功";
            }
            else
            {
                ret.result = false;
                ret.reason = "保存失败";
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
