using System;

namespace GitHubSharp.Models
{
    
    public class TeamShortModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public long Id { get; set; }
    }

    
    public class TeamModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public long Id { get; set; }
        public string Permission { get; set; }
        public int MembersCount { get; set; }
        public int ReposCount { get; set; }
    }
}

