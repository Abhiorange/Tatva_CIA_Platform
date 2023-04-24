using CI_platfom_apllication.Models;
using CI_platform.Entities.ViewModels;
using CI_platform.Entities.DataModels;
using CI_platform.Repositories.Interface;
using CI_platform.Repositories.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_platfom_apllication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
       

        public HomeController(ILogger<HomeController> logger,IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        /*for login page functionality*/
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }
        
        [HttpPost]
          public IActionResult Login(LoginViewModel user)
        {
            if(ModelState.IsValid)
            {
                var userlogin_detail = _userRepository.login(user);

                if (userlogin_detail == "user does not exist")
                {
                    ModelState.AddModelError("Email", userlogin_detail);
                    return View("Index");

                }
                if (userlogin_detail == "password is not correct")
                {
                    ModelState.AddModelError("Password", userlogin_detail);
                    return View("Index");

                }
                var user_detail = userlogin_detail.Split(',');
                HttpContext.Session.SetString("firstname", user_detail[0]);
                HttpContext.Session.SetString("userid", user_detail[1]);
                HttpContext.Session.SetString("avtar", user_detail[2]);
                TempData["success"] = "login is succesful.";
                return RedirectToAction("platformlanding","Mission");
            }

            return View("Index");
            
        }
        /*for registration functionality*/
        public IActionResult register()
        {

            return View();
        }
        [HttpPost]
        public IActionResult registration(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                var entity = _userRepository.register(register);
                if (entity == "user already exist")
                {
                    ModelState.AddModelError("Email", entity);
                    return View("register");
                }
                TempData["register"] = "registartion is done succesfully";
                return RedirectToAction("Index");
            }
            return View("register");
            
        }

        [HttpGet]
        public IActionResult forgetpassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult forgetpassword(ForgetViewModel forget)

        {
            if (ModelState.IsValid) {
                string url = Url.Action("reset", "home", new { email = forget.Email, token = "{token}" }, Request.Scheme);
                var user_token = _userRepository.forget(forget, url);
                if (user_token == "user does not exist")
                {
                    ModelState.AddModelError("Email", user_token);
                    return View("forgetpassword");
                }
                HttpContext.Session.SetString("Token", user_token);
            }
            TempData["register"] = "email sent succesfully";
            return RedirectToAction("forgetpassword", "home");
        }

        [HttpGet]   
        public IActionResult reset(string email,string token )
        {  
          /*  if(HttpContext.Session.GetString("Token")==null)
            {
                return NotFound("Link Expired");
            }*/
                  
            return View();
        }
        [HttpPost]
        public IActionResult reset(ResetViewModel reset)
        {
            if (ModelState.IsValid)
            {
               /* if (reset.ConfirmPassword == null || reset.Password == null)
                {
                    ModelState.AddModelError("ConfirmPassword", "enter password");
                    return RedirectToAction("reset", "home");
                }*/
                var token = HttpContext.Session.GetString("Token");
                var user = _userRepository.reset(reset, token);
                if (user == null)
                {
                    ModelState.AddModelError("ConfirmPassword", "Password does not match");
                    return RedirectToAction("reset", "home");
                }
                HttpContext.Session.Remove(token);
            }
            TempData["register"] = "assword is reset succesfully";
            return RedirectToAction("Index", "home");
           
        }

      
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult usereditdetail()
        {
            var user_id = long.Parse(HttpContext.Session.GetString("userid"));
            var entity = _userRepository.getdatauser(user_id);
            return View(entity);
            
        }

        public JsonResult Country()
        {
            var country = _userRepository.GetCountries();
            return Json(country);
        }
      
       public void AddSkills(List<int> skillids)
        {
            var user_id = long.Parse(HttpContext.Session.GetString("userid"));
           _userRepository.GetSkills(skillids,user_id);
            
        }
        public JsonResult City(long id)
        {
            var city = _userRepository.Getcity(id);
            return Json(city);
        }
        public IActionResult usereditdatabase(UserDetailViewModel model)
        {
            var user_id = long.Parse(HttpContext.Session.GetString("userid"));
            _userRepository.usereditdatabase(model,user_id);
            TempData["success"] = "User edited succefully";
            return RedirectToAction("platformlanding", "Mission");
        }
        public IActionResult changepassword(UserDetailViewModel model)
        {
            var user_id = long.Parse(HttpContext.Session.GetString("userid"));
            var check=_userRepository.changepass(model,user_id);
            if(check)
            {
                TempData["success"] = "password is updated";
                return RedirectToAction("usereditdetail", "home");
            }
            else
            {
                TempData["error"] = "old password is incorrect";
                return RedirectToAction("usereditdetail", "home");

            }

        }
        public IActionResult privacypolicy()
        {
            return View();
        }

        [HttpPost]
        public string editcontact(string subject,string message)
        {
            var user_id = long.Parse(HttpContext.Session.GetString("userid"));
           _userRepository.editcontact(subject,message, user_id);
            return "success";
           
        }

        public IActionResult AddImage(IFormFile Image)
        {
            
            var user_id = long.Parse(HttpContext.Session.GetString("userid"));
            var image=_userRepository.editimage(Image, user_id);
            HttpContext.Session.SetString("avtar", image);

            return Json(new { redirectUrl = Url.Action("usereditdetail", "Home") });

        }
        public IActionResult Addcontact(string Userid)
        {
           var contact= _userRepository.addcontact(Userid);
            return PartialView("_modalcontactus",contact);
        }


    }
}