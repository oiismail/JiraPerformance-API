
using JiraAPI.Models;
using System.Collections.Generic;

namespace JiraAPI.Services
{
    public interface IJiraIssuesService
    {
        List<IssuesModel> GetUserIssues(string AssigneeName, string sprint, string startDate, string endDate);
        List<UsersModel> GetUserStoryPointsInSprint(string AssigneeNames, string sprint, string startDate, string endDate);



    }
}
