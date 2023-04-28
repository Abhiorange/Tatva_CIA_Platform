using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Entities.ViewModels
{
    public class SkillAddViewModel
    {
        [Required(ErrorMessage = "Skill Name is Required...")]
        public string? SkillName { get; set; }
        public int SkillId { get; set; }
        [Required(ErrorMessage = "Skill Name is Required...")]

        public byte Status { get; set; }
    }
}
