using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    
    public class CommentModel
    {
        public string HtmlUrl { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public string Body { get; set; }
        public string BodyHtml { get; set; }
        public string Path { get; set; }
        public int? Position { get; set; }
        public int? Line { get; set; }
        public string CommitId { get; set; }
        public BasicUserModel User { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(CommentModel))
                return false;
            CommentModel other = (CommentModel)obj;
            return HtmlUrl == other.HtmlUrl && Url == other.Url && Id == other.Id && Body == other.Body && BodyHtml == other.BodyHtml && Path == other.Path && Position == other.Position && Line == other.Line && CommitId == other.CommitId && User == other.User && CreatedAt == other.CreatedAt && UpdatedAt == other.UpdatedAt;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (HtmlUrl != null ? HtmlUrl.GetHashCode() : 0) ^ (Url != null ? Url.GetHashCode() : 0) ^ (Id != null ? Id.GetHashCode() : 0) ^ (Body != null ? Body.GetHashCode() : 0) ^ (BodyHtml != null ? BodyHtml.GetHashCode() : 0) ^ (Path != null ? Path.GetHashCode() : 0) ^ (Position != null ? Position.GetHashCode() : 0) ^ (Line != null ? Line.GetHashCode() : 0) ^ (CommitId != null ? CommitId.GetHashCode() : 0) ^ (User != null ? User.GetHashCode() : 0) ^ (CreatedAt != null ? CreatedAt.GetHashCode() : 0) ^ (UpdatedAt != null ? UpdatedAt.GetHashCode() : 0);
            }
        }
    }

    
    public class CreateCommentModel
    {
        public string Body { get; set; }
        public string Path { get; set; }
        public int? Position { get; set; }
    }

    
    public class CommentForCreationOrEditModel
    {
        public string Body { get; set; }
    }
}

