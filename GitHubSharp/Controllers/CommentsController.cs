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
        public RepositoryController Repository { get; private set; }

        public CommentsController(Client client, RepositoryController repository)
            : base(client)
        {
            Repository = repository;
        }

        public GitHubResponse<List<CommentModel>> GetAll(int page = 1, int perPage = 100)
        {
            return Client.Get<List<CommentModel>>(Uri, page: page, perPage: perPage);
        }

        public GitHubResponse<CommentModel> Get(string id)
        {
            return Client.Get<CommentModel>(Uri + "/" + id);
        }

        public GitHubResponse<CommentModel> Update(string id, string body)
        {
            return Client.Patch<CommentModel>(Uri + "/" + id, new { Body = body });
        }

        public void Delete(string id)
        {
            Client.Delete(Uri + "/" + id);
        }

        public override string Uri
        {
            get { return Repository.Uri + "/comments"; }
        }
    }

    public class CommitCommentsController : Controller
    {
        public CommitController CommitController { get; private set; }

        public CommitCommentsController(Client client, CommitController commits)
            : base(client)
        {
            CommitController = commits;
        }

        public GitHubResponse<CommentModel> Create(CreateCommentModel model)
        {
            return Client.Post<CommentModel>(Uri, model.Serialize());
        }

        public GitHubResponse<List<CommentModel>> GetAll(int page = 1, int perPage = 100)
        {
            return Client.Get<List<CommentModel>>(Uri, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return CommitController.Uri + "/comments"; }
        }
    }
}
