using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static partial class CommonHelper
    {
        public static object[] LoadExcelToDataTable(string filename)
        {
            object[] ret = new object[2];
            if (!File.Exists(filename))
            {
                ret[0] = 0;
                ret[1] = filename + "文件不存在!";
            }
            else
            {
                DataTable table = new DataTable();
                try
                {
                    ExcelHelper ite = new ExcelHelper(filename);
                    ite.sheet = ite.getsheetbyindex(0);
                    table = ite.ReadDataTable();
                    if (table == null || table.Columns.Count == 0)
                    {
                        ret[0] = 0;
                        ret[1] = "excel的第一个sheet中无任何数据！";
                    }
                    else
                    {
                        ret[0] = 1;
                        ret[1] = table;
                    }
                }
                catch (Exception ex)
                {
                    ret[0] = 0;
                    if (ex.ToString().Contains("索引超出范围。必须为非负值并小于集合大小。") || ex.ToString().Contains("Sheet index (0) is out of range (0..-1)"))
                        ret[1] = "EXCEL被独占打开，请先关闭EXCEL再导入";
                    if (ex.ToString().Contains("的列已属于此 DataTable"))
                        ret[1] = "EXCEL中出现重复的表头，请删除其中一列，或将其中一列重命名之后再行导入！";
                    else
                        ret[1] = "读取EXCEL失败";
                }
            }
            return ret;
        }
    }
}
