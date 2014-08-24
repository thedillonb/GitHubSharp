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

        protected bool Equals(BasicUserModel other)
        {
            return string.Equals(Url, other.Url);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BasicUserModel) obj);
        }

        public override int GetHashCode()
        {
            return (Url != null ? Url.GetHashCode() : 0);
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

