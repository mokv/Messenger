using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.DataAccess
{
    public class BaseAccess
    {
        protected string mainUrl = ConfigurationManager.AppSettings.Get("serverMainUrl") + "/api";
        
        protected async Task<T> GetData<T>(string url)
        {
            var request = WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            request.Method = WebRequestMethods.Http.Get;
            string objectAsText = string.Empty;

            using (var response = await request.GetResponseAsync())
            {
                var sr = new StreamReader(response.GetResponseStream());
                objectAsText = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(objectAsText);
        }

        protected async Task PostData<T>(string url, T data)
        {
            var httpWebRequest = WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = WebRequestMethods.Http.Post;

            using (var strWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
            {
                string jsonObject = JsonConvert.SerializeObject(data);
                strWriter.Write(jsonObject);
                strWriter.Flush();

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    //Handle response
                }
            }
        }

        protected async Task PutData<T>(string url, T data)
        {
            var httpWebRequest = WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = WebRequestMethods.Http.Put;

            using (var strWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
            {
                string jsonObject = JsonConvert.SerializeObject(data);
                strWriter.Write(jsonObject);
                strWriter.Flush();

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    //Handle response
                }
            }
        }
    }
}
