using System;
using System.Collections.Generic;
using GitHubSharp.Models;

namespace GitHubSharp.Controllers
{
    public class MilestonesController : Controller
    {
        public RepositoryController RepositoryController { get; private set; }

        public MilestonesController(Client client, RepositoryController repository)
            : base(client)
        {
            RepositoryController = repository;
        }

        public GitHubResponse<List<MilestoneModel>> GetAll(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<MilestoneModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return RepositoryController.Uri + "/milestones"; }
        }
    }
}

