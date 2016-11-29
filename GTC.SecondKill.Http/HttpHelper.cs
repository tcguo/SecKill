/* ==============================================================================
 * 功能描述:
 * 创 建 者: gtc
 * 创建日期: 2013/9/10 16:29:12
 * 修 改 人:
 * 修改时间:
 * 修改备注:
 * @version 1.0
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Drawing;
using System.IO;
using System.IO.Compression;

using System.Web.Util;
using System.Web;
namespace GTC.SecondKill.Http
{
    public class HttpHelper
    {
        public static readonly string MyUserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/6.0)";

        public static Bitmap ImgageHttpWebRequest(ImageRequestModel model)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) =>
            {
                return true;
            };

            Bitmap bitmap;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(model.ImgUrl);
                request.Proxy = model.WebProxy;
                request.Referer = model.RefURL;
                request.Method = "GET";
                request.KeepAlive = true;
                request.AllowAutoRedirect = model.AllowRedirect;
                request.CookieContainer = model.CookieContainer;
                request.UserAgent = MyUserAgent;

                if (!string.IsNullOrEmpty(model.FakeIP))
                {
                    request.Headers.Add("X-Forwarded-For", model.FakeIP);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //response.Cookies = SiteCookie.GetCookies(request.RequestUri);
                
                if (response.Cookies != null && response.Cookies.Count > 0)
                {
                    model.CookieContainer.Add(response.Cookies);
                }
                Stream responseStream = response.GetResponseStream();
                bitmap = new Bitmap(responseStream);
                response.Close();
                responseStream.Close();
            }
            catch (Exception)
            {
                bitmap = null;
            }
            return bitmap;
        }

        public static string SendPostHttpRequest(PostRequestModel model)
        {
            string responseText;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                string temp = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(model.TheURL);
                request.Method = "POST";
                request.Proxy = model.WebProxy;
                request.Referer = model.RefURL;
                request.KeepAlive = true;
                request.AllowAutoRedirect = model.AllowRedirect;
                request.UserAgent = MyUserAgent;
                request.CookieContainer = model.CookieContainer;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
               
                if (!string.IsNullOrEmpty(model.FakeIP))
                {
                    request.Headers.Add("X-Forwarded-For", model.FakeIP);
                }
                
                //写form表单数据，拼成string，参数之间用&
                request.ServicePoint.Expect100Continue = false;
                byte[] bytes = model.TheCode.GetBytes(model.PostData);
                request.ContentLength = bytes.Length;
                request.GetRequestStream().Write(bytes, 0, bytes.Length);
                
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.Cookies != null && response.Cookies.Count > 0)
                {
                    model.CookieContainer.Add(response.Cookies);
                }

                if (model.IsReadAll)
                {
                    Stream responseStream;
                    StreamReader reader;
                    string contentType = response.ContentType;
                    string contentEncoding = response.ContentEncoding;
                    if (contentEncoding == "gzip")
                    {
                        responseStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    }
                    else
                    {
                        responseStream = response.GetResponseStream();
                    }
                    string characterSet = response.CharacterSet;
                    if (characterSet == "")
                    {
                        reader = new StreamReader(responseStream, Encoding.Default);
                    }
                    else
                    {
                        characterSet = characterSet.Trim().ToLower();
                        string str7 = characterSet;
                        if (str7 == "gb2312")
                        {
                            reader = new StreamReader(responseStream, Encoding.GetEncoding("gb2312"));
                        }
                        else if ((str7 == "utf-8") || (str7 == "iso-8859-1"))
                        {
                            reader = new StreamReader(responseStream, Encoding.UTF8);
                        }
                        else
                        {
                            reader = new StreamReader(responseStream, Encoding.Default);
                        }
                    }

                    temp = reader.ReadToEnd();
                    reader.Close();
                    responseStream.Close();
                }

                response.Close();
                request = null;
                response = null;
                bytes = null;

                responseText = temp;
            }
            catch (Exception)
            {
                responseText = string.Empty;
            }
            return responseText;
        }

        public static string SendGetHttpRequest(GetRequestModel model)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) =>
            {
                return true;
            };

            string returnVal;
            try
            {
                string tempVal = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(model.TheURL);
                request.Method = "GET";
                request.Proxy = model.WebProxy;
                request.Referer = model.RefURL;
                request.KeepAlive = true;
                request.CookieContainer = model.CookieContainer;
                request.AllowAutoRedirect = model.AllowRedirect;
                request.UserAgent = MyUserAgent;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");

                // add by tcguo
                //request.Headers.Add("Accept-Language", "zh-CN");
                //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

                if (!string.IsNullOrEmpty(model.FakeIP))
                {
                    request.Headers.Add("X-Forwarded-For", model.FakeIP);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.Cookies != null && response.Cookies.Count > 0)
                {
                    model.CookieContainer.Add(response.Cookies);
                }

                if (model.IsReadAll)
                {
                    Stream responseStream;
                    StreamReader reader;
                    string contentType = response.ContentType;
                    string contentEncoding = response.ContentEncoding;
                    if (contentEncoding == "gzip")
                    {
                        responseStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    }
                    else
                    {
                        responseStream = response.GetResponseStream();
                    }
                    string characterSet = response.CharacterSet;
                    if (characterSet == "")
                    {
                        reader = new StreamReader(responseStream, Encoding.Default);
                    }
                    else
                    {
                        characterSet = characterSet.Trim().ToLower();
                        string str7 = characterSet;
                        if (str7 == "gb2312")
                        {
                            reader = new StreamReader(responseStream, Encoding.GetEncoding("gb2312"));
                        }
                        else if ((str7 == "utf-8") || (str7 == "iso-8859-1"))
                        {
                            reader = new StreamReader(responseStream, Encoding.UTF8);
                        }
                        else
                        {
                            reader = new StreamReader(responseStream, Encoding.Default);
                        }
                    }
                    tempVal = reader.ReadToEnd();
                    reader.Close();
                    responseStream.Close();
                }

                response.Close();
                request = null;
                response = null;
                returnVal = tempVal;
            }
            catch (Exception)
            {
                returnVal = string.Empty;
            }

            return returnVal;
        }

        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static string UrlEncode(string url)
        {
            return HttpUtility.UrlEncode(url);
        }
    }
}
