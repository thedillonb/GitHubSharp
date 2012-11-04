using System;

namespace GitHubSharp.Models
{
    public class EventModel
    {
        public string Type { get; set; }
        public bool Public { get; set; }
        public string Payload { get; set; }
        public RepoModel Repo { get; set; }
        public BasicUserModel Actor { get; set; }
        public BasicUserModel Org { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }

        public class RepoModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}



