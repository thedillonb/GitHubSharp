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
            get { return new OrganizationController(Client, name); }
        }

        public OrganizationsController(Client client)
            : base(client)
        {
        }

        public override string Uri
        {
            get { return "orgs"; }
        }
    }

    public class OrganizationController : Controller
    {
        private readonly string _name;

        public OrganizationController(Client client, string name)
            : base(client)
        {
            _name = name;
        }

        public GitHubResponse<UserModel> GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<UserModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<List<BasicUserModel>> GetMembers(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/members", forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<List<BasicUserModel>> GetTeams(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/teams", forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<List<EventModel>> GetEvents(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<EventModel>>(Uri + "/events", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        public override string Uri
        {
            get { return "orgs/" + _name; }
        }
    }
}
