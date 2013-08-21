using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Controllers
{
    public abstract class Controller
    {
        protected readonly Client Client;

        protected Controller(Client client)
        {
            Client = client;
        }

        public abstract String Uri { get; }
    }
}
