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
        public string Path { get; set; }
        public int? Position { get; set; }
        public int? Line { get; set; }
        public string CommitId { get; set; }
        public BasicUserModel User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
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

