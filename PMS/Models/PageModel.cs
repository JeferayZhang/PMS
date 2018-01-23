using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class PageModel
    {
        /// <summary>
        /// 总数(分页)
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 状态码(成功0,失败其他)
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 实体对象集合
        /// </summary>
        public object data { get; set; }
    }
}