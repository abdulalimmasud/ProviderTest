using AlimProviderProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlimProviderProject.Controllers
{
    public class AccountController:Controller
    {
        public void Index()
        {
            string url = string.Format("https://accounts.google.com/o/oauth2/v2/auth?scope={0}&prompt=consent&access_type=offline&include_granted_scopes=true&state=google&redirect_uri={1}&response_type=code&client_id={2}", GoogleApiCredential.GoogleScopes, "http://localhost:33202", GoogleApiCredential.GoogleClientId);
            Response.Redirect(url);
        }
    }
}
