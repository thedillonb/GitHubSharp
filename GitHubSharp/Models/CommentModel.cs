using System;

namespace GitHubSharp.Models
{
    public class CommentModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public BasicUserModel User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CommentForCreationOrEditModel
    {
        public string Body { get; set; }
    }
}

