using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Common;
using System.Data.SqlClient;

namespace BLL
{
    public class OrderInfoBLL
    {
        OrderInfoDAL dal = new OrderInfoDAL();
        SubscriberDAL _SubscriberDAL = new SubscriberDAL();
        /// <summary>
        /// 带分页查询
        /// </summary>
        /// <param name="ID">订购主键</param>
        /// <param name="BKDH">报刊代号</param>
        /// <param name="OrderNo">单位编号</param>
        /// <param name="unitname">单位名称</param>
        /// <param name="dt1">录入时间</param>
        /// <param name="dt2">录入时间</param>
        /// <param name="pageLimit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public PageModel GetOrderInfo(int ID, string BKDH, string OrderNo, string unitname, string dt1, string dt2, string orgid, string chooseorg, string OrderState, string CostState, int pageLimit = 1, int pageIndex = 0, bool istj = false)
        {
            PageModel pg = new PageModel();
            try
            {
                DataTable dt = dal.GetOrderInfo(ID, BKDH, OrderNo, unitname, dt1, dt2,
                    pageLimit, pageIndex, orgid, chooseorg, OrderState, CostState);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    DataTable tj = dal.GetCount(ID, BKDH, OrderNo, unitname, dt1, dt2, orgid,
                        chooseorg, OrderState, CostState);
                    pg.code = 0;
                    pg.msg = "";
                    pg.count = tj.Rows[0]["num"]._ToInt32();
                    pg.data = dt;
                    pg.msg = "总订购月数:" + tj.Rows[0]["OrderMonths"]._ToInt32() + ",总订购份数:" + tj.Rows[0]["OrderNum"]._ToInt32() + ",总价:" + tj.Rows[0]["Money"]._ToInt32() + ",已缴费用:" + tj.Rows[0]["MoneyPayed"]._ToInt32();
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

        public PageModel GetCount(int ID, string BKDH, string OrderNo, string unitname, string dt1, string dt2, string orgid, string chooseorg, string OrderState, string CostState)
        {
            PageModel pg = new PageModel();
            try
            {
                DataTable tj = dal.GetCount(ID, BKDH, OrderNo, unitname, dt1, dt2, orgid,
                        chooseorg, OrderState, CostState);
                if (tj.Rows.Count > 0 && tj != null)
                {
                    pg.code = 0;
                    pg.msg = "总订购月数:" + tj.Rows[0]["OrderMonths"]._ToInt32() + ",总订购份数:" + tj.Rows[0]["OrderNum"]._ToInt32() + ",总价:" + tj.Rows[0]["Money"]._ToDecimal() + ",已缴费用:" + tj.Rows[0]["MoneyPayed"]._ToDecimal();
                }
                else
                {
                    pg.code = 1;
                    pg.msg = "未查询到数据";
                    pg.count = 0;
                    pg.data = tj;
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
        /// 根据订购流水号获取信息
        /// </summary>
        /// <param name="id">订购流水号</param>
        /// <returns></returns>
        public PageModel GetOrderInfoByPK(int ID)
        {
            PageModel pg = new PageModel();
            try
            {
                DataTable dt = dal.getByPK(ID);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    pg.code = 0;
                    pg.msg = "";
                    pg.count = 1;
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
        public retValue UpdateByPK(int ID, int months, int ordernum, string bkdh, int PersonID, int ModifyUser, int PosterID, string OrderDate,string guid = "")
        {
            retValue ret = new retValue();
            string res = dal.update(ID, ordernum, months, guid, bkdh, PersonID, ModifyUser, PosterID, OrderDate);
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


        public retValue TD(string ID, int ModifyUser, int type = 0, int months = 0, int OrderNum = 0)
        {
            retValue ret = new retValue();
            string res = dal.TD(ID, ModifyUser, type, months, OrderNum);
            if (string.IsNullOrEmpty(res))
            {
                ret.result = true; ret.data = "退订成功";
            }
            else
            {
                ret.result = false; ret.reason = res;
            }

            return ret;
        }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docid">报刊id</param>
        /// <param name="orderpeopleid">订户id</param>
        /// <param name="ordernum">订购数</param>
        /// <param name="ordermonths">订购多少月</param>
        /// <param name="orderDate">订购起始日期</param>
        /// <param name="inuser">录入员</param>
        /// <param name="posterid">投递员,相当于收订人</param>
        /// <returns></returns>
        public retValue Insert(SqlTransaction tran, string BKDH, int orderpeopleid, int ordernum, int ordermonths, string orderDate, string inuser, int posterid, decimal FullPrice = 0)
        {
            retValue ret = new retValue();
            string res = dal.Insert(BKDH, orderpeopleid, ordernum, ordermonths, orderDate, inuser, posterid, tran, FullPrice);
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

        /// <summary>
        /// 新增时,如果不存在订户,则新增订户
        /// </summary>
        /// <param name="docid">报刊id</param>
        /// <param name="ordernum">订购数</param>
        /// <param name="ordermonths">订购多少月</param>
        /// <param name="orderDate">订购起始日期</param>
        /// <param name="inuser">录入员</param>
        /// <param name="posterid">投递员,相当于收订人</param>
        /// <param name="OrderNo">订户单位编号</param>
        /// <param name="unitname">单位名称</param>
        /// <param name="address">单位地址</param>
        /// <param name="name">负责人名称</param>
        /// <param name="phone">电话</param>
        /// <param name="orgID">所属网点</param>
        /// <returns></returns>
        public retValue Insert(string BKDH, int ordernum, int ordermonths, string orderDate,
            string inuser, int posterid, string OrderNo,string unitname,
            string address,string name,string phone,string orgID,SqlTransaction tran,string oporgid) 
        {
            DataTable dt= _SubscriberDAL.getbyNo(OrderNo);
            retValue ret = new retValue();
            int orderpeopleid = 0;
            //如果根据机构编号找到了机构,那么就判断权限
            if (dt.Rows.Count>0)
            {
                orderpeopleid = dt.Rows[0]["ID"]._ToInt32();
                OrgInfoDAL orgInfoDAL = new OrgInfoDAL();
                string all = orgInfoDAL.getChilds(oporgid, orgID);
                if (all=="0")
                {
                    ret.result = false;
                    ret.reason = "您没有权限添加"+ OrderNo+"的订购,可能是因为该订户不属于您的机构";
                    return ret;
                }
            }
            else
            {
                ret = _SubscriberDAL.Insert(OrderNo, unitname, name, phone, address, orgID, inuser._ToInt32());
                if (ret.result)
                {
                    orderpeopleid = ret.data._ToInt32();
                }
                else
                {
                    return ret;
                }
            }
            ret = Insert(tran,BKDH, orderpeopleid, ordernum, ordermonths, orderDate, inuser, posterid);
            return ret;
        }
        
    }
}
