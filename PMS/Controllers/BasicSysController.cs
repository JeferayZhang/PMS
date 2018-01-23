using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using BLL;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace PMS.Controllers
{
    public class BasicSysController : Controller
    {
        //
        // GET: /BasicSys/

        
        #region 用户管理
        /// <summary>
        /// 用户信息管理
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfo()
        {
            return View();
        }

        #region 查询
        [HttpPost]
        public JsonResult UserInfos(string str)
        {
            BLL.UserBLL _UserBLL = new UserBLL();
            retValue ret = new retValue();

            JObject o = null;

            string content = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);

                string test1 = o["test1"]._ToStrTrim();
                string test2 = o["test2"]._ToStrTrim();
                string UserNo = o["UserNo"]._ToStrTrim();
                string NAME = o["NAME"]._ToStrTrim();
                string Sex = o["Sex"]._ToStrTrim();
                string Role = o["Role"]._ToStrTrim();
                string IDCard = o["IDCard"]._ToStrTrim();
                string Province = o["Province"]._ToStrTrim();
                string CompanyCity = o["CompanyCity"]._ToStrTrim();
                string CompanyUnderCity = o["CompanyUnderCity"]._ToStrTrim();
                string CompanyUnderArea = o["CompanyUnderArea"]._ToStrTrim();
                string State = o["State"]._ToStrTrim();
                string OrgID = CompanyUnderArea == "" ? (CompanyUnderCity == "" ?
                    (CompanyCity == "" ?
                    Province :
                    CompanyCity) :
                    CompanyUnderCity) :
                    CompanyUnderArea;
                ret = _UserBLL.GetUser(UserNo, NAME, Sex, Role,
                    OrgID, IDCard, State, test1, test2);
            }
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult UserInfo_Deletes(string str)
        {
            BLL.UserBLL _UserBLL = new UserBLL();
            retValue ret = new retValue();

            string content = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {

                ret = _UserBLL.DeleteByPK(str);
            }
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 注销
        [HttpPost]
        public JsonResult UserInfo_Update(string str)
        {
            BLL.UserBLL _UserBLL = new UserBLL();
            retValue ret = new retValue();

            string content = string.Empty;
            int count = 0;
            if (!string.IsNullOrEmpty(str))
            {
                string[] ids = str.Split(',');

                foreach (string item in ids)
                {
                    ret = _UserBLL.UpdateByPK(item._ToInt32(), "", "", "", "", "", "", "1", "");
                    if (ret.result)
                    {
                        count++;
                    }
                }
            }
            if (count > 0)
            {
                ret.result = true;
                ret.data = "注销成功";
            }
            else
            {
                ret.result = false;
                ret.reason = "注销失败";
            }
            content = ret.toJson();
            var js = JsonConvert.SerializeObject(ret);
            return Json(js, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 加载用户编辑页面
        public ActionResult UserInfo_AddEdit()
        {
            int addeditcode = Request["addeditcode"]._ToInt32();
            if (addeditcode > 0)
            {
                List<retValue> resultList = new List<retValue>();
                retValue ret = new retValue();
                BLL.UserBLL _UserBLL = new UserBLL();
                ret = _UserBLL.GetUserByPK(addeditcode);
                resultList.Add(ret);
                ViewData.Model = resultList;
            }
            else
            {
                ViewData.Model = null;
            }
            return View();
        } 
        #endregion

        #region 保存用户信息
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserInfo_AddEdits(string str)
        {
            BLL.UserBLL _UserBLL = new UserBLL();
            JObject o = null;

            string content = string.Empty;
            retValue ret = new retValue();
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);

                string ID = o["ID"]._ToStrTrim();
                string MGUID = o["MGUID"]._ToStrTrim();
                string USERNO = o["USERNO"]._ToStrTrim();
                string NAME = o["NAME"]._ToStrTrim();
                string Sex = o["Sex"]._ToStrTrim();
                string Role = o["Role"]._ToStrTrim();
                string Password = o["Password"]._ToStrTrim();
                string IDCard = o["IDCard"]._ToStrTrim();
                string PhoneNumber = o["PhoneNumber"]._ToStrTrim();
                string Email = o["Email"]._ToStrTrim();
                string Address = o["Address"]._ToStrTrim();
                string Province = o["Province"]._ToStrTrim();
                string CompanyCity = o["CompanyCity"]._ToStrTrim();
                string CompanyUnderCity = o["CompanyUnderCity"]._ToStrTrim();
                string CompanyUnderArea = o["CompanyUnderArea"]._ToStrTrim();
                string State = o["State"]._ToStrTrim();
                string OrgID = CompanyUnderArea == "" ? (CompanyUnderCity == "" ?
                    (CompanyCity == "" ?
                    Province :
                    CompanyCity) :
                    CompanyUnderCity) :
                    CompanyUnderArea;
                //新增
                if (string.IsNullOrEmpty(ID))
                {
                    ret = _UserBLL.Insert(USERNO, NAME, Sex, Role, OrgID, IDCard, Password, PhoneNumber, Address, Email, "");
                }
                //更新
                else
                {
                    ret = _UserBLL.UpdateByPK(ID._ToInt32(), USERNO, NAME, Sex, Role, OrgID, IDCard, State, MGUID);
                }
            }
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #endregion

        #region 文献管理
        public ActionResult DocInfo()
        {
            return View();
        }

        #region 根据条件查询报刊信息
        /// <summary>
        /// 获取报刊信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DocInfos(string str)
        {
            BLL.DocBLL _DocBLL = new DocBLL();
            retValue ret = new retValue();

            JObject o = null;

            string content = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);

                string test1 = o["test1"]._ToStrTrim();
                string test2 = o["test2"]._ToStrTrim();
                string Type = o["Type"]._ToStrTrim();
                string Name = o["Name"]._ToStrTrim();
                string ISSN = o["ISSN"]._ToStrTrim();
                string PublishArea = o["PublishArea"]._ToStrTrim();
                ret = _DocBLL.GetDoc("", Type, Name, ISSN, PublishArea, "", "", test1, test2);
            }
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAllDocInfo(string str)
        {
            BLL.DocBLL _BLL = new DocBLL();
            retValue ret = new retValue();
            ret = _BLL.GetDoc(str, "", "", "", "", "", "", "", "");
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 初始化子页面(Get)
        /// <summary>
        /// 初始化子页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DocInfo_AddEdit()
        {
            int addeditcode = Request["addeditcode"]._ToInt32();
            if (addeditcode > 0)
            {
                List<retValue> resultList = new List<retValue>();
                retValue ret = new retValue();
                BLL.DocBLL _DocBLL = new DocBLL();
                ret = _DocBLL.GetDoc(addeditcode._ToStr(), "", "", "", "", "", "", "", "");
                resultList.Add(ret);
                ViewData.Model = resultList;
            }
            else
            {
                ViewData.Model = null;
            }
            return View();
        }  
        #endregion

        #region 子页面新增或者修改
        /// <summary>
        /// 子页面新增或者修改
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DocInfo_AddEdits(string str)
        {
            BLL.DocBLL _DocBLL = new DocBLL();
            JObject o = null;

            string content = string.Empty;
            retValue ret = new retValue();
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);

                string ID = o["ID"]._ToStrTrim();
                string NGUID = o["NGUID"]._ToStrTrim();
                string Type = o["Type"]._ToStrTrim();
                string Name = o["Name"]._ToStrTrim();
                string ISSN = o["ISSN"]._ToStrTrim();
                string PublishArea = o["PublishArea"]._ToStrTrim();
                string Publisher = o["Publisher"]._ToStrTrim();
                string Price = o["Price"]._ToStrTrim();
                string PL = o["PL"]._ToStrTrim();
                string BKDH = o["BKDH"]._ToStrTrim();
                //新增
                if (string.IsNullOrEmpty(ID))
                {
                    ret = _DocBLL.Insert(Name, ISSN, Type, PublishArea, Publisher, Price, PL, BKDH, "");
                }
                //更新
                else
                {
                    ret = _DocBLL.UpdateByPK(ID._ToInt32(),Name,ISSN,Type,PublishArea,Publisher,Price,PL,BKDH,NGUID);
                }
            }
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        [HttpPost]
        public JsonResult DocInfo_Delete(string str)
        {
            BLL.DocBLL _DocBLL = new DocBLL();
            retValue ret = new retValue();

            string content = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {

                ret = _DocBLL.DeleteByPK(str);
            }
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 机构管理
        public ActionResult OrgInfo()
        {
            return View();
        }

        #region 查询
        [HttpPost]
        public JsonResult OrgInfos(string str)
        {
            BLL.OrgInfoBLL _BLL = new OrgInfoBLL();
            retValue ret = new retValue();
            string content = string.Empty;

            ret = _BLL.GetOrgByParentID(str._ToInt32());
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 加载机构新增修改页面
        public ActionResult OrgInfo_AddEdit()
        {
            string ID = Request["ID"]._ToStrTrim();
            string ParentID = Request["ParentID"]._ToStrTrim();
            List<retValue> resultList = new List<retValue>();
            //如果修改则传ID,新增则传上级ID
            if (!string.IsNullOrEmpty(ID))
            {
                retValue ret = new retValue();
                ret.result = true;
                ret.data = "修改";
                resultList.Add(ret);
                BLL.OrgInfoBLL _OrgInfoBLL = new OrgInfoBLL();
                ret = _OrgInfoBLL.GetOrgByPK(ID._ToInt32());
                resultList.Add(ret);
                ViewData.Model = resultList;
            }
            else if (!string.IsNullOrEmpty(ParentID))
            {
                retValue ret = new retValue();
                ret.result = false;
                ret.data = ParentID;
                resultList.Add(ret);
                ViewData.Model = resultList;
            }
            else
            {
                ViewData.Model = null;
            }
            return View();
        } 
        #endregion

        #region 机构新增修改
        [HttpPost]
        public ActionResult OrgInfo_AddEdits(string str)
        {
            BLL.OrgInfoBLL _BLL = new OrgInfoBLL();
            JObject o = null;

            string content = string.Empty;
            retValue ret = new retValue();
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);

                string ID = o["ID"]._ToStrTrim();
                string NAME = o["Name"]._ToStrTrim();
                string Address = o["Address"]._ToStrTrim();
                string parentID = o["ParentID"]._ToStrTrim();
                //新增
                if (string.IsNullOrEmpty(ID))
                {
                    ret = _BLL.insert(NAME, Address, parentID._ToInt32());
                }
                //更新
                else
                {
                    ret = _BLL.update(ID._ToInt32(), NAME, Address);
                }
            }
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 删除机构
        [HttpPost]
        public JsonResult OrgInfo_Deletes(string str)
        {
            BLL.OrgInfoBLL _BLL = new OrgInfoBLL();
            retValue ret = new retValue();

            string content = string.Empty;

            if (!string.IsNullOrEmpty(str))
            {
                ret = _BLL.DeleteByPK(str);
            }
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}
