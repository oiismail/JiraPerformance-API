using Atlassian.Jira;
using JiraPerformanceAPI.Models;
using JiraPerformanceAPI.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace JiraPerformanceAPI.Controllers
{
    public class JiraController : ApiController
    {

        private IJiraIssuesService _jiraIssuesService;
        // GET api/<controller>

        JiraController()
        {
            _jiraIssuesService = new JiraIssuesService();
        }
        [System.Web.Http.HttpGet, System.Web.Http.Route("Jira/GetUserJiraIssues")]
        public IHttpActionResult GetUserJiraIssues(string displayName = null, string startDate = null, string endDate = null)
        {
            List<IssuesModel> issuesModels = _jiraIssuesService.GetUserIssues(displayName, "", startDate, endDate);
            // var json = new JsonResult { Data = issuesModels, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


            var jsonResult = JsonConvert.SerializeObject(issuesModels);

            if (jsonResult != null)
            {
                return Ok(jsonResult);
            }
            return NotFound();
        }

        [System.Web.Http.HttpGet, System.Web.Http.Route("Jira/GetUserStoryPointsInSprint")]
        public JsonResult GetUserStoryPointsInSprint(string assignee = null, string sprint = null, string startDate = null, string endDate = null)
        {
            List<UsersModel> usersModels = _jiraIssuesService.GetUserStoryPointsInSprint(assignee, sprint, startDate, endDate);
            var json = new JsonResult { Data = usersModels, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

           // var jsonResult = JsonConvert.SerializeObject(usersModels);
            //var json = new JsonResult { Data = jsonResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           
            return json; 
            
            //if (jsonResult != null)
            //{
            //    return Ok(jsonResult);
            //}
            //return NotFound();
        }
    }
}