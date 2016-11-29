using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GTC.SecondKill.Http;
using System.Net;
using System.Collections;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using HtmlAgilityPack;
using ScrapySharp.Html;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace GTC.SecondKill
{
    public partial class Main : Form
    { 
        private CookieContainer cookie = new CookieContainer();

        private string cardName = string.Empty;
        private string cardID = string.Empty;
        private string cardAddr = string.Empty;
        private string mobileNum = string.Empty;
        private string proviceName = string.Empty;
        private string selectNum = string.Empty;
        private string priceLine = string.Empty;

        private HtmlAgilityPack.HtmlDocument _htmlDocument = null;
        StringBuilder sbMsg = new StringBuilder();
        Thread thread = null;

        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sbMsg.Clear();

            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
            }

            thread = new Thread(GotoKill);

            //Parametres para = new Parametres()
            //{
            //    RequestXml = requestXml,
            //    Index = _FileTableNames.IndexOf(item)
            //};

            thread.Start();
        }

        public void GotoKill()
        {
            cardName = this.txtCardName.Text.Trim();
            cardID = this.txtIdCard.Text.Trim();
            cardAddr = this.txtCardAddr.Text.Trim();

            selectNum = this.txtNum.Text.Trim();
            priceLine = this.txtPriceLine.Text.Trim();

            mobileNum = string.Empty;
            proviceName = string.Empty;

            string verifyCode = this.textBox1.Text.Trim();
            string userName = HttpHelper.UrlEncode(this.txtUserName.Text.Trim());
            string pwd = this.txtPwd.Text.Trim();

            if (string.IsNullOrEmpty(verifyCode))
            {
                MessageBox.Show("请输入验证码");
                return;
            }

            try
            {
                #region 1. 登录

                //1. 登录

                GetRequestModel login = new GetRequestModel()
                {
                    TheURL = string.Format("https://uac.10010.com/portal/Service/MallLogin?callback=jsonp1378882336174&userName={0}&password={1}&pwdType=01&productType=05&verifyCode={2}&redirectType=MALLLOGIN&areaCode=&redirectURL=http%3A%2F%2Fwww.10010.com&rememberMe=0&arrcity=%E4%B8%AD%E6%96%87%2F%E6%8B%BC%E9%9F%B3&iVersion=", userName, pwd, verifyCode),
                    RefURL = "",
                    AllowRedirect = true,
                    CookieContainer = cookie,
                    IsReadAll = true,
                    TheCode = System.Text.UTF8Encoding.UTF8,
                    WebProxy = null,
                    FakeIP = string.Empty
                };

                string json = HttpHelper.SendGetHttpRequest(login);
                string newjson = JsonHelper.JsonpString2Json(json);
                dynamic obj = JObject.Parse(newjson);

                string resultCode = obj.resultCode;
                if ("0000".Equals(resultCode))
                {
                    sbMsg.Append("登录成功！\r\n");
                }
                else
                {
                    sbMsg.Append("登录失败,请重新开始！\r\n");
                    if (this.pictureBox1.IsHandleCreated)
                    {
                        this.pictureBox1.Invoke((MethodInvoker)delegate()
                        {
                            this.pictureBox1.Image = GetVerifyCodeImage();
                        });
                    }
                }

                PrintMsg();
               
                #endregion

                #region 2. Get GoodDetail

                GetRequestModel goodDetailModel = new GetRequestModel()
                {
                    TheURL = "http://mall.10010.com/goodsdetail/111212076031.html",
                    RefURL = "",
                    AllowRedirect = true,
                    CookieContainer = cookie,
                    IsReadAll = true,
                    TheCode = System.Text.UTF8Encoding.UTF8,
                    WebProxy = null,
                    FakeIP = string.Empty
                };

                #endregion

                #region QueryAllNum

                string allNumUrl = "http://mall.10010.com/mall-web/GoodsComment/qryAllNum?goodsId=111212076031";

                GetRequestModel allNumModel = new GetRequestModel()
                {
                    TheURL = allNumUrl,
                    RefURL = string.Empty,
                    AllowRedirect = false,
                    CookieContainer = cookie,
                    IsReadAll = true,
                    TheCode = System.Text.UTF8Encoding.UTF8,
                    WebProxy = null,
                    FakeIP = string.Empty
                };
                string goodDetailResult = HttpHelper.SendGetHttpRequest(allNumModel);

                #endregion

                #region 1. occupyNumberAjax

                string occupyNumberdata = string.Format("error=false&goodsId=111212076031&cityName=%E5%8C%97%E4%BA%AC&cityCode=110&cityCode_Num=110&provinceCode=11&merchantId=1100000&usimPrice=0.0&brandCode=AP&moduleCode=iP516G&colorValue=9809120800036799&activityId=20130912202837015682&actType=4&preStoredFee=1500&allFee=3600&preSentedFee=0&firstMonthSentFee=0&perMonthSentFee=62&resouresFee=3199&activityProtper=24&productId=99003572&productType=iPhone&productValue=96&ARTICLE_CITY=110&numGroup=&numFrom=A000020V000001&initNumGroup=&isPreStore=&difPlace=0&localFee=0&remoteFee=0&isOnlinepay=1&isReceivepay=0&saleActType=0&defProVal=286&preFeeVal=0&numValue={0}&numMemo=%E9%9D%93%E5%8F%B7%E9%A2%84%E5%AD%98%E6%AC%BE0%E5%85%83%E4%BB%8E%E5%85%A5%E7%BD%91%E6%AC%A1%E6%9C%88%E8%B5%B7%E5%88%8612%E4%B8%AA%E6%9C%88%E8%BF%94%E8%BF%98&niceRuleTag=0&pirceRule=2&goodsPreFeeVal=0&goodsOldPreFee=0&goodsMorePayRestore=0&goodsUsimPrice=0&goodsTotalPrice=4499000&province=11&oldPreFee=&custTag=1", selectNum);

                PostRequestModel occupyNumberajax = new PostRequestModel()
                {
                    TheURL = "http://mall.10010.com/mall-web/GoodsDetailAjax/occupyNumberAjax ",
                    RefURL = "",
                    AllowRedirect = true,
                    CookieContainer = cookie,
                    IsReadAll = true,
                    TheCode = System.Text.UTF8Encoding.UTF8,
                    WebProxy = null,
                    FakeIP = string.Empty,
                    PostData = occupyNumberdata
                };

                string ajaxResult = HttpHelper.SendPostHttpRequest(occupyNumberajax);
                if (!string.IsNullOrEmpty(ajaxResult) && ajaxResult.Contains("SUCCESS"))
                {
                    sbMsg.Append("占用号码成功！\r\n");
                }
                else
                {
                    sbMsg.Append("占用号码失败, 速度去请重选号，重新开始！\r\n");
                    if (this.pictureBox1.IsHandleCreated)
                    {
                        this.pictureBox1.Invoke((MethodInvoker)delegate()
                        {
                            this.pictureBox1.Image = GetVerifyCodeImage();
                        });
                    }
                }
                PrintMsg();

                #endregion

                #region 2. promtlyBuy
                //
                //goodsId=111212076031&activityId=20130912202837015682
                //&activityType=4&activityProtper=24&provinceCode=11
                //&cityCode=110&number=18510162469
                //&numGroup=&numFrom=A000020V000001&numberFee=0
                //&preFeeVal=&oldPreFee=0&productId=99003572&difPlace=0
                //&localFee=0&remoteFee=0&isOnlinepay=1&isReceivepay=0
                //&totalPrice=4499000&usimPrice=0&productType=iPhone&productValue=96
                //&custTag=1&brandCode=AP&moduleCode=iP516G&colorCode=9809120800036799
                //&merchantId=1100000&tmpId=60000009&inventoryType=1&privilegePack=

                string colorCode = string.Empty;
                if (this.rbtnColorWhite.Checked)
                {
                    colorCode = "9809120800036821";
                }
                else
                    colorCode = "9809120800036799";

                var buyData = new StringBuilder();
                buyData.AppendFormat("goodsId={0}", "111212076031");
                buyData.AppendFormat("&activityId={0}", "20130912202837015682");
                buyData.AppendFormat("&activityType={0}", 4);
                buyData.AppendFormat("&activityProtper={0}", 24);
                buyData.AppendFormat("&provinceCode={0}", 11);
                buyData.AppendFormat("&cityCode={0}", 110);
                buyData.AppendFormat("&number={0}", selectNum);
                buyData.AppendFormat("&numGroup=", string.Empty);
                buyData.AppendFormat("&numFrom={0}", "A000020V000001");
                buyData.AppendFormat("&numberFee={0}", 0);
                buyData.AppendFormat("&preFeeVal={0}", string.Empty);
                buyData.AppendFormat("&oldPreFee={0}", 0);
                buyData.AppendFormat("&productId={0}", "99003572");
                buyData.AppendFormat("&difPlace={0}", 0);
                buyData.AppendFormat("&localFee={0}", 0);
                buyData.AppendFormat("&remoteFee={0}", 0);
                buyData.AppendFormat("&isOnlinepay={0}", 1);
                buyData.AppendFormat("&isReceivepay={0}", 0);
                buyData.AppendFormat("&totalPrice={0}", "4499000");
                buyData.AppendFormat("&usimPrice={0}", 0);
                buyData.AppendFormat("&productType={0}", "iPhone");
                buyData.AppendFormat("&productValue={0}", 96);
                buyData.AppendFormat("&custTag={0}", 1);
                buyData.AppendFormat("&brandCode={0}", "AP");
                buyData.AppendFormat("&moduleCode={0}", "iP516G");
                buyData.AppendFormat("&colorCode={0}", colorCode);
                buyData.AppendFormat("&merchantId={0}", "1100000");
                buyData.AppendFormat("&tmpId={0}", "60000009");
                buyData.AppendFormat("&inventoryType={0}", 1);
                buyData.AppendFormat("&privilegePack={0}", string.Empty);

                //获取相关表单数据
                PostRequestModel promtlyBuyModel = new PostRequestModel()
                {
                    TheURL = "http://mall.10010.com/mall-web/GoodsDetail/promtlyBuy",
                    RefURL = "",
                    AllowRedirect = true,
                    CookieContainer = cookie,
                    IsReadAll = true,
                    TheCode = System.Text.UTF8Encoding.UTF8,
                    WebProxy = null,
                    FakeIP = string.Empty,
                    PostData = buyData.ToString()
                };

                #endregion

                HtmlNode html = null;

                while (true)
                {
                    html = null;

                    string priceVal = GetPrice(promtlyBuyModel, out html);
                    if (string.IsNullOrEmpty(priceVal))
                    {
                        if (!sbMsg.ToString().Contains("沃妹"))
                        {
                            sbMsg.Append("沃妹，陪你一起等...继续监控价格...\r\n");
                            PrintMsg();
                        }
                    }
                    else
                    {
                        if (!sbMsg.ToString().Contains("进入填写订单页面成功"))
                        {
                            sbMsg.Append("进入填写订单页面成功, 正在监控价格....！\r\n");
                            this.button1.Invoke((MethodInvoker)delegate()
                            {
                                this.button1.Enabled = false;   
                            });
                            PrintMsg();
                        }

                        if (double.Parse(priceVal) < double.Parse(priceLine))
                        {
                            HtmlNode node_type = html.SelectSingleNode("/html[1]/body[1]/input[@id='inventoryType']");
                            HtmlNode node_token = html.SelectSingleNode("/html[1]/body[1]/input[@id='_m_token']");
                            HtmlNode node_state = html.SelectSingleNode("/html[1]/body[1]/input[@id='_m_state']");

                            string typeVal = node_type.Attributes["value"].Value;
                            string tokenVal = node_token.Attributes["value"].Value;
                            string stateNameVal = node_state.Attributes["name"].Value;
                            string stateVal = node_state.Attributes["value"].Value;

                            // 提交订单
                            bool isOk = ToKill(typeVal, tokenVal, stateNameVal, stateVal);

                            if (isOk) break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sbMsg.AppendFormat("我去。。服务器不响应啦,详细信息：{0}！\r\n", ex.Message);
                PrintMsg();
            }
        }
       
        private string GetPrice(PostRequestModel promtlyBuyModel, out HtmlNode html)
        {
            string priceVal = string.Empty;
            try
            {
                string formDataHtml = HttpHelper.SendPostHttpRequest(promtlyBuyModel);
                _htmlDocument.LoadHtml(formDataHtml);
                html = _htmlDocument.DocumentNode;

                HtmlNode node_price = html.SelectSingleNode("//span[@id='billingResult']");
                priceVal = node_price.InnerText.ToString();
            }
            catch (Exception)
            {
                html = null;
            }

            return priceVal;

            //HtmlNode titleNode = html.SelectSingleNode("//title");
            //string orderTitle = titleNode.InnerText;
            //if ("订单资料填写_中国联通_联通商城".Equals(orderTitle)) return true;

            //return false;
        }

        private bool ToKill(string typeVal, string tokenVal, string stateNameVal, string stateVal)
        {
            #region 3. PrePurchase47003271

            GetRequestModel purchase = new GetRequestModel()
            {
                TheURL = "http://mall.10010.com/PrePurNotice/1100000/PrePurchase47003271.html",
                RefURL = "",
                AllowRedirect = true,
                CookieContainer = cookie,
                IsReadAll = true,
                TheCode = System.Text.UTF8Encoding.UTF8,
                WebProxy = null,
                FakeIP = string.Empty
            };
            string purchaseResult = HttpHelper.SendGetHttpRequest(purchase);

            // 
            GetRequestModel initRequest = new GetRequestModel()
            {
                TheURL = "http://mall.10010.com/mall-web/FrontInput/init",
                RefURL = "",
                AllowRedirect = false,
                CookieContainer = cookie,
                IsReadAll = true,
                TheCode = System.Text.UTF8Encoding.UTF8,
                WebProxy = null,
                FakeIP = string.Empty
            };

            string initResult = HttpHelper.SendGetHttpRequest(initRequest);

            #endregion

            #region 4. ShowAddrList

            //3.post 获取收货地址 http://mall.10010.com/mall-web/OrderInputAjax/ShowAddrList 

            PostRequestModel addrModel = new PostRequestModel()
            {
                TheURL = "http://mall.10010.com/mall-web/OrderInputAjax/ShowAddrList",
                RefURL = "",
                AllowRedirect = true,
                CookieContainer = cookie,
                IsReadAll = true,
                TheCode = System.Text.UTF8Encoding.UTF8,
                WebProxy = null,
                FakeIP = string.Empty,
                PostData = string.Empty
            };

            string myaddr = string.Empty;

            try
            {
                myaddr = HttpHelper.SendPostHttpRequest(addrModel);
                if (!string.IsNullOrEmpty(myaddr))
                {
                    myaddr = myaddr.Replace("[", "").Replace("]", "");
                }

                dynamic addr = JObject.Parse(myaddr);
                mobileNum = addr.MobilePhone;
                proviceName = addr.ProvinceName;

                sbMsg .Append("第三步成功：获得收货地址成功\r\n");
                PrintMsg();
            }
            catch (Exception)
            {
                sbMsg.Append("第三步成功：获得收货地址失败\r\n");
                PrintMsg();
                return false;
            }
            #endregion

            #region 5. checkCustInfo

            // 4. post POST http://mall.10010.com/mall-web/OrderInputAjax/checkCustInfo 
            // post data: psptTypeCode=02&idCardVal=412823198601021214&provinceCode=11&cityCode=110&CertAdress=%E6%B2%B3%E5%8D%97%E7%9C%81%E9%81%82%E5%B9%B3%E5%8E%BF%E8%BD%A6%E7%AB%99%E9%95%87%E4%BA%BA%E6%89%8D%E4%BA%A4%E6%B5%81%E4%B8%AD%E5%BF%83&goodsId=111212076031&tmplId=60000009

            string cardVal = HttpHelper.UrlEncode(cardID);
            string cardAddress = HttpHelper.UrlEncode(cardAddr);
            string custPostData = string.Format("psptTypeCode=02&idCardVal={0}&provinceCode=11&cityCode=110&CertAdress={1}&goodsId=111212076031&tmplId=60000009", cardVal, cardAddress);

            PostRequestModel custModel = new PostRequestModel()
            {
                TheURL = "http://mall.10010.com/mall-web/OrderInputAjax/checkCustInfo",
                RefURL = "",
                AllowRedirect = true,
                CookieContainer = cookie,
                IsReadAll = true,
                TheCode = System.Text.UTF8Encoding.UTF8,
                WebProxy = null,
                FakeIP = string.Empty,
                PostData = custPostData
            };

            string custResult = HttpHelper.SendPostHttpRequest(custModel);
            if (custResult.Contains("0000"))
            {
                sbMsg.Append("第四步：检查用户信息成功\r\n");
                PrintMsg();
            }
            else
            {
                sbMsg.Append("第四步：检查用户信息失败\r\n");
                PrintMsg();
                return false;
            }

            #endregion

            #region 6. hasReceivePay

            // POST http://mall.10010.com/mall-web/OrderInputAjax/hasReceivePay 
            // data: AddrCityCode=110&NumberCityCode=110&goodsId=111212076031

            string receivePostData = "AddrCityCode=110&NumberCityCode=110&goodsId=111212076031";
            PostRequestModel receiveModel = new PostRequestModel()
            {
                TheURL = "http://mall.10010.com/mall-web/OrderInputAjax/hasReceivePay",
                RefURL = "",
                AllowRedirect = true,
                CookieContainer = cookie,
                IsReadAll = true,
                TheCode = System.Text.UTF8Encoding.UTF8,
                WebProxy = null,
                FakeIP = string.Empty,
                PostData = receivePostData
            };
            string receiveResult = HttpHelper.SendPostHttpRequest(receiveModel);
            if (receiveResult.ToLower() == "true")
            {
                sbMsg.Append("第五步，确认信息成功\r\n");
                PrintMsg();
            }
            else
            {
                sbMsg.Append("第五步，确认信息失败\r\n");
                PrintMsg();
                return false;
            }

            #endregion

            #region 7. toCheckUserLegal

            // 6. post POST http://mall.10010.com/mall-web/OrderInputAjax/toCheckUserLegal
            //data :tel=18610743176&pCardCode=412823198601021214&goodsId=111212076031&province=11&city=110
            //result :{"checkLegalResult":"0"}

            string checkUserPostData = string.Format("tel={0}&pCardCode={1}&goodsId=111212076031&province=11&city=110", mobileNum, cardID);
            PostRequestModel checkUserDataModel = new PostRequestModel()
            {
                TheURL = " http://mall.10010.com/mall-web/OrderInputAjax/toCheckUserLegal",
                RefURL = "",
                AllowRedirect = true,
                CookieContainer = cookie,
                IsReadAll = true,
                TheCode = System.Text.UTF8Encoding.UTF8,
                WebProxy = null,
                FakeIP = string.Empty,
                PostData = checkUserPostData
            };
            string checkUserResult = HttpHelper.SendPostHttpRequest(checkUserDataModel);
            if (!string.IsNullOrEmpty(checkUserResult) && checkUserResult.ToLower() != "false")
            {
                dynamic result = JObject.Parse(checkUserResult);
                string checkLegalResult = result.checkLegalResult;

                if (!string.IsNullOrEmpty(checkUserResult) && "0".Equals(checkLegalResult))
                {
                    sbMsg.Append("第六步，确认信息成\r\n");
                    PrintMsg();
                }
                else
                {
                    sbMsg.AppendFormat("第六步，确认信息失败,结果为：{0}\r\n", checkLegalResult);
                    PrintMsg();
                }
            }
            else
            {
                sbMsg.Append("第六步，确认信息失败\r\n");
                PrintMsg();
                return false;
            }

            #endregion

            #region 8. submitOrder


            // 8. 提交订单
            //POST http://mall.10010.com/MallApp/OrderSubmit/submitOrder
            //paramStr=%7B%22payment%22%3A%7B%22payTypeCode%22%3A%2202%22%2C%22payWayCode%22%3A%2202%22%2C%22payInstalmentBankCode%22%3A%22%22%2C%22payInstalmentTerm%22%3A%22%22%7D%2C%22delivery%22%3A%7B%22dispachCode%22%3A%2201%22%2C%22dlvTypeCode%22%3A%2202%22%7D%2C%22networkData%22%3A%7B%22hostName%22%3A%22%E9%83%AD%E5%A4%A9%E6%B1%A0%22%2C%22idCard%22%3A%22412823198601021214%22%2C%22psptTypeCode%22%3A%2202%22%2C%22idCardAddress%22%3A%22%E6%B2%B3%E5%8D%97%E7%9C%81%E9%81%82%E5%B9%B3%E5%8E%BF%E8%BD%A6%E7%AB%99%E9%95%87%E4%BA%BA%E6%89%8D%E4%BA%A4%E6%B5%81%E4%B8%AD%E5%BF%83%22%7D%2C%22nowGiftValue%22%3A0%2C%22postAddr%22%3A%7B%22CityName%22%3A%22%E5%8C%97%E4%BA%AC%E5%B8%82%22%2C%22UpdateTime%22%3A%222013-09-09T16%3A47%3A48%22%2C%22CityCode%22%3A%22110100%22%2C%22ReceiverPsptNo%22%3Anull%2C%22EssProvinceCode%22%3A%2211%22%2C%22PostId%22%3A%226613090903413783%22%2C%22PartitionId%22%3A%2216%22%2C%22Email%22%3Anull%2C%22DefaultTag%22%3A%221%22%2C%22PostCode%22%3A%22100192%22%2C%22EssDistrictCode%22%3Anull%2C%22ProvinceCode%22%3A%22110000%22%2C%22PostAddr%22%3A%22%E8%A5%BF%E5%B0%8F%E5%8F%A3%E8%B7%AF66%E5%8F%B7%E4%B8%AD%E5%85%B3%E6%9D%91%E4%B8%9C%E5%8D%87%E7%A7%91%E6%8A%80%E5%9B%AD%E5%8C%97%E9%A2%86%E5%9C%B0A2%22%2C%22FixPhone%22%3A%22%22%2C%22DistrictName%22%3A%22%E6%B5%B7%E6%B7%80%E5%8C%BA%22%2C%22MobilePhone%22%3A%2218610743176%22%2C%22ProvinceName%22%3A%22%E5%8C%97%E4%BA%AC%22%2C%22DistrictCode%22%3A%22110108%22%2C%22ReceiverName%22%3A%22%E9%83%AD%E5%A4%A9%E6%B1%A0%22%2C%22ReceiverPsptType%22%3Anull%2C%22EssCityCode%22%3A%22110%22%7D%2C%22billingInfo%22%3A%7B%22
            //moneyCardNum%22%3A%22%22%2C%22invoiceTitle%22%3A%22%22%2C%22invoiceContent%22%3A%2201%22%2C%22
            //orderRemarks%22%3A%22%22%2C%22referrerName%22%3A%22%22%2C%22referrerCode%22%3A%22%22%7D%7D
            
            //&inventoryType=1
            //&_m_token=ed9c4d66-2852-45c8-b5b8-b7100aa858c4
            //&ByMD4R231fKxyQCdAeGZr0FpAyaiQCygOWKMKAkjv%2BS3td6VZSmcaV6uJhw%2F6CkI=DMkHbQg6LJ%2FiO5hpCu66iyb%2FBuy0pocJE2Q6hCTyKw2zlfo4moMnLocFXsXxlK%2F%2F3rkJlFWoBpc2xOUUJfR%2B0TRBBGovmE4itckatjofcRlp4jYpsuxBvJFjdFuqO6M%2BYsJtBHR%2Fx3UQw0MuzbjXpw%3D%3D

            // result :<html><head><title>302 Moved Temporarily</title></head>
            //<body bgcolor="#FFFFFF">
            //<p>This document you requested has moved temporarily.</p>
            //<p>It's now at <a href="http://mall.10010.com/MallApp/OrderSuccess/showOrderInfo?enPara=rCcGwVYlHnUoJzFV1qCoK6yXYs66UKL0ZyiD&#37;2F&#37;2FrF9gY&#37;3D">http://mall.10010.com/MallApp/OrderSuccess/showOrderInfo?enPara=rCcGwVYlHnUoJzFV1qCoK6yXYs66UKL0ZyiD&#37;2F&#37;2FrF9gY&#37;3D</a>.</p>
            //</body></html>

            StringBuilder orderData = new StringBuilder();
            orderData.Append("{");
            //付款方式
            orderData.Append("\"payment\":{\"payTypeCode\":\"02\",\"payWayCode\":\"01\",\"payInstalmentBankCode\":\"\",\"payInstalmentTerm\":\"\"},");
            //配送方式
            orderData.Append("\"delivery\":{\"dispachCode\": \"01\", \"dlvTypeCode\":\"02\" },");

            //身份证
            orderData.Append("\"networkData\":{\"hostName\": \"" + cardName + "\",\"idCard\":\"" + cardID + "\",\"psptTypeCode\":\"02\",\"idCardAddress\":\"" + cardAddr + "\" },");
            //配货地址
            orderData.AppendFormat("\"postAddr\": {0},", myaddr);
            //发票
            orderData.Append("\"billingInfo\":{");
            orderData.Append("\"moneyCardNum\":\"\",\"invoiceTitle\":\"\",");
            orderData.AppendFormat("\"invoiceContent\":{0},", "\"01\"");
            orderData.Append("\"orderRemarks\":\"\", \"referrerName\":\"\", \"referrerCode\":\"\" ");
            orderData.Append("}");

            orderData.Append("}");

            string paramStr = HttpHelper.UrlEncode(orderData.ToString());
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("paramStr={0}", paramStr);

            sb.AppendFormat("&inventoryType={0}", typeVal);
            sb.AppendFormat("&_m_token={0}", tokenVal);
            sb.AppendFormat("&{0}={1}",HttpHelper.UrlEncode(stateNameVal),HttpHelper.UrlEncode(stateVal));

            string submitPostData = sb.ToString();

            PostRequestModel submitOrder = new PostRequestModel()
            {
                TheURL = "http://mall.10010.com/mall-web/OrderSubmit/submitOrder",
                RefURL = "",
                AllowRedirect = true,
                CookieContainer = cookie,
                IsReadAll = true,
                TheCode = System.Text.UTF8Encoding.UTF8,
                WebProxy = null,
                FakeIP = string.Empty,
                PostData = submitPostData
            };

            string submitResult = HttpHelper.SendPostHttpRequest(submitOrder);

            _htmlDocument.LoadHtml(submitResult);

            HtmlNode titleNode = _htmlDocument.DocumentNode.SelectSingleNode("//title");
            string title = titleNode.InnerText;
            if (!string.IsNullOrEmpty(title) && "购买成功_中国联通_联通商城".Equals(title.Trim()))
            {
                sbMsg.Append("订单秒杀成功！速速请吃饭\r\n");
                PrintMsg();
                return true;
            }

            sbMsg.Append("悲剧了，秒杀失败\r\n");
            PrintMsg();
            
            #endregion

            return false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = GetVerifyCodeImage();
            _htmlDocument =  new HtmlAgilityPack.HtmlDocument();

            //加载初始信息
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "UserInfo.json";

            
            FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs,System.Text.UTF8Encoding.UTF8);

            try
            {
                string userInfo = sr.ReadToEnd().Trim().Replace("\r", "").Replace("\n", "");
                if (!string.IsNullOrEmpty(userInfo) && !string.IsNullOrWhiteSpace(userInfo))
                {
                    dynamic user = JObject.Parse(userInfo);
                    this.txtUserName.Text = user.UserName;
                    this.txtPwd.Text = user.Password;
                    this.txtCardName.Text = user.CardName;
                    this.txtCardAddr.Text = user.CardAddress;
                    this.txtIdCard.Text = user.CardID;
                    this.txtNum.Text = user.NewMobile;
                    this.txtPriceLine.Text = user.Price;
                }
            }
            catch (Exception)
            {
                sr.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = GetVerifyCodeImage();
        }

        private Bitmap GetVerifyCodeImage()
        {
            ImageRequestModel image = new ImageRequestModel()
            {
                ImgUrl = "https://uac.10010.com/portal/Service/CreateImage",
                RefURL = "",
                AllowRedirect = false,
                CookieContainer = cookie,
                WebProxy = null,
                FakeIP = string.Empty
            };

            return HttpHelper.ImgageHttpWebRequest(image);
        }

        private void PrintMsg()
        {
            if (this.txtMsg.IsHandleCreated)
            {
                this.txtMsg.Invoke((MethodInvoker)delegate()
                {
                    this.txtMsg.Text = sbMsg.ToString();
                });
            }
        }
    }
}
