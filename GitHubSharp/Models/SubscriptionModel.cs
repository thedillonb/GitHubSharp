using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSharp.Models
{
    public class SubscriptionModel
    {
        public bool Subscribed { get; set; }
        public bool Ignored { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Url { get; set; }
        public string RepositoryUrl { get; set; }
    }
}
