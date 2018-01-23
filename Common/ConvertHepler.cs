using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Common
{
    public static class ConvertHepler
    {
        public static string _ToStr(this object s, string format = "")
        {
            string str = "";
            try
            {
                if (format == "")
                {
                    return s.ToString();
                }
                DateTime defaultValue = new DateTime();
                DateTime time = s._ToDateTime(defaultValue);
                if (time != new DateTime())
                {
                    return string.Format("{0:" + format + "}", time);
                }
                str = string.Format("{0:" + format + "}", s);
            }
            catch
            {
            }
            return str;
        }

        public static DataTable ToTable(this string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current);
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current]._ToStr();
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch (Exception ex)
            {
                string e = ex._ToStr();
            }
            result = dataTable;
            return result;
        }

        public static DateTime _ToDateTime(this object s, DateTime defaultValue = new DateTime())
        {
            DateTime time = defaultValue;
            try
            {
                time = Convert.ToDateTime(s._ToStrTrim());
            }
            catch
            {
            }
            return time;
        }

        public static DateTime? _ToDateTimeOrNull(this object s, DateTime defaultValue = new DateTime())
        {
            if (s == null)
                return null;
            if (s is string && s._ToStrTrim() == "")
                return null;
            try
            {
                return Convert.ToDateTime(s._ToStrTrim());
            }
            catch
            {
                return null;
            }
        }

        public static string _ToStrTrim(this object s)
        {
            string str = "";
            try
            {
                str = s.ToString().Trim();
            }
            catch
            {
            }
            return str;
        }

        public static int _ToInt32(this object s, int defaultValue = 0)
        {
            int num = defaultValue;
            try
            {
                num = Convert.ToInt32(s._ToStrTrim());
            }
            catch
            {
            }
            return num;
        }

        public static decimal _ToDecimal(this object s)
        {
            decimal num = 0;
            try
            {
                num = Convert.ToDecimal(s._ToStrTrim());
            }
            catch
            {
            }
            return num;
        }

        public static int? _ToInt32OrNull(this object n)
        {
            if (n == null)
                return null;
            if (n is string && n._ToStrTrim() == "")
                return null;
            try
            {
                return Convert.ToInt32(n._ToStrTrim());
            }
            catch
            {
                return null;
            }
        }

        public static long _ToInt64(this object s, long defaultValue = 0L)
        {
            long num = defaultValue;
            try
            {
                num = Convert.ToInt64(s._ToStrTrim());
            }
            catch
            {
            }
            return num;
        }

        public static double _ToDouble(this object s, int x = -1, double defaultValue = 0)
        {
            double num = defaultValue;
            try
            {
                if (x == -1)
                {
                    return Convert.ToDouble(_ToStrTrim(s));
                }
                num = Convert.ToDouble(string.Format("{0:F" + x.ToString() + "}", Convert.ToDouble(_ToStrTrim(s))));
            }
            catch
            {
            }
            return num;
        }


        public static float _ToFloat(this object s, int x = -1, float defaultValue = 0f)
        {
            float num = defaultValue;
            try
            {
                if (x == -1)
                {
                    return float.Parse(s._ToStrTrim());
                }
                num = float.Parse(string.Format("{0:F" + x.ToString() + "}", float.Parse(s._ToStrTrim())));
            }
            catch
            {
            }
            return num;
        }


        public static string _ToStrTrim(this object s, params char[] x)
        {
            string str = "";
            try
            {
                str = s.ToString().Trim(x);
            }
            catch
            {
            }
            return str;
        }

        public static string _FormatDatetimeWithNull(this DateTime? d)
        {
            if (d == null)
                return null;
            else
                return d._ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        }


        public static bool _checkStringIsFloat(this string str)
        {
            Regex reg = new Regex(@"^(\d{0,})(\.{0,1}\d{0,})$");
            if (reg.IsMatch(str))
                return true;
            return false;
        }

        public static IList<T> ConvertTo<T>(this DataTable table)
        {
            if (table == null)
                return null;

            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
                rows.Add(row);

            return ConvertTo<T>(rows);
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;
            if (rows != null)
            {
                list = new List<T>();
                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }
            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            string columnName;
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();
                foreach (DataColumn column in row.Table.Columns)
                {
                    columnName = column.ColumnName;
                    PropertyInfo prop = obj.GetType().GetProperty(columnName);
                    try
                    {
                        object value = (row[columnName].GetType() == typeof(DBNull))
                        ? null : row[columnName];
                        if (prop.CanWrite)
                            prop.SetValue(obj, value, null);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return obj;
        }
    }
}
