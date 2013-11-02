using System;
using RestSharp;
using System.Linq;
using System.Text;

namespace GitHubSharp.Authenticators
{
    internal class TwoFactorAuthenticator : IAuthenticator
    {
        private readonly string _twoFactor;
        private readonly string _username;
        private readonly string _password;

        public TwoFactorAuthenticator(string username, string password, string twoFactor)
        {
            _password = password;
            _username = username;
            _twoFactor = twoFactor;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (!request.Parameters.Any(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
            {
                var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _username, _password)));
                var authHeader = string.Format("Basic {0}", token);
                request.AddParameter("Authorization", authHeader, ParameterType.HttpHeader);
            }

            request.AddHeader("X-GitHub-OTP", _twoFactor);
        }
    }


}

