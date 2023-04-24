using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Entities.ViewModels
{
    public class star_rating
    {
        public long UserId { get; set; }

        public long MissionId { get; set; }

        public int Rating { get; set; }

    }
}
