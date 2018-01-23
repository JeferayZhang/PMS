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
        public PageModel GetOrderInfo(int ID, string BKDH, string OrderNo, string unitname, string dt1, string dt2, 
            int pageLimit = 1, int pageIndex = 0)
        {
            PageModel pg = new PageModel();
            try
            {
                DataTable dt = dal.GetOrderInfo(ID, BKDH, OrderNo, unitname, dt1, dt2, pageLimit, pageIndex);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    pg.code = 0;
                    pg.msg = "";
                    pg.count = dal.GetCount(ID, BKDH, OrderNo, unitname, dt1, dt2);
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

        public retValue UpdateByPK(int ID, int months, int ordernum = 1, string guid = "")
        {
            retValue ret = new retValue();
            string res = dal.update(ID, ordernum, months,guid);
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
        public retValue Insert(SqlTransaction tran, string BKDH, int orderpeopleid, int ordernum, int ordermonths, string orderDate, string inuser, int posterid)
        {
            retValue ret = new retValue();
            string res = dal.Insert(BKDH, orderpeopleid, ordernum, ordermonths, orderDate, inuser, posterid,tran);
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
            string address,string name,string phone,int orgID,SqlTransaction tran) 
        {
            int checkOrderPeople = _SubscriberDAL.GetCount(0, OrderNo, "", "", 0, "", "");
            int orderpeopleid = 0;
            retValue ret=new retValue();
            if (checkOrderPeople > 0)
            {
                orderpeopleid = _SubscriberDAL.GetSubscriber(0, OrderNo, "", "", 0, "", "").Rows[0]["ID"]._ToInt32();
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
