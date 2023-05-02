using CI_platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CI_platform.Entities.ViewModels
{
    public  class StoryViewModel
    {
       public IPagedList<Story> Stories { get; set; }
        public List<Mission> Missions { get; set; }
        public List<StoryMedium> StoryMedia { get; set; }
        
    }
}
