using CI_platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Entities.ViewModels
{
    public class ResetViewModel
    {   [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must contain atleast 1 lowercase,1 uppercase, 1 digit,1 special character and must be of 8 characters")]
        public string Password { get; set; } = null!;
        [Required]
        [Compare("Password", ErrorMessage = "Confirm Password is not match with Password")]
        public string ConfirmPassword { get; set; } = null!;
        List<Banner>? Banners { get; set; }
    }
}
