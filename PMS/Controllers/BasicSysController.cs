﻿using BLL;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PMS.App_Start;
using PMS.Models;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace PMS.Controllers
{
    /// <summary>
    /// NeedLoginFilter特性对整个控制器加了登录验证,意思就是说,在执行所有的方法之前,都会进行登录验证
    /// </summary>
    [NeedLoginFilter(Message = "Controller")]
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
        [HttpGet]
        public ActionResult UserInfos(int page, int limit, string test1, string test2,
            string Province, string CompanyCity, string CompanyUnderCity,
            string CompanyUnderArea, string UserNo, string NAME, string Role, string IDCard, string State)
        {
            PageModel _page = new PageModel();
            BLL.UserBLL _UserBLL = new UserBLL();

            UserModel user = Session["UserModel"] as UserModel;
            JObject o = null;
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
            _page = _UserBLL.GetUser(UserNo, NAME, "", Role,
                OrgID, IDCard, State, test1, test2, user.OrgID._ToStr(), limit,page);

            var js = JsonConvert.SerializeObject(_page);

            return Content(js);
        } 
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult UserInfo_Deletes(string str)
        {
            retValue ret = new retValue();

            BLL.UserBLL _UserBLL = new UserBLL();

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
            retValue ret = new retValue();

            BLL.UserBLL _UserBLL = new UserBLL();
           

            string content = string.Empty;
            int count = 0;
            if (!string.IsNullOrEmpty(str))
            {
                string[] ids = str.Split(',');

                foreach (string item in ids)
                {
                    ret = _UserBLL.UpdateByPK(item._ToInt32(), "", "", "", "", "", "", "1", "","","");
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

        #region 获取当前用户所有能操作的投递员
        /// <summary>
        /// 获取当前用户所有能操作的投递员
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPosters()
        {
            retValue ret = new retValue();
            UserModel user = Session["UserModel"] as UserModel;
            BLL.UserBLL _BLL = new UserBLL();
            ret.result = true;
            ret.data = _BLL.GetPosters(user.OrgID._ToStr());
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
            retValue ret = new retValue();
            UserModel user = Session["UserModel"] as UserModel;
            BLL.UserBLL _UserBLL = new UserBLL();
            JObject o = null;
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
                    ret = _UserBLL.Insert(USERNO, NAME, Sex, Role, OrgID, IDCard, Password, PhoneNumber, Address, Email, user._ID._ToStr(),user.Level);
                }
                //更新
                else
                {
                    ret = _UserBLL.UpdateByPK(ID._ToInt32(), USERNO, NAME, Sex, Role, OrgID, IDCard, State, MGUID, Email, PhoneNumber, Password);
                }
            }
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserLogin(string str)
        {
            retValue ret = new retValue();
            BLL.UserBLL _UserBLL = new UserBLL();
            JObject o = null;
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);
                string USERNO = o["UserNo"]._ToStrTrim();
                string Password = o["Password"]._ToStrTrim();
                ret = _UserBLL.Login(USERNO, Password);
                if (ret.result)
                {
                    UserModel user = new UserModel();

                    DataTable dt = ret.data as DataTable;
                    DataRow dr = dt.Rows[0];
                    user._ID = dr["ID"]._ToInt32();
                    UserModel.needlogin = false;
                    user.UserNo = dr["UserNo"]._ToStrTrim();
                    user.OrgID = dr["OrgID"]._ToInt32();
                    user.Name = dr["NAME"]._ToStrTrim();
                    user.Role = dr["Role"]._ToInt32();
                    user.Password = dr["Password"]._ToStrTrim();
                    user.Sex = dr["Sex"]._ToInt32();
                    user.IDCard = dr["IDCard"]._ToStrTrim();
                    user.State = dr["State"]._ToInt32();
                    user._OrgName = dr["OrgName"]._ToStrTrim();
                    user.OrgNo = dr["OrgNo"]._ToStrTrim();
                    user.Level = dr["Level"]._ToInt32();
                    Session["UserModel"] = user;
                }
            }
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 退出登录
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserLoginOut(string str)
        {
            retValue ret = new retValue();
            Session["UserModel"] = null;
            ret.result = true;
            ret.data = "NEEDLOGIN";
            UserModel.needlogin = true;
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
            retValue ret = new retValue();

            BLL.DocBLL _DocBLL = new DocBLL();

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
                string BKDH = o["BKDH"]._ToStrTrim().ToUpper();
                string PublishArea = o["PublishArea"]._ToStrTrim();
                ret = _DocBLL.GetDoc("", Type, Name, ISSN, PublishArea, "", "", test1, test2, BKDH);
            }
            content = ret.toJson();

            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }

        #region 获取所有的报纸信息
        /// <summary>
        /// 获取所有的报纸信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllDocInfo(string str)
        {
            retValue ret = new retValue();
            BLL.DocBLL _BLL = new DocBLL();
            ret = _BLL.GetDoc(str, "", "", "", "", "", "", "", "", "");
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        
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
                ret = _DocBLL.GetDoc(addeditcode._ToStr(), "", "", "", "", "", "", "", "","");
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
            retValue ret = new retValue();
            BLL.DocBLL _DocBLL = new DocBLL();
            UserModel usermodel = Session["UserModel"] as UserModel;
            JObject o = null;
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
                string BKDH = o["BKDH"]._ToStrTrim().ToUpper();
                //新增
                if (string.IsNullOrEmpty(ID))
                {
                    ret = _DocBLL.Insert(Name, ISSN, Type, PublishArea, Publisher, Price, PL, BKDH, usermodel._ID._ToStr());
                }
                //更新
                else
                {
                    ret = _DocBLL.UpdateByPK(ID._ToInt32(),Name,ISSN,Type,PublishArea,Publisher,Price,PL,BKDH,NGUID);
                }
            }
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 删除数据
        [HttpPost]
        public JsonResult DocInfo_Delete(string str)
        {
            retValue ret = new retValue();

            BLL.DocBLL _DocBLL = new DocBLL();

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
            retValue ret = new retValue();
            BLL.OrgInfoBLL _BLL = new OrgInfoBLL();
            UserModel user = Session["UserModel"] as UserModel;
            ret = _BLL.GetOrgByParentID(str._ToInt32(), user.OrgID,user.Level);
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OrgInfos2(int limit,int page,string Province, string CompanyCity, string CompanyUnderCity, string CompanyUnderArea)
        {
            PageModel ret = new PageModel();
            BLL.OrgInfoBLL _BLL = new OrgInfoBLL();
            UserModel user = Session["UserModel"] as UserModel;
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
            ret = _BLL.GetOrgByParentID(OrgID._ToInt32(), limit,page, user.OrgID, user.Level);
            var js = JsonConvert.SerializeObject(ret);

            return Content(js);
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
            retValue ret = new retValue();

            BLL.OrgInfoBLL _BLL = new OrgInfoBLL();
            JObject o = null;
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);

                string ID = o["ID"]._ToStrTrim();
                string NAME = o["Name"]._ToStrTrim();
                string OrgCode = o["OrgCode"]._ToStrTrim();
                string Address = o["Address"]._ToStrTrim();
                UserModel user = Session["UserModel"] as UserModel;
                //新增
                if (string.IsNullOrEmpty(ID))
                {
                    string Province = o["Province"]._ToStrTrim();
                    string CompanyCity = o["CompanyCity"]._ToStrTrim();
                    string CompanyUnderCity = o["CompanyUnderCity"]._ToStrTrim();
                    string CompanyUnderArea = o["CompanyUnderArea"]._ToStrTrim();
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
                    int level = 0;
                    ret = _BLL.GetOrgByPK(OrgID._ToInt32());
                    if (ret.result)
                    {
                        level = ((DataTable)ret.data).Rows[0]["Level"]._ToInt32() + 1;
                    }
                    if (user.Level > level)
                    {
                        ret.result = false;
                        ret.reason = "您不能添加当前级别的机构";
                    }
                    else
                    {
                        ret = _BLL.insert(NAME, Address, OrgCode, OrgID._ToInt32(), level);
                    }
                }
                //更新
                else
                {
                    int level = 0;
                    ret = _BLL.GetOrgByPK(ID._ToInt32());
                    if (ret.result)
                    {
                        level = ((DataTable)ret.data).Rows[0]["Level"]._ToInt32();
                        
                        if (user.Level > level)
                        {
                            ret.result = false;
                            ret.reason = "您不能更新当前级别的机构";
                        }
                        else
                        {
                            ret = _BLL.update(ID._ToInt32(), NAME, OrgCode, Address);
                        }
                    }                   
                }
            }
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 删除机构
        [HttpPost]
        public JsonResult OrgInfo_Deletes(string str)
        {
            retValue ret = new retValue();

            BLL.OrgInfoBLL _BLL = new OrgInfoBLL();
            UserModel user = Session["UserModel"] as UserModel;
            if (!string.IsNullOrEmpty(str))
            {
                ret = _BLL.DeleteByPK(str, user.Level);
            }
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}
