using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JiraAPI.Models
{
    public class Configs
    {
    }
    public class JiraClientCredintials
    {
        public string URL { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
    public class JsonFiles
    {
        public const string SiteSettings = "bin\\JsonFiles\\siteSettings.json";
    }
}