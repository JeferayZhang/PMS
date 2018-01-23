using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class retValue
    {
        private bool _result = false;
        /// <summary>
        /// 处理结果是正确还是错误
        /// </summary>
        public bool result { get { return _result; } set { _result = value; } }


        private string _errorcode = "00";
        /// <summary>
        /// 错误代码
        /// </summary>
        public string errorcode { get { return _errorcode; } set { _errorcode = value; } }


        private string _reason = "";
        /// <summary>
        /// 失败原因
        /// </summary>
        public string reason { get { return _reason; } set { _reason = value; } }


        private object _data = new object();
        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get { return _data; } set { _data = value; } }
    }
}
