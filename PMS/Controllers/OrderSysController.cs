using BLL;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using System.Data.SqlClient;
using System.IO;

namespace PMS.Controllers
{
    public class OrderSysController : Controller
    {
        //
        // GET: /OrderSys/

        #region 订单信息管理
        /// <summary>
        /// 订单信息管理
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderInfo()
        {
            return View();
        }
        #region 查询订单
        [HttpGet]
        public ActionResult OrderInfos(int page, int limit, string test1, string test2,
            string Province, string CompanyCity, string CompanyUnderCity,
            string CompanyUnderArea, string OrderNo, string UnitName, string BKDH, string OrderState, string CostState)
        {
            PageModel ret = new PageModel();
            BLL.OrderInfoBLL _BLL = new OrderInfoBLL();
            PMS.Models.UserModel userModel = Session["UserModel"] as PMS.Models.UserModel;
            string OrgID = "";
            if (Province._ToInt32() > 0)
            {
                OrgID = Province;
            }
            if (CompanyCity._ToInt32() > 0)
            {
                OrgID = CompanyCity;
            }
            if (CompanyUnderCity._ToInt32() > 0)
            {
                OrgID = CompanyUnderCity;
            }
            if (CompanyUnderArea._ToInt32() > 0)
            {
                OrgID = CompanyUnderArea;
            }
            PageModel pg = _BLL.GetOrderInfo(0, BKDH, OrderNo, UnitName, test1, test2, 
                userModel.OrgID._ToStr(),OrgID,OrderState,CostState, limit, page);
            var js = JsonConvert.SerializeObject(pg);
            return Content(js);
        }

        [HttpPost]
        public ActionResult Order_getCounts(string str)
        {
            retValue ret = new retValue();
            if (!authorize.checkFilterContext())
            {
                ret.result = true;
                ret.data = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            PMS.Models.UserModel userModel = Session["UserModel"] as PMS.Models.UserModel;
            int userid = userModel._ID;
            BLL.OrderInfoBLL _BLL = new OrderInfoBLL();
            JObject o = null;
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);

                string test1 = o["test1"]._ToStrTrim();
                string test2 = o["test2"]._ToStrTrim();
                string Province = o["Province"]._ToStrTrim();
                string CompanyCity = o["CompanyCity"]._ToStrTrim();
                string CompanyUnderCity = o["CompanyUnderCity"]._ToStrTrim();
                string CompanyUnderArea = o["CompanyUnderArea"]._ToStrTrim();
                string OrderNo = o["OrderNo"]._ToStrTrim();
                string UnitName = o["UnitName"]._ToStrTrim();
                string BKDH = o["BKDH"]._ToStrTrim();
                string OrderState = o["OrderState"]._ToStrTrim();
                string CostState = o["CostState"]._ToStrTrim();
                string OrgID = "";
                if (Province._ToInt32() > 0)
                {
                    OrgID = Province;
                }
                if (CompanyCity._ToInt32() > 0)
                {
                    OrgID = CompanyCity;
                }
                if (CompanyUnderCity._ToInt32() > 0)
                {
                    OrgID = CompanyUnderCity;
                }
                if (CompanyUnderArea._ToInt32() > 0)
                {
                    OrgID = CompanyUnderArea;
                }
                PageModel pg = _BLL.GetCount(0, BKDH, OrderNo, UnitName, test1, test2,
                userModel.OrgID._ToStr(), OrgID, OrderState, CostState);
                ret.result = true;
                ret.data = pg.msg;
            }
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 初始化订单新增修改界面
        /// <summary>
        /// 初始化订单新增修改界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Order_AddEdit()
        {
            PageModel ret = new PageModel();
            if (!authorize.checkFilterContext())
            {
                ret.code = 2;
                ret.msg = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            int addeditcode = Request["addeditcode"]._ToInt32();
            string from = Request["from"]._ToStr();
            if (addeditcode > 0)
            {
                BLL.OrderInfoBLL _BLL = new BLL.OrderInfoBLL();
                List<PageModel> lis = new List<PageModel>();
                PageModel pg = _BLL.GetOrderInfoByPK(addeditcode);
                lis.Add(pg);
                pg = new PageModel();
                pg.code = 0;
                pg.data = from;
                lis.Add(pg);
                ViewData.Model = lis;
            }
            else
            {
                ViewData.Model = null;
            }
            return View();
        }
        #endregion

        #region 保存订单的修改,同时修改缴费记录
        [HttpPost]
        public ActionResult Order_AddEdits(string str)
        {
            retValue ret = new retValue();
            if (!authorize.checkFilterContext())
            {
                ret.result = true;
                ret.data = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            PMS.Models.UserModel userModel = Session["UserModel"] as PMS.Models.UserModel;
            int userid = userModel._ID;
            BLL.OrderInfoBLL _BLL = new OrderInfoBLL();
            JObject o = null;            
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);

                string ID = o["ID"]._ToStrTrim();
                string NGUID = o["NGUID"]._ToStrTrim();
                string CostID = o["CostID"]._ToStrTrim();
                string Month = o["Month"]._ToStrTrim();
                string OrderNum = o["OrderNumber"]._ToStrTrim();
                string PersonID = o["OrderPeople"]._ToStrTrim();
                string Poster = o["Poster"]._ToStrTrim();
                string BKDH = o["NewspaperName"]._ToStrTrim();
                string FullPrice = o["FullPrice"]._ToStrTrim();
                string MoneyPayed = o["Pay"]._ToStrTrim();
                string test1 = o["test1"]._ToStrTrim();
                if (string.IsNullOrEmpty(ID))
                {
                    SqlHelp dbhelper = new SqlHelp();
                    SqlConnection conn = new SqlConnection(dbhelper.SqlConnectionString);
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            ret = _BLL.Insert(tran, BKDH, PersonID._ToInt32(), OrderNum._ToInt32(), Month._ToInt32(),
                                test1, userid._ToStr(), Poster._ToInt32(), FullPrice._ToDecimal());
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            ret.result = false;
                            ret.reason = ex.Message;
                            tran.Rollback();
                            throw;
                        }
                    }
                }
                else
                {
                    ret = _BLL.UpdateByPK(ID._ToInt32(), Month._ToInt32(), OrderNum._ToInt32(), BKDH,
                        PersonID._ToInt32(), userid, Poster._ToInt32(),test1, NGUID);
                    if (ret.result)
                    {
                        CostBLL costBLL = new CostBLL();
                        costBLL.UpdateByPK(CostID._ToInt32(), FullPrice._ToDecimal(), MoneyPayed._ToDecimal(), userid);
                    }
                }                
            }
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除订购信息
        [HttpPost]
        public JsonResult Order_Deletes(string str)
        {
            BLL.OrderInfoBLL _BLL = new OrderInfoBLL();
            retValue ret = new retValue();
            if (!authorize.checkFilterContext())
            {
                ret.result = true;
                ret.data = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }


            string content = string.Empty;
            DataTable dt = str.ToTable();
            string ids = "";
            string costs = "";
            foreach (DataRow item in dt.Rows)
            {
                if (!string.IsNullOrEmpty(item["ID"]._ToStrTrim()))
                {
                    ids += item["ID"]._ToStrTrim() + ",";
                    costs += item["CostID"]._ToStrTrim() + ",";
                }
            }
            ret = _BLL.DeleteByPK(ids.Remove(ids.Length - 1));
            if (ret.result)
            {
                CostBLL costBLL = new CostBLL();
                costBLL.DeleteByPK(costs);
            }
            content = ret.toJson();
            var js = JsonConvert.SerializeObject(ret);
            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 退订操作
        [HttpPost]
        public JsonResult Order_TD(string str)
        {
            BLL.OrderInfoBLL _BLL = new OrderInfoBLL();
            retValue ret = new retValue();
            if (!authorize.checkFilterContext())
            {
                ret.result = true;
                ret.data = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            JObject o = null;
            PMS.Models.UserModel userModel = Session["UserModel"] as PMS.Models.UserModel;
            int userid = userModel._ID;
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);
                string ids = o["ID"]._ToStrTrim();
                int Month = o["Month"]._ToInt32();
                int OrderNum = o["OrderNum"]._ToInt32();
                int Group_Type = o["Group_Type"]._ToInt32();
                //1全退,2部分退
                if (Group_Type == 1)
                {
                    Month = 0;
                    OrderNum = 0;
                }
                ret = _BLL.TD(ids.Remove(ids.Length - 1), userid, Group_Type, Month, OrderNum);
            }
            var js = JsonConvert.SerializeObject(ret);
            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 批量导入
        //// <summary>
        /// 这里保存上传的文件到服务器上面
        /// </summary>
        /// <param name="file">上传文件实例,名称应该和前台页面上的上传控件name属性一致</param>
        /// <returns></returns>
        public ActionResult Order_PL(HttpPostedFileBase file)
        {
            retValue ret = new retValue();
            if (!authorize.checkFilterContext())
            {
                ret.result = true;
                ret.data = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            ret.result = true;
            if (file == null)
            {
                ret.result = false;
                ret.reason = "请选择上传文件!";
            }
            else
            {
                PMS.Models.UserModel userModel = Session["UserModel"] as PMS.Models.UserModel;
                int userid = userModel._ID;
                Random r = new Random();
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(10000) + "." + fileExt;
                var filepath = System.IO.Path.Combine(Server.MapPath("~/Images"), filename);
                var savepath = "/Images/" + filename;
                file.SaveAs(filepath);
                BLL.OrderInfoBLL _bll = new OrderInfoBLL();
                BLL.OrgInfoBLL _Org = new OrgInfoBLL();
                DataTable dt = OpenFile(filepath);
                //删除文件
                FileInfo fileInfo = new FileInfo(filepath);
                fileInfo.Delete();
                if (dt == null || dt.Rows.Count == 0)
                {
                    ret.result = false;
                    ret.reason = "Excel中没有数据";
                }
                else
                {
                    int count = 0;
                    SqlHelp dbhelper = new SqlHelp();
                    SqlConnection conn = new SqlConnection(dbhelper.SqlConnectionString);
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                int orgid = _Org.GetIDByName(item["OrgName"]._ToStrTrim());
                                if (orgid == 0)
                                {
                                    ret.result = false;
                                    ret.reason ="第"+ (dt.Rows.IndexOf(item)+1)+"行数据机构填写不正确";
                                    break;
                                }
                                else if (string.IsNullOrEmpty(item["BKDH"]._ToStrTrim()))
                                {
                                    ret.result = false;
                                    ret.reason = "第" + (dt.Rows.IndexOf(item) + 1) + "行数据报刊代号不能为空"; break;
                                }
                                else if (item["OrderNum"]._ToInt32() <= 0)
                                {
                                    ret.result = false;
                                    ret.reason = "第" + (dt.Rows.IndexOf(item) + 1) + "行数据订购数必须大于0"; break;
                                }
                                else if (item["OrderMonths"]._ToInt32() <= 0)
                                {
                                    ret.result = false;
                                    ret.reason = "第" + (dt.Rows.IndexOf(item) + 1) + "行数据订购月数必须大于0"; break;
                                }
                                else if (string.IsNullOrEmpty(item["Indate"]._ToStrTrim()))
                                {
                                    ret.result = false;
                                    ret.reason = "第" + (dt.Rows.IndexOf(item) + 1) + "行数据订购起始日期不能为空"; break;
                                }
                                else if (string.IsNullOrEmpty(item["OrderNo"]._ToStrTrim()))
                                {
                                    ret.result = false;
                                    ret.reason = "第" + (dt.Rows.IndexOf(item) + 1) + "行数据订户单位编号不能为空"; break;
                                }
                                else
                                {
                                    ret= _bll.Insert(item["BKDH"]._ToStrTrim(), item["OrderNum"]._ToInt32(), item["OrderMonths"]._ToInt32(), item["Indate"]._ToStrTrim(), userid._ToStr(),
                                        0, item["OrderNo"]._ToStrTrim(), item["UnitName"]._ToStrTrim(), item["Address"]._ToStrTrim(), item["Name"]._ToStrTrim(), item["Phone"]._ToStrTrim(), orgid._ToStr(), tran, userid._ToStr());
                                    count++;
                                    if (!ret.result)
                                    {
                                        tran.Rollback();
                                        return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
                                    }                              
                                }
                            }
                            if (!ret.result)
                            {
                                tran.Rollback();
                            }
                            else
                            {
                                tran.Commit();
                                ret.result = true;
                                ret.data = "导入成功";
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            ret.result = false;
                            ret.reason = ex.Message;
                            tran.Rollback();
                        }
                    }
                    conn.Close();
                }
            }
            var js = JsonConvert.SerializeObject(ret);
            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 读取Excel中的数据
        /// <summary>
        /// 读取Excel中的数据
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private DataTable OpenFile(string filepath)
        {
            DataTable dt = new DataTable();
            object[] ret = CommonHelper.LoadExcelToDataTable(filepath);
            if (ret[0]._ToInt32() == 1)
                dt = ret[1] as DataTable;

            if (dt == null || dt.Rows.Count == 0)
            {
                return dt;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataColumn cl in dt.Columns)
                {
                    if (cl.ColumnName == "报刊代号")
                    {
                        cl.ColumnName = "BKDH";
                        continue;
                    }
                    if (cl.ColumnName == "订购数")
                    {
                        cl.ColumnName = "OrderNum";
                        continue;
                    }
                    if (cl.ColumnName == "订购月数")
                    {
                        cl.ColumnName = "OrderMonths";
                        continue;
                    }
                    if (cl.ColumnName == "订购起始日期")
                    {
                        cl.ColumnName = "Indate";
                        continue;
                    }
                    if (cl.ColumnName == "订户单位编号")
                    {
                        cl.ColumnName = "OrderNo";
                        continue;
                    }
                    if (cl.ColumnName == "单位名称")
                    {
                        cl.ColumnName = "UnitName";
                        continue;
                    }
                    if (cl.ColumnName == "单位地址")
                    {
                        cl.ColumnName = "Address";
                        continue;
                    }
                    if (cl.ColumnName == "负责人名称")
                    {
                        cl.ColumnName = "Name";
                        continue;
                    }
                    if (cl.ColumnName == "电话")
                    {
                        cl.ColumnName = "Phone";
                        continue;
                    }
                    if (cl.ColumnName == "所属网点")
                    {
                        cl.ColumnName = "OrgName";
                        continue;
                    }
                }
            }
            return dt;
        }
        #endregion

        #region 退订界面
        /// <summary>
        /// 续订
        /// </summary>
        /// <returns></returns>
        public ActionResult Order_Continue()
        {
            PageModel ret = new PageModel();
            if (!authorize.checkFilterContext())
            {
                ret.code = 2;
                ret.msg = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            string addeditcode = Request["addeditcode"]._ToStr();
            if (!string.IsNullOrEmpty(addeditcode))
            {
                ViewData.Model = addeditcode;
            }
            else
            {
                ViewData.Model = null;
            }
            return View();
        }
        #endregion 
        #endregion

        #region 订户管理
        /// <summary>
        /// 订户管理
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderPeopleInfo()
        {
            return View();
        }

        #region 查询订户信息----分页
        [HttpGet]
        public ActionResult OrderPeopleInfos(int page, int limit, string test1, string test2,
            string Province, string CompanyCity, string CompanyUnderCity,
            string CompanyUnderArea, string OrderNo, string Name)
        {
            PageModel ret = new PageModel();
            if (!authorize.checkFilterContext())
            {
                ret.code = 2;
                ret.msg = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }

            BLL.SubscriberBLL _SubscriberBLL = new SubscriberBLL();

            JObject o = null;

            string content = string.Empty;
            Models.UserModel user = Session["UserModel"] as Models.UserModel;
            string OrgID = "";
            if (Province._ToInt32() > 0)
            {
                OrgID = Province;
            }
            if (CompanyCity._ToInt32()>0)
            {
                OrgID = CompanyCity;
            }
            if (CompanyUnderCity._ToInt32() > 0)
            {
                OrgID = CompanyUnderCity;
            }
            if (CompanyUnderArea._ToInt32() > 0)
            {
                OrgID = CompanyUnderArea;
            }
            PageModel pg = _SubscriberBLL.GetSubscriber(0, OrderNo, Name, "", OrgID, test1, test2, user.OrgID._ToStr(), limit, page);
            var js = JsonConvert.SerializeObject(pg);
            return Content(js);
        }
        #endregion

        #region 获取所有订户
        [HttpPost]
        public JsonResult GetAllOrderPeopleInfos(string str)
        {
            PageModel ret = new PageModel();
            if (!authorize.checkFilterContext())
            {
                ret.code = 2;
                ret.msg = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            Models.UserModel user = Session["UserModel"] as Models.UserModel;
            BLL.SubscriberBLL _SubscriberBLL = new SubscriberBLL();
            PageModel pg = _SubscriberBLL.GetSubscriber(0, "", "", "", "", "", "", user.OrgID._ToStr(), 0, 0);
            var js = JsonConvert.SerializeObject(pg);
            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 初始化订户新增修改界面
        /// <summary>
        /// 初始化订户新增修改界面
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderPeopleInfo_AddEdit()
        {
            Models.UserModel user = Session["UserModel"] as Models.UserModel;
            int addeditcode = Request["addeditcode"]._ToInt32();
            if (addeditcode > 0)
            {
                BLL.SubscriberBLL _SubscriberBLL = new SubscriberBLL();
                PageModel pg = _SubscriberBLL.GetSubscriber(addeditcode, "", "", "", "", "", "", user.OrgID._ToStr());
                ViewData.Model = pg;
            }
            else
            {
                ViewData.Model = null;
            }
            return View();
        }
        #endregion

        #region 保存订户信息
        /// <summary>
        /// 保存订户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OrderPeopleInfo_AddEdits(string str)
        {
            BLL.SubscriberBLL _SubscriberBLL = new SubscriberBLL();
            JObject o = null;

            string content = string.Empty;
            retValue ret = new retValue();
            if (!authorize.checkFilterContext())
            {
                ret.result = true;
                ret.data = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            PMS.Models.UserModel userModel = Session["UserModel"] as PMS.Models.UserModel;
            int userid = userModel._ID;
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);

                string ID = o["ID"]._ToStrTrim();
                string MGUID = o["MGUID"]._ToStrTrim();
                string OrderNo = o["OrderNo"]._ToStrTrim();
                string UnitName = o["UnitName"]._ToStrTrim();
                string Address = o["Address"]._ToStrTrim();
                string Name = o["Name"]._ToStrTrim();
                string Phone = o["Phone"]._ToStrTrim();
                string Roads = o["Roads"]._ToStrTrim();
                string State = o["State"]._ToStrTrim();
                //新增
                if (string.IsNullOrEmpty(ID))
                {
                    ret = _SubscriberBLL.Insert(OrderNo, UnitName, Name, Phone, Address, Roads, userid);
                }
                //更新
                else
                {
                    ret = _SubscriberBLL.UpdateByPK(ID._ToInt32(), OrderNo, UnitName, Name, Phone, Address, "", MGUID);
                }
            }
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult OrderPeopleInfo_Deletes(string str)
        {
            BLL.SubscriberBLL _SubscriberBLL = new SubscriberBLL();
            retValue ret = new retValue();
            if (!authorize.checkFilterContext())
            {
                ret.result = true;
                ret.data = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }

            string content = string.Empty;
            DataTable dt = str.ToTable();
            string ids = "";
            foreach (DataRow item in dt.Rows)
            {
                if (!string.IsNullOrEmpty(item["ID"]._ToStrTrim()))
                {
                    ids += item["ID"]._ToStrTrim() + ",";
                }
            }
            ret = _SubscriberBLL.DeleteByPK(ids.Remove(ids.Length - 1));
            content = ret.toJson();
            var js = JsonConvert.SerializeObject(ret);
            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion 
        #endregion

    }
}
