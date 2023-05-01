﻿using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_platfom_apllication.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminRepository _adminRepository;
        public AdminController(ILogger<AdminController> logger, IAdminRepository adminRepository)
        {
            _logger = logger;
            _adminRepository = adminRepository;
        }
        public IActionResult UserpageInAdmin(string SearchInputdata="",int pageindex=1, int pageSize=10)
        {
            var model = _adminRepository.getuserdata(pageindex,pageSize,SearchInputdata);
            return View(model);
        }
        
        public IActionResult User(string SearchInputdata="", int pageindex = 1, int pageSize = 10)
        {
            var model = _adminRepository.getuserdata(pageindex, pageSize, SearchInputdata);
            return PartialView("_userpage",model);
        }
        
        public IActionResult Mission(string SearchInputdata = "",int pageindex=1,int pageSize=10)
        {
            var model = _adminRepository.getmissiondata(pageindex, pageSize, SearchInputdata);
            return PartialView("_missionpage",model);
        }
        public IActionResult Theme(string SearchInputdata = "", int pageindex = 1, int pageSize = 2)
        {
            var model = _adminRepository.getthemedata(pageindex, pageSize, SearchInputdata);
            return PartialView("_themepage",model);
        }
        public IActionResult Skill(string SearchInputdata = "", int pageindex = 1, int pageSize = 2)
        {
            var model = _adminRepository.getskilldata(pageindex, pageSize, SearchInputdata);
            return PartialView("_skillpage", model);
        }
        public IActionResult Story(string SearchInputdata = "", int pageindex = 1, int pageSize = 1)
        {
            var model = _adminRepository.getstorydata(pageindex, pageSize, SearchInputdata);
            return PartialView("_storypage",model);
        }
        public IActionResult Cmspage(string SearchInputdata = "", int pageindex = 1, int pageSize = 4)
        {
            var model = _adminRepository.getcmspagedata(pageindex, pageSize, SearchInputdata);
            return PartialView("_cmspage",model);
        }
        public IActionResult MissionApplication(string SearchInputdata = "", int pageindex = 1, int pageSize = 4)
        {
            var model = _adminRepository.getmissionapplicationdata(pageindex, pageSize, SearchInputdata);
            return PartialView("_missionapplicationpage", model);
        }
        public IActionResult Banner(string SearchInputdata = "", int pageindex = 1)
       {
            var model = _adminRepository.getbannerdata(pageindex, SearchInputdata);
            return PartialView("_bannerpage", model);
        }
        public IActionResult ApproveApplication(string Applicationid)
        {
            _adminRepository.approveapplication(Applicationid);
            return RedirectToAction("MissionApplication", new { SearchInputdata = "", pageindex = 1, pageSize = 4 });
        }
        public IActionResult ApproveStory(string storyid)
        {
            _adminRepository.approvestory(storyid);
            return RedirectToAction("Story", new { SearchInputdata = "", pageindex = 1, pageSize = 1 });

        }
        public IActionResult DeclineStory(string storyid)
        {
            _adminRepository.declinestory(storyid);
            return RedirectToAction("Story", new { SearchInputdata = "", pageindex = 1, pageSize = 1 });

        }
        public IActionResult DeclineApplication(string Applicationid)
        {
            _adminRepository.declineapplication(Applicationid);
            return RedirectToAction("MissionApplication", new { SearchInputdata = "", pageindex = 1, pageSize = 4 });
        }
        public JsonResult Country()
        {
            var countries = _adminRepository.getcountries();
            return Json(countries);
        }
        public JsonResult City(string countryid)
        {
            var cities = _adminRepository.getcities(countryid);
            return Json(cities);
        }
        public JsonResult GetThemes()
        {
            var themes = _adminRepository.getthemes();
            return Json(themes);
        }
        public IActionResult missionadd()
        {
            var model =_adminRepository.getmissionmodeldata();
            return PartialView("_missionedit",model);
        }
        public IActionResult useradd()
        {  
            return PartialView("_useradd");
        }
        public IActionResult themeadd()
        {
            return PartialView("_themeadd");
        }
        public IActionResult skilladd()
        {   
            return PartialView("_skilladd");
        }
        public IActionResult editskilldata(string skillid)
        {
            var model = _adminRepository.getskill(skillid);
            return PartialView("_skilladd", model);
        }
        public IActionResult banneradd()
        {
            return PartialView("_banneradd");
        }
        public IActionResult AddMission(MissionAddViewModel model,List<int> selectedSkills)
        {   
            if(model.MissionId==0)
            {
                _adminRepository.Addmission(model, selectedSkills);
                TempData["success"] = "Mission is added successfully";
            }
            else
            {
                _adminRepository.Editmission(model, selectedSkills);
                TempData["success"] = "Mission is edited successfully";
            }
           
            return RedirectToAction("Mission", new { SearchInputdata = "", pageindex = 1, pageSize = 10 });
        }
        public IActionResult AddBanner(BannerAddViewModel model)
        {
            if (model.BannerId == 0)
            {
                _adminRepository.addBanner(model);
                TempData["success"] = "Banner added successfully";
            }
            else
            {
                _adminRepository.editBanner(model);
                TempData["success"] = "Banner edited successfully";
            }
            return RedirectToAction("Banner");
        }
        public IActionResult AddUser(UserAddViewModel model)
        {
            if (model.UserId == 0)
            {
                _adminRepository.Adduser(model);
                TempData["success"] = "User is added succesfully";

                return RedirectToAction("User", new { SearchInputdata = "", pageindex = 1, pageSize = 10 });
            }
            else
            {
                _adminRepository.updateuser(model);
                TempData["success"] = "User is updated succesfully";
                return RedirectToAction("User", new { SearchInputdata = "", pageindex = 1, pageSize = 10 });
            }

        }
        public IActionResult AddTheme(ThemeAddViewModel model)
        {
            _adminRepository.Addtheme(model);
            return RedirectToAction("Theme", new { SearchInputdata = "", pageindex = 1, pageSize = 2 });
        }
        public IActionResult AddSkill(SkillAddViewModel model)
        {   
            if(model.SkillId==0)
            {
                _adminRepository.Addskill(model);
                TempData["success"] = "skill is added";
            }
            else
            {
                _adminRepository.editskilldatabase(model);
                TempData["success"] = "skill is updated";
            }
            return RedirectToAction("Skill", new { SearchInputdata = "", pageindex = 1, pageSize = 2 });

        }
        public IActionResult EditBanner(string id)
        {
            var model = _adminRepository.getBanner(id)
;
            return PartialView("_banneradd", model);
        }
        public IActionResult edituser(string id)
        {
            var usermodel = _adminRepository.edituserdata(id);
            return PartialView("_useradd", usermodel);
        }
        public IActionResult editmission(string id)
        {
            var model = _adminRepository.editmissondata(id);
            return PartialView("_missionedit", model);
        }
        public IActionResult DeleteMission(string missionid)
        {
            _adminRepository.deletemission(missionid);
            return RedirectToAction("Mission", new { SearchInputdata = "", pageindex = 1, pageSize = 10 });
            
        }
        public bool DeleteTheme(string themeid)
        {
           var delete= _adminRepository.deletetheme(themeid);
            return delete;
        }
        public bool DeleteSkill(string skillId)
        {
            var delete=_adminRepository.deleteskill(skillId);
            return delete;
        }
    }
}
