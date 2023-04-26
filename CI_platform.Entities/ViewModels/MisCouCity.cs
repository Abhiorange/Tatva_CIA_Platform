using CI_platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CI_platform.Entities.ViewModels
{
    public class MisCouCity
    {
       public IPagedList<Mission> Missions { get; set; }
        public List<Timesheet> timesheets { get; set; }
        public List<MissionSkill> MissionsSkill { get; set; }
        public List<GoalMission> GoalMissions { get; set; }
        public List<MissionRating> Missionrating { get; set; }
        public List<MissionApplication> MissionApplications { get; set; }
        public List<FavouriteMission> favouriteMissions { get; set; }
        public int totalrecord { get; set; }
        public int currentPage { get; set; }


    }
}
