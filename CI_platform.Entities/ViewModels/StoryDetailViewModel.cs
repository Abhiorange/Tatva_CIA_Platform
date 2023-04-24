using CI_platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Entities.ViewModels
{
    public class StoryDetailViewModel
    {
        public Story Stories { get; set; }
       public int Views { get; set; }
        public List<string> images { get; set; }
    }
}
