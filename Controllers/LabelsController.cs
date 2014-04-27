using System.Collections.Generic;
using GitHubSharp.Models;

namespace GitHubSharp.Controllers
{
    public class LabelsController : Controller
    {
        public RepositoryController Parent { get; private set; }

        public LabelController this[string name]
        {
            get { return new LabelController(Client, this, name); }
        }

        public LabelsController(Client client, RepositoryController parentController) 
            : base(client)
        {
            Parent = parentController;
        }

        public GitHubRequest<List<LabelModel>> GetAll(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<LabelModel>>(Uri, new { page, per_page = perPage });
        }

        public GitHubRequest<LabelModel> Create(string name, string color)
        {
            return GitHubRequest.Post<LabelModel>(Uri, new { name, color });
        }

        public override string Uri
        {
            get { return Parent.Uri + "/labels"; }
        }
    }

    public class LabelController : Controller
    {
        public LabelsController Parent { get; private set; }

        public string Name { get; private set; }

        public LabelController(Client client, LabelsController parentController, string name)
            : base(client)
        {
            Parent = parentController;
            Name = name;
        }

        public GitHubRequest Delete()
        {
            return GitHubRequest.Delete(Uri);
        }

        public GitHubRequest<LabelModel> Update(string title, string color)
        {
            return GitHubRequest.Patch<LabelModel>(Uri, new { title, color });
        }

        public override string Uri
        {
            get { return Parent.Uri + "/" + System.Uri.EscapeDataString(Name); }
        }
    }
}