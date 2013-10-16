using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    [Serializable]
    public class RepositoryModel
    {
        public ulong Id { get; set; }
        public BasicUserModel Owner { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public bool Fork { get; set; }
        public string Url { get; set; }
        public string Homepage { get; set; }
        public string Language { get; set; }
        public uint Forks { get; set; }
        public uint ForksCount { get; set; }
        public uint Watchers { get; set; }
        public uint WatchersCount { get; set; }
        public ulong Size { get; set; }
        public string MasterBranch { get; set; }
        public uint OpenIssues { get; set; }
        public DateTime PushedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public BasicUserModel Organization { get; set; }

        public RepositoryModel Source { get; set; }
        public RepositoryModel Parent { get; set; }

        public bool HasIssues { get; set; }
        public bool HasWiki { get; set; }
        public bool HasDownloads { get; set; }

        public string HtmlUrl { get; set; }
    }

    [Serializable]
    public class RepositorySearchModel
    {
        public List<RepositoryModel> Repositories { get; set; }

        [Serializable]
        public class RepositoryModel
        {
            public string Type { get; set; }
            public DateTime PushedAt { get; set; }
            public DateTime Pushed { get; set; }
            public uint Watchers { get; set; }
            public uint Forks { get; set; }
            public string Owner { get; set; }
            public string Description { get; set; }
            public string Language { get; set; }
            public DateTime Created { get; set; }
            public bool Fork { get; set; }
            public uint Followers { get; set; }
            public ulong Size { get; set; }
            public string Name { get; set; }
            public bool Private { get; set; }
            public DateTime CreatedAt { get; set; }
            public string Username { get; set; }
//            
//            public bool HasDownloads { get; set; }
//            public string Homepage { get; set; }
//            public string Url { get; set; }
//            public bool HasIssues { get; set; }
//            public bool HasWiki { get; set; }
//            public int OpenIssues { get; set; }
//            public float Score { get; set; }
        }
    }

    [Serializable]
    public class TagModel
    {
        public string Name { get; set; }
        public TagCommitModel Commit { get; set; }

        [Serializable]
        public class TagCommitModel
        {
            public string Sha { get; set; }
        }
    }

    [Serializable]
    public class BranchModel
    {
        public string Name { get; set; }
    }

    [Serializable]
    public class ContentModel
    {
        public string Type { get; set; }
        public string Sha { get; set; }
        public string Path { get; set; }
        public string GitUrl { get; set; }
        public string Encoding { get; set; }
        public string Url { get; set; }
        public ulong? Size { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}

