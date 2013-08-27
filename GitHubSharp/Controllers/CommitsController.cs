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

        public GitHubResponse<List<CommitModel>> GetAll(string sha = null, bool forceCacheInvalidation = false)
        {
            if (sha == null)
                return Client.Get<List<CommitModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation);
            else
                return Client.Get<List<CommitModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, additionalArgs: new { Sha = sha });
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

        public GitHubResponse<CommitModel> Get(bool forceCacheInvalidation = false)
        {
            return Client.Get<CommitModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public override string Uri
        {
            get { return RepositoryController.Uri + "/commits/" + Sha; }
        }
    }
}

