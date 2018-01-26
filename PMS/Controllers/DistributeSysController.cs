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

            return View();
        }

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
                string Group_Type = o["Group_Type"]._ToStrTrim();
                string NewspaperName = o["NewspaperName"]._ToStrTrim();
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
                if (Group_Type=="1")
                {
                    ret = _BLL.GetTableByBK(NewspaperName,userModel.OrgID._ToStr(), OrgID);
                }
                else
                {
                    ret = _BLL.GetTableByOrg(OrgID, userModel.OrgID._ToStr());
                }
            }
            var js = JsonConvert.SerializeObject(ret);

            return Json(js, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DistributeLog()
        {
            if (!authorize.checkFilterContext())
            {
                return Redirect("/Account/Login");
            }

            return View();
        }

    }
}
