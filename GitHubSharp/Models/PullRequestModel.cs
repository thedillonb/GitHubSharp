using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    
    public class PullRequestModel
    {
        public string Url { get; set; }

        public string HtmlUrl { get; set; }

        public string DiffUrl { get; set; }

        public string PatchUrl { get; set; }

        public string IssueUrl { get; set; }

        public long Number { get; set; }

        public string State { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string BodyHtml { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public DateTimeOffset? ClosedAt { get; set; }

        public DateTimeOffset? MergedAt { get; set; }

        public PullRequestCommitReferenceModel Head { get; set; }

        public PullRequestCommitReferenceModel Base { get; set; }

        public BasicUserModel User { get; set; }

        public bool? Merged { get; set; }

        public bool? Mergeable { get; set; }

        public BasicUserModel MergedBy { get; set; }

        public int? Comments { get; set; }

        public int? Commits { get; set; }

        public int? Additions { get; set; }

        public int? Deletions { get; set; }

        public int? ChangedFiles { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(PullRequestModel))
                return false;
            PullRequestModel other = (PullRequestModel)obj;
            return Url == other.Url && HtmlUrl == other.HtmlUrl && DiffUrl == other.DiffUrl && PatchUrl == other.PatchUrl && IssueUrl == other.IssueUrl && Number == other.Number && State == other.State && Title == other.Title && Body == other.Body && BodyHtml == other.BodyHtml && CreatedAt == other.CreatedAt && UpdatedAt == other.UpdatedAt && ClosedAt == other.ClosedAt && MergedAt == other.MergedAt && Head == other.Head && Base == other.Base && User == other.User && Merged == other.Merged && Mergeable == other.Mergeable && MergedBy == other.MergedBy && Comments == other.Comments && Commits == other.Commits && Additions == other.Additions && Deletions == other.Deletions && ChangedFiles == other.ChangedFiles;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Url != null ? Url.GetHashCode() : 0) ^ (HtmlUrl != null ? HtmlUrl.GetHashCode() : 0) ^ (DiffUrl != null ? DiffUrl.GetHashCode() : 0) ^ (PatchUrl != null ? PatchUrl.GetHashCode() : 0) ^ (IssueUrl != null ? IssueUrl.GetHashCode() : 0) ^ (Number != null ? Number.GetHashCode() : 0) ^ (State != null ? State.GetHashCode() : 0) ^ (Title != null ? Title.GetHashCode() : 0) ^ (Body != null ? Body.GetHashCode() : 0) ^ (BodyHtml != null ? BodyHtml.GetHashCode() : 0) ^ (CreatedAt != null ? CreatedAt.GetHashCode() : 0) ^ (UpdatedAt != null ? UpdatedAt.GetHashCode() : 0) ^ (ClosedAt != null ? ClosedAt.GetHashCode() : 0) ^ (MergedAt != null ? MergedAt.GetHashCode() : 0) ^ (Head != null ? Head.GetHashCode() : 0) ^ (Base != null ? Base.GetHashCode() : 0) ^ (User != null ? User.GetHashCode() : 0) ^ (Merged != null ? Merged.GetHashCode() : 0) ^ (Mergeable != null ? Mergeable.GetHashCode() : 0) ^ (MergedBy != null ? MergedBy.GetHashCode() : 0) ^ (Comments != null ? Comments.GetHashCode() : 0) ^ (Commits != null ? Commits.GetHashCode() : 0) ^ (Additions != null ? Additions.GetHashCode() : 0) ^ (Deletions != null ? Deletions.GetHashCode() : 0) ^ (ChangedFiles != null ? ChangedFiles.GetHashCode() : 0);
            }
        }
        
        public class PullRequestCommitReferenceModel
        {
            public RepositoryModel Repository { get; set; }

            public string Sha { get; set; }

            public string Label { get; set; }

            public BasicUserModel User { get; set; }

            public string Ref { get; set; }
        }
    }

    
    public class PullRequestMergeModel
    {
        public string Sha { get; set; }

        public bool Merged { get; set; }

        public string Message { get; set; }
    }
}