using System.Collections.Generic;
using GitHubSharp.Models;

namespace GitHubSharp
{
    public class API
    {
        private readonly Client _client;

        public API(Client client)
        {
            _client = client;
        }

        #region Users

        public UserModel GetUser(string username = null)
        {
            return username == null ? _client.Get<UserAuthenticatedModel>("/user") :
                                      _client.Get<UserModel>("/users/" + username);
        }

        public List<BasicUserModel> GetUserFollowers(string username = null)
        {
            return username == null ? _client.Get<List<BasicUserModel>>("/user/followers") :
                                      _client.Get<List<BasicUserModel>>("/users/" + username + "/followers");
        }

        public List<BasicUserModel> GetUserFollowing(string username = null)
        {
            return username == null ? _client.Get<List<BasicUserModel>>("/user/following") :
                                      _client.Get<List<BasicUserModel>>("/users/" + username + "/following"); 
        }

        public List<KeyModel> GetUserKeys()
        {
            return _client.Get<List<KeyModel>>("/user/keys");
        }

        #endregion

        #region Followers

        public BasicUserModel GetFollowers(string username = null)
        {
            return username == null ? _client.Get<BasicUserModel>("/user/followers") :
                                      _client.Get<BasicUserModel>("/users/" + username + "/followers");
        }

        public BasicUserModel GetFollowing(string username)
        {
            return _client.Get<BasicUserModel>("/users/" + username + "/following");
        }

        public BasicUserModel GetAuthenticatedFollowing()
        {
            return _client.Get<BasicUserModel>("/user/following");
        }

        #endregion

        #region Repositories

        public List<RepositoryModel> ListRepositories(string username = null)
        {
            return username == null ? _client.Get<List<RepositoryModel>>("/user/repos") : 
                _client.Get<List<RepositoryModel>>("/users/" + username + "/repos");
        }

        public List<RepositoryModel> GetOrginizationRepositories(string org)
        {
            return _client.Get<List<RepositoryModel>>("/orgs/" + org + "/repos");
        }

        public RepositoryModel GetRepository(string username, string repo)
        {
            return _client.Get<RepositoryModel>("/repos/" + username + "/" + repo);
        }

        public List<BasicUserModel> GetContributors(string username, string repo)
        {
            return _client.Get<List<BasicUserModel>>("/repos/" + username + "/" + repo + "/contributors");
        }

        public Dictionary<string, int> GetLanguages(string username, string repo)
        {
            return _client.Get<Dictionary<string, int>>("/repos/" + username + "/" + repo + "/languages");
        }

        public List<TagModel> GetTags(string username, string repo)
        {
            return _client.Get<List<TagModel>>("/repos/" + username + "/" + repo + "/tags");
        }

        public List<BranchModel> GetBranches(string username, string repo)
        {
            return _client.Get<List<BranchModel>>("/repos/" + username + "/" + repo + "/branches");
        }

        public RepositorySearchModel SearchRepositories(string keyword)
        {
            return _client.Get<RepositorySearchModel>("/legacy/repos/search/" + keyword);
        }

        #endregion

        #region Gists

        public List<GistModel> GetGists(string username = null)
        {
            return username == null ? _client.Get<List<GistModel>>("/gists") :
                                      _client.Get<List<GistModel>>("/users/" + username + "/gists");
        }

        public List<GistModel> GetPublicGists()
        {
            return _client.Get<List<GistModel>>("/gists/public");
        }

        public List<GistModel> GetStarredGists()
        {
            return _client.Get<List<GistModel>>("/gists/starred");
        }

        public GistModel GetGist(int id)
        {
            return _client.Get<GistModel>("/gists/" + id);
        }

        #endregion

        #region Organizations

        public List<BasicUserModel> GetOrganizations(string username = null)
        {
            return username == null ? _client.Get<List<BasicUserModel>>("/user/orgs") :
                                      _client.Get<List<BasicUserModel>>("/users/" + username + "/orgs");
        }

        public UserModel GetOrganization(string org)
        {
            return _client.Get<UserModel>("/orgs/" + org);
        }

        public List<BasicUserModel> GetOrganizationMembers(string org)
        {
            return _client.Get<List<BasicUserModel>>("/orgs/" + org + "/members");
        }

        public List<BasicUserModel> GetOrganizationTeams(string org)
        {
            return _client.Get<List<BasicUserModel>>("/orgs/" + org + "/teams");
        }

        #endregion

        #region Teams

        public List<RepositoryModel> GetTeamRepositories(int id)
        {
            return _client.Get<List<RepositoryModel>>("/teams/" + id + "/repos");
        }

        #endregion

        #region Commits

        public List<CommitModel> GetCommits(string user, string repo)
        {
            return _client.Get<List<CommitModel>>("/repos/" + user + "/" + repo + "/commits");
        }

        public CommitModel GetCommit(string user, string repo, string sha)
        {
            return _client.Get<CommitModel>("/repos/" + user + "/" + repo + "/commits/" + sha); 
        }

        #endregion

        #region Watching

        public List<RepositoryModel> GetRepositoriesWatching(string owner = null)
        {
            return owner == null
                           ? _client.Get<List<RepositoryModel>>("/users/subscriptions")
                           : _client.Get<List<RepositoryModel>>("/users/" + owner + "/subscriptions");
        }

        public List<BasicUserModel> GetRepositoryWatchers(string owner, string repo)
        {
            return _client.Get<List<BasicUserModel>>("/repos/" + owner + "/" + repo + "/subscribers");
        }

        #endregion

        #region Tree

        public TreeModel GetTree(string owner, string repo, string sha)
        {
            return _client.Get<TreeModel>("/repos/" + owner + "/" + repo + "/git/trees/" + sha);
        }

        #endregion

        #region Events

        public List<EventModel> GetEvents(string username = null)
        {
            return username == null
                           ? _client.Get<List<EventModel>>("/events")
                           : _client.Get<List<EventModel>>("/users/" + username + "/events");
        }

        public List<EventModel> GetRepositoryEvents(string owner, string repo)
        {
            return _client.Get<List<EventModel>>("/repos/" + owner + "/" + repo + "/events");
        }

        #endregion

        #region Content

        public List<ContentModel> GetRepositoryContent(string owner, string repo, string path = "/", string branch = "master")
        {
            var url = "/repos/" + owner + "/" + repo + "/contents" + path + "?ref=" + branch;
            return _client.Get<List<ContentModel>>(url);
        }

        #endregion
    }
}
