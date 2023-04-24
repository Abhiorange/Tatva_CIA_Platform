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
      /*  public long MissionId { get; set; }*/

        /* public long ThemeId { get; set; }

         public long CityId { get; set; }

         public long CountryId { get; set; }*/
        public Mission Missions { get; set; }
        public string? GoalObjectiveText = null;
        public List<Mission> Related_Mission { get; set; }
        public List<MissionRating> Missionrating { get; set; }
        public long MissionRatingId { get; set; }
        public List<GoalMission> Related_goal { get; set; }

        public int GoalValue = 0;
        public int missionid { get; set; }
        public MissionSkill MissionsSkill { get; set; }
        public List<FavouriteMission> favouriteMissions { get; set; }
        public List<Comment> comment { get; set; }
        public List<MissionApplication> MissionApplications { get; set; }
        public List<MissionDocument> MissionDocuments { get; set; }
        public IPagedList<User> recentvolunteers { get; set; }
      /*  public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MissionType { get; set; } = null!;

        public int? Status { get; set; }

        public string? OrganizationName { get; set; }

        public string? OrganizationDetail { get; set; }

        public string? Availability { get; set; }*/
    }
}
