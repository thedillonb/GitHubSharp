using System;
using GitHubSharp.Models;
using System.Collections.Generic;

namespace GitHubSharp.Controllers
{
    public class CommitsController : Controller
    {
        public RepositoryController RepositoryController { get; private set; }

        public CommitController this[string key]
        {
            get { return new CommitController(Client, RepositoryController, key); }
        }

        public CommitsController(Client client, RepositoryController repo)
            : base(client)
        {
            RepositoryController = repo;
        }

        public GitHubRequest<List<CommitModel>> GetAll(string sha = null)
        {
            if (sha == null)
                return GitHubRequest.Get<List<CommitModel>>(Uri);
            else
                return GitHubRequest.Get<List<CommitModel>>(Uri, new { sha = sha });
        }

        public override string Uri
        {
            get { return RepositoryController.Uri + "/commits"; }
        }
    }

    public class CommitController : Controller
    {
        public RepositoryController RepositoryController { get; private set; }

        public string Sha { get; private set; }

        public CommitCommentsController Comments
        {
            get { return new CommitCommentsController(Client, this); }
        }

        public CommitController(Client client, RepositoryController repositoryController, string sha)
            : base(client)
        {
            RepositoryController = repositoryController;
            Sha = sha;
        }

        public GitHubRequest<CommitModel> Get()
        {
            return GitHubRequest.Get<CommitModel>(Uri);
        }

        public override string Uri
        {
            get { return RepositoryController.Uri + "/commits/" + Sha; }
        }
    }
}

