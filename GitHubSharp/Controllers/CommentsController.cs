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

        public GitHubResponse<List<CommentModel>> GetAll(string sha, int page = 1, int perPage = 100)
        {
            return Client.Get<List<CommentModel>>(Repository.Uri + "/commits/" + sha + "/comments", page: page, perPage: perPage);
        }

        public GitHubResponse<CommentModel> Get(string id)
        {
            return Client.Get<CommentModel>(Uri + "/" + id);
        }

        public GitHubResponse<CommentModel> Create(string sha, CreateCommentModel model)
        {
            return Client.Post<CommentModel>(Repository.Uri + "/commits/" + sha + "/comments", model);
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
}
