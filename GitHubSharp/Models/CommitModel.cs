using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    
    public class CommitModel
    {
        public string Url { get; set; }
        public string HtmlUrl { get; set; }
        public string CommentsUrl { get; set; }
        public string Sha { get; set; }
        public CommitDetailModel Commit { get; set; }
        public BasicUserModel Author { get; set; }
        public BasicUserModel Committer { get; set; }
        public List<CommitParentModel> Parents { get; set; }

        public ChangeStatusModel Stats { get; set; }
        public List<CommitFileModel> Files { get; set; } 

        
        public class CommitParentModel
        {
            public string Sha { get; set; }
            public string Url { get; set; }
        }

        
        public class CommitFileModel
        {
            public string Filename { get; set; }
            public int Additions { get; set; }
            public int Deletions { get; set; }
            public int Changes { get; set; }
            public string Status { get; set; }
            public string RawUrl { get; set; }
            public string BlobUrl { get; set; }
            public string Patch { get; set; }
            public string ContentsUrl { get; set; }
        }

        
        public class SingleFileCommitModel : CommitModel
        {
            public List<SingleFileCommitFileReference> Added { get; set; }
            public List<SingleFileCommitFileReference> Removed { get; set; }
            public List<SingleFileCommitFileReference> Modified { get; set; }

            
            public class SingleFileCommitFileReference
            {
                public string Filename { get; set; }
            }
        }

        
        public class CommitDetailModel
        {
            public string Url { get; set; }
            public string Sha { get; set; }
            public AuthorModel Author { get; set; }
            public AuthorModel Committer { get; set; }
            public string Message { get; set; }
            public CommitParentModel Tree { get; set; }

            
            public class AuthorModel
            {
                public string Name { get; set; }
				public DateTimeOffset Date { get; set; }
                public string Email { get; set; }
            }
        }
    }
}

