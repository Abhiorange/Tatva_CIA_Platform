/*using CI_platform.Entities.DataModels;*/
using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Azure.Core;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace CI_platform.Repositories.Repository
{
    public class UserRepository : IUserRepository
    {   
        
        private readonly CiPlatformContext _ciplatformcontext;
        private readonly IWebHostEnvironment _hostEnvironment;

        /*public UserRepository() { }*/
        public UserRepository(CiPlatformContext ciplatformcontext, IWebHostEnvironment hostEnvironment)
        {
            _ciplatformcontext = ciplatformcontext;
            _hostEnvironment = hostEnvironment;

        }
        public string login(LoginViewModel user)
        {
            var email = _ciplatformcontext.Users.FirstOrDefault(c => c.Email.Equals(user.Email.ToLower()));
            
          
            if (email==null)
            {
                return "user does not exist";
            }
          
            var password_user = _ciplatformcontext.Users.FirstOrDefault(c => c.Password.Equals(user.Password) &&
            c.Email.Equals(user.Email.ToLower()));
            if(password_user.Avatar==null)
            {
                password_user.Avatar = @"\Images\f38f7d36-e789-477f-939b-2760507ce69d.png";
            }
            if (password_user!=null)
            {  
                var passwordsMatch = string.Compare(user.Password, password_user.Password, StringComparison.Ordinal) == 0;
                if(passwordsMatch == false)
                {
                    return "password is not correct";
                }
            }   
            if (password_user==null)
            {
                return "password is not correct";
            }
            return password_user.FirstName + "," + password_user.UserId + "," + password_user.Avatar;

        }

        public string register(RegisterViewModel user)
        {
            var email = _ciplatformcontext.Users.FirstOrDefault(c => c.Email.Equals(user.Email.ToLower()));
            if(email!=null)
            {
                return "user already exist";
            }
           
                var entry = _ciplatformcontext.Users.Add(
                    new User()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = user.Password,
                        PhoneNumber = user.PhoneNumber,
                        CityId = user.CityId,
                        CountryId = user.CountryId,
                    }
                        );
                _ciplatformcontext.SaveChanges();
                return "succesfully registered";
            
        }
        public string forget(ForgetViewModel forget,string url)
        {
            var user= _ciplatformcontext.Users.FirstOrDefault(c => c.Email.Equals(forget.Email.ToLower()));
            if(user != null)
            {
                var token = Guid.NewGuid().ToString();

                var passwordReset = new PasswordReset
                {
                    Email = forget.Email,
                    Token = token,
                };
                

                _ciplatformcontext.Add(passwordReset);
                _ciplatformcontext.SaveChanges();


                var resetLink = url.Replace("{token}", token);

                var from = new MailAddress("soniabhi0510@gmail.com", "abhishek");

                var to = new MailAddress(forget.Email);
                var subject = "Password reset request";
                var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("soniabhi0510@gmail.com", "gykrsgeknrwtcbam"),
                    EnableSsl = true
                };
                smtpClient.Send(message);

                return token;
            }
            return "user does not exist";
        }
        public string reset(ResetViewModel reset,string token)
        {   
            if(reset.Password!=reset.ConfirmPassword)
            {
                return "Confirm password is not matching with password";
            }
            var user = _ciplatformcontext.PasswordResets.FirstOrDefault(c => c.Token == token);
            if (user != null)
            {
                var email = _ciplatformcontext.Users.FirstOrDefault(c => c.Email == user.Email);
                email.Password = reset.Password;

                _ciplatformcontext.Users.Update(email);
                _ciplatformcontext.SaveChanges();

                return "password succesfuklly changed";
            }
            return null;
        }
        public List<Country> GetCountries()
        {
            var country = _ciplatformcontext.Countries.ToList();
            return country;
        }
        public void GetSkills(List<int> ids,long userid)
        {   
            
            var check = _ciplatformcontext.UserSkills.Where(s => s.UserId == userid).ToList();
            if(check==null)
            {
                foreach (var id in ids)
                {
                    var model = new UserSkill
                    {
                        UserId = userid,
                        SkillId = id

                    };
                    _ciplatformcontext.Add(model);
                  
                }
            }
            else
            {
                var removeskill = _ciplatformcontext.UserSkills.Where(s => s.UserId == userid);
                _ciplatformcontext.UserSkills.RemoveRange(removeskill);
                foreach (var id in ids)
                {
                    var model = new UserSkill
                    {
                        UserId = userid,
                        SkillId = id

                    };
                    _ciplatformcontext.Add(model);

                }
            }
            _ciplatformcontext.SaveChanges();

        }
       
        public List<City> Getcity(long id)
        {
            var city = _ciplatformcontext.Cities.Where(c => c.CountryId == id).ToList();
            return city;
        }
        public UserDetailViewModel getdatauser(long userid)
        {
            var user = _ciplatformcontext.Users.Include(u=>u.Country).Include(u=>u.City).SingleOrDefault(u => u.UserId == userid);
            if(user.Avatar==null)
            {
                user.Avatar= @"\Images\5dd5fbd9-5976-4a14-8d2c-b05018f7e929.png";
            }
            var userskill = _ciplatformcontext.UserSkills.Where(s => s.UserId == userid).ToList();
            var skills = _ciplatformcontext.Skills.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _ciplatformcontext.Countries.ToList();
            foreach (var item in temp)
            {
                list.Add(new SelectListItem() { Text = item.Name, Value = item.CountryId.ToString() });
            }
            List<SelectListItem> list1 = new List<SelectListItem>();
            var temp1 = _ciplatformcontext.Cities.Where(c=>c.CountryId==user.CountryId).ToList();
            foreach (var item in temp1)
            {
                list1.Add(new SelectListItem() { Text = item.Name, Value = item.CityId.ToString() });
            }

            var model=new UserDetailViewModel
            {
                FirstName=user.FirstName,
                LastName=user.LastName,
                EmployeeId=user.EmployeeId,
                Department=user.Department,
                ProfileText=user.ProfileText,
                WhyIVolunteer=user.WhyIVolunteer,
                CityId=user.CityId,
                CountryId=user.CountryId,
                LinkedInUrl=user.LinkedInUrl,
                countries=list,
                cities=list1,
                userSkills=userskill,
                skills=skills,
                Email=user.Email,   
                Avatar=user.Avatar
            };
            return model;
        }
        public void usereditdatabase(UserDetailViewModel model, long user_id)
        {
            var user = _ciplatformcontext.Users.SingleOrDefault(u => u.UserId == user_id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.EmployeeId = model.EmployeeId;
            user.Department = model.Department;
            user.ProfileText = model.ProfileText;
            user.WhyIVolunteer = model.WhyIVolunteer;
            user.CityId = model.CityId;
            user.CountryId = model.CountryId;
            user.LinkedInUrl = model.LinkedInUrl;
            _ciplatformcontext.Update(user);
            _ciplatformcontext.SaveChanges();
        }

        public bool changepass(UserDetailViewModel model,long userid)
        {
                var user = _ciplatformcontext.Users.FirstOrDefault(c => c.UserId==userid);
          
                var passwordsMatch = string.Compare(user.Password, model.oldpass, StringComparison.Ordinal) == 0;
                if (passwordsMatch == true)
                {
                    user.Password = model.newpass;
                    _ciplatformcontext.SaveChanges();
                }
                else
                {
                return false;
                }

            return true;
        }
        public void editcontact(string subject,string message, long userid)
        {
            var contact = _ciplatformcontext.Users.FirstOrDefault(c => c.UserId == userid);
            var model1 = new Contactu
            {
                Userid = userid,
                Subject=subject,
                Message=message

            };
            _ciplatformcontext.Add(model1);
            _ciplatformcontext.SaveChanges();
        }
        public string editimage(IFormFile Image, long userid)
        {
           
            var user = _ciplatformcontext.Users.FirstOrDefault(u => u.UserId == userid);
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string imagesFolderPath = Path.Combine(wwwRootPath, "Images");
            string MainfolderPath = Path.Combine(imagesFolderPath, "UserProfileImages");
            if (!Directory.Exists(MainfolderPath))
            {
                Directory.CreateDirectory(MainfolderPath);
            }
            string fileName = Guid.NewGuid().ToString();

            var uploads = Path.Combine(MainfolderPath,fileName + Path.GetExtension(Image.FileName));
        
            using (var fileStreams = new FileStream(uploads, FileMode.Create))
            {
                Image.CopyTo(fileStreams);
            }
            user.Avatar = @"\Images\UserProfileImages\" + fileName + Path.GetExtension(Image.FileName);
            _ciplatformcontext.SaveChanges();
            return user.Avatar;

        }
        public ContactUsViewModel addcontact(string userid)
        {
            var user = _ciplatformcontext.Users.FirstOrDefault(u => u.UserId.ToString() == userid);
            var model = new ContactUsViewModel
            {
               Email = user.Email,
               FirstName=user.FirstName,
            };
            return model;
        }
        

    }
}
