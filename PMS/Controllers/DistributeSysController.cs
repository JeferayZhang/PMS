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
using PMS.Models;
using DAL;

namespace PMS.Controllers
{
    public class DistributeSysController : Controller
    {
        //
        // GET: /DistributeSys/

        public ActionResult DistributeInfo()
        {
            return View();
        }
        public ActionResult DistributeArea()
        {
            return View();
        }
        /// <summary>
        /// 查询要分发的记录,应该只能查询出订购状态是正常的记录
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDis(int page,int limit,string test1, string Group_Type, string NewspaperName, string Province, string CompanyCity, string CompanyUnderCity, string CompanyUnderArea
            , string Roads)
        {
            PageModel ret = new PageModel();
            BLL.DistributeBLL _BLL = new DistributeBLL();
            
            JObject o = null;

            UserModel userModel = Session["userModel"] as UserModel;
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
            if (Roads._ToInt32() > 0)
            {
                OrgID = Roads; 
            }
            ret = _BLL.GetTable(NewspaperName, OrgID, userModel.OrgID._ToStr(), Group_Type, test1, limit, page);
            var js = JsonConvert.SerializeObject(ret);

            return Content(js);
        }

        [HttpGet]
        public ActionResult GetDis2(int page, int limit, string test1, string Group_Type, string NewspaperName,string Province, string CompanyCity, string CompanyUnderCity, string CompanyUnderArea
            , string Roads)
        {
            PageModel ret = new PageModel();
            BLL.DistributeBLL _BLL = new DistributeBLL();

            JObject o = null;

            UserModel userModel = Session["userModel"] as UserModel;
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
            if (Roads._ToInt32() > 0)
            {
                OrgID = Roads;  
            }
            ret = _BLL.GetTable2(NewspaperName, OrgID, userModel.OrgID._ToStr(), Group_Type, test1, limit, page);
            var js = JsonConvert.SerializeObject(ret);

            return Content(js);
        }

        #region 查询分发日志
        /// <summary>
        /// 分发日志
        /// </summary>
        /// <returns></returns>
        public ActionResult DistributeLog()
        {
            if (!authorize.checkFilterContext())
            {
                return Redirect("/Account/Login");
            }
            ViewData.Model = Request["from"]._ToStr();
            return View();
        }
        /// <summary>
        /// 查询分发日志
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHistory(int page, int limit, string test1, string test2, string User, string tttttt)
        {
            PageModel ret = new PageModel();
            if (!authorize.checkFilterContext())
            {
                ret.code = 2;
                ret.msg = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }

            BLL.DistributeBLL _BLL = new DistributeBLL();
            UserModel userModel = Session["userModel"] as UserModel;
            JObject o = null;
            if (tttttt == "area")
            {
                ret = _BLL.getlog2(test1, test2,userModel.OrgID, User, limit, page - 1);
            }
            else
            {
                ret = _BLL.getlog1(test1, test2, userModel.OrgID, User, limit, page - 1);
            }
            var js = JsonConvert.SerializeObject(ret);
            return Content(js);
        }
        #endregion

        #region 获取当前用户所有能看到的分发员,同一机构或者机构下
        /// <summary>
        /// 获取当前用户所有能看到的分发员
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetFFs()
        {
            retValue ret = new retValue();
            if (!authorize.checkFilterContext())
            {
                ret.result = true;
                ret.data = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            UserModel user = Session["UserModel"] as UserModel;
            BLL.UserBLL _BLL = new UserBLL();
            OrgInfoDAL _OrgInfoDAL = new OrgInfoDAL();
            ret.result = true;
            ret.data = _BLL.GetPosters("",2);
            var js = JsonConvert.SerializeObject(ret);
            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 执行分发操作

        public ActionResult DistributeInfo_Edit()
        {
            if (!authorize.checkFilterContext())
            {
                return Redirect("/Account/Login");
            }
            ViewData.Model = Request["ids"]._ToStr() + "|" + Request["action"]._ToStr();
            return View();
        }
        
        /// <summary>
        /// 分发之后,要将那些到了时间的订购记录,状态更改为过期
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DistributeLog_FF(string str)
        {
            retValue ret = new retValue();
            if (!authorize.checkFilterContext())
            {
                ret.result = true;
                ret.data = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            BLL.DistributeBLL _BLL = new DistributeBLL();
            JObject o = null;
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);
                UserModel userModel = Session["userModel"] as UserModel;
                string nianjuanqi = o["nianjuanqi"]._ToStrTrim();
                string action = o["action"]._ToStrTrim();
                //日志类型,0表示市,1表示县
                int type = 0;
                if (action=="area")
                {
                    type = 1;
                }
                string ids = o["ids"]._ToStrTrim();
                ret = _BLL.insertLog(ids, nianjuanqi, userModel._ID, type);
            }
            var js = JsonConvert.SerializeObject(ret);
            return Json(js, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分发之前的验证
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Check_FF(string str)
        {
            retValue ret = new retValue();
            if (!authorize.checkFilterContext())
            {
                ret.result = true;
                ret.data = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            BLL.DistributeBLL _BLL = new DistributeBLL();
            JObject o = null;
            if (!string.IsNullOrEmpty(str))
            {
                o = JObject.Parse(str);
                string nianjuanqi = o["nianjuanqi"]._ToStrTrim();
                string action = o["action"]._ToStrTrim();
                //日志类型,0表示市,1表示县
                int type = 0;
                if (action == "area")
                {
                    type = 1;
                }
                string ids = o["ids"]._ToStrTrim();
                if (_BLL.count(ids, type, nianjuanqi) > 0)
                {
                    ret.result = false;
                    ret.reason = "1000";
                }
                else
                {
                    ret.result = true;
                    ret.data = "";
                }
            }
            var js = JsonConvert.SerializeObject(ret);
            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
