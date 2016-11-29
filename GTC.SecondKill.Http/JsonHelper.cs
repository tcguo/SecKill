/* ==============================================================================
 * 功能描述:
 * 创 建 者: gtc
 * 创建日期: 2013/9/13 13:51:52
 * 修 改 人:
 * 修改时间:
 * 修改备注:
 * @version 1.0
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace GTC.SecondKill.Http
{
    public class JsonHelper
    {
        /// <summary>
        /// 将jsonp回调字符串转为json字符串
        /// </summary>
        /// <param name="jsonp"></param>
        /// <returns></returns>
        public static string JsonpString2Json(string jsonp)
        {
            if (string.IsNullOrEmpty(jsonp))
            {
                throw new ArgumentNullException("jsonp");
            }

            try
            {
                if (jsonp.Contains(";"))
                {
                    jsonp = jsonp.Replace(";", "");
                }

                int firstIndex = jsonp.IndexOf('(');
                jsonp = jsonp.Substring(firstIndex);
                jsonp = jsonp.Replace(")", "").Replace("(", "");

                string[] arr = jsonp.Split(new char[] { ',' });

                for (int i = 0, j = arr.Length; i < j; i++)
                {
                    int index = arr[i].LastIndexOf(':');
                    string val = arr[i].Substring(index + 1);

                    if (!val.Contains("\""))
                    {
                        if (val.Contains("}"))
                        {
                            string temp = val.Substring(0, val.IndexOf("}"));
                            val = string.Format("\"{0}\"", temp) + val.Substring(val.IndexOf("}"));
                            arr[i] = arr[i].Substring(0, index + 1) + val;
                        }
                        else
                        {
                            arr[i] = arr[i].Substring(0, index + 1) + string.Format("\"{0}\"", val);
                        }
                    }
                }

                return string.Join(",", arr);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("解析jsonp字符串出错,详细信息： {0}", ex.Message));
            }
        }

        public static string Array2Json(string[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentNullException("参数不能为空或长度为0");
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in array)
            {
                string value = item.Trim();
                sb.Append(item);
            }

            return sb.ToString();
        }
    }
}
