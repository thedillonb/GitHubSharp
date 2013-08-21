using GitHubSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    public class ExploreRepositoriesController : Controller
    {

        public ExploreRepositoriesController(Client client)
            : base(client)
        {
        }

        public GitHubResponse<RepositorySearchModel> SearchRepositories(string keyword)
        {
            return Client.Get<RepositorySearchModel>(Uri + "/search/" + keyword);
        }

        public override string Uri
        {
            get { return "legacy/repos"; }
        }
    }

    public class UserRepositoriesController : Controller
    {
        private readonly string User;

        public RepositoryController this[string key]
        {
            get { return new RepositoryController(Client, User, key); }
        }

        public UserRepositoriesController(Client client, string username)
            : base(client)
        {
            User = username;
        }

        public GitHubResponse<List<RepositoryModel>> GetAll(int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri, page: page, perPage: perPage);
        }

        public GitHubResponse<List<RepositoryModel>> GetWatching()
        {
            return Client.Get<List<RepositoryModel>>("users/" + User + "/subscriptions");
        }
   
        public override string Uri
        {
            get { return "users/" + User + "/repos"; }
        }
    }

    public class AuthenticatedRepositoriesController : Controller
    {
        public AuthenticatedRepositoriesController(Client client)
            : base(client)
        {
        }

        public GitHubResponse<List<RepositoryModel>> List(int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri + "/repos", page: page, perPage: perPage);
        }

        public GitHubResponse<List<RepositoryModel>> GetStarred(int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri + "/starred", page: page, perPage: perPage);
        }

        public GitHubResponse<List<RepositoryModel>> GetRepositoriesWatching(int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri + "/subscriptions", page: page, perPage: perPage);
        }


        public override string Uri
        {
            get { return "user"; }
        }
    }

    public class OrginzationRepositoriesController : Controller
    {
        private readonly string _name;

        public RepositoryController this[string key]
        {
            get { return new RepositoryController(Client, _name, key); }
        }

        public OrginzationRepositoriesController(Client client, string name)
            : base(client)
        {
            _name = name;
        }

        public GitHubResponse<List<RepositoryModel>> GetRepositories(int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return "orgs/" + _name + "/repos"; }
        }
    }

    public class RepositoryController : Controller
    {
        public string User { get; private set; }

        public string Repo { get; private set; }

        public CommentsController Comments
        {
            get { return new CommentsController(Client, this); }
        }

        public RepositoryController(Client client, string user, string repo)
            : base(client)
        {
            User = user;
            Repo = repo;
        }

        public GitHubResponse<RepositoryModel> GetInfo()
        {
            return Client.Get<RepositoryModel>(Uri);
        }

        public GitHubResponse<List<BasicUserModel>> GetContributors(int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/contributors", page: page, perPage: perPage);
        }

        public GitHubResponse<Dictionary<string, int>> GetLanguages()
        {
            return Client.Get<Dictionary<string, int>>(Uri + "/languages");
        }

        public GitHubResponse<List<TagModel>> GetTags()
        {
            return Client.Get<List<TagModel>>(Uri + "/tags");
        }

        public GitHubResponse<List<BranchModel>> GetBranches()
        {
            return Client.Get<List<BranchModel>>(Uri + "/branches");
        }

        public GitHubResponse<List<EventModel>> GetEvents(int page = 1, int perPage = 100)
        {
            return Client.Get<List<EventModel>>(Uri + "/events", page: page, perPage: perPage);
        }

        public GitHubResponse<List<EventModel>> GetNetworkEvents(int page = 1, int perPage = 100)
        {
            return Client.Get<List<EventModel>>("networks/" + User + "/" + Repo + "/events", page: page, perPage: perPage);
        }

        public GitHubResponse<ContentModel> GetReadme(string branch = "master")
        {
            return Client.Get<ContentModel>(Uri + "/readme", additionalArgs: new { Ref = branch });
        }

        public GitHubResponse<List<ContentModel>> GetContent(string path = "/", string branch = "master")
        {
            return Client.Get<List<ContentModel>>(Uri + "/contents" + path, additionalArgs: new { Ref = branch });
        }

        public GitHubResponse<TreeModel> GetTree(string sha)
        {
            return Client.Get<TreeModel>(Uri + "/git/trees/" + sha);
        }

        public GitHubResponse<List<CommitModel>> GetCommits(string user, string repo, string sha = null)
        {
            if (sha == null)
                return Client.Get<List<CommitModel>>(Uri + "/commits");
            else
                return Client.Get<List<CommitModel>>(Uri + "/commits", additionalArgs: new { Sha = sha });
        }

        public GitHubResponse<CommitModel> GetCommit(string user, string repo, string sha)
        {
            return Client.Get<CommitModel>(Uri + "/commits/" + sha);
        }

        public System.Net.HttpWebResponse GetFileRaw(string branch, string file, System.IO.Stream stream)
        {
            var uri = Client.RawUri + "/" + User + "/" + Repo + "/" + branch + "/";
            if (!uri.EndsWith("/") && !file.StartsWith("/"))
                file = "/" + file;

            var fileReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri + file);

            //Set the authentication!
            var authInfo = Client.Username + ":" + Client.Password;
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

        public bool IsWatching()
        {
            try
            {
                if (Client.Get<object>("user/subscriptions/" + User + "/" + Repo).StatusCode == 204)
                    return true;
            }
            catch (NotFoundException)
            {
            }

            return false;
        }

        public void Watch()
        {
            Client.Put("/user/subscriptions/" + User + "/" + Repo);
        }

        public void StopWatching()
        {
            Client.Delete("/user/subscriptions/" + User + "/" + Repo);
        }

        public GitHubResponse<SubscriptionModel> GetSubscription()
        {
            return Client.Get<SubscriptionModel>(Uri + "/subscription");
        }

        public GitHubResponse<SubscriptionModel> SetSubscription(bool subscribed, bool ignored)
        {
            return Client.Put<SubscriptionModel>(Uri + "/subscription", new { Subscribed = subscribed, Ignored = ignored });
        }

        public void DeleteSubscription()
        {
            Client.Delete(Uri + "/subscription");
        }

        public GitHubResponse<List<BasicUserModel>> GetStargazers(int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/stargazers", page: page, perPage: perPage);
        }


        public bool IsStarred()
        {
            try
            {
                if (Client.Get<object>("/user/starred/" + User + "/" + Repo).StatusCode == 204)
                    return true;
            }
            catch (NotFoundException)
            {
            }

            return false;
        }

        public void Star()
        {
            Client.Put("/user/starred/" + User + "/" + Repo);
        }

        public void Unstar()
        {
            Client.Delete("/user/starred/" + User + "/" + Repo);
        }

        public GitHubResponse<List<BasicUserModel>> GetCollaborators(int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/collaborators", page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return "repos/" + User + "/" + Repo; }
        }
    }
}
