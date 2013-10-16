using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    [Serializable]
    public class AuthorizationModel
    {
        public ulong Id { get; set; }
        public string Url { get; set; }
        public List<string> Scopes { get; set; }
        public string Token { get; set; }
        public AppModel App { get; set; }
        public string Note { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        [Serializable]
        public class AppModel
        {
            public string Url { get; set; }
            public string Name { get; set; }
            public string ClientId { get; set; }
        }
    }

    [Serializable]
    public class AccessTokenModel
    {
        public string AccessToken { get; set; }
        public string Scope { get; set; }
        public string TokenType { get; set; }
    }
}

