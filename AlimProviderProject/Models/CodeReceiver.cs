using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using System.Threading;
using System.Net.Http;

namespace AlimProviderProject.Models
{
    public class CodeReceiver : ICodeReceiver
    {
        public string RedirectUri { get; }
        public CodeReceiver(string uri)
        {
            this.RedirectUri = uri;
        }

        public async Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(AuthorizationCodeRequestUrl url, CancellationToken taskCancellationToken)
        {
            string getUrl = url.AuthorizationServerUrl + "?scope=" + url.Scope + "&access_type=offline&include_granted_scopes=true&state=google&redirect_uri=" + url.RedirectUri + "&response_type=code&client_id=" + url.ClientId;
            using(var response = await SDKHelper.SendRequest(HttpMethod.Get, getUrl))
            {
                var reader = await response.Content.ReadAsStringAsync();
                AuthorizationCodeResponseUrl authorization = new AuthorizationCodeResponseUrl();
                return authorization;
            }
            
        }
    }
}
