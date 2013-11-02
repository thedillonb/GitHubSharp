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

        public GitHubRequest<List<MilestoneModel>> GetAll(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<MilestoneModel>>(Uri, new { page = page, per_page = perPage });
        }

        public override string Uri
        {
            get { return RepositoryController.Uri + "/milestones"; }
        }
    }
}

