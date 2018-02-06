using PMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.App_Start
{
    /// <summary>
    /// 重写OnActionExecuting方法进行登录验证,如果没有登录则直接跳转到登录页
    /// </summary>
    public class NeedLoginFilter : ActionFilterAttribute
    {
        public string Message { get; set; }

        /// <summary>
        /// 这个方法在action之前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //如果本身是登录请求,就不用做登录验证了
            if (filterContext.ActionDescriptor.ActionName != "UserLogin")
            {
                if (UserModel.needlogin || HttpContext.Current.Session["UserModel"] == null)
                {
                    filterContext.Result = new RedirectResult("../Account/Login");
                }
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
    }
}