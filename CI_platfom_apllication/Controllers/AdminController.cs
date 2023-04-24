using CI_platform.Entities.DataModels;
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
        public IActionResult MissionApplication(string SearchInputdata = "", int pageindex = 1, int pageSize = 4)
        {
            var model = _adminRepository.getmissionapplicationdata(pageindex, pageSize, SearchInputdata);
            return PartialView("_missionapplicationpage", model);
        }
        public IActionResult ApproveApplication(string Applicationid)
        {
            _adminRepository.approveapplication(Applicationid);
            return RedirectToAction("MissionApplication", new { SearchInputdata = "", pageindex = 1, pageSize = 4 });

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
            return PartialView("_missionedit");
        }
        public IActionResult themeadd()
        {
            return PartialView("_themeadd");
        }
        public IActionResult skilladd()
        {
            return PartialView("_skilladd");
        }
        public IActionResult AddMission(MissionAddViewModel model)
        {
            _adminRepository.Addmission(model);
            return RedirectToAction("Mission", new { SearchInputdata = "", pageindex = 1, pageSize = 10 });
        }
        public IActionResult AddTheme(ThemeAddViewModel model)
        {
            _adminRepository.Addtheme(model);
            return RedirectToAction("Theme", new { SearchInputdata = "", pageindex = 1, pageSize = 2 });
        }
        public IActionResult AddSkill(SkillAddViewModel model)
        {
            _adminRepository.Addskill(model);
            return RedirectToAction("Skill", new { SearchInputdata = "", pageindex = 1, pageSize = 2 });

        }
        public IActionResult DeleteMission(string missionid)
        {
            _adminRepository.deletemission(missionid);
            return RedirectToAction("Mission", new { SearchInputdata = "", pageindex = 1, pageSize = 10 });
            
        }
        public IActionResult DeleteTheme(string themeid)
        {
            _adminRepository.deletetheme(themeid);
            return RedirectToAction("Theme", new { SearchInputdata = "", pageindex = 1, pageSize = 2 });
        }
    }
}
