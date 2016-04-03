using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    
    public class GistModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
		public bool? Public { get; set; }
        public BasicUserModel Owner { get; set; }
        public Dictionary<string, GistFileModel> Files { get; set; }
        public long Comments { get; set; }
        public string CommentsUrl { get; set; }
        public string HtmlUrl { get; set; }
        public string GitPullUrl { get; set; }
        public string GitPushUrl { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
        public List<ForkModel> Forks { get; set; }
        public List<HistoryModel> History { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(GistModel))
                return false;
            GistModel other = (GistModel)obj;
            return Url == other.Url && Id == other.Id && Description == other.Description && Public == other.Public && Owner == other.Owner && Files == other.Files && Comments == other.Comments && CommentsUrl == other.CommentsUrl && HtmlUrl == other.HtmlUrl && GitPullUrl == other.GitPullUrl && GitPushUrl == other.GitPushUrl && CreatedAt == other.CreatedAt && UpdatedAt == other.UpdatedAt && Forks == other.Forks && History == other.History;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Url != null ? Url.GetHashCode() : 0) ^ (Id != null ? Id.GetHashCode() : 0) ^ (Description != null ? Description.GetHashCode() : 0) ^ (Public != null ? Public.GetHashCode() : 0) ^ (Owner != null ? Owner.GetHashCode() : 0) ^ (Files != null ? Files.GetHashCode() : 0) ^ (Comments != null ? Comments.GetHashCode() : 0) ^ (CommentsUrl != null ? CommentsUrl.GetHashCode() : 0) ^ (HtmlUrl != null ? HtmlUrl.GetHashCode() : 0) ^ (GitPullUrl != null ? GitPullUrl.GetHashCode() : 0) ^ (GitPushUrl != null ? GitPushUrl.GetHashCode() : 0) ^ (CreatedAt != null ? CreatedAt.GetHashCode() : 0) ^ (UpdatedAt != null ? UpdatedAt.GetHashCode() : 0) ^ (Forks != null ? Forks.GetHashCode() : 0) ^ (History != null ? History.GetHashCode() : 0);
            }
        }
    }

    
    public class GistFileModel
    {
        public long Size { get; set; }
        public string Filename { get; set; }
        public string RawUrl { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(GistFileModel))
                return false;
            GistFileModel other = (GistFileModel)obj;
            return Size == other.Size && Filename == other.Filename && RawUrl == other.RawUrl && Content == other.Content && Type == other.Type && Language == other.Language;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Size != null ? Size.GetHashCode() : 0) ^ (Filename != null ? Filename.GetHashCode() : 0) ^ (RawUrl != null ? RawUrl.GetHashCode() : 0) ^ (Content != null ? Content.GetHashCode() : 0) ^ (Type != null ? Type.GetHashCode() : 0) ^ (Language != null ? Language.GetHashCode() : 0);
            }
        }
    }

    
    public class GistCommentModel
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string BodyHtml { get; set; }
        public BasicUserModel User { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(GistCommentModel))
                return false;
            GistCommentModel other = (GistCommentModel)obj;
            return Id == other.Id && Url == other.Url && Body == other.Body && BodyHtml == other.BodyHtml && User == other.User && CreatedAt == other.CreatedAt;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id != null ? Id.GetHashCode() : 0) ^ (Url != null ? Url.GetHashCode() : 0) ^ (Body != null ? Body.GetHashCode() : 0) ^ (BodyHtml != null ? BodyHtml.GetHashCode() : 0) ^ (User != null ? User.GetHashCode() : 0) ^ (CreatedAt != null ? CreatedAt.GetHashCode() : 0);
            }
        }
    }

    
    public class GistEditModel
    {
        public string Description { get; set; }
        public Dictionary<string, File> Files { get; set; }
    
        
        public class File
        {
            public string Filename { get; set; }
            public string Content { get; set; }
        }
    }

    
    public class GistCreateModel
    {
        public string Description { get; set; }
		public bool? Public { get; set; }
        public Dictionary<string, File> Files { get; set; }

        
        public class File
        {
            public string Content { get; set; }
        }
    }
}

