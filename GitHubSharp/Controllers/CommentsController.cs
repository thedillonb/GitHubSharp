using GitHubSharp.Models;
using System.Collections.Generic;

namespace GitHubSharp.Controllers
{
    public class CommentsController : Controller
    {
        public RepositoryController RepositoryController { get; private set; }

        public CommentController this[string key]
        {
            get { return new CommentController(Client, RepositoryController, key); }
        }

        public CommentsController(Client client, RepositoryController repository)
            : base(client)
        {
            RepositoryController = repository;
        }

        public GitHubRequest<List<CommentModel>> GetAll(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<CommentModel>>(Client, Uri, new { page = page, per_page = perPage });
        }

        public override string Uri
        {
            get { return RepositoryController.Uri + "/comments"; }
        }
    }

    public class CommentController : Controller
    {
        public RepositoryController RepositoryController { get; private set; }

        public string Id { get; private set; }

        public CommentController(Client client, RepositoryController repositoryController, string id)
            : base(client)
        {
            RepositoryController = repositoryController;
            Id = id;
        }

        public GitHubRequest<CommentModel> Get()
        {
            return GitHubRequest.Get<CommentModel>(Client, Uri);
        }

        public GitHubRequest<CommentModel> Update(string body)
        {
            return GitHubRequest.Patch<CommentModel>(Uri, new { body = body });
        }

        public GitHubRequest Delete()
        {
            return GitHubRequest.Delete(Uri);
        }

        public override string Uri
        {
            get { return RepositoryController.Uri + "/comments/" + Id; }
        }
    }

    public class CommitCommentsController : Controller
    {
        public CommitController CommitController { get; private set; }

        public CommentController this[string key]
        {
            get { return new CommentController(Client, CommitController.RepositoryController, key); }
        }

        public CommitCommentsController(Client client, CommitController commits)
            : base(client)
        {
            CommitController = commits;
        }

        public GitHubRequest<CommentModel> Create(string body, string path = null, int? position = null)
        {
            return GitHubRequest.Post<CommentModel>(Client, Uri, new { body = body, path = path, position = position });
        }

        public GitHubRequest<List<CommentModel>> GetAll(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<CommentModel>>(Client, Uri, new { page = page, per_page = perPage });
        }

        public override string Uri
        {
            get { return CommitController.Uri + "/comments"; }
        }
    }

    public class PullRequestCommentsController : Controller
    {
        public PullRequestController Parent { get; private set; }

        public PullRequestCommentsController(Client client, PullRequestController parent) 
            : base(client)
        {
            Parent = parent;
        }

        public GitHubRequest<List<CommentModel>> GetAll(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<CommentModel>>(Client, Uri, new { page = page, per_page = perPage });
        }

        public GitHubRequest<CommentModel> Create(string body, string commitId, string path, int position)
        {
            return GitHubRequest.Post<CommentModel>(Client, Uri, new { body = body, commit_id = commitId, path = path, position = position });
        }

        public GitHubRequest<CommentModel> ReplyTo(string body, long replyToId)
        {
            return GitHubRequest.Post<CommentModel>(Client, Uri, new { body = body, in_reply_to = replyToId });
        }

        public override string Uri
        {
            get { return Parent.Uri + "/comments"; }
        }
    }
}
