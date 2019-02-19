using log4net;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MigrationEf
{
 
    public class RestApi<T> where T : new()
    {
        private ILog _log;
        private string BaseUrl;

        public RestApi()
        {
            this._log = LogManager.GetLogger(Startup.repository.Name, typeof(restSharp));

            //添加 json 文件路径
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            //创建配置根对象
            var configurationRoot = builder.Build();
            BaseUrl = configurationRoot["PBaseUrl"];
        }

        public T Get(string url, object pars, Dictionary<string, string> header)
        {
            var type = Method.GET;
            IRestResponse<T> reval = GetApiInfo(url, pars, header, type);
            return reval.Data;
        }
        public T Post(string url, object pars, Dictionary<string, string> header)
        {
            var type = Method.POST;
            IRestResponse<T> reval = GetApiInfo(url, pars, header, type);
            return reval.Data;
        }
        public T Delete(string url, object pars, Dictionary<string, string> header)
        {
            var type = Method.DELETE;
            IRestResponse<T> reval = GetApiInfo(url, pars, header, type);
            return reval.Data;
        }
        public T Put(string url, object pars, Dictionary<string, string> header)
        {
            var type = Method.PUT;
            IRestResponse<T> reval = GetApiInfo(url, pars, header, type);
            return reval.Data;
        }

        private IRestResponse<T> GetApiInfo(string url, object pars, Dictionary<string, string> header, Method type)
        {
            var request = new RestRequest(type);

            if (header != null && header.Count > 0)
            {
                foreach (var item in header)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }

            switch (type)
            {

                case Method.GET:
                    request.AddObject(pars);
                    break;
                default:
                    request.AddJsonBody(pars);
                    break;
            }
              
  
            var client = new RestClient(BaseUrl + url);
            client.CookieContainer = new System.Net.CookieContainer();
            IRestResponse<T> reval = client.Execute<T>(request);
            if (reval.ErrorException != null)
            {
                _log.Error("请求出错", reval.ErrorException);
                throw new Exception("请求出错");
            }
            return reval;
        }
    }
}
