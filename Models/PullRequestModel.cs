using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    [Serializable]
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

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public DateTimeOffset? ClosedAt { get; set; }

        public DateTimeOffset? MergedAt { get; set; }

        public PullRequestCommitReferenceModel Head { get; set; }

        public PullRequestCommitReferenceModel Base { get; set; }

        public BasicUserModel User { get; set; }

        public bool? Merged { get; set; }

        public bool? Mergable { get; set; }

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
        public class PullRequestCommitReferenceModel
        {
            public RepositoryModel Repository { get; set; }

            public string Sha { get; set; }

            public string Label { get; set; }

            public BasicUserModel User { get; set; }

            public string Ref { get; set; }
        }
    }

    [Serializable]
    public class PullRequestMergeModel
    {
        public string Sha { get; set; }

        public bool Merged { get; set; }

        public string Message { get; set; }
    }
}