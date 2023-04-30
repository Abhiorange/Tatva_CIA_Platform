using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Repositories.Interface
{
    public  interface ITimeSheetRepository
    {
        public List<Mission> missionsbytime(long userid);
        public List<Mission> missionsbygoal(long userid);
        public void sheetdatabase(SheetViewModel model, long userid);
        public (List<SheetViewModel>, List<SheetViewModel>) getdatasheet();
        public void editimedatabase(SheetViewModel model, long timesheetid);
        public void deletedatabase(long timesheetid);
        public void editgoaldatabase(SheetViewModel model);
        public void deletedatabasegoal(long timesheetid);
        public int getgoalvalue(string missionid);


        /*public List<Timesheet> getdatasheet();*/
    }
}
