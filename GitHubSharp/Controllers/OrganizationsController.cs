using GitHubSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    public class OrganizationsController : Controller
    {
        public OrganizationController this[string name]
        {
            get { return new OrganizationController(Client, this, name); }
        }

        public OrganizationsController(Client client)
            : base(client)
        {
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/orgs"; }
        }
    }

    public class OrganizationController : Controller
    {
        public string Name { get; private set; }

        public OrganizationsController OrganizationsController { get; private set; }

        public OrganizationController(Client client, OrganizationsController organizationsController, string name)
            : base(client)
        {
            Name = name;
            OrganizationsController = organizationsController;
        }

        public GitHubRequest<UserModel> Get()
        {
            return GitHubRequest.Get<UserModel>(Uri);
        }

        public GitHubRequest<List<BasicUserModel>> GetMembers(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<BasicUserModel>>(Uri + "/members", new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<TeamShortModel>> GetTeams(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<TeamShortModel>>(Uri + "/teams", new { page = page, per_page = perPage });
        }

        public GitHubRequest<List<EventModel>> GetEvents(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<EventModel>>(Uri + "/events", new { page = page, per_page = perPage });
        }

        public override string Uri
        {
            get { return OrganizationsController.Uri + "/" + Name; }
        }
    }
}
