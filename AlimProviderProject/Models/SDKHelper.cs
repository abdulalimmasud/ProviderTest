using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
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
        static string[] _calendarScopes = { CalendarService.Scope.CalendarReadonly, CalendarService.Scope.Calendar };
        static string[] _gmailScopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailLabels, GmailService.Scope.GmailModify, GmailService.Scope.GmailSend, GmailService.Scope.GmailCompose, GmailService.Scope.GmailInsert };
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

        public static GmailService GetGmailService(string refreshToken)
        {

            ClientSecrets secrets = new ClientSecrets();
            secrets.ClientId = GoogleApiCredential.GoogleClientId;
            secrets.ClientSecret = GoogleApiCredential.GoogleClientSecret;

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets,
                Scopes = _gmailScopes
            });

            TokenResponse token = new TokenResponse();
            token.TokenType = "Bearer";
            token.ExpiresInSeconds = 3600;
            token.RefreshToken = refreshToken;

            UserCredential credential = new UserCredential(flow, "me", token);
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = GoogleApiCredential.ApplicationName
            });
            return service;
        }
        public static CalendarService GoogleCalendarService(string refreshToken)
        {
            ClientSecrets secrets = new ClientSecrets();
            secrets.ClientId = GoogleApiCredential.GoogleClientId;
            secrets.ClientSecret = GoogleApiCredential.GoogleClientSecret;

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets,
                Scopes = _calendarScopes
            });

            TokenResponse token = new TokenResponse();
            token.TokenType = "Bearer";
            token.ExpiresInSeconds = 3600;
            token.RefreshToken = refreshToken;
            UserCredential credential = new UserCredential(flow, "me", token);
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = GoogleApiCredential.ApplicationName
            });
            return service;
        }
    }
}
