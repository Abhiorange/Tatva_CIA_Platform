using CI_platfom_apllication.Models;
using CI_platform.Entities.ViewModels;
using CI_platform.Entities.DataModels;
using CI_platform.Repositories.Interface;
using CI_platform.Repositories.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Banners = _userRepository.GetBanners(),
            };
            return View(loginViewModel);
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
                    var model = new LoginViewModel();
                    model.Banners = _userRepository.GetBanners();
                    return View("Index",model);

                }
                if (userlogin_detail == "password is not correct")
                {
                    ModelState.AddModelError("Password", userlogin_detail);
                    var model =new LoginViewModel();
                    model.Banners = _userRepository.GetBanners();
                    return View("Index",model);

                }
                var user_detail = userlogin_detail.Split(',');
                HttpContext.Session.SetString("firstname", user_detail[0]);
                HttpContext.Session.SetString("userid", user_detail[1]);
                HttpContext.Session.SetString("avtar", user_detail[2]);
                HttpContext.Session.SetString("role", user_detail[3]);
                TempData["login"] = "successfully logged in";
                var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, user_detail[3]) },
                            CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                TempData["success"] = "login is succesful.";
                return RedirectToAction("platformlanding","Mission");
            }

            return View("Index");
            
        }
        /*for registration functionality*/
        [HttpGet]
        public IActionResult register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                Banners = _userRepository.GetBanners(),
            };
            return View(registerViewModel);
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
                    var model = new RegisterViewModel();
                    model.Banners = _userRepository.GetBanners();
                    return View("register",model);
                }
                TempData["register"] = "registartion is done succesfully";
                return RedirectToAction("Index");
            }
            return View("register");
            
        }

        [HttpGet]
        public IActionResult forgetpassword()
        {
            ForgetViewModel forgetViewModel = new ForgetViewModel()
            {
                Banners = _userRepository.GetBanners(),
            };
            return View(forgetViewModel);
        }
        [HttpPost]
        public IActionResult forgetpassword(ForgetViewModel forget)

        {
            if (ModelState.IsValid) {
                string url = Url.Action("reset", "home", new { email = forget.Email, token = "{token}" }, Request.Scheme);
/*                string url = Url.Action("reset", "home",null, Request.Scheme);
*/                var user_token = _userRepository.forget(forget, url);
                if (user_token == "user does not exist")
                {
                    ModelState.AddModelError("Email", user_token);
                    var model = new ForgetViewModel();
                    model.Banners = _userRepository.GetBanners();
                    return View("forgetpassword",model);
                }
                HttpContext.Session.SetString("Token", user_token);
            }
            TempData["register"] = "email sent succesfully";
            return RedirectToAction("forgetpassword", "home");
        }

        [HttpGet]   
        public IActionResult reset(string email,string token)
        {
            if (HttpContext.Session.GetString("Token") == null)
            {
                return NotFound("Link Expired");
            }
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Banners = _userRepository.GetBanners(),
            };
            return View(loginViewModel);
        }
        [HttpPost]
        public IActionResult reset(ResetViewModel reset)
        {
            if (ModelState.IsValid)
            {
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