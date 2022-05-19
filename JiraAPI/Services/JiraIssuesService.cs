using Atlassian.Jira;
using JiraAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace JiraAPI.Services
{
    public class JiraIssuesService : IJiraIssuesService
    {
        private Jira jiraClient;

        public JiraIssuesService()
        {
            var siteSettings = LoadJson();

            jiraClient = Jira.CreateRestClient(siteSettings.URL, siteSettings.Username, siteSettings.Token, new JiraRestClientSettings());
            // var siteSettings = JsonSettingsReader.Load<List<SiteSettings>>(Constants.JsonFiles.SiteSettings);

        }
        public List<IssuesModel> GetUserIssues(string AssigneeName, string sprint, string startDate, string endDate)
        {
            List<Issue> pagedQueryResult = Task.Run(() => GetIssues(AssigneeName, startDate, endDate)).GetAwaiter().GetResult();

            List<IssuesModel> issuesModels = new List<IssuesModel>();
            foreach (Issue issue in pagedQueryResult)
            {

                string storyPoints = issue.CustomFields.Where(x => x.Name == "Story Points").FirstOrDefault()?.Values?.FirstOrDefault() ?? "0";
                issuesModels.Add(new IssuesModel()
                {
                    Reporter = issue.ReporterUser?.DisplayName ?? "",
                    Assignee = issue.AssigneeUser?.DisplayName ?? "",
                    Title = issue.Summary,
                    StoryPoints = storyPoints,
                    IssueType = issue.Type.Name,
                    CreatedDate = issue.Created.Value,
                    Status = issue.Status?.Name,
                    Sprint = issue.CustomFields.Where(x => x.Name == "Sprint").FirstOrDefault()?.Values?.FirstOrDefault() ?? ""

                });
            }

            return issuesModels;
        }


        public List<UsersModel> GetUserStoryPointsInSprint(string AssigneeNames, string sprint, string startDate, string endDate)
        {
            List<Issue> pagedQueryResult = GetSprintIssues(AssigneeNames, sprint, startDate, endDate);

            List<UsersModel> users = new List<UsersModel>();
            foreach (Issue issue in pagedQueryResult)
            {
                if (issue.AssigneeUser != null)
                {


                    string assignee = issue.AssigneeUser?.DisplayName ?? "";
                    string storyPoints = issue.CustomFields.Where(x => x.Name == "Story Points").FirstOrDefault()?.Values?.FirstOrDefault() ?? "0";
                    var currentIssue = new IssuesModel()
                    {
                        Reporter = issue.ReporterUser?.DisplayName ?? "",
                        Assignee = issue.AssigneeUser?.DisplayName ?? "",
                        Title = issue.Summary,
                        StoryPoints = storyPoints,
                        IssueType = issue.Type.Name,
                        CreatedDate = issue.Created.Value,
                        Status = issue.Status?.Name,
                        Sprint = issue.CustomFields.Where(x => x.Name == "Sprint").FirstOrDefault()?.Values?.FirstOrDefault() ?? ""

                    };


                    var obj = users.FirstOrDefault(x => x.UserSisplayName == assignee);
                    if (obj != null)
                    {
                        obj.TotalPoints += Convert.ToInt32(storyPoints);
                        obj.TotalPoints += Convert.ToInt32(storyPoints);
                        obj.TotalIssues++;
                       // obj.Issues.Add(currentIssue);
                    }
                    else
                    {

                        users.Add(new UsersModel()
                        {
                            UserSisplayName = assignee,
                            TotalPoints = Convert.ToInt32(storyPoints),
                            //Issues = new List<IssuesModel>() { currentIssue },
                            TotalIssues = 1
                        });
                    }

                }
            }

            return users;
        }
        private List<Issue> GetIssues(string AssigneeName = "", string sprint = "", string startDate = "", string endDate = "")

        {

            string jqlString = PrepareJqlbyDates("WRE", AssigneeName, sprint, startDate, endDate);
            IssueSearchOptions options = new IssueSearchOptions(jqlString);
            int threshold = 100;
            options.MaxIssuesPerRequest = threshold;
            options.StartAt = 0;

            List<Issue> issuesList = new List<Issue>();
            while (true)
            {
                IPagedQueryResult<Issue> partialPullRequests = jiraClient.Issues.GetIssuesFromJqlAsync(options).Result;
                issuesList.AddRange(partialPullRequests);
                if (partialPullRequests.Count() < threshold) { break; }
                options.StartAt += threshold;
            }

            return issuesList;
        }


        private string PrepareJqlbyDates(string project, string user, string sprint, string beginDate, string endDate)
        {
            string jqlString = "project = " + project;

            if (!string.IsNullOrEmpty(user))
                jqlString += " AND assignee = " + user;

            if (!string.IsNullOrEmpty(sprint))
                jqlString += " AND Sprint = " + sprint;

            if (!string.IsNullOrEmpty(beginDate))
                jqlString += " AND createdDate >= " + beginDate;
            if (!string.IsNullOrEmpty(endDate))
                jqlString += " AND createdDate <= " + endDate;
            return jqlString;
        }

        private List<Issue> GetSprintIssues(string AssigneeName = "", string sprint = "", string startDate = "", string endDate = "")
        {

            string jqlString = PrepareJqlbyDates("WRE", AssigneeName, sprint, startDate, endDate);
            IssueSearchOptions options = new IssueSearchOptions(jqlString);
            int threshold = 100;
            options.MaxIssuesPerRequest = threshold;
            options.StartAt = 0;

            List<Issue> issuesList = new List<Issue>();
            while (true)
            {
                IPagedQueryResult<Issue> partialPullRequests = jiraClient.Issues.GetIssuesFromJqlAsync(options).Result;
                issuesList.AddRange(partialPullRequests);
                if (partialPullRequests.Count() < threshold) { break; }
                options.StartAt += threshold;
            }

            return issuesList;
        }

        public JiraClientCredintials LoadJson()
        {
            string ConfigPath = "D:\\Jira Performance\\JiraPerformance\\JiraAPI\\JsonFiles\\appsettings.json";
            using (StreamReader r = new StreamReader(ConfigPath))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<JiraClientCredintials>(json);
            }
        }

    }
}