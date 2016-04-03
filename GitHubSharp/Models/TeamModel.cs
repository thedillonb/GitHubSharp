using System;

namespace GitHubSharp.Models
{
    
    public class TeamShortModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public long Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(TeamShortModel))
                return false;
            TeamShortModel other = (TeamShortModel)obj;
            return Url == other.Url && Name == other.Name && Id == other.Id;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Url != null ? Url.GetHashCode() : 0) ^ (Name != null ? Name.GetHashCode() : 0) ^ (Id != null ? Id.GetHashCode() : 0);
            }
        }
    }

    
    public class TeamModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public long Id { get; set; }
        public string Permission { get; set; }
        public int MembersCount { get; set; }
        public int ReposCount { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(TeamModel))
                return false;
            TeamModel other = (TeamModel)obj;
            return Url == other.Url && Name == other.Name && Id == other.Id && Permission == other.Permission && MembersCount == other.MembersCount && ReposCount == other.ReposCount;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Url != null ? Url.GetHashCode() : 0) ^ (Name != null ? Name.GetHashCode() : 0) ^ (Id != null ? Id.GetHashCode() : 0) ^ (Permission != null ? Permission.GetHashCode() : 0) ^ (MembersCount != null ? MembersCount.GetHashCode() : 0) ^ (ReposCount != null ? ReposCount.GetHashCode() : 0);
            }
        }
        
    }
}

