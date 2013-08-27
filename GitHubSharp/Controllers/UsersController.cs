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
        /// <returns></returns>
        public GitHubResponse<List<BasicUserModel>> GetFollowing(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/following", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        /// <summary>
        /// Get a list of users following this user
        /// </summary>
        /// <returns></returns>
        public GitHubResponse<List<BasicUserModel>> GetFollowers(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/followers", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage);
        }

        /// <summary>
        /// Gets events for this user
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public GitHubResponse<List<EventModel>> GetEvents(bool forceCacheInvalidation = false, int page = 1, int perPage = 100)
        {
            string uri = string.IsNullOrEmpty(Uri) ? Client.ApiUri + "events" : Uri + "/events";
            return Client.Get<List<EventModel>>(uri, forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: 100);
        }

        /// <summary>
        /// Gets the organizations this user belongs to
        /// </summary>
        /// <returns></returns>
        public GitHubResponse<List<BasicUserModel>> GetOrganizations(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/orgs", forceCacheInvalidation: forceCacheInvalidation);
        }
    }

    /// <summary>
    /// A specific user
    /// </summary>
    public class UserController : BaseUserController
    {
        private readonly string _user;

        /// <summary>
        /// Gets the user's repositories
        /// </summary>
        public UserRepositoriesController Repositories
        {
            get { return new UserRepositoriesController(Client, _user); }
        }

        /// <summary>
        /// Gets the user's Gists
        /// </summary>
        public UserGistsController Gists
        {
            get { return new UserGistsController(Client, _user); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The GitHubSharp client object</param>
        /// <param name="name">The name of the user</param>
        public UserController(Client client, string name)
            : base(client)
        {
            _user = name;
        }

        /// <summary>
        /// Get the user's information
        /// </summary>
        /// <returns></returns>
        public GitHubResponse<UserModel> GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<UserModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public override string Uri
        {
            get { return Client.ApiUri + "users/" + _user; }
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

        public AuthenticatedUserController(Client client)
            : base(client)
        {
        }

        public GitHubResponse<UserAuthenticatedModel> GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<UserAuthenticatedModel>(Uri, forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<List<KeyModel>> GetKeys(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<KeyModel>>("user/keys", forceCacheInvalidation: forceCacheInvalidation);
        }

        public GitHubResponse<List<NotificationModel>> GetNotifications(bool forceCacheInvalidation = false, int page = 1, int perPage = 100, bool all = false, bool participating = false)
        {
            return Client.Get<List<NotificationModel>>("notifications", forceCacheInvalidation: forceCacheInvalidation, page: page, perPage: perPage, additionalArgs: new { All = all, Participating = participating });
        }

        public override string Uri
        {
            get { return Client.ApiUri + "user"; }
        }
    }
}
