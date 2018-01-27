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

namespace PMS.Controllers
{
    public class DistributeSysController : Controller
    {
        //
        // GET: /DistributeSys/

        public ActionResult DistributeInfo()
        {
            if (!authorize.checkFilterContext())
            {
                return Redirect("/Account/Login");
            }
            ViewData.Model= Request["from"]._ToStr();
            return View();
        }

        /// <summary>
        /// 查询要分发的记录
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDis(string str)
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
                string NewspaperName = o["NewspaperName"]._ToStrTrim();
                string dt1 = o["test1"]._ToStrTrim();
                string Group_Type = o["Group_Type"]._ToStrTrim();
                string action = o["action"]._ToStrTrim();
                string CompanyCity = o["CompanyCity"]._ToStrTrim();
                string CompanyUnderCity = o["CompanyUnderCity"]._ToStrTrim();
                string CompanyUnderArea = o["CompanyUnderArea"]._ToStrTrim();
                string Roads = o["Roads"]._ToStrTrim();
                string OrgID = "";
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
                if (action=="city")
                {
                    ret = _BLL.GetTable(NewspaperName, OrgID, userModel.OrgID._ToStr(), Group_Type, dt1);
                }
                else
                {
                    ret = _BLL.GetTable2(NewspaperName, OrgID, userModel.OrgID._ToStr(), Group_Type, dt1);
                }
            }
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
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

            JObject o = null;
            if (tttttt == "area")
            {
                ret = _BLL.getlog2(test1, test2, User, limit, page - 1);
            }
            else
            {
                ret = _BLL.getlog1(test1, test2, User, limit, page - 1);
            }
            var js = JsonConvert.SerializeObject(ret);
            return Content(js);
        } 
        #endregion

        #region 执行分发操作

        public ActionResult DistributeInfo_Edit()
        {
            if (!authorize.checkFilterContext())
            {
                return Redirect("/Account/Login");
            }
            ViewData.Model = Request["ids"]._ToStr();
            return View();
        }

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
                string ids = o["ids"]._ToStrTrim();
                ret = _BLL.insertLog(ids, nianjuanqi,userModel._ID);
            }
            var js = JsonConvert.SerializeObject(ret);
            return Json(js, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
