using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    
    public class RepositoryModel
    {
        public long Id { get; set; }
        public BasicUserModel Owner { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public bool Fork { get; set; }
        public string Url { get; set; }
        public string Homepage { get; set; }
        public string Language { get; set; }
        public int Forks { get; set; }
        public int ForksCount { get; set; }
        public int Watchers { get; set; }
        public int WatchersCount { get; set; }
        public int SubscribersCount { get; set; }
        public long Size { get; set; }
        public string DefaultBranch { get; set; }
        public int OpenIssues { get; set; }
		public DateTimeOffset? PushedAt { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
		public int StargazersCount { get; set; }

        public BasicUserModel Organization { get; set; }

        public RepositoryModel Source { get; set; }
        public RepositoryModel Parent { get; set; }

        public bool HasIssues { get; set; }
        public bool HasWiki { get; set; }
        public bool HasDownloads { get; set; }

        public string HtmlUrl { get; set; }

		public RepositoryPermissions Permissions { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(RepositoryModel))
                return false;
            RepositoryModel other = (RepositoryModel)obj;
            return Id == other.Id && Owner == other.Owner && Name == other.Name && FullName == other.FullName && Description == other.Description && Private == other.Private && Fork == other.Fork && Url == other.Url && Homepage == other.Homepage && Language == other.Language && Forks == other.Forks && ForksCount == other.ForksCount && Watchers == other.Watchers && WatchersCount == other.WatchersCount && SubscribersCount == other.SubscribersCount && Size == other.Size && DefaultBranch == other.DefaultBranch && OpenIssues == other.OpenIssues && PushedAt == other.PushedAt && CreatedAt == other.CreatedAt && UpdatedAt == other.UpdatedAt && StargazersCount == other.StargazersCount && Organization == other.Organization && Source == other.Source && Parent == other.Parent && HasIssues == other.HasIssues && HasWiki == other.HasWiki && HasDownloads == other.HasDownloads && HtmlUrl == other.HtmlUrl && Permissions == other.Permissions;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id != null ? Id.GetHashCode() : 0) ^ (Owner != null ? Owner.GetHashCode() : 0) ^ (Name != null ? Name.GetHashCode() : 0) ^ (FullName != null ? FullName.GetHashCode() : 0) ^ (Description != null ? Description.GetHashCode() : 0) ^ (Private != null ? Private.GetHashCode() : 0) ^ (Fork != null ? Fork.GetHashCode() : 0) ^ (Url != null ? Url.GetHashCode() : 0) ^ (Homepage != null ? Homepage.GetHashCode() : 0) ^ (Language != null ? Language.GetHashCode() : 0) ^ (Forks != null ? Forks.GetHashCode() : 0) ^ (ForksCount != null ? ForksCount.GetHashCode() : 0) ^ (Watchers != null ? Watchers.GetHashCode() : 0) ^ (WatchersCount != null ? WatchersCount.GetHashCode() : 0) ^ (SubscribersCount != null ? SubscribersCount.GetHashCode() : 0) ^ (Size != null ? Size.GetHashCode() : 0) ^ (DefaultBranch != null ? DefaultBranch.GetHashCode() : 0) ^ (OpenIssues != null ? OpenIssues.GetHashCode() : 0) ^ (PushedAt != null ? PushedAt.GetHashCode() : 0) ^ (CreatedAt != null ? CreatedAt.GetHashCode() : 0) ^ (UpdatedAt != null ? UpdatedAt.GetHashCode() : 0) ^ (StargazersCount != null ? StargazersCount.GetHashCode() : 0) ^ (Organization != null ? Organization.GetHashCode() : 0) ^ (Source != null ? Source.GetHashCode() : 0) ^ (Parent != null ? Parent.GetHashCode() : 0) ^ (HasIssues != null ? HasIssues.GetHashCode() : 0) ^ (HasWiki != null ? HasWiki.GetHashCode() : 0) ^ (HasDownloads != null ? HasDownloads.GetHashCode() : 0) ^ (HtmlUrl != null ? HtmlUrl.GetHashCode() : 0) ^ (Permissions != null ? Permissions.GetHashCode() : 0);
            }
        }
        
    }

	public class RepositoryPermissions
	{
		public bool Admin { get; set; }
		public bool Push { get; set; }
		public bool Pull { get; set; }
	}

    
    public class RepositorySearchModel
    {
		public int TotalCount { get; set; }
		public List<RepositoryModel> Items { get; set; }

        
        public class RepositoryModel
        {
			public long Id { get; set; }
			public string Name { get; set; }
			public string FullName { get; set; }
			public BasicUserModel Owner { get; set; }
			public bool Private { get; set; }
			public string HtmlUrl { get; set; }
			public string Description { get; set; }
			public bool Fork { get; set; }
			public string Url { get; set; }
			public DateTimeOffset CreatedAt { get; set; }
			public DateTimeOffset UpdatedAt { get; set; }
			public DateTimeOffset? PushedAt { get; set; }
			public string Homepage { get; set; }
			public int StargazersCount { get; set; }
			public int WatchersCount { get; set; }
			public string Language { get; set; }
			public int ForksCount { get; set; }
			public int OpenIssuesCount { get; set; }
			public float Score { get; set; }
        }
    }

    
    public class TagModel
    {
        public string Name { get; set; }
        public TagCommitModel Commit { get; set; }

        
        public class TagCommitModel
        {
            public string Sha { get; set; }
        }
    }

    
    public class BranchModel
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(BranchModel))
                return false;
            BranchModel other = (BranchModel)obj;
            return Name == other.Name;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name != null ? Name.GetHashCode() : 0);
            }
        }
        
    }

    
    public class ContentModel
    {
        public string Type { get; set; }
        public string Sha { get; set; }
        public string Path { get; set; }
        public string GitUrl { get; set; }
		public string HtmlUrl { get; set; }
        public string DownloadUrl { get; set; }
        public string Encoding { get; set; }
        public string Url { get; set; }
        public long? Size { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(ContentModel))
                return false;
            ContentModel other = (ContentModel)obj;
            return Type == other.Type && Sha == other.Sha && Path == other.Path && GitUrl == other.GitUrl && HtmlUrl == other.HtmlUrl && DownloadUrl == other.DownloadUrl && Encoding == other.Encoding && Url == other.Url && Size == other.Size && Name == other.Name && Content == other.Content;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (Type != null ? Type.GetHashCode() : 0) ^ (Sha != null ? Sha.GetHashCode() : 0) ^ (Path != null ? Path.GetHashCode() : 0) ^ (GitUrl != null ? GitUrl.GetHashCode() : 0) ^ (HtmlUrl != null ? HtmlUrl.GetHashCode() : 0) ^ (DownloadUrl != null ? DownloadUrl.GetHashCode() : 0) ^ (Encoding != null ? Encoding.GetHashCode() : 0) ^ (Url != null ? Url.GetHashCode() : 0) ^ (Size != null ? Size.GetHashCode() : 0) ^ (Name != null ? Name.GetHashCode() : 0) ^ (Content != null ? Content.GetHashCode() : 0);
            }
        }
        
    }
}

