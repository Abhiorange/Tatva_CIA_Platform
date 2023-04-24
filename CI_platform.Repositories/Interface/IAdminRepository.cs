using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Repositories.Interface
{
    public interface IAdminRepository
    {
        public UserAdminViewModel getuserdata(int pageindex, int pageSize, string SearchInputdata);
        public UserAdminViewModel getmissiondata(int pageindex, int pageSize, string SearchInputdata);
        public List<Country> getcountries();
        public List<City> getcities(string countryid);
        public List<MissionTheme> getthemes();
        public void deletemission(string missionid);
        public void Addmission(MissionAddViewModel model);
        public void Addskill(SkillAddViewModel model);
        public UserAdminViewModel getthemedata(int pageindex, int pageSize, string SearchInputdata);
        public void Addtheme(ThemeAddViewModel model);
        public void deletetheme(string themeid);
        public UserAdminViewModel getskilldata(int pageindex, int pageSize, string SearchInputdata);
        public UserAdminViewModel getmissionapplicationdata(int pageindex, int pageSize, string SearchInputdata);
        public void approveapplication(string applicationid);
        public void declineapplication(string applicationid);





    }
}
