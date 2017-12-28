using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace sohan.Tools
{
    class HttpRequests
    {
        /// <summary>
        /// httpget请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string HttpGet(string Url, string getDataStr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (getDataStr == "" ? "" : "?") + getDataStr);
                request.Method = "GET";
               // request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.ContentType = "text/xml;charset = UTF-8";//提交xml    
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception ex)
            {
                // AddLog.UpdateLog(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// httpPost请求
        /// </summary>
        /// <param name="URL">地址</param>
        /// <param name="strPostdata">数据</param>
        /// <returns></returns>
        public static string PostReadWithHttps(string URL, string strPostdata)//, string strEncoding
        {
            try
            {
                Encoding encoding = Encoding.Default;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "post";
                //request.Accept = "";//获取标头的值
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
                request.Headers.Add("X-Requested-With: XMLHttpRequest");
                request.ContentType = "application/x-www-form-urlencoded";//提交xml 
                request.Accept= "application/json, text/javascript, */*; q=0.01";
                byte[] buffer = encoding.GetBytes(strPostdata);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                //得到 response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))//,Encoding.GetEncoding(strEncoding)
                {
                    string content = reader.ReadToEnd();
                    return content;
                }
            }
            catch (Exception ex)
            {
                //AddLog.UpdateLog(ex.Message);
            }
            return null;

        }

    }
}
