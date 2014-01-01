using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace GitHubSharp.Controllers
{
    public class MarkdownController : Controller
    {
        public MarkdownController(Client client)
            : base(client)
        {
        }

		public async Task<string> GetMarkdown(string text)
        {
			var req = new HttpRequestMessage(HttpMethod.Post, Uri);
			req.Content = new StringContent(text, Encoding.UTF8, "text/plain");
			var data = await Client.ExecuteRequest(req).ConfigureAwait(false);
			return await data.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public override string Uri
        {
            get { return Client.ApiUri + "/markdown/raw"; }
        }
    }
}
