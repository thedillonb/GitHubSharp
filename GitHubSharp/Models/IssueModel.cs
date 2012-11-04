using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    public class IssueModel
    {
        public int Number { get; set; }
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

        public class LabelModel
        {
            public string Url { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }
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



    public class CommentForIssue
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
        public int Id { get; set; }
        public string User { get; set; }
    }
}
