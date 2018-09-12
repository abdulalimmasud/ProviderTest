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
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util;
using System.Net;
using System.Net.Mail;

namespace AlimProviderProject.Controllers
{
    public class HomeController : Controller
    {
        static string[] Scopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailLabels, GmailService.Scope.GmailModify, GmailService.Scope.GmailSend, GmailService.Scope.GmailCompose, GmailService.Scope.GmailInsert };
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
                //Test();
                //LabelTest();
                //ReadMessage();
                //SendTest();
                ForwardTest();
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

        public void SendTest()
        {
            try
            {
                ClientSecrets secrets = new ClientSecrets();
                secrets.ClientId = GoogleApiCredential.GoogleClientId;
                secrets.ClientSecret = GoogleApiCredential.GoogleClientSecret;

                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = secrets,
                    Scopes = Scopes

                });

                TokenResponse token = new TokenResponse();
                token.TokenType = "Bearer";
                token.RefreshToken = "1/vJEXT-kecHgCupvniV5lSkLYyPUKFouagAWkQn2HDbs";
                UserCredential credential = new UserCredential(flow, "me", token);
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress("aa@hoxro.com"));
                mail.From = new MailAddress("alimcu08@gmail.com");
                mail.Subject = "Test";
                mail.Body = "This is body";
                FileStream stream = System.IO.File.Open(@"G:\MatterFunctionalDesign.docx", FileMode.Open);
                mail.Attachments.Add(new Attachment(stream, "test.docx"));

                MimeKit.MimeMessage mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mail);
                Message message = new Message();
                message.Raw = Base64UrlEncode(mimeMessage.ToString());

                var response = service.Users.Messages.Send(message, "me").Execute();
                string res = JsonConvert.SerializeObject(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ForwardTest()
        {
            try
            {
                ClientSecrets secrets = new ClientSecrets();
                secrets.ClientId = GoogleApiCredential.GoogleClientId;
                secrets.ClientSecret = GoogleApiCredential.GoogleClientSecret;

                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = secrets,
                    Scopes = Scopes

                });

                TokenResponse token = new TokenResponse();
                token.TokenType = "Bearer";
                token.RefreshToken = "1/vJEXT-kecHgCupvniV5lSkLYyPUKFouagAWkQn2HDbs";
                UserCredential credential = new UserCredential(flow, "me", token);
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                var getRequest = service.Users.Messages.Get("me", "165c1f98cb8e4795");
                getRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Raw;
                getRequest.MetadataHeaders = new Repeatable<string>(new[] { "Subject", "Date", "From", "Created at", "To", "Cc", "Bcc", "Body" });
                var response = getRequest.Execute();

                byte[] reader = FromBase64ForUrlString(response.Raw);
                //string decodedString = Encoding.UTF8.GetString(reader);
                Stream stream = new MemoryStream(reader);
                MimeKit.MimeMessage mimeMessage = MimeKit.MimeMessage.Load(stream);
                int tos = mimeMessage.To.Select(x => x.Name).ToList().Count;
                for (int i = 0; i < tos; i++)
                {
                    mimeMessage.To.RemoveAt(i);
                }
                mimeMessage.To.Add(MimeKit.InternetAddress.Parse("aa@hoxro.com"));

                //string raw = decodedString.Insert(index, " ");
                //response.Payload.Headers.FirstOrDefault(x => x.Name == "To").Value = "aa@hoxro.com";
                Message forMessage = new Message();
                forMessage.Raw = Base64UrlEncode(mimeMessage.ToString());
                var send = service.Users.Messages.Send(forMessage, "me").Execute();
                //service.Users.Settings.ForwardingAddresses.Create();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ReadMessage()
        {
            try
            {
                ClientSecrets secrets = new ClientSecrets();
                secrets.ClientId = GoogleApiCredential.GoogleClientId;
                secrets.ClientSecret = GoogleApiCredential.GoogleClientSecret;

                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = secrets,
                    Scopes = Scopes

                });

                TokenResponse token = new TokenResponse();
                token.TokenType = "Bearer";
                token.RefreshToken = "1/vJEXT-kecHgCupvniV5lSkLYyPUKFouagAWkQn2HDbs";
                UserCredential credential = new UserCredential(flow, "me", token);
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                var getRequest = service.Users.Messages.Get("me", "165c858574bdaa1b");
                getRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;
                getRequest.MetadataHeaders = new Repeatable<string>(new[] { "Subject", "Date", "From", "Created at", "To", "Cc", "Bcc", "Body" });

                var response = getRequest.Execute();
                string res = JsonConvert.SerializeObject(response);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void LabelTest()
        {
            ClientSecrets secrets = new ClientSecrets();
            secrets.ClientId = GoogleApiCredential.GoogleClientId;
            secrets.ClientSecret = GoogleApiCredential.GoogleClientSecret;

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets,
                Scopes = Scopes

            });

            TokenResponse token = new TokenResponse();
            token.TokenType = "Bearer";
            token.RefreshToken = "1/vJEXT-kecHgCupvniV5lSkLYyPUKFouagAWkQn2HDbs";
            UserCredential credential = new UserCredential(flow, "me", token);
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            var labels = service.Users.Labels.List("me").Execute();
            for (int i = 0; i < labels.Labels.Count; i++)
            {
                labels.Labels[i] = service.Users.Labels.Get("me", labels.Labels[i].Id).Execute();
            }
        }

        public void Test()
        {
            try
            {
                ClientSecrets secrets = new ClientSecrets();
                secrets.ClientId = GoogleApiCredential.GoogleClientId;
                secrets.ClientSecret = GoogleApiCredential.GoogleClientSecret;

                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = secrets,
                    Scopes = Scopes

                });

                TokenResponse token = new TokenResponse();
                token.TokenType = "Bearer";
                //token.ExpiresInSeconds = 3600;
                token.RefreshToken = "1/vJEXT-kecHgCupvniV5lSkLYyPUKFouagAWkQn2HDbs";
                //token.IdToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImQ5NjQ4ZTAzMmNhYzU4NDI0ZTBkMWE3YzAzMGEzMTk4ZDNmNDZhZGIifQ.eyJhenAiOiIxMDE0ODUxNzY0OTEtOGc2Ymlpb2c5bGN1YjVnb2ViZ2RuZW8xdDRqazRob20uYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiIxMDE0ODUxNzY0OTEtOGc2Ymlpb2c5bGN1YjVnb2ViZ2RuZW8xdDRqazRob20uYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMTQxNzY0NzkzOTgxMTQ2MTIxMTIiLCJlbWFpbCI6ImFsaW1jdTA4QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhdF9oYXNoIjoicFpiazFDN2F5Z2h5c2JnVFhuZDRoUSIsImV4cCI6MTUzNjY0MTMxNiwiaXNzIjoiaHR0cHM6Ly9hY2NvdW50cy5nb29nbGUuY29tIiwiaWF0IjoxNTM2NjM3NzE2fQ.epsXBxilHWUdhVBi_mbAtX0pMObeFcgHRL_NoxY0mNbdY0q85JQlwNM18Bg9010u8nLMzKI8IMaIfYibmGAULeoVkhBBLXpfdrKGc_9I0OJJy7gaeRy4Mrp_NfoAcbSffruQFKCgAKInHUT9Uq3Sa5Zqj7xAIyshVPO9AGGOo69sk3WoAqFNFz5W3IJTsjjaiD7ecQfOn9Htln10wTqvjQMzaue6q1VVk1MXdiQ_si4Yra4-6XWvo9kABX6IM9PWJsZjvDT-3FTZpdsVaUwLAQ7ZlEOlVVo1hMf9nK29R32BI8tymQa1pEuKUxcBTmgh8-H3vjtb7AkZO-TROHxhtg";
                //token.AccessToken = "ya29.GlwVBncSrsZCyGm4-rRBTfqcIQsUpQeAOnxFIyXc3WAT5Kn5b_97VpIYCH0aUjyVfabLJOewVN5gK53n5El1ycwaR4WTX6Lr0oJ7SzUe0wUOXJUnbuayAw5rtYOsNg";

                UserCredential credential = new UserCredential(flow, "me", token);
                // Create Gmail API service. 
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                var listRequest = service.Users.Messages.List("me");
                listRequest.LabelIds = "INBOX";
                listRequest.MaxResults = 20;
                var messages = new List<Message>();
                var emailMessages = new List<EmailMessage>();
                var listMessagesResponse = listRequest.Execute();
                messages.AddRange(listMessagesResponse.Messages);
                for (var i = 0; i < messages.Count; i++)
                {
                    var message = messages[i];
                    var getRequest = service.Users.Messages.Get("me", message.Id);
                    getRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Metadata;
                    getRequest.MetadataHeaders = new Repeatable<string>(new[] { "Subject", "Date", "From" });
                    var res = getRequest.Execute();
                    messages[i] = getRequest.Execute();
                    var unread = false;
                    foreach (var label in messages[i].LabelIds.Where(label => label.Equals("UNREAD")))
                    {
                        unread = true;
                    }
                    emailMessages.Add(new EmailMessage()
                    {
                        Id = messages[i].Id,
                        Body = new EmailBody() { Content = WebUtility.HtmlDecode(messages[i].Snippet) },
                        IsRead = unread,
                        From = new EmailAddress { Address = messages[i].Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value },
                        Subject = messages[i].Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value,
                        ReceivedDateTime = messages[i].InternalDate.ToString()
                    });
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
        private string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }
    }
}
