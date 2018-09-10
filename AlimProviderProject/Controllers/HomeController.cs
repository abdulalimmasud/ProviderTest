using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlimProviderProject.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Google.Apis.Gmail.v1;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;

namespace AlimProviderProject.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }
        public IActionResult Index(string state = "", string code="")
        {
            if (!string.IsNullOrEmpty(code))
            {
                var res = GetAccessToken(code).Result;
            }
            else
            {
                string token = "ya29.GlwUBg0d_RL3VF7l66YFmyQcHfqhOB2bX8SLCXrKpVYR-pONYoYrw6hmf7grNKK4OLTeX3KtTiqTp6RWVvsXKRyy4kQeO51V8VsceC8zfNJNay_P-RQla9UuLWuh2A";
                //var res = RefreshToken().Result;
                Test();
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        async Task<Token> GetAccessToken(string code)
        {
            try
            {
                var content = new Dictionary<string, string>
                {
                    { "code", code },
                    { "client_id", GoogleApiCredential.GoogleClientId },
                    { "client_secret", GoogleApiCredential.GoogleClientSecret },
                    { "redirect_uri", "http://localhost:33202" },
                    { "grant_type", "authorization_code" }
                };
                using (var response = await SDKHelper.SendUrlEncodingRequest(HttpMethod.Post, "https://accounts.google.com/o/oauth2/token", content))
                {
                    var reader = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Token>(reader);
                    return result;
                }
            } 
            catch(Exception ex)
            {
                throw ex;
            }
        }
        async Task<string> VerifyToken(string token)
        {
            try
            {
                using (var response = await SDKHelper.SendRequest(HttpMethod.Get, $"https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={token}", token))
                {
                    var reader = await response.Content.ReadAsStringAsync();
                    return reader;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        async Task<string> RefreshToken()
        {
            try
            {
                var content = new Dictionary<string, string>
                {
                    { "client_id", GoogleApiCredential.GoogleClientId },
                    { "refresh_token", "1/vJEXT-kecHgCupvniV5lSkLYyPUKFouagAWkQn2HDbs"},
                    { "client_secret", GoogleApiCredential.GoogleClientSecret },
                    { "grant_type", "refresh_token" }
                };
                using(var response = await SDKHelper.SendUrlEncodingRequest(HttpMethod.Post, "https://www.googleapis.com/oauth2/v4/token", content))
                {
                    var reader = await response.Content.ReadAsStringAsync();
                    return reader;
                }
            }
            catch(Exception ex)
            {
                
                throw ex;
            }
        }
        public void Test()
        {
            try
            {
                UserCredential credential;
                string[] Scopes = { GmailService.Scope.GmailReadonly };
                using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "AlimProviderProject",
                });
                UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");

                // List labels.
                IList<Label> labels = request.Execute().Labels;
                if (labels != null && labels.Count > 0)
                {
                    foreach (var labelItem in labels)
                    {
                        string name = labelItem.Name;
                    }
                }
                else
                {

                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
