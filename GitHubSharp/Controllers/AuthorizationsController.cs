using System;
using GitHubSharp.Controllers;
using System.Collections.Generic;
using GitHubSharp.Models;

namespace GitHubSharp
{
    public class AuthorizationsController : Controller
    {
        public AuthorizationsController(Client client)
            : base(client)
        {
        }

        public GitHubRequest<AuthorizationModel> GetOrCreate(string clientId, string clientSecret, List<string> scopes, string note, string noteUrl)
        {
            return GitHubRequest.Put<AuthorizationModel>(Uri + "/clients/" + clientId, new { client_secret = clientSecret, scopes = scopes, note = note, note_url = noteUrl });
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/authorizations"; }
        }
    }
}

