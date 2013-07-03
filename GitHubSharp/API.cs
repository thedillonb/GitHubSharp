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

        public GitHubResponse<UserAuthenticatedModel> GetAuthenticatedUser()
        {
            return _client.Get<UserAuthenticatedModel>("/user");
        }

        public GitHubResponse<UserModel> GetUser(string username = null)
        {
            return _client.Get<UserModel>("/users/" + username);
        }

        public GitHubResponse<List<BasicUserModel>> GetUserFollowers(string username = null, int page = 1, int perPage = 100)
        {
            var sb = new System.Text.StringBuilder();
            if (username == null)
                sb.Append("/user/followers");
            else
                sb.Append("/users/").Append(username).Append("/followers");
            sb.Append("?page=").Append(page).Append("&per_page=").Append(perPage);
            return _client.Get<List<BasicUserModel>>(sb.ToString());
        }

        public GitHubResponse<List<BasicUserModel>> GetUserFollowing(string username = null)
        {
            return username == null ? _client.Get<List<BasicUserModel>>("/user/following") :
                                      _client.Get<List<BasicUserModel>>("/users/" + username + "/following"); 
        }

        public GitHubResponse<List<KeyModel>> GetUserKeys()
        {
            return _client.Get<List<KeyModel>>("/user/keys");
        }

        #endregion

        #region Followers

        public GitHubResponse<BasicUserModel> GetFollowers(string username = null)
        {
            return username == null ? _client.Get<BasicUserModel>("/user/followers") :
                                      _client.Get<BasicUserModel>("/users/" + username + "/followers");
        }

        public GitHubResponse<BasicUserModel> GetFollowing(string username)
        {
            return _client.Get<BasicUserModel>("/users/" + username + "/following");
        }

        public GitHubResponse<BasicUserModel> GetAuthenticatedFollowing()
        {
            return _client.Get<BasicUserModel>("/user/following");
        }

        #endregion

        #region Repositories

        public GitHubResponse<List<RepositoryModel>> ListRepositories(string username = null, int page = 1, int perPage = 100)
        {
            return username == null ? _client.Get<List<RepositoryModel>>("/user/repos?page=" + page + "&per_page=" + perPage) : 
                _client.Get<List<RepositoryModel>>("/users/" + username + "/repos?page=" + page + "&per_page=" + perPage);
        }

        public GitHubResponse<List<RepositoryModel>> GetOrginizationRepositories(string org, int page = 1, int perPage = 100)
        {
            return _client.Get<List<RepositoryModel>>("/orgs/" + org + "/repos?page=" + page + "&per_page=" + perPage);
        }

        public GitHubResponse<RepositoryModel> GetRepository(string username, string repo)
        {
            return _client.Get<RepositoryModel>("/repos/" + username + "/" + repo);
        }

        public GitHubResponse<List<BasicUserModel>> GetContributors(string username, string repo)
        {
            return _client.Get<List<BasicUserModel>>("/repos/" + username + "/" + repo + "/contributors");
        }

        public GitHubResponse<Dictionary<string, int>> GetLanguages(string username, string repo)
        {
            return _client.Get<Dictionary<string, int>>("/repos/" + username + "/" + repo + "/languages");
        }

        public GitHubResponse<List<TagModel>> GetTags(string username, string repo)
        {
            return _client.Get<List<TagModel>>("/repos/" + username + "/" + repo + "/tags");
        }

        public GitHubResponse<List<BranchModel>> GetBranches(string username, string repo)
        {
            return _client.Get<List<BranchModel>>("/repos/" + username + "/" + repo + "/branches");
        }

        public GitHubResponse<RepositorySearchModel> SearchRepositories(string keyword)
        {
            return _client.Get<RepositorySearchModel>("/legacy/repos/search/" + keyword);
        }

        public GitHubResponse<List<RepositoryModel>> GetRepositoriesStarred()
        {
            return _client.Get<List<RepositoryModel>>("/user/starred");
        }

        #endregion

        #region Gists

        public GitHubResponse<List<GistModel>> GetGists(string username = null, int page = 1, int perPage = 100)
        {
            return username == null ? _client.Get<List<GistModel>>("/gists?page=" + page + "&per_page=" + perPage) :
               _client.Get<List<GistModel>>("/users/" + username + "/gists?page=" + page + "&per_page=" + perPage);
        }

        public GitHubResponse<List<GistModel>> GetPublicGists(int page = 1, int perPage = 100)
        {
            return _client.Get<List<GistModel>>("/gists/public?page=" + page + "&per_page=" + perPage);
        }

        public GitHubResponse<List<GistModel>> GetStarredGists(int page = 1, int perPage = 100)
        {
            return _client.Get<List<GistModel>>("/gists/starred?page=" + page + "&per_page=" + perPage);
        }

        public GitHubResponse<GistModel> GetGist(string id)
        {
            return _client.Get<GistModel>("/gists/" + id);
        }

        public string GetGistFile(string url)
        {
            var a = _client.ExecuteRequest(url, RestSharp.Method.GET, null);
            return a.Content;
        }

        public void StarGist(string id)
        {
            _client.Put("/gists/" + id + "/star");
        }

        public GitHubResponse<List<GistCommentModel>> GetGistComments(string id)
        {
            return _client.Get<List<GistCommentModel>>("/gists/" + id + "/comments");
        }

        public GitHubResponse<GistModel> ForkGist(string id)
        {
            return _client.Request<GistModel>("/gists/" + id + "/forks", RestSharp.Method.POST, null);
        }

        public void DeleteGist(string id)
        {
            _client.Delete("/gists/" + id);
        }

        public void UnstarGist(string id)
        {
            _client.Delete("/gists/" + id + "/star");
        }

        public bool IsGistStarred(string id)
        {
            try
            {
                var response = _client.ExecuteRequest(Client.ApiUrl + "/gists/" + id + "/star", RestSharp.Method.GET, null);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
            }
            catch (NotFoundException e)
            {
                //This means the gist was NOT starred...
            }
            return false;

        }

        public GitHubResponse<GistCommentModel> CreateGistComment(string id, string body)
        {
            return _client.RequestWithJson<GistCommentModel>("/gists/" + id + "/comments", RestSharp.Method.POST, new { body = body });
        }

        #endregion

        #region Organizations

        public GitHubResponse<List<BasicUserModel>> GetOrganizations(string username = null)
        {
            return username == null ? _client.Get<List<BasicUserModel>>("/user/orgs") :
                                      _client.Get<List<BasicUserModel>>("/users/" + username + "/orgs");
        }

        public GitHubResponse<UserModel> GetOrganization(string org)
        {
            return _client.Get<UserModel>("/orgs/" + org);
        }

        public GitHubResponse<List<BasicUserModel>> GetOrganizationMembers(string org)
        {
            return _client.Get<List<BasicUserModel>>("/orgs/" + org + "/members");
        }

        public GitHubResponse<List<BasicUserModel>> GetOrganizationTeams(string org)
        {
            return _client.Get<List<BasicUserModel>>("/orgs/" + org + "/teams");
        }

        #endregion

        #region Teams

        public GitHubResponse<List<RepositoryModel>> GetTeamRepositories(int id)
        {
            return _client.Get<List<RepositoryModel>>("/teams/" + id + "/repos");
        }

        #endregion

        #region Commits

        public GitHubResponse<List<CommitModel>> GetCommits(string user, string repo, string sha = null)
        {
            if (sha == null)
                return _client.Get<List<CommitModel>>("/repos/" + user + "/" + repo + "/commits");
            else
                return _client.Get<List<CommitModel>>("/repos/" + user + "/" + repo + "/commits?sha=" + sha);
        }

        public GitHubResponse<CommitModel> GetCommit(string user, string repo, string sha)
        {
            return _client.Get<CommitModel>("/repos/" + user + "/" + repo + "/commits/" + sha); 
        }

        #endregion

        #region Watching

        public GitHubResponse<List<RepositoryModel>> GetRepositoriesWatching(string owner = null)
        {
            return owner == null
                           ? _client.Get<List<RepositoryModel>>("/users/subscriptions")
                           : _client.Get<List<RepositoryModel>>("/users/" + owner + "/subscriptions");
        }

        public GitHubResponse<List<BasicUserModel>> GetRepositoryWatchers(string owner, string repo, int page = 1, int perPage = 100)
        {
            return _client.Get<List<BasicUserModel>>("/repos/" + owner + "/" + repo + "/watchers?page=" + page + "&per_page=" + perPage);
        }

        #endregion

        #region Tree

        public GitHubResponse<TreeModel> GetTree(string owner, string repo, string sha)
        {
            return _client.Get<TreeModel>("/repos/" + owner + "/" + repo + "/git/trees/" + sha);
        }

        #endregion

        #region Events

        public GitHubResponse<List<EventModel>> GetEvents(string username = null, int page = 1)
        {
            return username == null
                           ? _client.Get<List<EventModel>>("/events?page=" + page)
                           : _client.Get<List<EventModel>>("/users/" + username + "/events?page=" + page);
        }

        public GitHubResponse<List<EventModel>> GetRepositoryEvents(string owner, string repo, int page = 1)
        {
            return _client.Get<List<EventModel>>("/repos/" + owner + "/" + repo + "/events?page=" + page);
        }

        #endregion

        #region Content

        public GitHubResponse<List<ContentModel>> GetRepositoryContent(string owner, string repo, string path = "/", string branch = "master")
        {
            var url = "/repos/" + owner + "/" + repo + "/contents" + path + "?ref=" + branch;
            return _client.Get<List<ContentModel>>(url);
        }

        public System.Net.HttpWebResponse GetFileRaw(string owner, string repo, string branch, string file, System.IO.Stream stream)
        {
            var uri = Client.RawUrl + "/" + owner + "/" + repo + "/" + branch + "/";
            if (!uri.EndsWith("/") && !file.StartsWith("/"))
                file = "/" + file;
            
            var fileReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri + file);
            
            //Set the authentication!
            var authInfo = _client.Username + ":" + _client.Password;
            authInfo = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authInfo));
            fileReq.Headers["Authorization"] = "Basic " + authInfo;
            
            var resp = (System.Net.HttpWebResponse)fileReq.GetResponse();
            if (resp != null)
            {
                using (var dstream = resp.GetResponseStream())
                {
                    var buffer = new byte[1024];
                    while (true)
                    {
                        var bytesRead = dstream.Read(buffer, 0, 1024);
                        if (bytesRead <= 0)
                            break;
                        stream.Write(buffer, 0, bytesRead);
                    }
                }
            }
            
            return resp;
        }

        public GitHubResponse<ContentModel> GetRepositoryReadme(string owner, string repo, string branch = "master")
        {
            return _client.Get<ContentModel>("/repos/" + owner + "/" + repo + "/readme?ref=" + branch);
        }

        #endregion

        #region Notifications

        public GitHubResponse<List<NotificationModel>> GetNotifications(bool all = false, bool participating = false)
        {
            return _client.Get<List<NotificationModel>>("/notifications?all=" + (all.ToString().ToLower()) + "&participating=" + (participating.ToString().ToLower()));
        }

        #endregion
    
        #region Markdown

        public string GetMarkdown(string text)
        {
            var request = new RestSharp.RestRequest(Client.ApiUrl + "/markdown/raw", RestSharp.Method.POST);
            request.AddParameter("text/plain", text, RestSharp.ParameterType.RequestBody);
            var response = _client.ExecuteRequest(request);
            return response.Content;
        }

        #endregion
    }
}
