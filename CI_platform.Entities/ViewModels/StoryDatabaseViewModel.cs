using CI_platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Entities.ViewModels
{
    public  class StoryDatabaseViewModel
    {
        public long missionid { get; set; }
        public long storyid { get; set; }
        public long userid { get; set; }
        public string title { get; set; }
        public string Missiontitle { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string aboutstatus { get; set; }
        public string videourl { get; set; }
        public List<string> images { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}
