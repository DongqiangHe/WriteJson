using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Collections;

namespace WriteJson
{
    public class JsonHelper
    {

        #region

        /// <summary>
        /// Convert Dictionary to json string with custom method.
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string Dictionary2Json(Dictionary<string, object> dic)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");

            foreach (var vr in dic)
            {
                json.Append("\"" + FormatString(vr.Key.ToString()) + "\":");
                json.Append(ToJsonString(vr.Value) + ",");
            }

            json.Remove(json.Length - 1, 1);
            json.Append("}");

            return json.ToString();
        }

        public static string Dictionary2Json2(Dictionary<string, object> dic)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            int i = 0;
            foreach (var vr in dic)
            {
                if (i > 0)
                    json.Append(",");
                json.Append("\"" + FormatString(vr.Key.ToString()) + "\":");
                json.Append(ToJsonString(vr.Value));
                ++i;
            }

            json.Append("}");

            return json.ToString();
        }

        /// <summary>
        /// Convert Dictionary to json string, and make the key lower case.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public static string Dictionary2Json(Dictionary<string, object> dic, bool lowerCase)
        {
            if (lowerCase == true)
            {
                StringBuilder json = new StringBuilder();
                json.Append("{");

                foreach (var vr in dic)
                {
                    json.Append("\"" + FormatString(vr.Key.ToString().ToLower()) + "\":");
                    json.Append(ToJsonString(vr.Value) + ",");
                }

                json.Remove(json.Length - 1, 1);
                json.Append("}");

                return json.ToString();
            }
            else
            {
                return Dictionary2Json(dic);
            }

        }

        /// <summary>
        /// Convert List<Dictionary<string, object>> to json string using JavaScriptSerializer.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string DictionaryList2Json(List<Dictionary<string, object>> list)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            if (list != null)
            {
                return jss.Serialize(list);
            }

            return null;
        }

        /// <summary>
        /// Convert List<Dictionary<string, object>> to Json string using custom methond.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string DictionaryList2Json2(List<Dictionary<string, object>> list)
        {
            if (list != null)
            {
                StringBuilder json = new StringBuilder();
                json.Append("[");

                foreach (var dic in list)
                {
                    json.Append(Dictionary2Json(dic) + ",");
                }

                json.Remove(json.Length - 1, 1);
                json.Append("]");

                return json.ToString();
            }

            return null;
        }

        public static string DictionaryList2Json3(List<Dictionary<string, object>> list)
        {
            if (list != null)
            {
                StringBuilder json = new StringBuilder();
                json.Append("[");
                int i = 0;
                foreach (var dic in list)
                {
                    if (i > 0)
                        json.Append(",");
                    json.Append(Dictionary2Json(dic));
                    ++i;
                }

                json.Append("]");

                return json.ToString();
            }

            return null;
        }

        /// <summary>
        /// Convert List<Dictionary<string, object>> to Json string using custom methond,
        /// and make the key lower case.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public static string DictionaryList2Json2(List<Dictionary<string, object>> list, bool lowerCase)
        {
            if (list != null)
            {
                if (lowerCase == true)
                {
                    StringBuilder json = new StringBuilder();
                    json.Append("[");

                    foreach (var dic in list)
                    {
                        json.Append(Dictionary2Json(dic, true) + ",");
                    }

                    json.Remove(json.Length - 1, 1);
                    json.Append("]");

                    return json.ToString();
                }
                else
                {
                    return DictionaryList2Json2(list);
                }

            }

            return null;
        }

        #endregion

        #region
        /// <summary>
        /// Convert a single DbDataReader row to a Dictionary<string, Object>.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Dictionary<string, object> Row2Dictionary(DbDataReader r)
        {
            Dictionary<string, object> drow = new Dictionary<string, object>();

            if (r != null)
            {
                for (int i = 0; i < r.FieldCount; ++i)
                {
                    string key = r.GetName(i);
                    if (!drow.ContainsKey(key))
                    {
                        drow.Add(key, r.GetValue(i));
                    }
                }
            }

            return drow;
        }

        /// <summary>
        /// Convert DbDataReader with multirows to List<Dictionary<string, object>>.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> Rows2DictionaryList(DbDataReader r)
        {
            List<Dictionary<string, object>> rowList = new List<Dictionary<string, object>>();

            if (r != null)
            {
                while(r.Read())
                {
                    rowList.Add(Row2Dictionary(r));
                }
            }

            return rowList;
        }

        /// <summary>
        /// Convert Multi-rows DbDataReader to Json String using JavaScriptSerializer.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string Rows2JsonString(DbDataReader r)
        {
            return DictionaryList2Json(Rows2DictionaryList(r));
        }

        /// <summary>
        /// Convert Multi-rows DbDataReader to Json String using custom methond.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string Rows2JsonString2(DbDataReader r)
        {
            return DictionaryList2Json2(Rows2DictionaryList(r));
        }

        /// <summary>
        ///  Convert Multi-rows DbDataReader to Json String using custom methond,
        ///  and make the key lower case.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public static string Rows2JsonString2(DbDataReader r, bool lowerCase)
        {
            if (lowerCase == true)
            {
                return DictionaryList2Json2(Rows2DictionaryList(r), true);
            }

            return DictionaryList2Json2(Rows2DictionaryList(r));
        }

        /// <summary>
        /// Convet a single DbDataReader row to a json string with JavaScriptSerializer.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string Row2JsonString(DbDataReader r)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> drow = new Dictionary<string, object>();

            if (r != null)
            {
                for (int i = 0; i < r.FieldCount; ++i)
                {
                    string key = r.GetName(i);
                    if (!drow.ContainsKey(key))
                    {
                        drow.Add(key, r.GetValue(i)); 
                    }
                }
            }

            return jss.Serialize(drow);
        }

        public static string Row2JsonString2(DbDataReader r, bool lowerCase)
        {
            return Dictionary2Json(Row2Dictionary(r), lowerCase);
        }

        #endregion

        #region
        /// <summary>
        /// Save Datatable data  into List<Dictionary<string, object>>.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> DataTable2DictionaryList(DataTable table)
        {
            if (table != null)
            {
                List<Dictionary<string, object>> tableList = new List<Dictionary<string, object>>();
                Dictionary<string, object> drow;

                foreach(DataRow row in table.Rows)
                {
                    drow = new Dictionary<string, object>();
                    foreach (DataColumn col in table.Columns)
                    {
                        drow.Add(col.ColumnName, row[col]);
                    }
                    tableList.Add(drow);
                }

                return tableList;
            }

            return null;
        }

        /// <summary>
        /// Convert DataTable data to json string using JavaScriptSerializer.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string DataTable2Json(DataTable table)
        {
            if (table != null)
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                return jss.Serialize(DataTable2DictionaryList(table));
            }
            return null;
        }

        /// <summary>
        /// Convert DataTable data to json string using yourself custom method.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string DataTable2Json2(DataTable table)
        {
            if (table != null)
            {
                return DictionaryList2Json2(DataTable2DictionaryList(table));
            }
            return null;
        }

        /// <summary>
        /// Convert DataTable data to json string using yourself custom method,
        /// and make the key lower case.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public static string DataTable2Json2(DataTable table, bool lowerCase)
        {
            if (table != null)
            {
                if (lowerCase == true)
                    return DictionaryList2Json2(DataTable2DictionaryList(table), true);
                return DataTable2Json2(table);
            }
            return null;
        }

        /// <summary>
        /// Convert DataTable data to json string using custom method.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DataSet2Json2(DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                //自定义无数据是返回值，根据自己的前端处理情况
                return "{\"successed\":false}";
            }
            StringBuilder json = new StringBuilder();
            json.Append("{");
            int i = 0;
            foreach (DataTable table in ds.Tables)
            {
                if (i > 0)
                    json.Append(",");
                json.Append("\"" + table.TableName + "\":");
                json.Append(DataTable2Json2(table));
                ++i;
            }
            json.Append("}");

            return json.ToString();
        }

        /// <summary>
        /// Convert DataTable data to json string using custom method,
        /// and make the key lower case;
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public static string DataSet2Json2(DataSet ds, bool lowerCase)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                //自定义无数据是返回值，根据自己的前端处理情况
                return "{\"successed\":false}";
            }
            if (lowerCase == true)
            {
                StringBuilder json = new StringBuilder();
                json.Append("{");
                int i = 0;

                foreach (DataTable table in ds.Tables)
                {
                    if (i > 0)
                        json.Append(",");
                    json.Append("\"" + table.TableName + "\":");
                    json.Append(DataTable2Json2(table,true));
                    ++i;
                }
                json.Append("}");

                return json.ToString();
            }

            return DataSet2Json(ds);
        }

        /// <summary>
        /// Convert DataTable data to json string using JavaScriptSerializer.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DataSet2Json(DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                //自定义无数据是返回值，根据自己的前端处理情况
                return "{\"successed\":false}";
            }
            StringBuilder json = new StringBuilder();
            json.Append("{");
            int i = 0;
            foreach (DataTable table in ds.Tables)
            {
                if (i > 0)
                    json.Append(",");
                json.Append("\"" + table.TableName + "\":");
                json.Append(DataTable2Json(table));
                ++i;
            }
            json.Append("}");

            return json.ToString();
        }

        #endregion

        #region

        /// <summary>
        /// Convert object to json string.
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns></returns>
        public static string Object2Json(object obj)
        {
            string json = "{";
            PropertyInfo[] pi = obj.GetType().GetProperties();
            for (int i = 0; i < pi.Length; i++)
            {
                object pvalue = pi[i].GetGetMethod().Invoke(obj, null);
                string value = null;
                if (pvalue is string)// because string is IEnumerable.
                {
                    value = ToJsonString(pvalue);
                }
                else if (pvalue is IEnumerable)
                {
                    value = IEnumerable2Json((IEnumerable)pvalue);
                }
                else
                {
                    value = ToJsonString(pvalue);
                }
                json += ToJsonString(pi[i].Name) + ":" +value + ",";
            }
            json = json.Remove(json.Length - 1);

            return json + "}";
        }

        /// <summary> 
        /// Convert Object collection to json string. 
        /// </summary> 
        /// <param name="arr">object collection</param> 
        /// <returns></returns> 
        public static string IEnumerable2Json(IEnumerable arr)
        {
            string json = "[";
            foreach (object obj in arr)
            {
                json += Object2Json(obj) + ",";
            }
            json = json.Remove(json.Length - 1, 1);

            return json + "]";
        }

        /// <summary>
        /// Convert non-object collection to json string.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string Array2String(IEnumerable arr)
        {
            string json = "[";
            foreach (object item in arr)
            {
                json = ToJsonString(item.ToString()) + ",";
            }
            json = json.Remove(json.Length - 1, 1);
            return json + "]";
        }

        /// <summary>
        /// Convert List<T> to json string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ListToJson<T>(IList<T> list, string key)
        {
            StringBuilder json = new StringBuilder();
            if (string.IsNullOrEmpty(key))
                key = list[0].GetType().Name; //<1> TypeOf():得到一个Class的Type  
            json.Append("{\"" + key + "\":[");//<2> GetType():得到一个Class的实例的Type

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    T obj = Activator.CreateInstance<T>();//创建类型的一个实例，该类型由指定的泛型类型参数指定。
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    json.Append("{");//获取对象的所有公有属性GetProperties()和所有公有方法GetMethods()
                    for (int j = 0; j < pi.Length; ++j)
                    {
                        json.Append("\"" + pi[j].Name.ToString() + "\":" + ToJsonString(pi[j].GetValue(list[i], null)));
                        if (j < pi.Length - 1)
                        {
                            json.Append(",");
                        }
                    }
                    json.Append("}");
                    if (i < list.Count - 1)
                    {
                        json.Append(",");
                    }
                }
            }
            json.Append("]}");
            return json.ToString();
        }

        public static string List2Json<T>(IList<T> list)
        {
            Object obj = list[0];
            return ListToJson<T>(list, obj.GetType().Name);
        }

        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = FormatString(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            return str;
        }

        #endregion

        #region

        /// <summary>
        /// Convert Dictionary Value to string fit in with json.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJsonString(object value)
        {
            string str = null;
            Type type = value.GetType();

            if (value is DBNull)
            {
                str = "null";
            }
            else if (value is DateTime)
            {
                DateTime dt = (DateTime)value;
                str = "\"" + dt.ToString("yyyy-MM-dd") + "\"";
            }
            else if (value is TimeSpan)
            {
                TimeSpan ts = (TimeSpan)value;
                str = "\"" + FormatHelper.TSFormat1(ts) + "\"";
            }
            else if (value is DateTimeOffset)
            {
                DateTimeOffset dto = (DateTimeOffset)value;
                str = "\"" + FormatString(FormatHelper.DTOFormat(dto, "u")) + "\"";
            }
            else if (value is Boolean)
            {
                str = value.ToString().ToLower();
            }
            else if (value is Int16 || value is Int32 || value is Int64)
            {
                str = value.ToString();
            }
            else if (value is Double || value is Single)
            {
                str = value.ToString();
            }
            else if (value is Decimal || value is Byte)
            {
                str = value.ToString();
            }
            else if (value is Byte[])
            {
                byte[] b = (byte[])value;
                str = "\"" + FormatHelper.ByteArrayFormat(b) + "\"";
            }
            else if (value is Guid)
            {
                str = "\"" + value.ToString() + "\"";
            }
            else
            {
                str = "\"" + FormatString(value.ToString()) + "\"";
            }

            return str;
        }

        /// <summary>
        /// 将string 格式化为json合理的串.(处理一些特殊字符)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string FormatString(String str)
        {
            StringBuilder json = new StringBuilder();
            for (int i = 0; i < str.Length; ++i)
            {
                char c = str.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        json.Append("\\\""); break;
                    case '\\':
                        json.Append("\\\\"); break;
                    case '/':
                        json.Append("\\/"); break;
                    case '\b':
                        json.Append("\\b"); break;
                    case '\f':
                        json.Append("\\f"); break;
                    case '\n':
                        json.Append("\\n"); break;
                    case '\r':
                        json.Append("\\r"); break;
                    case '\t':
                        json.Append("\\t"); break;
                    default:
                        json.Append(c); break;
                }
            }
            return json.ToString();
        }

        #endregion
    }
}
