﻿using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            var users = _ciplatformcontext.Users.Where(u => ((SearchInputdata == null) || (u.FirstName.Contains(SearchInputdata)) || (u.LastName.Contains(SearchInputdata))) && u.DeletedAt==null ).ToList();
            var model = new UserAdminViewModel();

            model.users = users.ToPagedList(pageindex, 10);
            return model;
        }
        public UserAdminViewModel getthemedata(int pageindex, int pageSize, string SearchInputdata)
        {
            var themes = _ciplatformcontext.MissionThemes.Where(t => ((SearchInputdata == null) || (t.Title.Contains(SearchInputdata))) && t.DeletedAt==null ).OrderByDescending(m => m.Status).ToList();
            var model = new UserAdminViewModel();
            model.MissionThemes = themes.ToPagedList(pageindex, 2);
            return model;

        }
        public UserAdminViewModel getskilldata(int pageindex, int pageSize, string SearchInputdata)
        {
            var skills = _ciplatformcontext.Skills.Where(s => ((SearchInputdata == null) || (s.SkillName.Contains(SearchInputdata))) && s.DeletedAt==null).OrderByDescending(s => s.Status).ToList();
            var missionskill = _ciplatformcontext.MissionSkills.ToList();
            var model = new UserAdminViewModel();
            model.MissionSkills = missionskill;
            model.Skills = skills.ToPagedList(pageindex, 2);
            return model;
        }
        public UserAdminViewModel getcmspagedata(int pageindex, int pageSize, string SearchInputdata)
        {
            var pages = _ciplatformcontext.CmsPages.Where(c => ((SearchInputdata == null) || (c.Title.Contains(SearchInputdata))) && c.DeletedAt == null).OrderByDescending(c => c.Status).ToList();
            var model = new UserAdminViewModel();
            model.Cmspages = pages.ToPagedList(pageindex, 4);
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
        public ThemeAddViewModel gettheme(string themeid)
        {
            var themes = _ciplatformcontext.MissionThemes.FirstOrDefault(mt => mt.MissionThemeId.ToString() == themeid);
            var model = new ThemeAddViewModel
            {
               MissionThemeId=themes.MissionThemeId,
               Title=themes.Title,
               Status=themes.Status
            };
            return model;
        }
        public CmsAddViewModel getcmsdata(string cmspageid)
        {
            var cmspage = _ciplatformcontext.CmsPages.FirstOrDefault(c => c.CmsPageId.ToString() == cmspageid);
            var model = new CmsAddViewModel
            {
                CmsPageId=cmspage.CmsPageId,
                Title=cmspage.Title,
                Description=cmspage.Description,
                Slug=cmspage.Slug,
                Status=cmspage.Status,
            };
            return model;
        }
        public bool editskilldatabase(SkillAddViewModel model)
        {
            List<string> skillnames = _ciplatformcontext.Skills.Select(s => s.SkillName).ToList();
            if(skillnames.Contains(model.SkillName))
            {
                return false;
            }
            else
            {
                var skill = _ciplatformcontext.Skills.FirstOrDefault(s => s.SkillId == model.SkillId);
                skill.SkillName = model.SkillName;
                skill.Status = model.Status;
                _ciplatformcontext.SaveChanges();
                return true;
            }
           
        }
        public void editthemedatabase(ThemeAddViewModel model)
        {
            var theme = _ciplatformcontext.MissionThemes.FirstOrDefault(s => s.MissionThemeId == model.MissionThemeId);
            theme.Title = model.Title;
            theme.Status = model.Status;
            _ciplatformcontext.SaveChanges();
        }
        public void editcmspage(CmsAddViewModel model)
        {
            var cmspage = _ciplatformcontext.CmsPages.SingleOrDefault(c => c.CmsPageId == model.CmsPageId);
            cmspage.Title = model.Title;
            cmspage.Slug = model.Slug;
            cmspage.Description = model.Description;
            cmspage.Status = model.Status;
            _ciplatformcontext.SaveChanges();
        }
        public UserAdminViewModel getstorydata(int pageindex, int pageSize, string SearchInputdata)
        {
        var stories =_ciplatformcontext.Stories.Include(s=>s.User).Include(s=>s.Mission).Where(s=>s.Status!="DRAFT" && s.DeletedAt==null && ((SearchInputdata == null)|| (s.Mission.Title.Contains(SearchInputdata)) ||(s.User.FirstName.Contains(SearchInputdata)))).ToList();
            var model = new UserAdminViewModel();
            model.Stories = stories.ToPagedList(pageindex, 1);
            return model;
        }
        public UserAdminViewModel getmissiondata(int pageindex, int pageSize, string SearchInputdata)
        {
           
            var missions = _ciplatformcontext.Missions.Where(m => ((SearchInputdata == null) || (m.Title.Contains(SearchInputdata)) || (m.MissionType.Contains(SearchInputdata))) && m.DeletedAt==null).OrderByDescending(m => m.Status).ToList();

            var model = new UserAdminViewModel();
            
            model.Missions = missions.ToPagedList(pageindex, 10);
            return model;
        }
        public UserAdminViewModel getbannerdata(int pageindex, string SearchInputdata)
        {
            var banners=_ciplatformcontext.Banners.Where(b=> ((SearchInputdata == null) || (b.Title.Contains(SearchInputdata)) || (b.Text.Contains(SearchInputdata))) && b.DeletedAt==null).OrderByDescending(m => m.Status).ToList();
            var model = new UserAdminViewModel();
            model.Banners = banners.ToPagedList(pageindex, 5);
            return model;
        }
        public BannerAddViewModel getBanner(string bannerid)
        {
            Banner banner = _ciplatformcontext.Banners.SingleOrDefault(b=>b.BannerId.ToString() == bannerid);
            string wwwRootPath = _hostEnvironment.WebRootPath;
            
            string fullPath = wwwRootPath + banner.Image;
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                IFormFile file = new FormFile(stream, 0, new FileInfo(fullPath).Length, null, Path.GetFileName(fullPath));

                BannerAddViewModel model = new BannerAddViewModel()
                {
                    BannerId = banner.BannerId,
                    Text = banner.Text,
                    Title = banner.Title,
                    Image = file,
                    SortOrder = banner.SortOrder,
                    Status = banner.Status
                };
                return model;
            }
        }
        public UserAdminViewModel getmissionapplicationdata(int pageindex, int pageSize, string SearchInputdata)
        {
            var missionapplication = _ciplatformcontext.MissionApplications.Include(m => m.Mission).Include(m=>m.User).Where(m => ((SearchInputdata == null) || (m.Mission.Title.Contains(SearchInputdata)) || (m.User.FirstName.Contains(SearchInputdata))) && m.DeletedAt==null).ToList();
            var model = new UserAdminViewModel();
            model.MissionApplications = missionapplication.ToPagedList(pageindex, 4);
            return model;
        }
        public MissionAddViewModel editmissondata(string missonid)
        {
            var mission = _ciplatformcontext.Missions.FirstOrDefault(m => m.MissionId.ToString() == missonid);
            var goalmission = _ciplatformcontext.GoalMissions.Include(g => g.Mission).FirstOrDefault(g=>g.MissionId.ToString()==missonid);
            List<MissionMedium> missionMedia = _ciplatformcontext.MissionMedia.Where(m => m.MissionId.ToString() == missonid).ToList();
            List<MissionDocument> missionDoc = _ciplatformcontext.MissionDocuments.Where(m => m.MissionId.ToString() == missonid).ToList();
            List<IFormFile> imageFiles = new List<IFormFile>();
            List<IFormFile> docFiles = new List<IFormFile>();
            List<SelectListItem> list = new List<SelectListItem>();
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var temp = _ciplatformcontext.Countries.ToList();
            foreach (var item in temp)
            {
                list.Add(new SelectListItem() { Text = item.Name, Value = item.CountryId.ToString() });
            }
            List<SelectListItem> list1 = new List<SelectListItem>();
            var temp1 = _ciplatformcontext.Cities.Where(c => c.CountryId == mission.CountryId).ToList();
            foreach (var item in temp1)
            {
                list1.Add(new SelectListItem() { Text = item.Name, Value = item.CityId.ToString() });
            }
            List<SelectListItem> list2 = new List<SelectListItem>();
            var temp2 = _ciplatformcontext.MissionThemes.ToList();
            foreach (var item in temp2)
            {
                list2.Add(new SelectListItem() { Text = item.Title, Value = item.MissionThemeId.ToString() });
            }
            var skills = _ciplatformcontext.Skills.ToList();
            var skillids_mission = _ciplatformcontext.MissionSkills.Include(m=>m.Skill).Where(m => m.MissionId == mission.MissionId).Select(m=>m.SkillId).ToList();
          
            var model = new MissionAddViewModel
            {
                countries = list,
                cities = list1,
                Themes = list2,
                Title = mission.Title,
                CountryId = mission.CountryId,
                CityId = mission.CityId,
                ThemeId = mission.ThemeId,
                Description = mission.Description,
                ShortDescription = mission.ShortDescription,
                TotalSeats = mission.TotalSeats,
                StartDate = mission.StartDate,
                EndDate = mission.EndDate,
                Status = mission.Status,
                MissionType = mission.MissionType,
                OrganizationDetail = mission.OrganizationDetail,
                OrganizationName = mission.OrganizationName,
                Availibility = mission.Availibility,
                MissionId=mission.MissionId,
                skillids=skillids_mission,
                Skills=skills,
                RegistrationDeadline=mission.RegistrationDeadline
            };
            foreach (var m in missionMedia)
            {
                string fullPath = wwwRootPath + m.MediaPath;
                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    IFormFile file = new FormFile(stream, 0, new FileInfo(fullPath).Length, null, Path.GetFileName(fullPath));
                    imageFiles.Add(file);
                }
            }
            foreach (var m in missionDoc)
            {
                string fullPath = wwwRootPath + m.DocumentPath;
                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    IFormFile file = new FormFile(stream, 0, new FileInfo(fullPath).Length, null, Path.GetFileName(fullPath));
                    docFiles.Add(file);
                }
            }
            model.Images = imageFiles;
            model.Documents = docFiles;
            if (model.MissionType == "goal")
            {
                model.GoalObjectiveText = goalmission.GoalObjectiveText;
                model.GoalValue = goalmission.GoalValue;
            };
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
                avtar=user.Avatar,
                UserId=user.UserId
            };     
            return model;
        }
        public void editBanner(BannerAddViewModel model)
        {
            Banner banner = _ciplatformcontext.Banners.SingleOrDefault(b => b.BannerId == model.BannerId);
            if (model.Image != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string imageFolderPath = Path.Combine(wwwRootPath, "Images");
                string mainFolderPath = Path.Combine(imageFolderPath, "Banner");
                String[] files = Directory.GetFiles(mainFolderPath);
                if (!Directory.Exists(mainFolderPath))
                {
                    Directory.CreateDirectory(mainFolderPath);
                }
                string fullPath = Path.Combine(mainFolderPath, model.Image.FileName);
                if (!File.Exists(fullPath))
                {
                    using (var fileStreams = new FileStream(Path.Combine(mainFolderPath, model.Image.FileName), FileMode.Create))
                    {
                        model.Image.CopyTo(fileStreams);
                    }
                }
                banner.Image = @"\Images\Banner\" + model.Image.FileName;
            }
            banner.SortOrder = model.SortOrder;
            banner.Text = WebUtility.HtmlDecode(model.Text);
            banner.Title = model.Title;
            banner.Status = model.Status;
            _ciplatformcontext.Update(banner);
            _ciplatformcontext.SaveChanges();
        }
       public void  updateuser(UserAddViewModel model)
        {
            var user = _ciplatformcontext.Users.FirstOrDefault(u => u.UserId == model.UserId);
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string imagesFolderPath = Path.Combine(wwwRootPath, "Images");
            string MainfolderPath = Path.Combine(imagesFolderPath, "UserProfileImages");
            
            String[] files = Directory.GetFiles(MainfolderPath);
            if (!Directory.Exists(MainfolderPath))
            {
                Directory.CreateDirectory(MainfolderPath);
            }
            if(model.Avatar==null)
            {
                var oldImagePath = user.Avatar;
                user.Avatar = oldImagePath;
            }
            else
            {
                string fileName_exist = model.Avatar.FileName;
                string fullPath = Path.Combine(MainfolderPath, fileName_exist);
                string uploads = Path.Combine(MainfolderPath, fileName_exist);
                if (!File.Exists(fullPath))
                {
                    string fileName = fileName_exist;
                    string filePath = Path.Combine(MainfolderPath, fileName);
                    using (var inputStream = model.Avatar.OpenReadStream())
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            inputStream.CopyTo(fileStream);
                        }
                    }
                    user.Avatar = @"\Images\UserProfileImages\" + fileName;
                }
                else
                {
                    user.Avatar = @"\Images\UserProfileImages\" + fileName_exist;

                }
            }
          
            user.FirstName = model.FirstName;
            user.LastName=model.LastName;
            user.Password = model.Password;
            user.LinkedInUrl = model.LinkedInUrl;
            user.EmployeeId = model.EmployeeId;
            user.Department = model.Department;
            user.Email = model.Email;
            user.CountryId = model.CountryId;
            user.CityId = model.CityId;
            user.ProfileText = model.ProfileText;
            user.Status = model.Status;
            user.Title = model.Title;
            _ciplatformcontext.SaveChanges();
        }
        public void approveapplication(string applicationid,string userid)
        {
            var missionapplication = _ciplatformcontext.MissionApplications.FirstOrDefault(m => m.MissionApplicationId.ToString() == applicationid);
            missionapplication.ApprovalStatus = "APPROVE";
            var mission = _ciplatformcontext.Missions.SingleOrDefault(m => m.MissionId == missionapplication.MissionId);
            mission.TotalSeats = mission.TotalSeats - 1;
          
            var enable_check = _ciplatformcontext.EnableUserStatuses.SingleOrDefault(e => e.NotificationId == 8 && e.UserId.ToString() == userid)?.Status;
            if(enable_check==1)
            {
                var message = new MessageTable
                {
                    NotificationId = 8,
                    Message = $"Volunterring Request has been approved for Mission-{mission.Title}"
                };
                _ciplatformcontext.Add(message);
                var userpermission = new Userpermission
                {
                    UserId = long.Parse(userid)
                };
                message.Userpermissions.Add(userpermission);
            }
           
            _ciplatformcontext.SaveChanges();
        }
        public void approvestory(string storyid,string userid)
        {
            var story = _ciplatformcontext.Stories.FirstOrDefault(s => s.StoryId.ToString() == storyid);
            story.Status = "PUBLISHED";
          
            var enable_check = _ciplatformcontext.EnableUserStatuses.SingleOrDefault(e => e.NotificationId == 4 && e.UserId.ToString() == userid)?.Status;
            if (enable_check == 1)
            {
                var message = new MessageTable
                {
                    NotificationId = 4,
                    Message = $"Volunterring Request has been approved for Story-{story.Title}"
                };
                _ciplatformcontext.Add(message);
                var userpermission = new Userpermission
                {
                    UserId = long.Parse(userid)
                };
                message.Userpermissions.Add(userpermission);
            }
            _ciplatformcontext.SaveChanges();
        }
        
        public void declineapplication(string applicationid,string userid)
        {
            var missionapplication = _ciplatformcontext.MissionApplications.FirstOrDefault(m => m.MissionApplicationId.ToString() == applicationid);
            var mission = _ciplatformcontext.Missions.SingleOrDefault(m => m.MissionId == missionapplication.MissionId);
            if (missionapplication.ApprovalStatus == "APPROVE")
            {
                mission.TotalSeats = mission.TotalSeats + 1;
            }
            missionapplication.ApprovalStatus = "DECLINE";
           
           
            var enable_check = _ciplatformcontext.EnableUserStatuses.SingleOrDefault(e => e.NotificationId == 8 && e.UserId.ToString() == userid)?.Status;
            if (enable_check == 1)
            {
                var message = new MessageTable
                {
                    NotificationId = 8,
                    Message = $"Volunterring Request has been declined for Mission-{mission.Title}"
                };
                _ciplatformcontext.Add(message);
                var userpermission = new Userpermission
                {
                    UserId = long.Parse(userid)
                };
                message.Userpermissions.Add(userpermission);
            }
            _ciplatformcontext.SaveChanges();
        }
        public void declinestory(string storyid,string userid)
        {
            var story = _ciplatformcontext.Stories.FirstOrDefault(s => s.StoryId.ToString() == storyid);
            story.Status = "DECLINED";
           
            var enable_check = _ciplatformcontext.EnableUserStatuses.SingleOrDefault(e => e.NotificationId == 4 && e.UserId.ToString() == userid)?.Status;
            if (enable_check == 1)
            { var url = "";
                if (story.Status == "DECLINED")
                {
                     url = $"https://localhost:7292/StoryListing/addstory?missionid={story.MissionId}";
                }
                else
                {
                    url = $"https://localhost:7292/StoryListing/storydetail?story_id={story.StoryId}";
                }
                var message = new MessageTable
                {
                    NotificationId = 4,
                    Url=url,
                    Message = $"Volunterring Request has been declined for Story-{story.Title}"
                };
                _ciplatformcontext.Add(message);
                var userpermission = new Userpermission
                {
                    UserId = long.Parse(userid)
                };
                message.Userpermissions.Add(userpermission);
            }
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
        public bool Addtheme(ThemeAddViewModel model)
        {
            List<string> Themetitle = _ciplatformcontext.MissionThemes.Select(s => s.Title).ToList();
            if(Themetitle.Contains(model.Title))
            {
                return false;
            }
            else
            {
                var model1 = new MissionTheme
                {
                    Title = model.Title,
                    Status = model.Status,
                };
                _ciplatformcontext.Add(model1);
                _ciplatformcontext.SaveChanges();
                return true;
            }
           
        }
        public bool Addskill(SkillAddViewModel model)
        {
            List<string> Skillnames = _ciplatformcontext.Skills.Select(s => s.SkillName).ToList();
            if(Skillnames.Contains(model.SkillName))
            {
                return false;
            }
            else
            {
                var model1 = new Skill
                {
                    SkillName = model.SkillName,
                    Status = 0,
                };
                _ciplatformcontext.Add(model1);
                _ciplatformcontext.SaveChanges();
                return true;
            }
          
        }
        public void Addcms(CmsAddViewModel model)
        {
            var model1 = new CmsPage
            {
                Title = model.Title,
                Description=model.Description,
                Slug=model.Slug,
                Status=model.Status
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
                Status=model.Status,
                LinkedInUrl=model.LinkedInUrl
            };
            _ciplatformcontext.Add(model1);
          if(model.Avatar!=null)
            {
                var user = _ciplatformcontext.Users.FirstOrDefault(u => u.UserId == model1.UserId);
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string imagesFolderPath = Path.Combine(wwwRootPath, "Images");
                string MainfolderPath = Path.Combine(imagesFolderPath, "UserProfileImages");
                if (!Directory.Exists(MainfolderPath))
                {
                    Directory.CreateDirectory(MainfolderPath);
                }
                string fileName = model.Avatar.FileName;

                var uploads = Path.Combine(MainfolderPath, fileName);

                using (var fileStreams = new FileStream(uploads, FileMode.Create))
                {
                    model.Avatar.CopyTo(fileStreams);
                }
               model1.Avatar = @"\Images\UserProfileImages\" + fileName;
            }
            else
            {
                model1.Avatar = @"\Images\f38f7d36-e789-477f-939b-2760507ce69d.png";
            }
           
            _ciplatformcontext.SaveChanges();
            
        }
        public void addBanner(BannerAddViewModel model)
        {
           
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string imageFolderPath = Path.Combine(wwwRootPath, "Images");
                string mainFolderPath = Path.Combine(imageFolderPath, "Banner");
                if (!Directory.Exists(mainFolderPath))
                {
                    Directory.CreateDirectory(mainFolderPath);
                }
                string fullPath = Path.Combine(mainFolderPath, model.Image.FileName);
                if (!File.Exists(fullPath))
                {
                    using (var fileStreams = new FileStream(Path.Combine(mainFolderPath, model.Image.FileName), FileMode.Create))
                    {
                        model.Image.CopyTo(fileStreams);
                    }
                }
                Banner banner = new Banner
                {
                    Text = model.Text,
                    Title = model.Title,
                    Image = @"\Images\Banner\" + model.Image.FileName,
                    SortOrder = model.SortOrder,
                    Status = model.Status,
                };
                _ciplatformcontext.Add(banner);
                _ciplatformcontext.SaveChanges();
        }
        public void Editmission(MissionAddViewModel model, List<int> selectedSkills)
        {
            var mission = _ciplatformcontext.Missions.SingleOrDefault(m => m.MissionId == model.MissionId);
            List<MissionMedium> missionMedia = _ciplatformcontext.MissionMedia.Where(m => m.MissionId == model.MissionId).ToList();
            List<MissionDocument> missionDoc = _ciplatformcontext.MissionDocuments.Where(m => m.MissionId == model.MissionId).ToList();
            var missionskills = _ciplatformcontext.MissionSkills.Where(m=>m.MissionId==model.MissionId).ToList();
            if(missionskills.Count()>0)
            {
                _ciplatformcontext.MissionSkills.RemoveRange(missionskills);
            }
            var goals = _ciplatformcontext.GoalMissions.Where(g=>g.MissionId==model.MissionId).ToList();
            if(model.MissionType=="goal")
            {
                _ciplatformcontext.GoalMissions.RemoveRange(goals);
                var goalmodel = new GoalMission
                {
                    GoalObjectiveText=model.GoalObjectiveText,
                    GoalValue=model.GoalValue
                };
                mission.GoalMissions.Add(goalmodel);
            }
            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (model.Title != mission.Title && model.Images == null)
            {
                string imagesFolderPath = Path.Combine(wwwRootPath, "Images");
                string MainfolderPath = Path.Combine(imagesFolderPath, "Mission");
                string sourceDir = Path.Combine(MainfolderPath, mission.Title);
                string destDir = Path.Combine(MainfolderPath, model.Title);
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
                string[] imageFiles = Directory.GetFiles(sourceDir);

                foreach (string imageFile in imageFiles)
                {
                    string fileName = Path.GetFileName(imageFile);
                    string destFile = Path.Combine(destDir, fileName);
                    File.Copy(imageFile, destFile, true);
                }


            }
            if (model.Title != mission.Title && model.Documents == null)
            {
                string docFolderPath = Path.Combine(wwwRootPath, "Documents");
                string missionDocPath = Path.Combine(docFolderPath, "Mission");
                string sourceDocDir = Path.Combine(missionDocPath, mission.Title);
                string destDocDir = Path.Combine(missionDocPath, model.Title);
                if (!Directory.Exists(destDocDir))
                {
                    Directory.CreateDirectory(destDocDir);
                }
                string[] files = Directory.GetFiles(sourceDocDir);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(destDocDir, fileName);
                    File.Copy(file, destFile, true);
                }
            }
            foreach (var skill in selectedSkills)
            {
                MissionSkill skillmodel = new MissionSkill
                {    
                    SkillId = skill,                
                };
                mission.MissionSkills.Add(skillmodel);
            }
            if(model.Images!=null)
            {  
                if(missionMedia.Count()!=0)
                {
                    _ciplatformcontext.MissionMedia.RemoveRange(missionMedia);
                }

                string imagesFolderPath = Path.Combine(wwwRootPath, "Images");
                string MainfolderPath = Path.Combine(imagesFolderPath, "Mission");
                if (!Directory.Exists(MainfolderPath))
                {
                    Directory.CreateDirectory(MainfolderPath);
                }
                string folderName = model.Title;
                string folderPath = Path.Combine(MainfolderPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                foreach (var Image in model.Images)
                {
                    string fileName = Image.FileName;
                    var uploads = Path.Combine(folderPath, fileName);
                    using (var fileStreams = new FileStream(uploads, FileMode.Create))
                    {
                        Image.CopyTo(fileStreams);
                    }
                    var viewModel = new MissionMedium
                    {
                        MediaName = fileName,
                        MediaType = "Imag",
                        MediaPath = @"\Images\Mission\" + folderName + @"\" + fileName,
                    };
                    mission.MissionMedia.Add(viewModel);
                }
            }
            if(model.Documents!=null)
            {  
                if (missionDoc.Count() != 0)
                {
                    _ciplatformcontext.MissionDocuments.RemoveRange(missionDoc);
                }

                string docFolderPath = Path.Combine(wwwRootPath, "Documents");
                string docMainfolderPath = Path.Combine(docFolderPath, "Mission");
                if (!Directory.Exists(docMainfolderPath))
                {
                    Directory.CreateDirectory(docMainfolderPath);
                }
                string folderName = model.Title;
                string docfolderPath = Path.Combine(docMainfolderPath, folderName);
                if (!Directory.Exists(docfolderPath))
                {
                    Directory.CreateDirectory(docfolderPath);
                }
                foreach (var doc in model.Documents)
                {
                    string fileName = doc.FileName;
                    var uploads = Path.Combine(docfolderPath, fileName + Path.GetExtension(doc.FileName));
                    using (var fileStreams = new FileStream(uploads, FileMode.Create))
                    {
                        doc.CopyTo(fileStreams);
                    }
                    MissionDocument docModel = new MissionDocument()
                    {
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
                    mission.MissionDocuments.Add(docModel);
                }
            }
            mission.Title = model.Title;
            mission.CityId = model.CityId;
            mission.CountryId = mission.CountryId;
            mission.OrganizationDetail = mission.OrganizationDetail;
            mission.OrganizationName = mission.OrganizationName;
            mission.ShortDescription = model.ShortDescription;
            mission.Description = model.Description;
            mission.StartDate = model.StartDate;
            mission.EndDate = model.EndDate;
            mission.TotalSeats = model.TotalSeats;
            mission.Availibility = model.Availibility;
            mission.ThemeId = model.ThemeId;
            mission.MissionType = model.MissionType;
            mission.Status = model.Status;
            mission.RegistrationDeadline = model.RegistrationDeadline;
            _ciplatformcontext.SaveChanges();
        }
        public void Addmission(MissionAddViewModel model, List<int> selectedSkills,string userid)
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
                MissionType=model.MissionType,
                RegistrationDeadline=model.RegistrationDeadline
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
                string fileName = Image.FileName;
                var uploads = Path.Combine(folderPath, fileName);
                using (var fileStreams = new FileStream(uploads, FileMode.Create))
                {
                    Image.CopyTo(fileStreams);
                }
                var viewModel = new MissionMedium
                {
                    MissionId = mission.MissionId,
                    MediaName=fileName,
                    MediaType="Imag",
                    MediaPath= @"\Images\Mission\"+folderName+@"\" +fileName,
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
                string fileName = doc.FileName;
                var uploads = Path.Combine(missiondocfolderPath, fileName);
                using (var fileStreams = new FileStream(uploads, FileMode.Create))
                {
                    doc.CopyTo(fileStreams);
                }
                MissionDocument docModel = new MissionDocument()
                {
                    MissionId = mission.MissionId,
                    DocumentName = doc.FileName,
                    DocumentPath = @"\Documents\Mission\" + folderName + @"\"+fileName,
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
                        break;
                }
                _ciplatformcontext.MissionDocuments.Add(docModel);
            }
            var message = new MessageTable
            {
                NotificationId=5,
                Message = $"New Mission-{model.Title} is added",
                Url= $"https://localhost:7292/Mission/volunteermission/{model1.MissionId}"
            };
            _ciplatformcontext.Add(message);
            var users = _ciplatformcontext.EnableUserStatuses.Where(e => e.NotificationId == 5).Select(e => e.UserId).ToList();
            foreach (var userId in users)
            {
                var userpermission = new Userpermission
                {
                    UserId = userId,

                };
                message.Userpermissions.Add(userpermission);
            }     
            _ciplatformcontext.SaveChanges();

        }
        public void deletemission(string missionid)
        {
            var mission = _ciplatformcontext.Missions.FirstOrDefault(m => m.MissionId.ToString() == missionid);
            mission.Status = 0;
            mission.DeletedAt = DateTime.Now;
            _ciplatformcontext.SaveChanges();
        }
        public void deletecmspage(string cmspageid)
        {
            var cmspage = _ciplatformcontext.CmsPages.FirstOrDefault(c => c.CmsPageId.ToString() == cmspageid);
            cmspage.Status = 0;
            cmspage.DeletedAt = DateTime.Now;
            _ciplatformcontext.SaveChanges();
        }
        public void deleteuser(string userid)
        {
            User user = _ciplatformcontext.Users.SingleOrDefault(u => u.UserId.ToString() == userid);
            user.Status = 0;
            user.DeletedAt = DateTime.Now;
            _ciplatformcontext.SaveChanges();
        }
        public void deletestory(string storyid)
        {
            Story story = _ciplatformcontext.Stories.SingleOrDefault(s => s.StoryId.ToString() == storyid);
            story.Status = "PENDING";
            story.DeletedAt = DateTime.Now;
            _ciplatformcontext.SaveChanges();
        }
        public bool deletetheme(string themeid)
        {
            var theme = _ciplatformcontext.MissionThemes.FirstOrDefault(t => t.MissionThemeId.ToString() == themeid);
            var MissionTheme = _ciplatformcontext.Missions.Select(m => m.ThemeId).ToList();
            if(MissionTheme.Contains(int.Parse(themeid)))
            {
                return false;
            }
            else
            {
                theme.Status = 0;
                theme.DeletedAt = DateTime.Now;
                _ciplatformcontext.SaveChanges();
                return true;
            }
           
        }
        public bool deleteskill(string skillid)
        {
            var skill = _ciplatformcontext.Skills.FirstOrDefault(s => s.SkillId.ToString() == skillid);
            var MissionSkill = _ciplatformcontext.MissionSkills.Select(mp => mp.SkillId).ToList();
            var UserSkill = _ciplatformcontext.UserSkills.Select(us => us.SkillId).ToList();
            if(MissionSkill.Contains(int.Parse(skillid)) || UserSkill.Contains(int.Parse(skillid)))
            {
                return false;
            }
            else
            {
                skill.Status = 0;
                skill.DeletedAt = DateTime.Now;
                _ciplatformcontext.SaveChanges();
                return true;
            }
           
        }
    }
}
