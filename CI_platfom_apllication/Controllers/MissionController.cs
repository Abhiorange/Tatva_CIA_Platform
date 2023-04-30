using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CI_platfom_apllication.Controllers
{
    public class MissionController : Controller
    {
        private readonly ILogger<MissionController> _logger;
        private readonly IMissionRepository _missionRepository;
        

        public MissionController(ILogger<MissionController> logger, IMissionRepository missionRepository)
        {
            _logger = logger;
            _missionRepository = missionRepository;
           
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult platformlanding(List<long> skillids, List<long> themeids, List<long> cityids, List<long> countryids, int id=0,int pageindex=1,int pageSize=9,string? SearchInputdata = "") 
       {
            /* var firstname_session = HttpContext.Session.GetString("firstname");
             if(firstname_session == null)
             {
                 return RedirectToAction("Index", "home");
             }*/
            var user_id = HttpContext.Session.GetString("userid");
            var entity = _missionRepository.getmiscoucity(pageindex,pageSize,id,SearchInputdata,countryids,cityids,themeids,skillids,user_id);
            entity.currentPage = pageindex;
            return View(entity);
        }
       
        public IActionResult VolunteerMission(int id,int pageindex=1,int pagesize=1)
        {
            var user_id = HttpContext.Session.GetString("userid");
            var entity = _missionRepository.getvolunteermission(id,pageindex,pagesize,user_id);
            return View(entity);
        }
        public JsonResult Country()
        {
            var country = _missionRepository.GetCountries();
            return Json(country);
        }

        public JsonResult getusers()
        {
            var users = _missionRepository.GetUsers();
            return Json(users);
        }

        public JsonResult City(int[] ids)
        {
            var cities = new List<City>();

            foreach (var id in ids)
            {                    
                var countryCities = _missionRepository.GetCities(id);
                cities.AddRange(countryCities);
            }

            return Json(cities);
        }

        public IActionResult usersthrouid(int[] ids,int missionid,int from_id)
        { 
            foreach (var id in ids)
            {
                string url = Url.Action("VolunteerMission", "Mission", new { id =missionid}, Request.Scheme);
                var users_ids = _missionRepository.GetUsers_id(id,url,missionid,from_id);             
            }
            

            return RedirectToAction("volunteermission", new { id = missionid });
        }
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("platformlanding", "Mission");
        }
        public JsonResult Theme()
        {
            var themes = _missionRepository.GetThemes();
            return Json(themes);
        }

        public JsonResult Skill()
        {
            var skills = _missionRepository.GetSkills();
            return Json(skills);
        }
        public IActionResult updaterating(int missionid, int rating, int userid)
        {
            var success_rate = _missionRepository.updateandaddrate(missionid, rating, userid);
           return  RedirectToAction("volunteermission", new { id = missionid });
        }

        public IActionResult favroite_mission(int missionid, int userid,string actionmethod)
        {
            var fav_mission = _missionRepository.fav_mission(missionid, userid);
            if(actionmethod== "VolunteerMission")
            {
                return RedirectToAction("volunteermission", new { id = missionid });
            }
            else
            {
                return RedirectToAction("platformlanding");
            }
        }
           
        
        public IActionResult DownloadDocument(string fileType,int id)
        {
            var document = _missionRepository.GetByDocumentType(fileType,id);
            if (document == null)
            {
                return NotFound();
            }

            var contentType = "";
            switch (fileType)
            {
                case "doc":
                    contentType = "application/msword";
                    break;
                case "xlsx":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case "pdf":
                    contentType = "application/pdf";
                    break;
                default:
                    return NotFound();
            }

            var stream = new FileStream(document.DocumentPath, FileMode.Open);
            return new FileStreamResult(stream, contentType);
        }

        public IActionResult comment(int missionid, string userid, string commentsDiscription)
        {
            var comment = _missionRepository.AddComment(missionid, userid, commentsDiscription);
          
            return RedirectToAction("volunteermission", new { id = missionid });
        }

        public IActionResult applymission(int missionid,string userid)
        {
            var apply = _missionRepository.apply_mission(missionid, userid);
            return RedirectToAction("volunteermission", new { id = missionid });
        }
    }
}
