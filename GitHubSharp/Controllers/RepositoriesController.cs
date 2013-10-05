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

        public GitHubRequest<RepositorySearchModel> SearchRepositories(string keyword)
        {
            return GitHubRequest.Get<RepositorySearchModel>(Client, Uri + "/search/" + keyword);
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

        public GitHubRequest<List<RepositoryModel>> GetAll(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<RepositoryModel>>(Client, Uri, new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<RepositoryModel>> GetWatching()
        {
            return GitHubRequest.Get<List<RepositoryModel>>(Client, UserController.Uri + "/subscriptions");
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

        public GitHubRequest<List<RepositoryModel>> GetAll(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<RepositoryModel>>(Client, Uri + "/repos",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<RepositoryModel>> GetStarred(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<RepositoryModel>>(Client, Uri + "/starred",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<RepositoryModel>> GetWatching(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<RepositoryModel>>(Client, Uri + "/subscriptions",  new { page = page, per_page = perPage });
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

        public GitHubRequest<List<RepositoryModel>> GetAll(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<RepositoryModel>>(Client, Uri,  new { page = page, per_page = perPage });
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

        public MilestonesController Milestones
        {
            get { return new MilestonesController(Client, this); }
        }

        public RepositoryController(Client client, string user, string repo)
            : base(client)
        {
            User = user;
            Repo = repo;
        }

        public GitHubRequest<RepositoryModel> Get()
        {
            return GitHubRequest.Get<RepositoryModel>(Client, Uri);
        }

        public GitHubRequest<List<BasicUserModel>> GetContributors(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<BasicUserModel>>(Client, Uri + "/contributors", new { page = page, per_page = perPage });
        }

        public GitHubRequest<Dictionary<string, int>> GetLanguages(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<Dictionary<string, int>>(Client, Uri + "/languages",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<TagModel>> GetTags(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<TagModel>>(Client, Uri + "/tags",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<BranchModel>> GetBranches(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<BranchModel>>(Client, Uri + "/branches",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<EventModel>> GetEvents(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<EventModel>>(Client, Uri + "/events",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<EventModel>> GetNetworkEvents(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<EventModel>>(Client, Uri + "/networks/" + User + "/" + Repo + "/events",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<ContentModel> GetReadme(string branch = "master")
        {
            return GitHubRequest.Get<ContentModel>(Client, Uri + "/readme", new { Ref = branch });
        }

        public GitHubRequest<List<ContentModel>> GetContent(string path = "/", string branch = "master")
        {
            return GitHubRequest.Get<List<ContentModel>>(Client, Uri + "/contents" + path, new { Ref = branch });
        }

        public GitHubRequest<TreeModel> GetTree(string sha)
        {
            return GitHubRequest.Get<TreeModel>(Client, Uri + "/git/trees/" + sha);
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

        public GitHubRequest<bool> IsWatching()
        {
            return GitHubRequest.Get<bool>(Client, Client.ApiUri + "/user/subscriptions/" + User + "/" + Repo);
        }

        public GitHubRequest Watch()
        {
            return GitHubRequest.Put(Client.ApiUri + "/user/subscriptions/" + User + "/" + Repo);
        }

        public GitHubRequest StopWatching()
        {
            return GitHubRequest.Delete(Client.ApiUri + "/user/subscriptions/" + User + "/" + Repo);
        }

        public GitHubRequest<SubscriptionModel> GetSubscription()
        {
            return GitHubRequest.Get<SubscriptionModel>(Client, Uri + "/subscription");
        }

        public GitHubRequest<SubscriptionModel> SetSubscription(bool subscribed, bool ignored)
        {
            var uri = Uri + "/subscription";
            return GitHubRequest.Put<SubscriptionModel>(uri, new { Subscribed = subscribed, Ignored = ignored });
        }

        public GitHubRequest DeleteSubscription()
        {
            return GitHubRequest.Delete(Uri + "/subscription");
        }

        public GitHubRequest<List<BasicUserModel>> GetStargazers(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<BasicUserModel>>(Client, Uri + "/stargazers",  new { page = page, per_page = perPage });
        }


        public GitHubRequest<bool> IsStarred()
        {
            return GitHubRequest.Get<bool>(Client, Client.ApiUri + "/user/starred/" + User + "/" + Repo);
        }

        public GitHubRequest Star()
        {
            return GitHubRequest.Put(Client.ApiUri + "/user/starred/" + User + "/" + Repo);
        }

        public GitHubRequest Unstar()
        {
            return GitHubRequest.Delete(Client.ApiUri + "/user/starred/" + User + "/" + Repo);
        }

        public GitHubRequest<List<BasicUserModel>> GetCollaborators(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<BasicUserModel>>(Client, Uri + "/collaborators",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<RepositoryModel>> GetForks(string sort = "newest", int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<RepositoryModel>>(Client, Uri + "/forks",  new { page = page, per_page = perPage, sort = sort });
        }

        public GitHubRequest<List<LabelModel>> GetLabels(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<LabelModel>>(Client, Uri + "/labels",  new { page = page, per_page = perPage });
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/repos/" + User + "/" + Repo; }
        }
    }
}
