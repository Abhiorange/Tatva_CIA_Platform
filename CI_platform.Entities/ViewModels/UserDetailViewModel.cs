using CI_platform.Entities.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Entities.ViewModels
{
    public class UserDetailViewModel
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string? FirstName { get; set; }
        public string? Avatar { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "EmployeeId is required")]
        public string? EmployeeId { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please Provide Valid Email")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Status is required")]
        public int Status { get; set; }
        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
        [Required(ErrorMessage = "Text is required")]
        public string? WhyIVolunteer { get; set; }
        public string? LinkedInUrl { get; set; }
        public List<SelectListItem> countries { get; set; }
        public List<SelectListItem> cities { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public string? Department { get; set; }
        [Required(ErrorMessage = "City is required")]

        public long CityId { get; set; }
        [Required(ErrorMessage = "Profile Text is required")]
        public string? ProfileText { get; set; }
        [Required(ErrorMessage = "Country is required")]

        public long CountryId { get; set; }

        [Required(ErrorMessage = "Enter Old Password")]
        public string oldpass { get; set; }

        [Required(ErrorMessage = "Enter New Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must contain atleast 1 lowercase,1 uppercase, 1 digit,1 special character and must be of 8 characters")]
        public string newpass { get; set; }

        [Required(ErrorMessage = "Enter ConfirmPassword")]
        [Compare("newpass", ErrorMessage = "Confirm Password is not match with Password")]
        public string confirmpass { get; set; }
        public List<UserSkill> userSkills { get; set; }
        public List<Skill> skills { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }
    }
}
