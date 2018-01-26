using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Data.SqlClient;
using DAL;

namespace BLL
{
    public class DistributeBLL
    {
        DAL.DistributeDAL _dal = new DistributeDAL();
        /// <summary>
        /// 根据机构统计
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public retValue GetTableByOrg(string orgid, string userorg)
        {
            retValue ret = new retValue();
            DataTable dt = _dal.GetTableByOrg(orgid,userorg);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.result = true;
                ret.data = dt;
            }
            else
            {
                ret.result = false;
                ret.reason = "未能查询到数据";
            }
            return ret;
        }

        /// <summary>
        /// 根据报纸统计
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public retValue GetTableByBK(string BKDH, string userorg, string chooseorg)
        {
            retValue ret = new retValue();
            DataTable dt = _dal.GetTableByBK(BKDH,userorg,chooseorg);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.result = true;
                ret.data = dt;
            }
            else
            {
                ret.result = false;
                ret.reason = "未能查询到数据";
            }
            return ret;
        }

        public retValue insertLog(string orderids, string nianjuanqi,int userid)
        {
            retValue ret = new retValue();
            string res = _dal.insertLog(orderids, nianjuanqi, userid);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true;
                ret.data = "操作成功";
            }
            else
            {
                ret.result = false;
                ret.reason = res;
            }
            return ret;
        }
    }
}
