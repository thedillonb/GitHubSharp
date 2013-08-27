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

        public GitHubResponse<UserModel> Get(bool forceCacheInvalidation = false)
        {
            return Client.Get<UserModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<List<BasicUserModel>> GetMembers(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/members", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<BasicUserModel>> GetTeams(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/teams", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public GitHubResponse<List<EventModel>> GetEvents(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<EventModel>>(Uri + "/events", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return OrganizationsController.Uri + "/" + Name; }
        }
    }
}
