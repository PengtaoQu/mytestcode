using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace MigrationEf
{
    public class sRestClient
    {
        private string BaseUri;
        private static string HeadKey = "";
        private static string Headvalue = "";
        public sRestClient(string baseUri)
        {
            this.BaseUri = baseUri;
        }

        #region Delete方式
        public string Delete(string data, string uri, out int status)
        {
            return CommonHttpRequest(data, uri, "DELETE",out status);
        }

        public string Delete(string uri)
        {
            //Web访问对象64
            string serviceUrl = string.Format("{0}/{1}", this.BaseUri, uri);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
            myRequest.Method = "DELETE";

            if (!string.IsNullOrEmpty(Headvalue))
                myRequest.Headers.Add(HeadKey, Headvalue);

            // 获得接口返回值68
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string ReturnXml = HttpUtility.UrlDecode(reader.ReadToEnd());
            string ReturnXml = reader.ReadToEnd();
            reader.Close();
            myResponse.Close();
            return ReturnXml;
        }
        #endregion

        #region Put方式
        public string Put(string data, string uri,out int status)
        {
            return CommonHttpRequest(data, uri, "PUT",out status);
        }
        #endregion

        #region POST方式实现

        public string Post(string data, string uri,out int status)
        {
         
            return CommonHttpRequest(data, uri, "POST",out status);
        }

        public string CommonHttpRequest(string data, string uri, string type,out int status)
        {

            //Web访问对象，构造请求的url地址
            string serviceUrl = string.Format("{0}/{1}", this.BaseUri, uri);

            try
            {
                //构造http请求的对象
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
                //转成网络流
                byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);
                //设置
                myRequest.Method = type;

                if (!string.IsNullOrEmpty(Headvalue))
                    myRequest.Headers.Add(HeadKey, Headvalue);

                myRequest.ContentLength = buf.Length;
                myRequest.ContentType = "application/json";
                myRequest.MaximumAutomaticRedirections = 1;
                myRequest.AllowAutoRedirect = true;
                // 发送请求
                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(buf, 0, buf.Length);
                newStream.Close();
                // 获得接口返回值
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string ReturnXml = reader.ReadToEnd();
                reader.Close();
                myResponse.Close();
                status = 1;
                return ReturnXml;
            }
            catch (Exception ex)
            {
                status = -1;
                return ex.Message;

            }
        }
        #endregion

        #region GET方式实现
        public string Get(string uri, IDictionary<string, string> data, out int status)
        {

            string serviceUrl = string.Format("{0}/{1}", this.BaseUri, uri);

            if (data != null && data.Count > 0)
            {
                StringBuilder param = new StringBuilder();
                int i = 0;
                foreach (var item in data)
                {
                    if (i == 0)
                        param.Append("?");
                    else
                        param.Append("&");
                    param.Append(item.Key).Append("=").Append(HttpUtility.UrlEncode(item.Value));
                    i++;
                }
                serviceUrl += param;
            }

            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
                myRequest.Timeout = 20000;

                if (!string.IsNullOrEmpty(Headvalue))
                    myRequest.Headers.Add(HeadKey, Headvalue);

                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

                string ReturnXml = reader.ReadToEnd();
                reader.Close();
                myResponse.Close();
                status = 1;
                return ReturnXml;
            }
            catch (Exception ex)
            {
                status = -1;
                return ex.Message;
            }

        }
        #endregion
        public string PostMulita()
        {

            using (var client = new HttpClient())
            {
                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
                    var values = new[]
                    {
                        new KeyValuePair<string, string>("c", "3"),
                        new KeyValuePair<string, string>("c", "2"),
                        new KeyValuePair<string, string>("d", "2")
    
                    };

                    foreach (var keyValuePair in values)
                    {
                        multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                            String.Format("\"{0}\"", keyValuePair.Key));
                    }

                    multipartFormDataContent.Add(new ByteArrayContent(File.ReadAllBytes(@"E:\CodeFirst\MigrationEf\MigrationEf\wwwroot\test.jpg")),
                        "\"pic\"",
                        "\"test.jpg\"");

                    var requestUri = "http://localhost:8080";
                    var html = client.PostAsync(requestUri, multipartFormDataContent).Result.Content.ReadAsStringAsync().Result;
                }
                return ";";
            }


        }
    }


}
