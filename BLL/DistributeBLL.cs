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
        /// 统计-市-县级分发
        /// </summary>
        /// <param name="BKDH">报刊代号</param>
        /// <param name="chooseorg">所选机构ID</param>
        /// <param name="userorg">用户所属机构ID</param>
        /// <param name="Group_Type">排序方式:1根据报刊名称排序.2根据机构排序</param>
        /// <param name="dt1">分发日期</param>
        /// <param name="type">如果为0,则表示分发行为;如果为1,则表示查看日志</param>
        /// <returns></returns>
        public retValue GetTable(string BKDH, string orgid, string userorg,string Group_Type,string dt1,int type=0)
        {
            retValue ret = new retValue();
            DataTable dt = _dal.GetTable(BKDH, orgid,userorg,Group_Type, dt1, type);
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
        /// 统计-县-段道级分发
        /// </summary>
        /// <param name="BKDH">报刊代号</param>
        /// <param name="chooseorg">所选机构ID</param>
        /// <param name="userorg">用户所属机构ID</param>
        /// <param name="Group_Type">排序方式:1根据报刊名称排序.2根据机构排序</param>
        /// <param name="dt1">分发日期</param>
        /// <param name="type">如果为0,则表示分发行为;如果为1,则表示查看日志</param>
        /// <returns></returns>
        public retValue GetTable2(string BKDH, string orgid, string userorg, string Group_Type, string dt1, int type = 0)
        {
            retValue ret = new retValue();
            DataTable dt = _dal.GetTable2(BKDH, orgid, userorg,Group_Type, dt1, type);
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
        /// 统计-市-县级分发历史
        /// </summary>
        /// <param name="dt1">分发日期</param>
        /// <param name="dt2">分发日期</param>
        /// <param name="userid">分发员</param>
        /// <returns></returns>
        public PageModel getlog1(string dt1, string dt2, int userorg, string userid, int limit = 0, int index = 0)
        {
            PageModel pg = new PageModel();
            try
            {
                DataTable dt = _dal.getlog1(dt1, dt2, userorg, userid, limit, index);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    pg.code = 0;
                    pg.msg = "";
                    pg.count = _dal.getlog1(dt1, dt2, userorg, userid).Rows.Count;
                    pg.data = dt;
                }
                else
                {
                    pg.code = 1;
                    pg.msg = "未查询到数据";
                    pg.count = 0;
                    pg.data = dt;
                }
            }
            catch (Exception ex)
            {
                pg.code = 1;
                pg.msg = ex.Message;
            }
            return pg;
        }

        /// <summary>
        /// 统计-县-段道级分发历史
        /// </summary>
        /// <param name="dt1">分发日期</param>
        /// <param name="dt2">分发日期</param>
        /// <param name="userid">分发员</param>
        /// <returns></returns>
        public PageModel getlog2(string dt1, string dt2, int userorg, string userid, int limit = 0, int index = 0)
        {
            PageModel pg = new PageModel();
            try
            {
                DataTable dt = _dal.getlog2(dt1, dt2, userorg, userid, limit, index);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    pg.code = 0;
                    pg.msg = "";
                    pg.count = _dal.getlog2(dt1, dt2, userorg, userid).Rows.Count;
                    pg.data = dt;
                }
                else
                {
                    pg.code = 1;
                    pg.msg = "未查询到数据";
                    pg.count = 0;
                    pg.data = dt;
                }
            }
            catch (Exception ex)
            {
                pg.code = 1;
                pg.msg = ex.Message;
            }
            return pg;
        }

        public retValue insertLog(string orderids, string nianjuanqi,int userid,int type=0)
        {
            retValue ret = new retValue();
            string res = _dal.insertLog(orderids, nianjuanqi, userid, type);
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
