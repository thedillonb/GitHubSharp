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


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(CommitModel))
                return false;
            CommitModel other = (CommitModel)obj;
            return Url == other.Url && HtmlUrl == other.HtmlUrl && CommentsUrl == other.CommentsUrl && Sha == other.Sha && Commit == other.Commit && Author == other.Author && Committer == other.Committer && Parents == other.Parents && Stats == other.Stats && Files == other.Files;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Url != null ? Url.GetHashCode() : 0) ^ (HtmlUrl != null ? HtmlUrl.GetHashCode() : 0) ^ (CommentsUrl != null ? CommentsUrl.GetHashCode() : 0) ^ (Sha != null ? Sha.GetHashCode() : 0) ^ (Commit != null ? Commit.GetHashCode() : 0) ^ (Author != null ? Author.GetHashCode() : 0) ^ (Committer != null ? Committer.GetHashCode() : 0) ^ (Parents != null ? Parents.GetHashCode() : 0) ^ (Stats != null ? Stats.GetHashCode() : 0) ^ (Files != null ? Files.GetHashCode() : 0);
            }
        }
        
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

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != typeof(CommitFileModel))
                    return false;
                CommitFileModel other = (CommitFileModel)obj;
                return Filename == other.Filename && Additions == other.Additions && Deletions == other.Deletions && Changes == other.Changes && Status == other.Status && RawUrl == other.RawUrl && BlobUrl == other.BlobUrl && Patch == other.Patch && ContentsUrl == other.ContentsUrl;
            }
            

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Filename != null ? Filename.GetHashCode() : 0) ^ (Additions != null ? Additions.GetHashCode() : 0) ^ (Deletions != null ? Deletions.GetHashCode() : 0) ^ (Changes != null ? Changes.GetHashCode() : 0) ^ (Status != null ? Status.GetHashCode() : 0) ^ (RawUrl != null ? RawUrl.GetHashCode() : 0) ^ (BlobUrl != null ? BlobUrl.GetHashCode() : 0) ^ (Patch != null ? Patch.GetHashCode() : 0) ^ (ContentsUrl != null ? ContentsUrl.GetHashCode() : 0);
                }
            }
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

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != typeof(CommitDetailModel))
                    return false;
                CommitDetailModel other = (CommitDetailModel)obj;
                return Url == other.Url && Sha == other.Sha && Author == other.Author && Committer == other.Committer && Message == other.Message && Tree == other.Tree;
            }
            

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Url != null ? Url.GetHashCode() : 0) ^ (Sha != null ? Sha.GetHashCode() : 0) ^ (Author != null ? Author.GetHashCode() : 0) ^ (Committer != null ? Committer.GetHashCode() : 0) ^ (Message != null ? Message.GetHashCode() : 0) ^ (Tree != null ? Tree.GetHashCode() : 0);
                }
            }
            
        }
    }
}

