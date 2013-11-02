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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The GitHubSharp client object</param>
        protected Controller(Client client)
        {
            Client = client;
        }

        /// <summary>
        /// A URI representing this controller 
        /// </summary>
        public abstract String Uri { get; }
    }
}
