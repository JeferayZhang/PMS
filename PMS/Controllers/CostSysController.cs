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
            BLL.CostBLL _BLL = new CostBLL();

            JObject o = null;

            string content = string.Empty;

            PageModel pg = _BLL.GetCostRecords(0, State, OrderID._ToInt32(), OrderNo, UnitName, limit, page);
           
            var js = JsonConvert.SerializeObject(pg);
            return Content(js);
        }
    }
}
