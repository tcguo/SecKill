using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GTC.SecondKill.Http
{
    public class RequestModelFactory
    {
        public static GetRequestModel CreateGetModel(string url, CookieContainer cookie,  string refUrl = "",
                                              WebProxy proxy = null, bool allowRedirect = true,
                                              bool isReadAll = true, string fakeIP = "")
        {

            return new GetRequestModel()
            {
                TheURL = url,
                RefURL = refUrl,
                CookieContainer = cookie,
                WebProxy = proxy,
                AllowRedirect = allowRedirect,
                IsReadAll = isReadAll,
                TheCode = UTF8Encoding.UTF8,
                FakeIP = fakeIP
            };
        }

        public static PostRequestModel CreatePostModel(string url, string postData, CookieContainer cookie,
                                              WebProxy proxy = null, bool allowRedirect = true, string refUrl = "",
                                              bool isReadAll = true, string fakeIP = "")
        {

            PostRequestModel submitOrder = new PostRequestModel()
            {
                TheURL = url,
                RefURL = refUrl,
                AllowRedirect = true,
                CookieContainer = cookie,
                IsReadAll = true,
                TheCode = System.Text.UTF8Encoding.UTF8,
                WebProxy = null,
                FakeIP = string.Empty,
                PostData = postData
            };

            return submitOrder;
        }
    }
}
