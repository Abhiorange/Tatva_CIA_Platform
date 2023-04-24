using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Repositories.Repository
{
    public  class TimeSheetRepository: ITimeSheetRepository
    {
        private readonly CiPlatformContext _ciplatformcontext;

        public TimeSheetRepository(CiPlatformContext ciplatformcontext)
        {
            _ciplatformcontext = ciplatformcontext;
        }

        public List<Mission> missionsbytime(long userid)
        {
            var timesheet = _ciplatformcontext.Timesheets.Select(t => t.MissionId).ToList();
            var missions = _ciplatformcontext.MissionApplications.Include(m => m.Mission).Include(m => m.User).Where(m => m.User.UserId == userid && m.Mission.MissionType == "time").Select(u => u.MissionId);
                return _ciplatformcontext.Missions.Where(u => missions.Contains(u.MissionId)).ToList();
        }

        public List<Mission> missionsbygoal(long userid)
        {
            var timesheet = _ciplatformcontext.Timesheets.Select(t => t.MissionId).ToList();
            var missions = _ciplatformcontext.MissionApplications.Include(m => m.Mission).Include(m => m.User).Where(m => m.User.UserId == userid && m.Mission.MissionType == "goal").Select(u => u.MissionId);
            return _ciplatformcontext.Missions.Where(u => missions.Contains(u.MissionId)).ToList();
        }

        public void  sheetdatabase(SheetViewModel model,long userid)

        {
            var hour = model.hour;
            var minute = model.minute;
            var ticks = (hour * TimeSpan.TicksPerHour) + (minute * TimeSpan.TicksPerMinute);
            var time = new TimeSpan(ticks);
            
           
            var entry = _ciplatformcontext.Timesheets.Add(
                      new Timesheet()
                      {
                          UserId=userid,
                          MissionId=model.MissionId,
                          Time=time,
                          Action=model.Action,
                          DateVolunteered=(DateTime)model.DateVolunteered,
                          Notes=model.Notes,
                          Status="SUBMIT_FOR_APPROVAL"

                      }
                          );
            _ciplatformcontext.SaveChanges();
        }
        public void editimedatabase(SheetViewModel model,long timesheetid)
        {
            var timesheet = _ciplatformcontext.Timesheets.FirstOrDefault(t => t.TimesheetId == timesheetid);
            var hour = model.hour;
            var minute = model.minute;
            var ticks = (hour * TimeSpan.TicksPerHour) + (minute * TimeSpan.TicksPerMinute);
            var time = new TimeSpan(ticks);

            timesheet.Time = time;
            _ciplatformcontext.Update(timesheet);
            _ciplatformcontext.SaveChanges();
        }
        public void editgoaldatabase(SheetViewModel model)
        {
            var timesheet = _ciplatformcontext.Timesheets.FirstOrDefault(t => t.TimesheetId == model.timesheetid);
            timesheet.Action = model.Action;
            timesheet.Notes = model.Notes;
            _ciplatformcontext.SaveChanges();
        }
        public void deletedatabase(long timesheetid)
        {
            var timesheet = _ciplatformcontext.Timesheets.FirstOrDefault(t => t.TimesheetId == timesheetid);
            _ciplatformcontext.Remove(timesheet);
            _ciplatformcontext.SaveChanges();

        }
        public void deletedatabasegoal(long timesheetid)
        {
            var timesheet = _ciplatformcontext.Timesheets.FirstOrDefault(t => t.TimesheetId == timesheetid);
            _ciplatformcontext.Remove(timesheet);
            _ciplatformcontext.SaveChanges();

        }

        public (List<SheetViewModel>, List<SheetViewModel>) getdatasheet()
        {
            var timesheets = _ciplatformcontext.Timesheets.ToList();

            var missions = _ciplatformcontext.Missions.ToList();
            var viewModelListtime = new List<SheetViewModel>();
            var viewModelListgoal = new List<SheetViewModel>();
            foreach (var timesheet in timesheets)
            {
                // Find the corresponding mission for this timesheet
                var mission = missions.FirstOrDefault(m => m.MissionId == timesheet.MissionId && m.MissionType=="time");
                if(mission!=null)
                {
                    long hours = (long)timesheet.Time.Value.Hours;
                    long minutes = (long)timesheet.Time.Value.Minutes;
                    // Create a view model that contains the timesheet and its mission title
                    var viewModel = new SheetViewModel
                    {   timesheetid=timesheet.TimesheetId,
                        missiontitle = mission != null ? mission.Title : null,
                        hour = hours,
                        minute = minutes,
                        DateVolunteered = timesheet.DateVolunteered,
                        Notes=timesheet.Notes
                    };

                    // Add the view model to the list
                    viewModelListtime.Add(viewModel);
                }
                
            }
            foreach (var timesheet in timesheets)
            {
               
                var mission = missions.FirstOrDefault(m => m.MissionId == timesheet.MissionId && m.MissionType == "goal");
               
               if(mission!=null)
                {
                    var viewModel1 = new SheetViewModel
                    {
                        missiontitle = mission != null ? mission.Title : null,
                        timesheetid = timesheet.TimesheetId,
                        DateVolunteered = timesheet.DateVolunteered,
                        Action = (int)timesheet.Action,
                        Notes=timesheet.Notes
                     

                    };

                    // Add the view model to the list
                    viewModelListgoal.Add(viewModel1);

                }
            }
            return (viewModelListtime, viewModelListgoal);
        }
    }
}
