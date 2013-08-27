using GitHubSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public GitHubResponse<CommentModel> Create(string body)
        {
            return Client.Post<CommentModel>(Uri, new Dictionary<string, object> {{ "body", body }});
        }

        public GitHubResponse<List<CommentModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<CommentModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
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

        public GitHubResponse<CommentModel> Get(bool forceCacheInvalidation = false)
        {
            return Client.Get<CommentModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<CommentModel> Update(string body)
        {
            return Client.Patch<CommentModel>(Uri, new { Body = body });
        }

        public void Delete()
        {
            Client.Delete(Uri);
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

        public GitHubResponse<CommentModel> Create(string body, string path, int position)
        {
            return Client.Post<CommentModel>(Uri, new Dictionary<string, object> {{ "body", body }, { "path", path }, { "position", position }});
        }

        public GitHubResponse<List<CommentModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<CommentModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return CommitController.Uri + "/comments"; }
        }
    }
}
