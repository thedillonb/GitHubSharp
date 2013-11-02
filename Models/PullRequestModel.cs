using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    [Serializable]
    public class PullRequestModel
    {
        public string Url { get; set; }
        public string DiffUrl { get; set; }
        public string PatchUrl { get; set; }
        public string IssueUrl { get; set; }
        public ulong Number { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? MergedAt { get; set; }
        public PullRequestCommitReferenceModel Head { get; set; }
        public PullRequestCommitReferenceModel Base { get; set; }
        public BasicUserModel User { get; set; }
        public bool? Merged { get; set; }
        public bool? Mergable { get; set; }
        public BasicUserModel MergedBy { get; set; }

        public uint? Comments { get; set; }
        public uint? Commits { get; set; }
        public uint? Additions { get; set; }
        public uint? Deletions { get; set; }
        public uint? ChangedFiles { get; set; }

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