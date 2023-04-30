using CI_platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CI_platform.Entities.ViewModels
{
    public  class VolunteerViewModel
    {
     
        public Mission Missions { get; set; }
        public string? GoalObjectiveText = null;
        public List<Mission> Related_Mission { get; set; }
        public List<MissionRating> Missionrating { get; set; }
        public long MissionRatingId { get; set; }
        public List<GoalMission> Related_goal { get; set; }
        public bool checkApply { get; set; }
        public bool checkClosed { get; set; }
        public int GoalValue { get; set; }
        public int missionid { get; set; }
        public List<MissionSkill> MissionsSkill { get; set; }
        public List<FavouriteMission> favouriteMissions { get; set; }
        public List<Comment> comment { get; set; }
        public List<MissionApplication> MissionApplications { get; set; }
        public List<MissionDocument> MissionDocuments { get; set; }
        public IPagedList<User> recentvolunteers { get; set; }
        public List<Timesheet> timesheets { get; set; }
    }
}
