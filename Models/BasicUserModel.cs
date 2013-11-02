using System;

namespace GitHubSharp.Models
{
    [Serializable]
    public class BasicUserModel
    {
        public string Login { get; set; }
        public ulong Id { get; set; }
        public string AvatarUrl { get; set; }
        public string GravatarId { get; set; }
        public string Url { get; set; }
    }

    [Serializable]
    public class KeyModel
    {
        public string Url { get; set; }
        public ulong Id { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
    }
}

