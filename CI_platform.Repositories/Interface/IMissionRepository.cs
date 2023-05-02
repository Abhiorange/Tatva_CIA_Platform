using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Repositories.Interface
{
     public interface IMissionRepository
    {
        public MisCouCity getmiscoucity(int pageindex, int pageSize, int id, string keyword, List<long> countryids, List<long> cityids, List<long> themeids, List<long> skillids, string user_id);
        public List<Country> GetCountries();
        public List<User> GetUsers();
        public List<City> GetCities(int id);
        public string GetUsers_id(int id, string url, int missionid, int from_id);
        public List<MissionTheme> GetThemes();
        public List<Skill> GetSkills();
        public string updateandaddrate(int missionid, int rating, int userid);
        public VolunteerViewModel getvolunteermission(int id, int pageindex, int pagesize, string userid);
        public string fav_mission(int missionid, int userid);
        public string AddComment(int missionid, string userid, string commentsDiscription);
        public string apply_mission(int missionid, string userid);
    }
}
