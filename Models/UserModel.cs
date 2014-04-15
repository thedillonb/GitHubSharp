using System;

namespace GitHubSharp.Models
{
    [Serializable]
    public class UserModel
    {
        public string Login { get; set; }

        public long Id { get; set; }

        public string AvatarUrl { get; set; }

        public string GravatarId { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public string Blog { get; set; }

        public string Location { get; set; }

        public string Email { get; set; }

        public bool Hireable { get; set; }

        public string Bio { get; set; }

        public long PublicRepos { get; set; }

        public long PublicGists { get; set; }

        public long Followers { get; set; }

        public long Following { get; set; }

        public string HtmlUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Type { get; set; }
    }

    [Serializable]
    public class UserAuthenticatedModel : UserModel
    {
        public int TotalPrivateRepos { get; set; }

        public int OwnedPrivateRepos { get; set; }

        public long PrivateGists { get; set; }

        public long DiskUsage { get; set; }

        public long Collaborators { get; set; }
        //        public UserAuthenticatedPlanModel Plan { get; set; }
        //
        //        [Serializable]
        //        public class UserAuthenticatedPlanModel
        //        {
        //            public string Name { get; set; }
        //            public long Collaborators { get; set; }
        //            public long Space { get; set; }
        //            public long PrivateRepos { get; set; }
        //        }
    }

    [Serializable]
    public class PublicKeyModel
    {
        public string Title { get; set; }

        public long Id { get; set; }

        public string Key { get; set; }
    }
}

