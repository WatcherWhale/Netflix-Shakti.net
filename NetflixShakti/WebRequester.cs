using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti
{
    internal class WebRequester
    {
        public static Response DoRequest(string url, CookieContainer cookies)
        {
            WebRequest request = WebRequest.Create(url);
            HttpWebRequest webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = cookies;

            webRequest.BeginGetResponse((les) => 
            {
                var res = webRequest.GetResponse();
                return null;
            }, null);
            
            

            return new Response()
            {
                Cookies = (res as HttpWebResponse).Cookies,
                WebResponse = res
            };
        }

        public static Response DoPostRequest(string url, CookieContainer cookies, string header, string postInput)
        {
            WebRequest request = WebRequest.Create(url);
            HttpWebRequest webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = cookies;

            webRequest.Method = "POST";
            webRequest.ContentType = header;

            using (var writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                writer.Write(postInput);
                writer.Flush();
            }

            var res = webRequest.GetResponse();

            return new Response()
            {
                Cookies = (res as HttpWebResponse).Cookies,
                WebResponse = res
            };
        }
    }

    internal class Response
    {
        public CookieCollection Cookies { get; set; }
        public WebResponse WebResponse { get;set;}

        public T DeserializeResponse<T>()
        {
            using (var res = WebResponse)
            using (var resStream = res.GetResponseStream())
            using (var reader = new StreamReader(resStream))
            {
                string json = reader.ReadToEnd();
                T obj = JsonConvert.DeserializeObject<T>(json);

                return obj;
            }
        }
    }
}
