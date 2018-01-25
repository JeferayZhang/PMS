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

                string Group_Type = o["Group_Type"]._ToStrTrim();
                string NewspaperName = o["NewspaperName"]._ToStrTrim();
                string CompanyCity = o["CompanyCity"]._ToStrTrim();
                string CompanyUnderCity = o["CompanyUnderCity"]._ToStrTrim();
                if (Group_Type=="1")
                {
                    ret = _BLL.GetTableByBK(NewspaperName);
                }
                else
                {
                    if (string.IsNullOrEmpty(CompanyUnderCity))
                    {
                        ret = _BLL.GetTableByOrg(CompanyCity);
                    }
                    else
                    {
                        ret = _BLL.GetTableByOrg(CompanyUnderCity);
                    }
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
