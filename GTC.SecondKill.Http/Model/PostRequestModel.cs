﻿/* ==============================================================================
 * 功能描述:
 * 创 建 者: gtc
 * 创建日期: 2013/9/11 13:50:43
 * 修 改 人:
 * 修改时间:
 * 修改备注:
 * @version 1.0
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace GTC.SecondKill.Http
{
    public class PostRequestModel
    {
        public string TheURL { get; set; }
        public string RefURL { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public string PostData { get; set; }
        public WebProxy WebProxy { get; set; }
        public bool AllowRedirect { get; set; }
        public bool IsReadAll { get; set; }
        public Encoding TheCode { get; set; }
        public string FakeIP { get; set; }
    }
}
