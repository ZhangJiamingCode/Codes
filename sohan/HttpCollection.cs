using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using sohan.Tools;
using Ejiaofei.Tools;
using Newtonsoft.Json;
using sohan.SoHanClass;
using HtmlAgilityPack;

namespace sohan
{
    class HttpCollection
    {
       public static XRequest reuest = new XRequest(new CookieContainer());

        /// <summary>
        /// 登录
        /// </summary>
        public static void LoginPT()
        {
            try
            {
                
                string url = "http://oshop.sohan.hk/shop-login.do";//账号登录
                string account = "EX_18606592298";
                var verifycode = "2393";
                string fresh = "0.9467887859438537";
                string password = "sr7852369";
                string passwordkey = Md5.StringToMD5Hash(verifycode + "" + Md5.StringToMD5Hash(password).ToLower()).ToLower();
                string postDataStr = $"verifycode={verifycode}&usbname=&macsn=&passwordkey={passwordkey}&account={account}&fresh={fresh}&loginErrorNum=";
                string content = reuest.PostHtml(url, postDataStr);

                string time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                reuest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                reuest.Referer = "http://oshop.sohan.hk/admin/listTacclist.do";
                string queryHtml = reuest.GetHtml($"http://oshop.sohan.hk/admin/listTacclists.do?event=&tfeeTypeId=-1&startDate={time}&pageId=0&endDate={time}");
                if (queryHtml!=null&&queryHtml.Contains("缴费成本"))
                {
                    var response = reuest.GetStream($"http://oshop.sohan.hk/admin/excelTacclist.do?event=&tfeeTypeId=-1&startDate={time}&endDate={time}");


                    FileStream fileStream = new FileStream("账目清单"+time+".csv",FileMode.Create,FileAccess.Write);

                }
                //HtmlDocument doc = new HtmlDocument();
                //doc.LoadHtml(queryHtml);
                //var tableTrAll = doc.DocumentNode.SelectNodes("//table/tr");

                //foreach ( var oneTr in tableTrAll)
                //{
                //    doc.LoadHtml(oneTr.InnerHtml);

                //    var td = doc.DocumentNode.SelectNodes("//td");
                //    var ss =td[1].InnerHtml;
                //}










            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

             // AddLog.UpdateLog(ex.Message);
            }
        }
        


    }
}
