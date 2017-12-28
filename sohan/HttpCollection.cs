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
using log4net;
using log4net.Config;
using System.Reflection;

namespace sohan
{
    class HttpCollection
    {
       public static XRequest reuest = new XRequest(new CookieContainer());
       private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

                if(content.Contains("success"))
                {
                    Log.Info("登录成功!");
                    Console.WriteLine("登录成功!");
                }
                else
                {
                    Log.Info("登录失败!");
                    Console.WriteLine("登录失败!");
                }

                string time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                reuest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                Log.Info("访问:http://oshop.sohan.hk/admin/listTacclist.do");
                reuest.Referer = "http://oshop.sohan.hk/admin/listTacclist.do";
                Log.Info("开始查寻账单");
                string queryHtml = reuest.GetHtml($"http://oshop.sohan.hk/admin/listTacclists.do?event=&tfeeTypeId=-1&startDate={time}&pageId=0&endDate={time}");
                if (queryHtml!=null&&queryHtml.Contains("缴费成本"))
                {
                    string readLine = null;
                    Log.Info("开始导出账单");
                    var response = reuest.GetStream($"http://oshop.sohan.hk/admin/excelTacclist.do?event=&tfeeTypeId=-1&startDate={time}&endDate={time}");
                  
                    using (StreamReader reader = new StreamReader(response, Encoding.GetEncoding("gb2312")))//,Encoding.GetEncoding(strEncoding)
                    {
                        readLine = reader.ReadToEnd();
                    }

                    FileStream fs = new FileStream("账目清单" + time + ".csv", FileMode.Create,FileAccess.Write);
                    StreamWriter bw = new StreamWriter(fs, Encoding.GetEncoding("gb2312"));
                    bw.Write(readLine);
                    bw.Close();
                    fs.Close();
                    Log.Info("Excel导出成功!");
                    Console.WriteLine("Excel导出成功!");
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error(ex.Message);

            }
        }

        public void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件 
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }
        
    }
}
