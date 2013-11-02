using System;

namespace GitHubSharp.Models
{
    [Serializable]
    public class UserInCollectionModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public ulong Followers { get; set; }
        public string Username { get; set; }
        public string Language { get; set; }
        public string Fullname { get; set; }
        public ulong Repos { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime Pushed { get; set; }
        public float Score { get; set; }
        public DateTime Created { get; set; }
    }

    [Serializable]
    public class UserModel
    {
        public string Login { get; set; }
        public ulong Id { get; set; }
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
        public ulong PublicRepos { get; set; }
        public ulong PublicGists { get; set; }
        public ulong Followers { get; set; }
        public ulong Following { get; set; }
        public string HtmlUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Type { get; set; }
    }

    [Serializable]
    public class UserAuthenticatedModel : UserModel
    {
        public uint TotalPrivateRepos { get; set; }
        public uint OwnedPrivateRepos { get; set; }
        public ulong PrivateGists { get; set; }
        public ulong DiskUsage { get; set; }
        public ulong Collaborators { get; set; }
//        public UserAuthenticatedPlanModel Plan { get; set; }
//
//        [Serializable]
//        public class UserAuthenticatedPlanModel
//        {
//            public string Name { get; set; }
//            public ulong Collaborators { get; set; }
//            public ulong Space { get; set; }
//            public ulong PrivateRepos { get; set; }
//        }
    }

    [Serializable]
    public class PublicKeyModel
    {
        public string Title { get; set; }
        public ulong Id { get; set; }
        public string Key { get; set; }
    }
}

