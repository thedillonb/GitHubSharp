﻿using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    [Serializable]
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

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? ClosedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public PullRequestModel PullRequest { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(IssueModel))
                return false;
            IssueModel other = (IssueModel)obj;
            return Url == other.Url;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Url != null ? Url.GetHashCode() : 0);
            }
        }
        

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

        protected bool Equals(LabelModel other)
        {
            return string.Equals(Url, other.Url);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LabelModel) obj);
        }

        public override int GetHashCode()
        {
            return (Url != null ? Url.GetHashCode() : 0);
        }
    }

    [Serializable]
    public class MilestoneModel
    {
        public int Number { get; set; }

        public string State { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public BasicUserModel Creator { get; set; }

        public int OpenIssues { get; set; }

        public int ClosedIssues { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? DueOn { get; set; }

        public string Url { get; set; }

        protected bool Equals(MilestoneModel other)
        {
            return string.Equals(Url, other.Url);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MilestoneModel) obj);
        }

        public override int GetHashCode()
        {
            return (Url != null ? Url.GetHashCode() : 0);
        }
    }

    [Serializable]
    public class IssueCommentModel
    {
        public long Id { get; set; }

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
