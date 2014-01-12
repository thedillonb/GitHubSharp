using System;

namespace GitHubSharp.Models
{
    [Serializable]
    public class NotificationModel
    {
        public string Id { get; set; } // NB: API currently returns this as string which is Weird
        public RepositoryModel Repository { get; set; }
        public SubjectModel Subject { get; set; }
        public string Reason { get; set; }
        public bool Unread { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
		public DateTimeOffset? LastReadAt { get; set; }
        public string Url { get; set; }

        [Serializable]
        public class SubjectModel
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string LatestCommentUrl { get; set; }
            public string Type { get; set; }
        }
    }
}

