using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlimProviderProject.Models
{
    public class GoogleApiCredential
    {
        public static string ApplicationName = "AlimProviderProject";
        public static string GoogleClientId = "101485176491-8g6biiog9lcub5goebgdneo1t4jk4hom.apps.googleusercontent.com";
        public static string GoogleCalendarId = "556735548508-jksv9ebhl9j29dllrfu8i3lt3809e4us.apps.googleusercontent.com";
        public static string GmailClientId = "556735548508-4hi427afgh00q53hmjgrht064qi6gnac.apps.googleusercontent.com";
        public static string GoogleClientSecret = "CvrhwJk1yScWrIylN5AN1Xe8";
        public static string GmailSecret = "W58xbuva4-0TOh-yi1LNJKpn";
        public static string GoogleCalendarSecret = "r772SQkU9VUK7xUvtkTePwX9";
        public static string GoogleScopes = "https%3A%2F%2Fmail.google.com%2F https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.email https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fgmail.send https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fgmail.modify https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fgmail.labels https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcalendar";
    }
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
    }
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string link { get; set; }
        public string picture { get; set; }
        public string gender { get; set; }
        public string locale { get; set; }
    }
}
