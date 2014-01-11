using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    [Serializable]
    public class CommentModel
    {
        public string HtmlUrl { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public string Body { get; set; }
        public string Path { get; set; }
        public uint? Position { get; set; }
        public uint? Line { get; set; }
        public string CommitId { get; set; }
        public BasicUserModel User { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
    }

    [Serializable]
    public class CreateCommentModel
    {
        public string Body { get; set; }
        public string Path { get; set; }
        public uint? Position { get; set; }
    }

    [Serializable]
    public class CommentForCreationOrEditModel
    {
        public string Body { get; set; }
    }
}

