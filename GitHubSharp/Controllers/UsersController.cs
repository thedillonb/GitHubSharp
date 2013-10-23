using GitHubSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    /// <summary>
    /// A collection of users
    /// </summary>
    public class UsersController : Controller
    {
        /// <summary>
        /// Get a specific user
        /// </summary>
        /// <param name="key">The username</param>
        /// <returns>A controller which represents the user selected</returns>
        public UserController this[string key]
        {
            get { return new UserController(Client, key); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The GitHubSharp client object</param>
        public UsersController(Client client)
            : base(client)
        {
        }

        public override string Uri
        {
            get { throw new NotImplementedException(); }
        }
    }

    public abstract class BaseUserController : Controller
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The GitHubSharp client object</param>
        public BaseUserController(Client client)
            : base(client)
        {
        }

        /// <summary>
        /// Get a list of users this user is following
        /// </summary>
        public GitHubRequest<List<BasicUserModel>> GetFollowing(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<BasicUserModel>>(Uri + "/following", new { page = page, per_page = perPage });
        }

        /// <summary>
        /// Get a list of users following this user
        /// </summary>
        public GitHubRequest<List<BasicUserModel>> GetFollowers(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<BasicUserModel>>(Uri + "/followers", new { page = page, per_page = perPage });
        }

        /// <summary>
        /// Gets the organizations this user belongs to
        /// </summary>
        public GitHubRequest<List<BasicUserModel>> GetOrganizations()
        {
            return GitHubRequest.Get<List<BasicUserModel>>(Uri + "/orgs");
        }
    }

    /// <summary>
    /// A specific user
    /// </summary>
    public class UserController : BaseUserController
    {
        public string Name { get; private set; }

        /// <summary>
        /// Gets the user's repositories
        /// </summary>
        public UserRepositoriesController Repositories
        {
            get { return new UserRepositoriesController(Client, this); }
        }

        /// <summary>
        /// Gets the user's Gists
        /// </summary>
        public UserGistsController Gists
        {
            get { return new UserGistsController(Client, Name); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The GitHubSharp client object</param>
        /// <param name="name">The name of the user</param>
        public UserController(Client client, string name)
            : base(client)
        {
            Name = name;
        }

        /// <summary>
        /// Get the user's information
        /// </summary>
        public GitHubRequest<UserModel> Get()
        {
            return GitHubRequest.Get<UserModel>(Uri);
        }

        /// <summary>
        /// Gets events for this user
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public GitHubRequest<List<EventModel>> GetEvents(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<EventModel>>(Uri + "/events", new { page = page, per_page = perPage });
        }

        /// <summary>
        /// Gets received events for this user
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public GitHubRequest<List<EventModel>> GetReceivedEvents(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<EventModel>>(Uri + "/received_events", new { page = page, per_page = perPage });
        }

        /// <summary>
        /// Get Organization events
        /// </summary>
        /// <returns>The events.</returns>
        /// <param name="forceCacheInvalidation">If set to <c>true</c> force cache invalidation.</param>
        /// <param name="page">Page.</param>
        /// <param name="perPage">Per page.</param>
        public GitHubRequest<List<EventModel>> GetOrganizationEvents(string org, int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<EventModel>>(Uri + "/events/orgs/" + org, new { page = page, per_page = perPage });
        }


        public override string Uri
        {
            get { return Client.ApiUri + "/users/" + Name; }
        }
    }

    public class AuthenticatedUserController : BaseUserController
    {
        public AuthenticatedRepositoriesController Repositories
        {
            get { return new AuthenticatedRepositoriesController(Client); }
        }

        public OrganizationsController Organizations
        {
            get { return new OrganizationsController(Client); }
        }

        public AuthenticatedUserIssuesController Issues
        {
            get { return new AuthenticatedUserIssuesController(Client); }
        }

        public AuthenticatedGistsController Gists
        {
            get { return new AuthenticatedGistsController(Client); }
        }

        public AuthenticatedUserController(Client client)
            : base(client)
        {
        }

        /// <summary>
        /// Gets events for this user
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public GitHubRequest<List<EventModel>> GetPublicEvents(int page = 1, int perPage = 100)
        {
            return GitHubRequest.Get<List<EventModel>>(Client.ApiUri + "/events", new { page = page, per_page = perPage });
        }

        public GitHubRequest<UserAuthenticatedModel> GetInfo()
        {
            return GitHubRequest.Get<UserAuthenticatedModel>(Uri);
        }

        public GitHubRequest<List<KeyModel>> GetKeys()
        {
            return GitHubRequest.Get<List<KeyModel>>(Uri + "/keys");
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/user"; }
        }
    }
}
