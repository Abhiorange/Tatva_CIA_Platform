using System;
using System.Collections.Generic;
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
        public long hour { get; set; } = new long();
        public long minute { get; set; } = new long();
        public TimeSpan Time { get; set; }
        public string? Notes { get; set; }
        public int Action { get; set; } = new int();

        public DateTime? DateVolunteered { get; set; }

      



       

    }
}
