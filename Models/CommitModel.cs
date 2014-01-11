using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    [Serializable]
    public class CommitModel
    {
        public string Url { get; set; }
        public string Sha { get; set; }
        public CommitDetailModel Commit { get; set; }
        public BasicUserModel Author { get; set; }
        public BasicUserModel Committer { get; set; }
        public List<CommitParentModel> Parents { get; set; }

        public ChangeStatusModel Stats { get; set; }
        public List<CommitFileModel> Files { get; set; } 

        [Serializable]
        public class CommitParentModel
        {
            public string Sha { get; set; }
            public string Url { get; set; }
        }

        [Serializable]
        public class CommitFileModel
        {
            public string Filename { get; set; }
            public uint Additions { get; set; }
            public uint Deletions { get; set; }
            public uint Changes { get; set; }
            public string Status { get; set; }
            public string RawUrl { get; set; }
            public string BlobUrl { get; set; }
            public string Patch { get; set; }
            public string ContentsUrl { get; set; }
        }

        [Serializable]
        public class SingleFileCommitModel : CommitModel
        {
            public List<SingleFileCommitFileReference> Added { get; set; }
            public List<SingleFileCommitFileReference> Removed { get; set; }
            public List<SingleFileCommitFileReference> Modified { get; set; }

            [Serializable]
            public class SingleFileCommitFileReference
            {
                public string Filename { get; set; }
            }
        }

        [Serializable]
        public class CommitDetailModel
        {
            public string Url { get; set; }
            public string Sha { get; set; }
            public AuthorModel Author { get; set; }
            public AuthorModel Committer { get; set; }
            public string Message { get; set; }
            public CommitParentModel Tree { get; set; }

            [Serializable]
            public class AuthorModel
            {
                public string Name { get; set; }
				public DateTimeOffset Date { get; set; }
                public string Email { get; set; }
            }
        }
    }
}

