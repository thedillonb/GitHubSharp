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
            get { return Client.ApiUri + "/legacy/repos"; }
        }
    }

    public class UserRepositoriesController : Controller
    {
        public UserController UserController { get; private set; }

        public RepositoryController this[string key]
        {
            get { return new RepositoryController(Client, UserController.Name, key); }
        }

        public UserRepositoriesController(Client client, UserController userController)
            : base(client)
        {
            UserController = userController;
        }

        public GitHubResponse<List<RepositoryModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<RepositoryModel>> GetWatching(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<RepositoryModel>>(UserController.Uri + "/subscriptions", forceCacheInvalidation: forceCacheInvalidation);
        }
   
        public override string Uri
        {
            get { return UserController.Uri + "/repos"; }
        }
    }

    public class AuthenticatedRepositoriesController : Controller
    {
        public RepositoryController this[string key]
        {
            get { return new RepositoryController(Client, Client.Username, key); }
        }

        public AuthenticatedRepositoriesController(Client client)
            : base(client)
        {
        }

        public GitHubResponse<List<RepositoryModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri + "/repos", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<RepositoryModel>> GetStarred(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri + "/starred", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<RepositoryModel>> GetWatching(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri + "/subscriptions", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/user"; }
        }
    }

    public class OrginzationRepositoriesController : Controller
    {
        public OrganizationController OrganizationController { get; private set; }

        public RepositoryController this[string key]
        {
            get { return new RepositoryController(Client, OrganizationController.Name, key); }
        }

        public OrginzationRepositoriesController(Client client, OrganizationController organizationController)
            : base(client)
        {
            OrganizationController = organizationController;
        }

        public GitHubResponse<List<RepositoryModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return OrganizationController.Uri + "/repos"; }
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

        public CommitsController Commits
        {
            get { return new CommitsController(Client, this); }
        }

        public RepositoryIssuesController Issues
        {
            get { return new RepositoryIssuesController(Client, this); }
        }

        public PullRequestsController PullRequests
        {
            get { return new PullRequestsController(Client, this); }
        }

        public RepositoryController(Client client, string user, string repo)
            : base(client)
        {
            User = user;
            Repo = repo;
        }

        public GitHubResponse<RepositoryModel> Get(bool forceCacheInvalidation = false)
        {
            return Client.Get<RepositoryModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<List<BasicUserModel>> GetContributors(int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/contributors", page: page, perPage: perPage);
        }

        public GitHubResponse<Dictionary<string, int>> GetLanguages(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<Dictionary<string, int>>(Uri + "/languages", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<TagModel>> GetTags(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<TagModel>>(Uri + "/tags", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<BranchModel>> GetBranches(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<BranchModel>>(Uri + "/branches", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<EventModel>> GetEvents(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<EventModel>>(Uri + "/events", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<EventModel>> GetNetworkEvents(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<EventModel>>("networks/" + User + "/" + Repo + "/events", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<ContentModel> GetReadme(bool forceCacheInvalidation = false, string branch = "master")
        {
            return Client.Get<ContentModel>(Uri + "/readme", forceCacheInvalidation: forceCacheInvalidation, additionalArgs: new { Ref = branch });
        }

        public GitHubResponse<List<ContentModel>> GetContent(bool forceCacheInvalidation = false, string path = "/", string branch = "master")
        {
            return Client.Get<List<ContentModel>>(Uri + "/contents" + path, forceCacheInvalidation: forceCacheInvalidation, additionalArgs: new { Ref = branch });
        }

        public GitHubResponse<TreeModel> GetTree(string sha, bool forceCacheInvalidation = false)
        {
            return Client.Get<TreeModel>(Uri + "/git/trees/" + sha, forceCacheInvalidation: forceCacheInvalidation);
        }

        public string GetFileRaw(string branch, string file, System.IO.Stream stream)
        {
            var uri = Client.RawUri + "/" + User + "/" + Repo + "/" + branch + "/";
            if (!uri.EndsWith("/") && !file.StartsWith("/"))
                file = "/" + file;

            var request = new RestSharp.RestRequest(uri + file);
            request.ResponseWriter = (s) => s.CopyTo(stream);
            return Client.ExecuteRequest(request).ContentType;
        }

        public bool IsWatching(bool forceCacheInvalidation = false)
        {
            try
            {
                if (Client.Get(Client.ApiUri + "/user/subscriptions/" + User + "/" + Repo, forceCacheInvalidation: forceCacheInvalidation).StatusCode == 204)
                    return true;
            }
            catch (NotFoundException)
            {
            }

            return false;
        }

        public void Watch()
        {
            var uri = Client.ApiUri + "/user/subscriptions/" + User + "/" + Repo;
            Client.Put(uri);
            Client.InvalidateCacheObjects(uri);
        }

        public void StopWatching()
        {
            var uri = Client.ApiUri + "/user/subscriptions/" + User + "/" + Repo;
            Client.Delete(uri);
            Client.InvalidateCacheObjects(uri);
        }

        public GitHubResponse<SubscriptionModel> GetSubscription(bool forceCacheInvalidation = false)
        {
            return Client.Get<SubscriptionModel>(Uri + "/subscription", forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<SubscriptionModel> SetSubscription(bool subscribed, bool ignored)
        {
            var uri = Uri + "/subscription";
            var ret = Client.Put<SubscriptionModel>(uri, new { Subscribed = subscribed, Ignored = ignored });
            Client.InvalidateCacheObjects(uri);
            return ret;
        }

        public void DeleteSubscription()
        {
            Client.Delete(Uri + "/subscription");
        }

        public GitHubResponse<List<BasicUserModel>> GetStargazers(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/stargazers", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }


        public bool IsStarred()
        {
            try
            {
                if (Client.Get(Client.ApiUri + "/user/starred/" + User + "/" + Repo).StatusCode == 204)
                    return true;
            }
            catch (NotFoundException)
            {
            }

            return false;
        }

        public void Star()
        {
            var uri = Client.ApiUri + "/user/starred/" + User + "/" + Repo;
            Client.Put(uri);

            //Invalidate this URI and the current user's starred repos since we've changed it indirectly
            Client.InvalidateCacheObjects(uri);
            Client.InvalidateCacheObjects(Client.ApiUri + "/user/starred");
        }

        public void Unstar()
        {
            var uri = Client.ApiUri + "/user/starred/" + User + "/" + Repo;
            Client.Delete(uri);

            //Invalidate this URI and the current user's starred repos since we've changed it indirectly
            Client.InvalidateCacheObjects(uri);
            Client.InvalidateCacheObjects(Client.ApiUri + "/user/starred");
        }

        public GitHubResponse<List<BasicUserModel>> GetCollaborators(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/collaborators", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<RepositoryModel>> GetForks(string sort = "newest", bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<RepositoryModel>>(Uri + "/forks", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage, additionalArgs: new { sort = sort });
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/repos/" + User + "/" + Repo; }
        }
    }
}
