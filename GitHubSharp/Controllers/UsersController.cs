using GitHubSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    public class UsersController : Controller
    {
        public UserController this[string key]
        {
            get { return new UserController(Client, key); }
        }

        public UsersController(Client client)
            : base(client)
        {
        }

        public override string Uri
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class UserController : Controller
    {
        private readonly string _user;

        public UserRepositoriesController Repositories
        {
            get { return new UserRepositoriesController(Client, _user); }
        }

        public UserGistsController Gists
        {
            get { return new UserGistsController(Client, _user); }
        }

        public UserController(Client client, string name)
            : base(client)
        {
            _user = name;
        }

        public GitHubResponse<UserModel> GetInfo()
        {
            return Client.Get<UserModel>(Uri);
        }

        public GitHubResponse<List<BasicUserModel>> GetFollowing()
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/following");
        }

        public GitHubResponse<List<BasicUserModel>> GetFollowers(int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/followers", page: page, perPage: perPage);
        }

        public GitHubResponse<List<EventModel>> GetEvents(int page = 1, int perPage = 100)
        {
            return Client.Get<List<EventModel>>(Uri + "/events", page: page, perPage: 100);
        }

        public GitHubResponse<List<BasicUserModel>> GetOrganizations()
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/orgs");
        }

        public override string Uri
        {
            get { return "users/" + _user; }
        }
    }

    public class AuthenticatedUserController : Controller
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

        public GitHubResponse<UserAuthenticatedModel> GetInfo()
        {
            return Client.Get<UserAuthenticatedModel>(Uri);
        }

        public GitHubResponse<List<KeyModel>> GetKeys()
        {
            return Client.Get<List<KeyModel>>("user/keys");
        }

        public GitHubResponse<List<BasicUserModel>> GetFollowing()
        {
            return Client.Get<List<BasicUserModel>>("user/following");
        }

        public GitHubResponse<List<BasicUserModel>> GetFollowers(int page = 1, int perPage = 100)
        {
            return Client.Get<List<BasicUserModel>>("user/followers", page: page, perPage: perPage);
        }

        public GitHubResponse<List<NotificationModel>> GetNotifications(int page = 1, int perPage = 100, bool all = false, bool participating = false)
        {
            return Client.Get<List<NotificationModel>>("notifications", page: page, perPage: perPage, additionalArgs: new { All = all, Participating = participating });
        }

        public GitHubResponse<List<EventModel>> GetEvents(int page = 1, int perPage = 100)
        {
            return Client.Get<List<EventModel>>("events", page: page, perPage: 100);
        }

        public GitHubResponse<List<BasicUserModel>> GetOrganizations()
        {
            return Client.Get<List<BasicUserModel>>(Uri + "/orgs");
        }


        public override string Uri
        {
            get { return "user"; }
        }
    }
}
