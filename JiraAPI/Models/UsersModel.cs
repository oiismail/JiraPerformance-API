using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JiraAPI.Models
{
    public class UsersModel
    {
        public string UserSisplayName { get; set; }
        //public string Email { get; set; }
        //public string UserId { get; set; }
        public int TotalIssues { get; set; }
        public int TotalPoints { get; set; }
        
        //public List<IssuesModel> Issues { get; set; }

    }
}