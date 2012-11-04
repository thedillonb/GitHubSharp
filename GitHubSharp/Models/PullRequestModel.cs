using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitHubSharp.Models
{
    public class PullRequestModel
    {
        public DateTime IssueUpdated { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? Closed { get; set; }
        public int Position { get; set; }
        public int Number { get; set; }
        public int Votes { get; set; }
        public string PatchUrl { get; set; }
        public int Comments { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public IssueUserModel IssueUser { get; set; }
        public IssueUserModel User { get; set; }
        public PullRequestCommitReferenceModel Base { get; set; }
        public PullRequestCommitReferenceModel Head { get; set; }
        string State { get; set; }
        public string[] Labels { get; set; }
        public string DiffUrl { get; set; }
        public List<IssueUserModel> Discussion { get; set; }

        public class DiscussionEntryModel
        {
            public string Type { get; set; }
            public DateTime Created { get; set; }
            public string Id { get; set; }
            public string Body { get; set; }
            public string Message { get; set; }
            public IssueUserModel User { get; set; }
            public IssueUserModel Author { get; set; }
        }

        public class IssueUserModel
        {
            public string GravatarId { get; set; }
            public string Type { get; set; }
            public string Login { get; set; }
            public string Name { get; set;}
            public string Email { get; set; }
        }

        public class PullRequestCommitReferenceModel
        {
            public RepositoryModel Repository { get; set; }
            public string Sha { get; set; }
            public string Label { get; set; }
            public IssueUserModel User { get; set; }
            public string Ref { get; set; }
        }
    }
}