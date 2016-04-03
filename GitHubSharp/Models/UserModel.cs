using System;

namespace GitHubSharp.Models
{
    
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

        public bool? Hireable { get; set; }

        public string Bio { get; set; }

        public long PublicRepos { get; set; }

        public long PublicGists { get; set; }

        public long Followers { get; set; }

        public long Following { get; set; }

        public string HtmlUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Type { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(UserModel))
                return false;
            UserModel other = (UserModel)obj;
            return Login == other.Login && Id == other.Id && AvatarUrl == other.AvatarUrl && GravatarId == other.GravatarId && Url == other.Url && Name == other.Name && Company == other.Company && Blog == other.Blog && Location == other.Location && Email == other.Email && Hireable == other.Hireable && Bio == other.Bio && PublicRepos == other.PublicRepos && PublicGists == other.PublicGists && Followers == other.Followers && Following == other.Following && HtmlUrl == other.HtmlUrl && CreatedAt == other.CreatedAt && Type == other.Type;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Login != null ? Login.GetHashCode() : 0) ^ (Id != null ? Id.GetHashCode() : 0) ^ (AvatarUrl != null ? AvatarUrl.GetHashCode() : 0) ^ (GravatarId != null ? GravatarId.GetHashCode() : 0) ^ (Url != null ? Url.GetHashCode() : 0) ^ (Name != null ? Name.GetHashCode() : 0) ^ (Company != null ? Company.GetHashCode() : 0) ^ (Blog != null ? Blog.GetHashCode() : 0) ^ (Location != null ? Location.GetHashCode() : 0) ^ (Email != null ? Email.GetHashCode() : 0) ^ (Hireable != null ? Hireable.GetHashCode() : 0) ^ (Bio != null ? Bio.GetHashCode() : 0) ^ (PublicRepos != null ? PublicRepos.GetHashCode() : 0) ^ (PublicGists != null ? PublicGists.GetHashCode() : 0) ^ (Followers != null ? Followers.GetHashCode() : 0) ^ (Following != null ? Following.GetHashCode() : 0) ^ (HtmlUrl != null ? HtmlUrl.GetHashCode() : 0) ^ (CreatedAt != null ? CreatedAt.GetHashCode() : 0) ^ (Type != null ? Type.GetHashCode() : 0);
            }
        }
        
    }

    
    public class UserAuthenticatedModel : UserModel
    {
        public int TotalPrivateRepos { get; set; }

        public int OwnedPrivateRepos { get; set; }

        public long PrivateGists { get; set; }

        public long DiskUsage { get; set; }

        public long Collaborators { get; set; }
        //        public UserAuthenticatedPlanModel Plan { get; set; }
        //
        //        
        //        public class UserAuthenticatedPlanModel
        //        {
        //            public string Name { get; set; }
        //            public long Collaborators { get; set; }
        //            public long Space { get; set; }
        //            public long PrivateRepos { get; set; }
        //        }
    }

    
    public class PublicKeyModel
    {
        public string Title { get; set; }

        public long Id { get; set; }

        public string Key { get; set; }
    }
}

