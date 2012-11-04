using System;

namespace GitHubSharp.Models
{
    public class UserInCollectionModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int Followers { get; set; }
        public string Username { get; set; }
        public string Language { get; set; }
        public string Fullname { get; set; }
        public int Repos { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime Pushed { get; set; }
        public float Score { get; set; }
        public DateTime Created { get; set; }
    }

    public class UserModel
    {
        public string Login { get; set; }
        public int Id { get; set; }
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
        public int PublicRepos { get; set; }
        public int PublicGists { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public string HtmlUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Type { get; set; }
    }

    public class UserAuthenticatedModel : UserModel
    {
        public int TotalPrivateRepos { get; set; }
        public int OwnedPrivateRepos { get; set; }
        public int PrivateGists { get; set; }
        public long DiskUsage { get; set; }
        public int Collaborators { get; set; }
        public UserAuthenticatedPlanModel Plan { get; set; }
        public class UserAuthenticatedPlanModel
        {
            public string Name { get; set; }
            public int Collaborators { get; set; }
            public long Space { get; set; }
            public int PrivateRepos { get; set; }
        }
    }

    public class PublicKeyModel
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string Key { get; set; }
    }
}

