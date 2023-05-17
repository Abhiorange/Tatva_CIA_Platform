using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Repositories.Interface
{
    public interface IUserRepository
    {
        public string login(LoginViewModel user);
        public string register(RegisterViewModel user);
        public string forget(ForgetViewModel model,string url);
        public ContactUsViewModel addcontact(string userid);
        public List<Banner> GetBanners();
        public void clearall(string userid);
        public Tuple<List<NotificationTitle>, List<long>> gettitles(string userId);
        public void setstatus(string userid, List<string> titles);
        public List<Tuple<string, long, string, string, int, int, string>> getnotification(string userId);
        public void changestatus(int messageid, string userid);

        public string reset(ResetViewModel model, string token);
        public List<Country> GetCountries();
        public void GetSkills(List<int> ids, long userid);
        public void editcontact(string subject, string message, long userid);
        public string editimage(IFormFile Image, long userid);

        public bool changepass(UserDetailViewModel model, long userid);
        public List<City> Getcity(long id);
        public UserDetailViewModel getdatauser(long userid);
        public void usereditdatabase(UserDetailViewModel model, long user_id);

    }
}
