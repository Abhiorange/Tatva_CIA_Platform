using CI_platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CI_platform.Entities.ViewModels
{
    public class UserAdminViewModel
    {
       public IPagedList<User> users { get; set; }
        public IPagedList<Mission> Missions { get; set; }
        public IPagedList<MissionTheme> MissionThemes { get; set; }
        public IPagedList<Skill> Skills { get; set; }
        public IPagedList<MissionApplication> MissionApplications { get; set; }
        public IPagedList<Story> Stories { get; set; }
    }
}
