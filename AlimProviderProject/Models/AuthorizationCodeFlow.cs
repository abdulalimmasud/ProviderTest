using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlimProviderProject.Models
{
    public class AuthorizationCodeFlow : IAuthorizationCodeFlow
    {
        public AuthorizationCodeFlow()
        {

        }
        public IAccessMethod AccessMethod { get; }

        public IClock Clock { get; }

        public IDataStore DataStore { get; }

        public AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTokenAsync(string userId, CancellationToken taskCancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public Task<TokenResponse> ExchangeCodeForTokenAsync(string userId, string code, string redirectUri, CancellationToken taskCancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponse> LoadTokenAsync(string userId, CancellationToken taskCancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponse> RefreshTokenAsync(string userId, string refreshToken, CancellationToken taskCancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RevokeTokenAsync(string userId, string token, CancellationToken taskCancellationToken)
        {
            throw new NotImplementedException();
        }

        public bool ShouldForceTokenRetrieval()
        {
            throw new NotImplementedException();
        }
    }
}
