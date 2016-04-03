using System;

namespace GitHubSharp.Models
{
    
    public class BasicUserModel
    {
        public string Login { get; set; }
        public long Id { get; set; }
        public string AvatarUrl { get; set; }
        public string GravatarId { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(BasicUserModel))
                return false;
            BasicUserModel other = (BasicUserModel)obj;
            return Login == other.Login && Id == other.Id && AvatarUrl == other.AvatarUrl && GravatarId == other.GravatarId && Url == other.Url && Type == other.Type;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Login != null ? Login.GetHashCode() : 0) ^ (Id != null ? Id.GetHashCode() : 0) ^ (AvatarUrl != null ? AvatarUrl.GetHashCode() : 0) ^ (GravatarId != null ? GravatarId.GetHashCode() : 0) ^ (Url != null ? Url.GetHashCode() : 0) ^ (Type != null ? Type.GetHashCode() : 0);
            }
        }
        
    }

    
    public class KeyModel
    {
        public string Url { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
    }
}

