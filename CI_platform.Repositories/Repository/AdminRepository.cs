using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CI_platform.Repositories.Repository
{
    public class AdminRepository : IAdminRepository
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
            var missionskill = _ciplatformcontext.MissionSkills.ToList();
            var model = new UserAdminViewModel();
            model.MissionSkills = missionskill;
            model.Skills = skills.ToPagedList(pageindex, 2);
            return model;
        }
        public SkillAddViewModel getskill(string skillid)
        {
            var skill = _ciplatformcontext.Skills.FirstOrDefault(s => s.SkillId.ToString() == skillid);
            var model = new SkillAddViewModel
            {
               SkillId=skill.SkillId,
               SkillName=skill.SkillName,
               Status=skill.Status
            };
            return model;
        }
        public void editskilldatabase(SkillAddViewModel model)
        {
            var skill = _ciplatformcontext.Skills.FirstOrDefault(s => s.SkillId == model.SkillId);
            skill.SkillName = model.SkillName;
            skill.Status = model.Status;
            _ciplatformcontext.SaveChanges();
        }
        public UserAdminViewModel getstorydata(int pageindex, int pageSize, string SearchInputdata)
        {
        var stories =_ciplatformcontext.Stories.Include(s=>s.User).Include(s=>s.Mission).Where(s=>s.Status!="DRAFT" && ((SearchInputdata == null)|| (s.Mission.Title.Contains(SearchInputdata)) ||(s.User.FirstName.Contains(SearchInputdata)))).ToList();
            var model = new UserAdminViewModel();
            model.Stories = stories.ToPagedList(pageindex, 1);
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
        public UserAddViewModel edituserdata(string userid)
        {
            var user = _ciplatformcontext.Users.FirstOrDefault(u => u.UserId.ToString() == userid);
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _ciplatformcontext.Countries.ToList();
            foreach (var item in temp)
            {
                list.Add(new SelectListItem() { Text = item.Name, Value = item.CountryId.ToString() });
            }
            List<SelectListItem> list1 = new List<SelectListItem>();
            var temp1 = _ciplatformcontext.Cities.Where(c => c.CountryId == user.CountryId).ToList();
            foreach (var item in temp1)
            {
                list1.Add(new SelectListItem() { Text = item.Name, Value = item.CityId.ToString() });
            }
            var model = new UserAddViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileText = user.ProfileText,
                Password = user.Password,
                Department = user.Department,
                LinkedInUrl = user.LinkedInUrl,
                EmployeeId = user.EmployeeId,
                CityId = user.CityId,
                CountryId = user.CountryId,
                cities = list1,
                countries = list,
                Email=user.Email,
                Status=user.Status,
                avtar=user.Avatar
            };
           /* string contentRootPath = _hostEnvironment.ContentRootPath;
            string imagesFolderPath = Path.Combine(contentRootPath, "wwwroot");
            string imagepath = Path.Combine(imagesFolderPath, user.Avatar);
            model.Avatar = new FormFile(new FileStream(imagepath, FileMode.Open), 0, new FileInfo(imagepath).Length, null, Path.GetFileName(imagepath));*/
            return model;
             
        }
       public void  updateuser(UserAddViewModel model)
        {
            var user = _ciplatformcontext.Users.FirstOrDefault(u => u.UserId == model.UserId);

        }
        public void approveapplication(string applicationid)
        {
            var missionapplication = _ciplatformcontext.MissionApplications.FirstOrDefault(m => m.MissionApplicationId.ToString() == applicationid);
            missionapplication.ApprovalStatus = "APPROVE";
            _ciplatformcontext.SaveChanges();
        }
        public void approvestory(string storyid)
        {
            var story = _ciplatformcontext.Stories.FirstOrDefault(s => s.StoryId.ToString() == storyid);
            story.Status = "PUBLISHED";
            _ciplatformcontext.SaveChanges();
        }
        
        public void declineapplication(string applicationid)
        {
            var missionapplication = _ciplatformcontext.MissionApplications.FirstOrDefault(m => m.MissionApplicationId.ToString() == applicationid);
            missionapplication.ApprovalStatus = "DECLINE";
            _ciplatformcontext.SaveChanges();
        }
        public void declinestory(string storyid)
        {
            var story = _ciplatformcontext.Stories.FirstOrDefault(s => s.StoryId.ToString() == storyid);
            story.Status = "DECLINED";
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
        public MissionAddViewModel getmissionmodeldata()
        {
            var skills = _ciplatformcontext.Skills.ToList();
            var model = new MissionAddViewModel
            {
                Skills = skills
            };
            return model;
        }
       
        public void Adduser(UserAddViewModel model)
        {
            var model1 = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                EmployeeId=model.EmployeeId,
                Department=model.Department,
                CityId=model.CityId,
                CountryId=model.CountryId,
                ProfileText=model.ProfileText,
                Status=model.Status
            };
            _ciplatformcontext.Add(model1);
            _ciplatformcontext.SaveChanges();
            var user = _ciplatformcontext.Users.FirstOrDefault(u => u.UserId == model1.UserId);
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string imagesFolderPath = Path.Combine(wwwRootPath, "Images");
            string MainfolderPath = Path.Combine(imagesFolderPath, "UserProfileImages");
            if (!Directory.Exists(MainfolderPath))
            {
                Directory.CreateDirectory(MainfolderPath);
            }
            string fileName = Guid.NewGuid().ToString();

            var uploads = Path.Combine(MainfolderPath, fileName + Path.GetExtension(model.Avatar.FileName));

            using (var fileStreams = new FileStream(uploads, FileMode.Create))
            {
                model.Avatar.CopyTo(fileStreams);
            }
            user.Avatar = @"\Images\UserProfileImages\" + fileName + Path.GetExtension(model.Avatar.FileName);
            _ciplatformcontext.SaveChanges();
            
        }
        public void Addmission(MissionAddViewModel model, List<int> selectedSkills)
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
            foreach (var skill in selectedSkills)
            {
                var model3 = new MissionSkill
                {
                    SkillId = skill,
                };
                model1.MissionSkills.Add(model3);
            }
            if (model.MissionType=="goal")
            {
                var model2 = new GoalMission
                {
                    GoalObjectiveText = model.GoalObjectiveText,
                    GoalValue=model.GoalValue,
                   
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

            string documentFolderPath = Path.Combine(wwwRootPath, "Documents");
            string missiondocPath = Path.Combine(documentFolderPath, "Mission");
            string missiondocfolderPath = Path.Combine(missiondocPath, folderName);
            if (!Directory.Exists(missiondocfolderPath))
            {
                Directory.CreateDirectory(missiondocfolderPath);
            }
            foreach (var doc in model.Documents)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(folderPath, fileName + Path.GetExtension(doc.FileName));
                using (var fileStreams = new FileStream(uploads, FileMode.Create))
                {
                    doc.CopyTo(fileStreams);
                }
                MissionDocument docModel = new MissionDocument()
                {
                    MissionId = mission.MissionId,
                    DocumentName = doc.FileName,
                    DocumentPath = @"\Documents\Mission\" + folderName + @"\" + fileName + Path.GetExtension(doc.FileName),
                };
              
                switch (Path.GetExtension(doc.FileName))
                {
                    case ".doc":
                    case ".docx":
                        docModel.DocumentType = "DOCX";
                        break;
                    case ".xls":
                    case ".xlsx":
                        docModel.DocumentType = "XLSX";
                        break;
                    case ".pdf":
                        docModel.DocumentType = "PDF";
                        break;
                    default:
                        // Handle other types of documents here
                        break;
                }
                _ciplatformcontext.MissionDocuments.Add(docModel);
            }
            _ciplatformcontext.SaveChanges();

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
        public bool deleteskill(string skillid)
        {
            var skill = _ciplatformcontext.Skills.FirstOrDefault(s => s.SkillId.ToString() == skillid);
            var MissionSkill = _ciplatformcontext.MissionSkills.Select(mp => mp.SkillId).ToList();
            var UserSkill = _ciplatformcontext.UserSkills.Select(us => us.SkillId).ToList();
            if(MissionSkill.Contains(int.Parse(skillid)) && UserSkill.Contains(int.Parse(skillid)))
            {
                return false;
            }
            else
            {
                skill.Status = 0;
                _ciplatformcontext.SaveChanges();
                return true;
            }
           
        }
    }
}
