/* ==============================================================================
 * 功能描述:
 * 创 建 者: gtc
 * 创建日期: 2013/9/12 14:38:04
 * 修 改 人:
 * 修改时间:
 * 修改备注:
 * @version 1.0
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;
using ScrapySharp.Html;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System.Net;
using System.IO;

namespace GTC.SecondKill
{
    public class ScrapySharpTest
    {
        public string Test(string baidu)
        {
            var uri = new Uri("http://diy.118100.cn/");
            var browser = new ScrapingBrowser();
            browser.AllowMetaRedirect = false;
            browser.AutoDownloadPagesResources = false;
            browser.AllowAutoRedirect = false;
            browser.KeepAlive = true;
            browser.IgnoreCookies = false;
            //browser.SendChunked = true;
            //browser.TransferEncoding = "utf-8";
            //browser.Headers.Add();

            string myhtml = browser.DownloadString(uri);

            Cookie cookie = browser.GetCookie(uri, "");

            WebResponse respsonse = browser.ExecuteRequest(uri, HttpVerb.Post, "");
            
            //respsonse.

            //Stream stream = respsonse.GetResponseStream();
            //StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("gb2312"));

            //string html1 = reader.ReadToEnd();


            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(baidu);

            var html = htmlDocument.DocumentNode;

            var title = html.CssSelect("title");
            var lk = html.CssSelect("#lk").First();
            var nodes = lk.CssSelect("a");

            foreach (var item in nodes)
            {
                string href = item.GetAttributeValue("href");
                string name = item.Name;
                string value = item.InnerText;
            }
            foreach (var htmlNode in title)
            {
                return htmlNode.InnerHtml;
            }

            return null;
        }
       
    }
}
