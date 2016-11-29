using GTC.SecondKill.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace GTC.SecondKill
{
    public partial class XiaoMi : Form
    {
        private CookieContainer cookie = new CookieContainer();
        StringBuilder sbMsg = new StringBuilder();
        private string account = "tianchi021@163.com";

        public XiaoMi()
        {
            InitializeComponent();
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            GetRequestModel index = RequestModelFactory.CreateGetModel("http://www.xiaomi.com", cookie);
            string resIndex = HttpHelper.SendGetHttpRequest(index);

            string loginUrl = "https://account.xiaomi.com/pass/serviceLoginAuth2";
            string loginData = "user=tianchi021%40163.com&sid=eshop&callback=http%3A%2F%2Forder.xiaomi.com%2Flogin%2Fcallback%3Ffollowup%3Dhttp%253A%252F%252Fwww.xiaomi.com%252Findex.php%26sign%3DZjEwMWVlOTY3MWM1OGE3YjYxNGRiZjQ5MzJmYjI5NDE0ZWY0NzY5Mw%2C%2C&_sign=g7K1HSZPYIaO4tSlhS1xdDJBPV8%3D&_json=true&pwd=tianchi860102";

            var login = RequestModelFactory.CreatePostModel(loginUrl, loginData, cookie);
            string prejson = HttpHelper.SendPostHttpRequest(login);

            //排队
            string lineUrl = string.Format("http://tc.hd.xiaomi.com/hdget?callback=hdcontrol&_={0}", account);
            var lineMod = RequestModelFactory.CreateGetModel(lineUrl, cookie);
            string lineRes = HttpHelper.SendGetHttpRequest(lineMod);

            // 选机型

            // 提交订单

            // 是否成功

            //1. 登录
            //string loginURL = "http://order.xiaomi.com/login/callback?followup=http%3A%2F%2Fwww.xiaomi.com%2F&sign=Mjk4NmVlYzNiOWY5YTkwMjExNzg5MjkyNzE3ZmIxOGM4YTBiMDk1Mg%2C%2C&pwd=1&usr=tianchi021%40163.com&auth=B%2F1iGdQ7qBtzATJcGsb1pVouCLK%2BxxMZrgO3sPCdLpyHz1kf%2F8MTlYKPeq5MSLR0N5sPf0MAKq%2F5OmtiEnUCmV0%2FCiL1FaH1tyyUxj1hJ1GHASy74wj%2BPollseDmK3FU2qDo%2FQlRzCqaGGU1T5hMvIbIitMc4zoLp1fvT7%2FYLj8%3D&nonce=ehf3r4%2Fec6MBX6J%2B&_ssign=Ql%2Bnv7fZqyJHb1ONszX0ImMW9Bs%3D";
            //GetRequestModel login = new GetRequestModel()
            //{
            //    TheURL = loginURL,
            //    RefURL = "",
            //    AllowRedirect = true,
            //    CookieContainer = cookie,
            //    IsReadAll = true,
            //    TheCode = System.Text.UTF8Encoding.UTF8,
            //    WebProxy = null,
            //    FakeIP = string.Empty
            //};

            //string json = HttpHelper.SendGetHttpRequest(login);
            //string newjson = JsonHelper.JsonpString2Json(json);
            //dynamic obj = JObject.Parse(newjson);

            //string resultCode = obj.resultCode;
            //if ("0000".Equals(resultCode))
            //{
            //    sbMsg.Append("登录成功！\r\n");
            //}
            //else
            //{
            //    sbMsg.Append("登录失败,请重新开始！\r\n");
            //}
        }
    }
}
