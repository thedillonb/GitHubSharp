using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    public class IssueModel
    {
        public string Url { get; set; }
        public string HtmlUrl { get; set; }
        public long Number { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public BasicUserModel User { get; set; }
        public List<LabelModel> Labels { get; set; }
        public BasicUserModel Assignee { get; set; }
        public MilestoneModel Milestone { get; set; }
        public int Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public PullRequestModel PullRequest { get; set; }

        public class LabelModel
        {
            public string Url { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }
        }

        public class PullRequestModel
        {
            public string HtmlUrl { get; set; }
            public string DiffUrl { get; set; }
            public string PatchUrl { get; set; }
        }
    }

    public class MilestoneModel
    {
        public int Number { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BasicUserModel Creator { get; set; }
        public int OpenIssues { get; set; }
        public int ClosedIssues { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueOn { get; set; }
    }

    public class IssueCommentModel
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public BasicUserModel User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class IssueEventModel
    {
        public string Url { get; set; }
        public BasicUserModel Actor { get; set; }
        public string Event { get; set; }
        public string CommitId { get; set; }
        public DateTime CreatedAt { get; set; }
        public IssueModel Issue { get; set; }

        public static class EventTypes
        {
            public static string Closed = "closed", Reopened = "reopened", Subscribed = "subscribed",
                                 Merged = "merged", Referenced = "referenced", Mentioned = "mentioned",
                                 Assigned = "assigned";
        }
    }
}
