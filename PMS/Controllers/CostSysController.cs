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
    public class CostSysController : Controller
    {
        //
        // GET: /CostInfo/

        public ActionResult CostInfo()
        {
            return View();
        }
         [HttpGet]
        public ActionResult CostInfos(int page, int limit, string OrderNo, string OrderID, string State, string UnitName)
        {
            PageModel ret = new PageModel();
            if (!authorize.checkFilterContext())
            {
                ret.code = 2;
                ret.msg = "NEEDLOGIN";
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            BLL.CostBLL _BLL = new CostBLL();
            PMS.Models.UserModel userModel = Session["UserModel"] as PMS.Models.UserModel;
            PageModel pg = _BLL.GetCostRecords(0, State, OrderID._ToInt32(), OrderNo, UnitName, limit, page,userModel.OrgID);
           
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
             BLL.CostBLL _BLL = new CostBLL();
             JObject o = null;
             if (!string.IsNullOrEmpty(str))
             {
                 o = JObject.Parse(str);

                 string OrderNo = o["OrderNo"]._ToStrTrim();
                 string OrderID = o["OrderID"]._ToStrTrim();
                 string State = o["CostState"]._ToStrTrim();
                 string UnitName = o["UnitName"]._ToStrTrim();
                 ret = _BLL.GetCostRecords(0, State, OrderID._ToInt32(), OrderNo, UnitName, userModel.OrgID);
             }
             var js = JsonConvert.SerializeObject(ret);

             return Json(js, JsonRequestBehavior.AllowGet);
         }
    }
}
