using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CI_platform.Repositories.Repository
{
    public class AdminRepository: IAdminRepository
    {
        private readonly CiPlatformContext _ciplatformcontext;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AdminRepository(CiPlatformContext ciplatformcontext, IWebHostEnvironment hostEnvironment)
        {
            _ciplatformcontext = ciplatformcontext;
            _hostEnvironment = hostEnvironment;
        }
        public UserAdminViewModel getuserdata(int pageindex, int pageSize, string SearchInputdata)
        {

            var users = _ciplatformcontext.Users.Where(u => (SearchInputdata == null) || (u.FirstName.Contains(SearchInputdata)) || (u.LastName.Contains(SearchInputdata))).ToList(); 
            var model = new UserAdminViewModel();
            
            model.users = users.ToPagedList(pageindex, 10);
            return model;
        }
        public UserAdminViewModel getthemedata(int pageindex, int pageSize, string SearchInputdata)
        {
            var themes = _ciplatformcontext.MissionThemes.Where(t => (SearchInputdata == null) || (t.Title.Contains(SearchInputdata))).OrderByDescending(m => m.Status).ToList();
            var model = new UserAdminViewModel();
            model.MissionThemes = themes.ToPagedList(pageindex, 2);
            return model;
            
        }
        public UserAdminViewModel getskilldata(int pageindex, int pageSize, string SearchInputdata)
        {
            var skills = _ciplatformcontext.Skills.Where(s => (SearchInputdata == null) || (s.SkillName.Contains(SearchInputdata))).OrderByDescending(s => s.Status).ToList();
            var model = new UserAdminViewModel();
            model.Skills = skills.ToPagedList(pageindex, 2);
            return model;
        }
        public UserAdminViewModel getmissiondata(int pageindex, int pageSize, string SearchInputdata)
        {
           
            var missions = _ciplatformcontext.Missions.Where(m => (SearchInputdata == null) || (m.Title.Contains(SearchInputdata)) || (m.MissionType.Contains(SearchInputdata))).OrderByDescending(m => m.Status).ToList();

            var model = new UserAdminViewModel();
            
            model.Missions = missions.ToPagedList(pageindex, 10);
            return model;
        }
        public UserAdminViewModel getmissionapplicationdata(int pageindex, int pageSize, string SearchInputdata)
        {
            var missionapplication = _ciplatformcontext.MissionApplications.Include(m => m.Mission).Include(m=>m.User).Where(m => (SearchInputdata == null) || (m.Mission.Title.Contains(SearchInputdata)) || (m.User.FirstName.Contains(SearchInputdata))).ToList();
            var model = new UserAdminViewModel();
            model.MissionApplications = missionapplication.ToPagedList(pageindex, 4);
            return model;
        }
        public void approveapplication(string applicationid)
        {
            var missionapplication = _ciplatformcontext.MissionApplications.FirstOrDefault(m => m.MissionApplicationId.ToString() == applicationid);
            missionapplication.ApprovalStatus = "APPROVE";
            _ciplatformcontext.SaveChanges();
        }
        public void declineapplication(string applicationid)
        {
            var missionapplication = _ciplatformcontext.MissionApplications.FirstOrDefault(m => m.MissionApplicationId.ToString() == applicationid);
            missionapplication.ApprovalStatus = "DECLINE";
            _ciplatformcontext.SaveChanges();
        }
        public List<Country> getcountries()
        {
            var countries = _ciplatformcontext.Countries.ToList();
            return countries;
        }
        public List<City> getcities(string countryid)
        {
            var cities = _ciplatformcontext.Cities.Where(c=>c.CountryId.ToString()==countryid).ToList();
            return cities;
        }
        public List<MissionTheme> getthemes()
        {
            var themes = _ciplatformcontext.MissionThemes.ToList();
            return themes;
        }
        public void Addtheme(ThemeAddViewModel model)
        {
            var model1 = new MissionTheme
            {
                Title = model.Title,
                Status = model.Status,
            };
            _ciplatformcontext.Add(model1);
            _ciplatformcontext.SaveChanges();
        }
        public void Addskill(SkillAddViewModel model)
        {
            var model1 = new Skill
            {
                SkillName=model.SkillName,
                Status=model.Status,
            };
            _ciplatformcontext.Add(model1);
            _ciplatformcontext.SaveChanges();
        }
        public void Addmission(MissionAddViewModel model)
        {
            var model1 = new Mission
            {
                Title = model.Title,
                CityId = model.CityId,
                CountryId = model.CountryId,
                OrganizationDetail = model.OrganizationDetail,
                OrganizationName = model.OrganizationName,
                Description = model.Description,
                ShortDescription=model.ShortDescription,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
               TotalSeats=model.TotalSeats,
                Availibility=model.Availibility,
                ThemeId=model.ThemeId,
                Status=model.Status,
                MissionType=model.MissionType
            };
            
            _ciplatformcontext.Add(model1);
            if(model.MissionType=="goal")
            {
                var model2 = new GoalMission
                {
                    GoalObjectiveText = model.GoalObjectiveText
                };
                model1.GoalMissions.Add(model2);
            }
            _ciplatformcontext.SaveChanges();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string imagesFolderPath = Path.Combine(wwwRootPath, "Images");
            string MainfolderPath = Path.Combine(imagesFolderPath, "Mission");
            if (!Directory.Exists(MainfolderPath))
            {
                Directory.CreateDirectory(MainfolderPath);
            }
            string folderName = model.Title;
            string folderPath = Path.Combine(MainfolderPath, folderName);
            var mission = _ciplatformcontext.Missions.FirstOrDefault(m => m.MissionId == model1.MissionId);
            // Create a new directory if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
           
            foreach (var Image in model.Images)
            {   
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(folderPath, fileName + Path.GetExtension(Image.FileName));
                using (var fileStreams = new FileStream(uploads, FileMode.Create))
                {
                    Image.CopyTo(fileStreams);
                }
                var viewModel = new MissionMedium
                {
                    MissionId = mission.MissionId,
                    MediaName=fileName,
                    MediaType="Imag",
                    MediaPath= @"\Images\Mission\"+folderName+@"\" +fileName + Path.GetExtension(Image.FileName),
                };
                _ciplatformcontext.Add(viewModel);
                _ciplatformcontext.SaveChanges();
            }
        }
        public void deletemission(string missionid)
        {
            var mission = _ciplatformcontext.Missions.FirstOrDefault(m => m.MissionId.ToString() == missionid);
            mission.Status = 0;
            _ciplatformcontext.SaveChanges();
        }
        public void deletetheme(string themeid)
        {
            var theme = _ciplatformcontext.MissionThemes.FirstOrDefault(t => t.MissionThemeId.ToString() == themeid);
            theme.Status = 0;
            _ciplatformcontext.SaveChanges();
        }
    }
}
