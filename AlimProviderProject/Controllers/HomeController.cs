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
using MimeKit;
using Google.Apis.Calendar.v3;
using Google.Apis.Requests;
using Google.Apis.Calendar.v3.Data;

namespace AlimProviderProject.Controllers
{
    public class HomeController : Controller
    {
        string _refreshToken = "1/vJEXT-kecHgCupvniV5lSkLYyPUKFouagAWkQn2HDbs";
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
                //ForwardTest();
                //ReplyTest();
                //CalendarTest();
                //AttachmentTest();
                var res = RefClass.AddCalenderEvents(_refreshToken, "#contacts@group.v.calendar.google.com");
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
        public void AttachmentTest()
        {
            try
            {
                var service = SDKHelper.GetGmailService(_refreshToken);
                var getRequest = service.Users.Messages.Attachments.Get("me", "165c858574bdaa1b", "ANGjdJ-HZdjO8V1NC8A3_2NJ_9LtGUc11qZ-KGATsOmqMkdadqxAxan3k7pEblVlkRVaTtSbwEljPCdIV_HdIs2QOvRc5QccRxhSfglI7cU5N3FeB4jT8eybV9mNb3QuyU5vfNrIWraBYOvRCyz57Gyku5-MjtR4NMhH80pSKAvFGsqwBBuEvE9V-AZji0l1BeNu8eAM14u-1Q6BEm-Jtx8Ma1QpQuY1p2Qz9xiNO1jkfSEN-08X5WqESl282mfOkp77tkpPsXOhY_L_EymYgu1AU8OjY_v-PuklOW6e1RfDDGB1j1DgIkNMGI8q8YfoJEJszG4mwo3kt-TwCkUzxUeOJACBL5LlEputPUxy8H7L7j69Tm9p_tNu6227857o2cOZ0lg6AqMIbn65B82Q");
                //getRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;
                //getRequest.MetadataHeaders = new Repeatable<string>(new[] { "Subject", "Date", "From", "Created at", "To", "Cc", "Bcc", "Body" });

                var response = getRequest.Execute();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ReplyTest()
        {
            try
            {
                var service = SDKHelper.GetGmailService(_refreshToken);

                var getRequest = service.Users.Messages.Get("me", "165c858574bdaa1b");
                getRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Raw;
                getRequest.MetadataHeaders = new Repeatable<string>(new[] { "Subject", "Message-ID", "From", "References", "To", "Cc", "Bcc", "Body" });
                var response = getRequest.Execute();

                byte[] readContent = FromBase64ForUrlString(response.Raw);
                var readStream = new MemoryStream(readContent);
                MimeMessage readMessage = MimeMessage.Load(readStream);

                MailMessage mail = new MailMessage();
                mail.Body = "This is reply body";
                FileStream stream = System.IO.File.Open(@"G:\MatterFunctionalDesign.docx", FileMode.Open);
                mail.Attachments.Add(new Attachment(stream, "test.docx"));
                mail.To.Add(readMessage.From.ToString());

                MimeMessage mimeMessage = MimeMessage.CreateFromMailMessage(mail);
                mimeMessage.InReplyTo = readMessage.MessageId;
                //mimeMessage.ReplyTo.AddRange(readMessage.To);
                //mimeMessage.ReplyTo.AddRange(readMessage.Cc);
                //mimeMessage.To.AddRange(readMessage.From);
                //mimeMessage.Subject = "Re: " + readMessage.Subject;

                //if (readMessage.References.Count > 0)
                //{
                //    mimeMessage.References.AddRange(readMessage.References);
                //}
                mimeMessage.References.Add(readMessage.MessageId);
                //mimeMessage.Body = new TextPart("palin")
                //{
                //    Text = "This is reply body"
                //};
                //FileStream stream = System.IO.File.Open(@"G:\MatterFunctionalDesign.docx", FileMode.Open);
                //MimeEntity entity = MimeEntity.Load(stream);

                //mimeMessage.Attachments.ToList().Add(entity);
               

                Message message = new Message();
                message.Raw = Base64UrlEncode(mimeMessage.ToString());

                var resp = service.Users.Messages.Send(message, "me").Execute();
                string res = JsonConvert.SerializeObject(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SendTest()
        {
            try
            {
                var service = SDKHelper.GetGmailService(_refreshToken);

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
                var service = SDKHelper.GetGmailService(_refreshToken);
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
                var service = SDKHelper.GetGmailService(_refreshToken);
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
            var service = SDKHelper.GetGmailService(_refreshToken);
            var labels = service.Users.Labels.List("me").Execute();
            for (int i = 0; i < labels.Labels.Count; i++)
            {
                labels.Labels[i] = service.Users.Labels.Get("me", labels.Labels[i].Id).Execute();
            }
        }
        public async Task CalendarTest()
        {
            try
            {
                var service = SDKHelper.GoogleCalendarService(_refreshToken);

                //var calendars = await service.CalendarList.List().ExecuteAsync();

                var events = await service.Events.List("#contacts@group.v.calendar.google.com").ExecuteAsync();

                var response = JsonConvert.SerializeObject(events);
            }
            catch (Exception ex)
            {

            }
        }
        public void Test()
        {
            try
            {
                var service = SDKHelper.GetGmailService(_refreshToken);

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
