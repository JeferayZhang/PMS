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
    public class CostBLL
    {
        CostDAL dal = new CostDAL();

        #region 查询缴费记录
        /// <summary>
        /// 查询缴费记录
        /// </summary>
        /// <param name="id">缴费单据流水号</param>
        /// <param name="OrderNo">单位编号</param>
        /// <param name="unitname">单位名称</param>
        /// <param name="dt1">录入时间</param>
        /// <param name="dt2">录入时间</param>
        /// <param name="pagesize">每页显示数量</param>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        public PageModel GetCostRecords(int id, int state, int orderid, string OrderNo, string unitname, int pagesize, int pageindex)
        {
            PageModel pg = new PageModel();
            try
            {
                DataTable dt = dal.GetCostRecords(id, state,orderid, OrderNo, unitname, pagesize, pageindex);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    pg.code = 0;
                    pg.msg = "";
                    pg.count = dal.GetCount(id, orderid, OrderNo, unitname);
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
        #endregion

        #region 更新
        public retValue UpdateByPK(int id, decimal money,decimal moneypayed, int userid)
        {
            retValue ret = new retValue();
            string res = dal.update(id, money, moneypayed, userid);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true; ret.data = "保存成功";
            }
            else
            {
                ret.result = false; ret.reason = res;
            }

            return ret;
        } 
        #endregion

        #region 根据主键删除数据
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="IDs">主键,多个用英文逗号分隔</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public retValue DeleteByPK(string IDs)
        {
            retValue ret = new retValue();
            string res = dal.DeleteByPK(IDs);
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
        #endregion
    }
}
