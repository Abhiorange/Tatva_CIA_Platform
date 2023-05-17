using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Entities.ViewModels
{
    public class ThemeAddViewModel
    {
        [Required(ErrorMessage = "theme Name is Required...")]

        public string? Title { get; set; }
        public long MissionThemeId { get; set; }
        [Required(ErrorMessage = "Status is Required...")]

        public byte Status { get; set; }
    }
}
