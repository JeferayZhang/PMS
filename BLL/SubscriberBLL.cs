using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Data.SqlClient;
using System.Data;
using DAL;

namespace BLL
{
    public class SubscriberBLL
    {
        SubscriberDAL dal = new SubscriberDAL();

        #region 获取订户
        /// <summary>
        /// 获取订户
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="OrderNo">公司代码,一般是纳税号</param>
        /// <param name="UnitName">单位名称</param>
        /// <param name="name">负责人名称</param>
        /// <param name="OrgID">所属网点</param>
        /// <param name="dt1">录入时间</param>
        /// <param name="dt2">录入时间</param>
        /// <returns>data为DataTable</returns>
        public PageModel GetSubscriber(int ID, string OrderNo, string UnitName, string name, int OrgID,
            string dt1, string dt2, int pageLimit = 1, int pageIndex = 0)
        {
            PageModel pg = new PageModel();
            try
            {
                DataTable dt = dal.GetSubscriber(ID, OrderNo, UnitName, name, OrgID, dt1, dt2, pageLimit, pageIndex);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    pg.code = 0;
                    pg.msg = "";
                    pg.count = dal.GetCount(ID, OrderNo, UnitName, name, OrgID, dt1, dt2);
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

        #region 根据主键更新数据
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="OrderNo">公司编号</param>
        /// <param name="UnitName">单位名称</param>
        /// <param name="name">负责人名称</param>
        /// <param name="phone">联系方式</param>
        /// <param name="address">地址</param>
        /// <param name="OrgID">所属网点</param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public retValue UpdateByPK(int ID, string OrderNo, string UnitName, string name, string phone,
            string address, int OrgID, string guid = "")
        {
            retValue ret = new retValue();
            string res = dal.UpdateByPK(ID, OrderNo, UnitName, name, phone, address, OrgID, guid);
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

        #region 插入订户
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="OrderNo">公司编号</param>
        /// <param name="UnitName">单位名称</param>
        /// <param name="name">负责人名称</param>
        /// <param name="phone">联系方式</param>
        /// <param name="address">地址</param>
        /// <param name="OrgID">所属网点</param>
        /// <param name="InUser">录入人</param>
        /// <returns>成功返回空值,否则返回提示</returns>
        public retValue Insert(string OrderNo, string UnitName, string name, string phone,
            string address, int OrgID, int InUser)
        {
            retValue ret = new retValue();
            ret = dal.Insert(OrderNo, UnitName, name, phone, address, OrgID, InUser);
            return ret;
        }
        #endregion
    }
}
