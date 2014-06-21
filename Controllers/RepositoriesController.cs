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

		public GitHubRequest<RepositorySearchModel> SearchRepositories(string[] keywords, string[] languages, string sort = null, int page = 1)
        {
			var sb = new StringBuilder();
			sb.Append(string.Join(" ", keywords));
			sb.Append(' ');
			foreach (var l in languages)
				sb.Append("language:").Append(l).Append(' ');


			return GitHubRequest.Get<RepositorySearchModel>(Uri, new { q = sb.ToString().Trim(), page = page, sort = sort });
        }

        public override string Uri
        {
			get { return Client.ApiUri + "/search/repositories"; }
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
            return GitHubRequest.Get<List<RepositoryModel>>(Uri, new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<RepositoryModel>> GetWatching()
        {
            return GitHubRequest.Get<List<RepositoryModel>>(UserController.Uri + "/subscriptions");
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
            return GitHubRequest.Get<List<RepositoryModel>>(Uri + "/repos",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<RepositoryModel>> GetStarred(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<RepositoryModel>>(Uri + "/starred",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<RepositoryModel>> GetWatching(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<RepositoryModel>>(Uri + "/subscriptions",  new { page = page, per_page = perPage });
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
            return GitHubRequest.Get<List<RepositoryModel>>(Uri,  new { page = page, per_page = perPage });
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

        public LabelsController Labels
        {
            get { return new LabelsController(Client, this); }
        }

        public RepositoryController(Client client, string user, string repo)
            : base(client)
        {
            User = user;
            Repo = repo;
        }

        public GitHubRequest<RepositoryModel> Get()
        {
            return GitHubRequest.Get<RepositoryModel>( Uri);
        }

        public GitHubRequest<List<BasicUserModel>> GetContributors(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<BasicUserModel>>( Uri + "/contributors", new { page = page, per_page = perPage });
        }

        public GitHubRequest<Dictionary<string, int>> GetLanguages(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<Dictionary<string, int>>( Uri + "/languages",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<TagModel>> GetTags(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<TagModel>>( Uri + "/tags",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<BranchModel>> GetBranches(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<BranchModel>>(Uri + "/branches",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<EventModel>> GetEvents(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<EventModel>>(Uri + "/events",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<EventModel>> GetNetworkEvents(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<EventModel>>(Uri + "/networks/" + User + "/" + Repo + "/events",  new { page = page, per_page = perPage });
        }

        public GitHubRequest<ContentModel> GetReadme(string branch = "master")
        {
            return GitHubRequest.Get<ContentModel>(Uri + "/readme", new { Ref = branch });
        }

        public GitHubRequest<List<ContentModel>> GetContent(string path = "/", string branch = "master")
        {
            return GitHubRequest.Get<List<ContentModel>>(Uri + "/contents" + path, new { Ref = branch });
        }

		public GitHubRequest<ContentModel> GetContentFile(string path = "/", string branch = "master")
		{
			return GitHubRequest.Get<ContentModel>(Uri + "/contents" + path, new { Ref = branch });
		}

        public GitHubRequest<List<ReleaseModel>> GetReleases(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<ReleaseModel>>(Uri + "/releases", new { page = page, per_page = perPage });
        } 

		public GitHubRequest<ContentUpdateModel> UpdateContentFile(string path, string message, string content, string sha, string branch = "master")
		{
			if (null == content)
				throw new Exception("Content cannot be null!");
			if (string.IsNullOrEmpty(message))
				throw new Exception("Commit message cannot be empty!");

			content = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(content));
			return GitHubRequest.Put<ContentUpdateModel>(Uri + "/contents" + path, new { message, content, sha });
		}

        public GitHubRequest<TreeModel> GetTree(string sha)
        {
            return GitHubRequest.Get<TreeModel>(Uri + "/git/trees/" + sha);
        }

//        public string GetFileRaw(string branch, string file, System.IO.Stream stream)
//        {
//            var uri = Uri + "/contents/";
//            if (!uri.EndsWith("/") && !file.StartsWith("/"))
//                file = "/" + file;
//
//            var request = new RestSharp.RestRequest(uri + file);
//            request.AddHeader("Accept", "application/vnd.github.raw");
//            request.ResponseWriter = (s) => s.CopyTo(stream);
//            var response = Client.ExecuteRequest(request);
//            return response.ContentType;
//        }
//
        public GitHubRequest<bool> IsWatching()
        {
            return GitHubRequest.Get<bool>(Client.ApiUri + "/user/subscriptions/" + User + "/" + Repo);
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
            return GitHubRequest.Get<SubscriptionModel>(Uri + "/subscription");
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
            return GitHubRequest.Get<List<BasicUserModel>>(Uri + "/stargazers",  new { page = page, per_page = perPage });
        }


        public GitHubRequest<bool> IsStarred()
        {
            return GitHubRequest.Get<bool>(Client.ApiUri + "/user/starred/" + User + "/" + Repo);
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
            return GitHubRequest.Get<List<BasicUserModel>>(Uri + "/collaborators",  new { page = page, per_page = perPage });
        }

		public GitHubRequest<List<BasicUserModel>> GetAssignees(int page = 1, int perPage = 100)
		{
			return GitHubRequest.Get<List<BasicUserModel>>(Uri + "/assignees",  new { page = page, per_page = perPage });
		}

        public GitHubRequest<List<RepositoryModel>> GetForks(string sort = "newest", int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<RepositoryModel>>(Uri + "/forks",  new { page = page, per_page = perPage, sort = sort });
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/repos/" + User + "/" + Repo; }
        }
    }
}
