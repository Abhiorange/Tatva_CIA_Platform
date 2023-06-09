﻿using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CI_platfom_apllication.Controllers
{
    [Authorize(Roles = "Admin")]
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
            var userid = HttpContext.Session.GetString("userid");
            _adminRepository.approveapplication(Applicationid,userid);
            return RedirectToAction("MissionApplication", new { SearchInputdata = "", pageindex = 1, pageSize = 4 });
        }
        public IActionResult ApproveStory(string storyid)
        {
            var userid = HttpContext.Session.GetString("userid");
            _adminRepository.approvestory(storyid,userid);
            return RedirectToAction("Story", new { SearchInputdata = "", pageindex = 1, pageSize = 1 });

        }
        public IActionResult DeclineStory(string storyid)
        {
            var userid = HttpContext.Session.GetString("userid");
            _adminRepository.declinestory(storyid,userid);
            return RedirectToAction("Story", new { SearchInputdata = "", pageindex = 1, pageSize = 1 });

        }
        public IActionResult DeclineApplication(string Applicationid)
        {
            var userid = HttpContext.Session.GetString("userid");
            _adminRepository.declineapplication(Applicationid,userid);
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
        public IActionResult cmsadd()
        {
            return PartialView("_cmsadd");
        }
        public IActionResult editskilldata(string skillid)
        {
            var model = _adminRepository.getskill(skillid);
            return PartialView("_skilladd", model);
        }
        public IActionResult editthemedata(string themeid)
        {
            var model = _adminRepository.gettheme(themeid);
            return PartialView("_themeadd", model);
        }
        public IActionResult editcmsdata(string cmsid)
        {
            var model = _adminRepository.getcmsdata(cmsid);
            return PartialView("_cmsadd", model);
        }
        public IActionResult banneradd()
        {
            return PartialView("_banneradd");
        }
        public IActionResult AddMission(MissionAddViewModel model,List<int> selectedSkills)
        {   
            if(model.MissionId==0)
            {
                var userid = HttpContext.Session.GetString("userid");
                _adminRepository.Addmission(model, selectedSkills,userid);
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
            if (model.MissionThemeId == 0)
            {
                var check = _adminRepository.Addtheme(model);
                if (check)
                {
                    TempData["success"] = "Theme is added";
                }
                else
                {
                    ModelState.AddModelError("Title", "ThemeTitle already exist");
                    return PartialView("_themeadd");
                }
            }
            
            else
            {
                _adminRepository.editthemedatabase(model);
                TempData["success"] = "Theme is updated";

            }
            return RedirectToAction("Theme", new { SearchInputdata = "", pageindex = 1, pageSize = 2 });

        }
        public IActionResult AddSkill(SkillAddViewModel model)
        {   
            if(model.SkillId==0)
            {
              var check= _adminRepository.Addskill(model);
                if(check)
                {
                    TempData["success"] = "skill is added";
                }
                else
                {
                    ModelState.AddModelError("SkillName", "Skillname already exist");
                    return PartialView("_skilladd");
                }
            }
            else
            {
                var check=_adminRepository.editskilldatabase(model);
                if(check)
                {
                    TempData["success"] = "skill is edited";

                }
                else
                {
                        ModelState.AddModelError("SkillName", "Skillname already exist");
                    return PartialView("_skilladd");
                }
            }
            return RedirectToAction("Skill", new { SearchInputdata = "", pageindex = 1, pageSize = 2 });

        }
        public IActionResult AddCms(CmsAddViewModel model)
        {
            if(model.CmsPageId==0)
            {
                _adminRepository.Addcms(model);
                TempData["success"] = "CMS page is added";

            }
            else
            {
                _adminRepository.editcmspage(model);
                TempData["success"] = "CMS page is edited";

            }
            return RedirectToAction("Cmspage", new { SearchInputdata = "", pageindex = 1});
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
        public IActionResult DeleteCMSPage(string cmspageId)
        {
            _adminRepository.deletecmspage(cmspageId);
            return RedirectToAction("Cmspage", new { SearchInputdata = "", pageindex = 1});
        }
        public IActionResult DeleteUser(string userid)
        {
            _adminRepository.deleteuser(userid);
            return RedirectToAction("User", new { SearchInputdata = "", pageindex = 1, pageSize = 10 });
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
        public IActionResult DeleteStory(string storyid)
        {
           _adminRepository.deletestory(storyid);
            return RedirectToAction("Story", new { SearchInputdata = "", pageindex = 1, pageSize = 1 });

        }
    }
}
