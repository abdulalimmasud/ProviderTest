using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AlimProviderProject.Models
{
    public class SDKHelper
    {
        public static async Task<HttpResponseMessage> SendRequest(HttpMethod method, string endPoint, dynamic content = null)
        {
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(method, endPoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (content != null)
                    {
                        string c;
                        if (content is string)
                            c = content;
                        else
                            c = JsonConvert.SerializeObject(content);
                        request.Content = new StringContent(c, Encoding.UTF8, "application/json");
                    }

                    response = await client.SendAsync(request);
                }
            }
            return response;
        }
        public static async Task<HttpResponseMessage> SendRequest(HttpMethod method, string endPoint, string accessToken, dynamic content = null)
        {
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(method, endPoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    if (content != null)
                    {
                        string c;
                        if (content is string)
                            c = content;
                        else
                            c = JsonConvert.SerializeObject(content);
                        request.Content = new StringContent(c, Encoding.UTF8, "application/json");
                    }

                    response = await client.SendAsync(request);
                }
            }
            return response;
        }
        public static async Task<HttpResponseMessage> SendUrlEncodingRequest(HttpMethod method, string endPoint, Dictionary<string, string> content = null)
        {
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(method, endPoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (content != null)
                    {
                        request.Content = new FormUrlEncodedContent(content);
                    }
                    response = await client.SendAsync(request);
                }
            }
            return response;
        }
    }
}
