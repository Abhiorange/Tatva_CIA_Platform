using CI_platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Entities.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "enter firstname")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "enter lastname")]
        public string? LastName { get; set; }
       
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please Provide Valid Email")]
        public string Email { get; set; } = null!;
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must contain atleast 1 lowercase,1 uppercase, 1 digit,1 special character and must be of 8 characters")]
        public string Password { get; set; } = null!;
        [Compare("Password", ErrorMessage = "Confirm Password is not match with Password")]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Invalid Mobile no.")]
        public long? PhoneNumber { get; set; }

        public long CityId = 1;

        public long CountryId = 1;
       public  List<Banner>? Banners { get; set; }


    }
}
