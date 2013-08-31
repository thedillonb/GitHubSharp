using GitHubSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    /// <summary>
    /// A collection of teams
    /// </summary>
    public class TeamsController : Controller
    {
        /// <summary>
        /// Gets a specific team
        /// </summary>
        /// <param name="id">The id of the team</param>
        /// <returns></returns>
        public TeamController this[long id]
        {
            get { return new TeamController(Client, this, id); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The GitHubSharp client</param>
        public TeamsController(Client client)
            : base(client)
        {
        }

        /// <summary>
        /// Gets all the teams
        /// </summary>
        /// <param name="forceCacheInvalidation"></param>
        /// <returns></returns>
        public GitHubResponse<List<TeamShortModel>> GetAll(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<TeamShortModel>>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/teams"; }
        }
    }

    /// <summary>
    /// A single team
    /// </summary>
    public class TeamController : Controller
    {
        /// <summary>
        /// Gets the id of this team
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Gets the parent controller
        /// </summary>
        public TeamsController TeamsController { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The GitHubSharp client</param>
        /// <param name="teamsController">The parent controller</param>
        /// <param name="id">The id of this team</param>
        public TeamController(Client client, TeamsController teamsController, long id)
            : base(client)
        {
            Id = id;
            TeamsController = teamsController;
        }

        /// <summary>
        /// Gets information about this team
        /// </summary>
        /// <param name="forceCacheInvalidation"></param>
        /// <returns></returns>
        public GitHubResponse<TeamModel> Get(bool forceCacheInvalidation = false)
        {
            return Client.Get<TeamModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<List<BasicUserModel>> GetMembers(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/members", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }
        
        public GitHubResponse<List<RepositoryModel>> GetRepositories()
        {
            return Client.Get<List<RepositoryModel>>(Uri + "/repos");
        }

        public override string Uri
        {
            get { return TeamsController.Uri + "/" + Id; }
        }
    }
}
