using System;

namespace GitHubSharp.Models
{
    [Serializable]
    public class TeamShortModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public ulong Id { get; set; }
    }

    [Serializable]
    public class TeamModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public ulong Id { get; set; }
        public string Permission { get; set; }
        public uint MembersCount { get; set; }
        public uint ReposCount { get; set; }
    }
}

