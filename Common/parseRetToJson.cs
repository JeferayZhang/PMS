using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class parseRetToJson
    {
        public static string toJson(this retValue ret)
        {
            if (!ret.result)
            {
                return "{\"RESULT\":\"false\",\"ERRORCODE\":\"" + ret.errorcode + "\",\"REASON\":\"" + ret.reason + "\"}";
            }
            else
            {
                if (ret.data is DataTable)
                {
                    DataTable dt = ret.data as DataTable;
                    return "{\"RESULT\":\"true\",\"DATA\":" + dt.ConvertDtToJson() + "}";
                }
                if (ret.data is DataRow)
                {
                    DataRow dr = ret.data as DataRow;
                    return "{\"RESULT\":\"true\",\"DATA\":" + dr.ConvertDrToJson() + "}";
                }
                return "{\"RESULT\":\"true\",\"ERRORCODE\":\"\",\"REASON\":\"\"}";
            }

        }

        public static string PageToJson(this retValue ret, string curr, string totalPage)
        {
            if (!ret.result)
            {
                return "{\"RESULT\":\"false\",\"ERRORCODE\":\"" + ret.errorcode + "\",\"REASON\":\"" + ret.reason + "\"}";
            }
            else
            {
                if (ret.data is DataTable)
                {
                    DataTable dt = ret.data as DataTable;
                    if (dt.Rows.Count < 0)
                    {
                        return "{\"RESULT\":\"false\",\"ERRORCODE\":\"" + ret.errorcode + "\",\"REASON\":\"未找到相关数据\"}";
                    }
                    return "{\"RESULT\":\"true\",\"REASON\":\"\",\"Page\": {\"currentPage\":" + curr + ",\"totalPage\":" + totalPage + ",\"data\":" + dt.ConvertDtToJson() + "}}";
                }
                return "{\"RESULT\":\"true\",\"ERRORCODE\":\"\",\"REASON\":\"\"}";
            }

        }

        public static string ConvertDtToJson(this DataTable dtb)
        {
            if (dtb == null || dtb.Rows.Count == 0)
                return "[]";
            string retValue = "[";
            foreach (DataRow item in dtb.Rows)
            {
                retValue += item.ConvertDrToJson() + ",";
            }
            retValue = retValue.TrimEnd(',');
            retValue += "]";
            return retValue;
        }

        public static string ConvertDrToJson(this DataRow dr)
        {
            string retValue = "";
            retValue += "{";
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                retValue += "\"" + dr.Table.Columns[i].ColumnName + "\":\"" + dr[i]._ToStr().Replace("\n", "\\n").Replace("\r\n", "\\r\\n").Replace("\"", "\\\"") + "\",";
            }
            retValue = retValue.TrimEnd(',');
            retValue += "}";
            return retValue;
        }
    }
}
