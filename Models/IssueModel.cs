using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    [Serializable]
    public class IssueModel
    {
        public string Url { get; set; }
        public string HtmlUrl { get; set; }
        public ulong Number { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public BasicUserModel User { get; set; }
        public List<LabelModel> Labels { get; set; }
        public BasicUserModel Assignee { get; set; }
        public MilestoneModel Milestone { get; set; }
        public uint Comments { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset? ClosedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
        public PullRequestModel PullRequest { get; set; }

        [Serializable]
        public class PullRequestModel
        {
            public string HtmlUrl { get; set; }
            public string DiffUrl { get; set; }
            public string PatchUrl { get; set; }
        }
    }

    [Serializable]
    public class LabelModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(LabelModel))
                return false;
            LabelModel other = (LabelModel)obj;
            return Name == other.Name;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }

    [Serializable]
    public class MilestoneModel
    {
        public uint Number { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BasicUserModel Creator { get; set; }
        public uint OpenIssues { get; set; }
        public uint ClosedIssues { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset? DueOn { get; set; }
    }

    [Serializable]
    public class IssueCommentModel
    {
        public ulong Id { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public BasicUserModel User { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
    }

    [Serializable]
    public class IssueEventModel
    {
        public string Url { get; set; }
        public BasicUserModel Actor { get; set; }
        public string Event { get; set; }
        public string CommitId { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
        public IssueModel Issue { get; set; }

        public static class EventTypes
        {
            public static string Closed = "closed", Reopened = "reopened", Subscribed = "subscribed",
                                 Merged = "merged", Referenced = "referenced", Mentioned = "mentioned",
                                 Assigned = "assigned";
        }
    }
}
