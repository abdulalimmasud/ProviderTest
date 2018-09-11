using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlimProviderProject.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using Google.Apis.Gmail.v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;
using System.Text;

namespace AlimProviderProject.Controllers
{
    public class HomeController : Controller
    {
        static string[] Scopes = { GmailService.Scope.GmailReadonly };
        static string ApplicationName = "AlimProviderProject";
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
                ClientSecrets secrets = new ClientSecrets();
                secrets.ClientId = GoogleApiCredential.GoogleClientId;
                secrets.ClientSecret = GoogleApiCredential.GoogleClientSecret;
                string credPath = System.Environment.GetFolderPath(
                  System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart.json");
                if (System.IO.File.Exists(credPath))
                {
                    System.IO.File.Delete(credPath);
                }
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                      secrets,
                      Scopes,
                      "user",
                      CancellationToken.None,
                      new FileDataStore(credPath, true)).Result;

                // Create Gmail API service. 
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                
                var inboxlistRequest = service.Users.Messages.List("eyJhbGciOiJSUzI1NiIsImtpZCI6ImQ5NjQ4ZTAzMmNhYzU4NDI0ZTBkMWE3YzAzMGEzMTk4ZDNmNDZhZGIifQ.eyJhenAiOiIxMDE0ODUxNzY0OTEtOGc2Ymlpb2c5bGN1YjVnb2ViZ2RuZW8xdDRqazRob20uYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiIxMDE0ODUxNzY0OTEtOGc2Ymlpb2c5bGN1YjVnb2ViZ2RuZW8xdDRqazRob20uYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMTQxNzY0NzkzOTgxMTQ2MTIxMTIiLCJlbWFpbCI6ImFsaW1jdTA4QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhdF9oYXNoIjoiQ016ZndmTkxlQ0VYMjFITWh4V2d4QSIsImV4cCI6MTUzNjYyNTE1NywiaXNzIjoiaHR0cHM6Ly9hY2NvdW50cy5nb29nbGUuY29tIiwiaWF0IjoxNTM2NjIxNTU3fQ.Yd4xbe-xJdfpRpfC0EiCx3Na1eNIgU6uPCwdiqUhaWeCGUbrTGTwjS8h3lqBt_L65VhPUv1O6T18zI18010Si3iL0eX9RiDrqaG5UlDVlTWNMEOwDfmWwoBohv4lxmZNDBee8fTDFIO9rC8QG68i81t6It-M9dYzH6gB0DvaKcjcYlV4m6fvcVrM757aECIO_I3HpuVY2DwiWAeAw5CkYkL6bY70D0zilV-c_8SESRINhgioXHADwZ34Rx1DOBRLST-9mvLWS4G6oyij0gOH0LfJ4kBuDIHyYjKo0XVwOmWdWNRAVqtssWSckg4deRVR-KWTL1VyTzSr-5ekI2dNPg");
                inboxlistRequest.LabelIds = "INBOX";
                inboxlistRequest.IncludeSpamTrash = false;
                var emailListResponse = inboxlistRequest.Execute();

                if (emailListResponse != null && emailListResponse.Messages != null)
                {
                    //loop through each email and get what fields you want... 
                    foreach (var email in emailListResponse.Messages)
                    {

                        var emailInfoRequest = service.Users.Messages.Get("your email id", email.Id);
                        var emailInfoResponse = emailInfoRequest.Execute();

                        if (emailInfoResponse != null)
                        {
                            String from = "";
                            String date = "";
                            String subject = "";

                            //loop through the headers to get from,date,subject, body  
                            foreach (var mParts in emailInfoResponse.Payload.Headers)
                            {
                                if (mParts.Name == "Date")
                                {
                                    date = mParts.Value;
                                }
                                else if (mParts.Name == "From")
                                {
                                    from = mParts.Value;
                                }
                                else if (mParts.Name == "Subject")
                                {
                                    subject = mParts.Value;
                                }

                                if (date != "" && from != "")
                                {

                                    foreach (MessagePart p in emailInfoResponse.Payload.Parts)
                                    {
                                        if (p.MimeType == "text/html")
                                        {
                                            byte[] data = FromBase64ForUrlString(p.Body.Data);
                                            string decodedString = Encoding.UTF8.GetString(data);

                                        }
                                    }



                                }

                            }
                        }
                    }

                }
            }
            catch(Exception ex)
            {

            }
        }
        public static byte[] FromBase64ForUrlString(string base64ForUrlInput)
        {
            int padChars = (base64ForUrlInput.Length % 4) == 0 ? 0 : (4 - (base64ForUrlInput.Length % 4));
            StringBuilder result = new StringBuilder(base64ForUrlInput, base64ForUrlInput.Length + padChars);
            result.Append(String.Empty.PadRight(padChars, '='));
            result.Replace('-', '+');
            result.Replace('_', '/');
            return Convert.FromBase64String(result.ToString());
        }
    }
}
