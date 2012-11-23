using System;
using System.Collections.Generic;

namespace GitHubSharp.Models
{
    public class GistModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Public { get; set; }
        public BasicUserModel User { get; set; }
        public Dictionary<string, GistFileModel> Files { get; set; }
        public int Comments { get; set; }
        public string HtmlUrl { get; set; }
        public string GitPullUrl { get; set; }
        public string GitPushUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public List<ForkModel> Forks { get; set; }
        public List<HistoryModel> History { get; set; }
    }

    public class GistFileModel
    {
        public long Size { get; set; }
        public string Filename { get; set; }
        public string RawUrl { get; set; }
        public string Content { get; set; }
    }

    public class GistToCreateOrEditModel
    {
        public string Description { get; set; }
        public Dictionary<string, GistFileForCreationModel> Files { get; set; }
    
        public class GistFileForCreationModel
        {
            public string Filename { get; set; }
            public string Content { get; set; }
        }
    }
}
