using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    public class RepositoryModel
    {
        public int Id { get; set; }
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
        public long Size { get; set; }
        public string MasterBranch { get; set; }
        public int OpenIssues { get; set; }
        public DateTime PushedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public BasicUserModel Organization { get; set; }

        public RepositoryModel Source { get; set; }
        public RepositoryModel Parent { get; set; }

        public bool HasIssues { get; set; }
        public bool HasWiki { get; set; }
        public bool HasDownloads { get; set; }

        public string WatchersURL { get { return string.Format("http://github.com/{0}/{1}/watchers", Owner, Name); } }
        public string DownloadURL { get { return string.Format("http://github.com/{0}/{1}/zipball/master", Owner, Name); } }
        public string ForksURL { get { return string.Format("http://github.com/{0}/{1}/network/members", Owner, Name); } }
        public string IssuesURL { get { return string.Format("http://github.com/{0}/{1}/issues", Owner, Name); } }
        public string WikiURL { get { return string.Format("http://wiki.github.com/{0}/{1}", Owner, Name); } }
        public string GraphsURL { get { return string.Format("http://github.com/{0}/{1}/graphs", Owner, Name); } }
        public string ForkQuoueURL { get { return string.Format("http://github.com/{0}/{1}/forkqueue", Owner, Name); } }
        public string GitCloneURL { get { return string.Format("git://github.com/{0}/{1}.git", Owner, Name); } }
        public string HttpCloneURL { get { return string.Format("http://github.com/{0}/{1}.git", Owner, Name); } }
        public string ForkURL { get { return string.Format("http://github.com/{0}/{1}/fork", Owner, Name); } }
        public string WatchURL { get { return string.Format("http://github.com/{0}/{1}/toggle_watch", Owner, Name); } }
    }

    public class RepositorySearchModel
    {
        public List<RepositoryFromSearchModel> Repositories { get; set; }

        public class RepositoryFromSearchModel
        {
            public string Type { get; set; }
            public DateTime PushedAt { get; set; }
            public DateTime Pushed { get; set; }
            public int Watchers { get; set; }
            public int Forks { get; set; }
            public string Owner { get; set; }
            public string Description { get; set; }
            public string Language { get; set; }
            public DateTime Created { get; set; }
            public bool Fork { get; set; }
            public int Followers { get; set; }
            public long Size { get; set; }
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
    }

    public class ContentModel
    {
        public string Type { get; set; }
        public string HtmlUrl { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string Sha { get; set; }
        public long? Size { get; set; }
        public string Name { get; set; }
        public string GitUrl { get; set; }
    }
}
