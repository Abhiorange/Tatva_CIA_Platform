using CI_platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Entities.ViewModels
{
    public class SheetViewModel
    {
        public long UserId { get; set; }
        public long timesheetid { get; set; }
        public long MissionId { get; set; }
        public string missiontitle { get; set; }
        [RegularExpression("^([0-2][0-4]|[0-9])$", ErrorMessage = "Please Enter proper hours")]
        public long hour { get; set; } = new long();
        [RegularExpression("^([0-5][0-9]|[0-9])$", ErrorMessage = "Please Enter proper minutes")]
        public long minute { get; set; } = new long();
        public TimeSpan Time { get; set; }
        [Required(ErrorMessage = "enter notes")]
        public string? Notes { get; set; }
        [Required(ErrorMessage = "enter valid Action")]
        public int? Action { get; set; }
        [Required(ErrorMessage = "enter valid date")]
        public DateTime? DateVolunteered { get; set; }
        public int GoalValue { get; set; }
        public int? Totalgoalachieved { get; set; }

    }
}
