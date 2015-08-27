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

        public GitHubRequest<List<MilestoneModel>> GetAll(bool opened = true, string sort = "due_date", string direction = "desc")
        {
            return GitHubRequest.Get<List<MilestoneModel>>(Uri, new { state = opened ? "open" : "closed", sort, direction });
        }

        public GitHubRequest<MilestoneModel> Get(int milestoneNumber)
        {
            return GitHubRequest.Get<MilestoneModel>(Uri + "/" + milestoneNumber);
        }

        public override string Uri
        {
            get { return RepositoryController.Uri + "/milestones"; }
        }

        public GitHubRequest<MilestoneModel> Create(string title, bool open, string description, DateTimeOffset dueDate)
        {
            return GitHubRequest.Post<MilestoneModel>(Uri,
                new {Title = title, State = open ? "open" : "closed", Description = description, DueOn = dueDate});
        }

        public GitHubRequest Delete(string url)
        {
            return GitHubRequest.Delete(url);
        }
    }
}

