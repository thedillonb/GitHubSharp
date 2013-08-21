using GitHubSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    public class TeamsController : Controller
    {
        public TeamController this[int id]
        {
            get { return new TeamController(Client, id); }
        }

        public TeamsController(Client client)
            : base(client)
        {
        }

        public override string Uri
        {
            get { return "teams"; }
        }
    }

    public class TeamController : Controller
    {
        private readonly int _id;

        public TeamController(Client client, int id)
            : base(client)
        {
            _id = id;
        }
        
        public GitHubResponse<List<RepositoryModel>> GetRepositories()
        {
            return Client.Get<List<RepositoryModel>>(Uri + "/repos");
        }

        public override string Uri
        {
            get { return "teams/" + _id; }
        }
    }
}
