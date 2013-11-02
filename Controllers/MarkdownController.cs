using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    public class MarkdownController : Controller
    {
        public MarkdownController(Client client)
            : base(client)
        {
        }

        public string GetMarkdown(string text)
        {
            var request = new RestSharp.RestRequest(Uri, RestSharp.Method.POST);
            request.AddParameter("text/plain", text, RestSharp.ParameterType.RequestBody);
            var response = Client.ExecuteRequest(request);
            return response.Content;
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/markdown/raw"; }
        }
    }
}
