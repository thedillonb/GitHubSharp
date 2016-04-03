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

        public string BodyHtml { get; set; }

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
            return Url == other.Url && HtmlUrl == other.HtmlUrl && Number == other.Number && State == other.State && Title == other.Title && Body == other.Body && BodyHtml == other.BodyHtml && User == other.User && Labels == other.Labels && Assignee == other.Assignee && Milestone == other.Milestone && Comments == other.Comments && CreatedAt == other.CreatedAt && ClosedAt == other.ClosedAt && UpdatedAt == other.UpdatedAt && PullRequest == other.PullRequest;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Url != null ? Url.GetHashCode() : 0) ^ (HtmlUrl != null ? HtmlUrl.GetHashCode() : 0) ^ (Number != null ? Number.GetHashCode() : 0) ^ (State != null ? State.GetHashCode() : 0) ^ (Title != null ? Title.GetHashCode() : 0) ^ (Body != null ? Body.GetHashCode() : 0) ^ (BodyHtml != null ? BodyHtml.GetHashCode() : 0) ^ (User != null ? User.GetHashCode() : 0) ^ (Labels != null ? Labels.GetHashCode() : 0) ^ (Assignee != null ? Assignee.GetHashCode() : 0) ^ (Milestone != null ? Milestone.GetHashCode() : 0) ^ (Comments != null ? Comments.GetHashCode() : 0) ^ (CreatedAt != null ? CreatedAt.GetHashCode() : 0) ^ (ClosedAt != null ? ClosedAt.GetHashCode() : 0) ^ (UpdatedAt != null ? UpdatedAt.GetHashCode() : 0) ^ (PullRequest != null ? PullRequest.GetHashCode() : 0);
            }
        }
        
        
        public class PullRequestModel
        {
            public string HtmlUrl { get; set; }

            public string DiffUrl { get; set; }

            public string PatchUrl { get; set; }
        }
    }

    
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

    
    public class IssueCommentModel
    {
        public long Id { get; set; }

        public string Url { get; set; }

        public string Body { get; set; }

        public string BodyHtml { get; set; }

        public BasicUserModel User { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }

    
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
