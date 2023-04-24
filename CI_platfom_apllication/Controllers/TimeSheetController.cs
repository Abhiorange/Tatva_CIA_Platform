using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_platfom_apllication.Controllers
{
    public class TimeSheetController : Controller
    {

        private readonly ILogger<TimeSheetController> _logger;
        private readonly ITimeSheetRepository _timesheetRepository;

        public TimeSheetController(ILogger<TimeSheetController> logger, ITimeSheetRepository timesheetRepository)
        {
            _logger = logger;
            _timesheetRepository = timesheetRepository;

        }
        public IActionResult volunteersheet()
        {
            
            var (entity1,entity2) = _timesheetRepository.getdatasheet();
            var model1 = new SheetViewModel();
            var tuple = new Tuple<SheetViewModel, List<SheetViewModel>, List<SheetViewModel>>(model1, entity1, entity2);
            return View(tuple);

        }

        public JsonResult getmissionsbytime()
        {
            var user_id = long.Parse(HttpContext.Session.GetString("userid"));
            List<Mission> entity = _timesheetRepository.missionsbytime(user_id);
            return Json(entity);
        }

        public JsonResult getmissionsbygoal()
        {
            var user_id = long.Parse(HttpContext.Session.GetString("userid"));
            List<Mission> entity = _timesheetRepository.missionsbygoal(user_id);
            return Json(entity);
        }
        [HttpPost]
        public IActionResult sheetdatabase([Bind(Prefix = "Item1")] SheetViewModel model)
        {     
            var user_id = long.Parse(HttpContext.Session.GetString("userid"));
            _timesheetRepository.sheetdatabase(model, user_id);

            return RedirectToAction("volunteersheet","TimeSheet");
        }
        public IActionResult edittime(long timesheetid,[Bind(Prefix = "Item1")] SheetViewModel model)
        {
            _timesheetRepository.editimedatabase(model,timesheetid);
            return RedirectToAction("volunteersheet", "TimeSheet");
        }
        public IActionResult editgoal([Bind(Prefix = "Item1")] SheetViewModel model)
        {
            _timesheetRepository.editgoaldatabase(model);
            return RedirectToAction("volunteersheet", "TimeSheet");
        }
        public IActionResult deletedatabase(long timesheetid)
        {
            _timesheetRepository.deletedatabase(timesheetid);
            return RedirectToAction("volunteersheet", "TimeSheet");
        }
        public IActionResult deletedatabasegoal(long timesheetid)
        {
            _timesheetRepository.deletedatabasegoal(timesheetid);
            return RedirectToAction("volunteersheet", "TimeSheet");
        }
    }
}
