using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    [Serializable]
    public class TreeModel
    {
        public string Sha { get; set; }
        public string Url { get; set; }
        public List<TreePathModel> Tree { get; set; }

        [Serializable]
        public class TreePathModel
        {
            public string Path { get; set; }
            public string Mode { get; set; }
            public string Type { get; set; }
            public ulong Size { get; set; }
            public string Sha { get; set; }
            public string Url { get; set; }
        }
    }
}

