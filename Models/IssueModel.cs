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

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? ClosedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public PullRequestModel PullRequest { get; set; }

        protected bool Equals(IssueModel other)
        {
            return string.Equals(Url, other.Url) && string.Equals(HtmlUrl, other.HtmlUrl) && string.Equals(State, other.State) && Number == other.Number && string.Equals(Title, other.Title) && string.Equals(Body, other.Body) && Equals(User, other.User) && Equals(Labels, other.Labels) && Equals(Assignee, other.Assignee) && Equals(Milestone, other.Milestone) && Comments == other.Comments && CreatedAt.Equals(other.CreatedAt) && UpdatedAt.Equals(other.UpdatedAt) && ClosedAt.Equals(other.ClosedAt) && Equals(PullRequest, other.PullRequest);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IssueModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Url != null ? Url.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (HtmlUrl != null ? HtmlUrl.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (State != null ? State.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Number.GetHashCode();
                hashCode = (hashCode*397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Body != null ? Body.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (User != null ? User.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Labels != null ? Labels.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Assignee != null ? Assignee.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Milestone != null ? Milestone.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Comments;
                hashCode = (hashCode*397) ^ CreatedAt.GetHashCode();
                hashCode = (hashCode*397) ^ UpdatedAt.GetHashCode();
                hashCode = (hashCode*397) ^ ClosedAt.GetHashCode();
                hashCode = (hashCode*397) ^ (PullRequest != null ? PullRequest.GetHashCode() : 0);
                return hashCode;
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
