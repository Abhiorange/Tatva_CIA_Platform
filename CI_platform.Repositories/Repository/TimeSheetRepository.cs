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
            var missions = _ciplatformcontext.MissionApplications.Include(m => m.Mission).Include(m => m.User).Where(m => m.User.UserId == userid && m.Mission.MissionType == "time" && m.ApprovalStatus=="APPROVE").Select(u => u.MissionId);
                return _ciplatformcontext.Missions.Where(u => missions.Contains(u.MissionId)).ToList();
        }

        public List<Mission> missionsbygoal(long userid)
        {
            var timesheet = _ciplatformcontext.Timesheets.Select(t => t.MissionId).ToList();
            var missions = _ciplatformcontext.MissionApplications.Include(m => m.Mission).Include(m => m.User).Where(m => m.User.UserId == userid && m.Mission.MissionType == "goal"&&  m.ApprovalStatus == "APPROVE").Select(u => u.MissionId);
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
        public int getgoalvalue(string missionid)
        {
            var goaltimesheets = _ciplatformcontext.Timesheets.Where(t => t.MissionId.ToString()==missionid).ToList();
            int Totalgoalachieved = 0;
            if (goaltimesheets.Count()!=0)
            {
                 Totalgoalachieved = (int)goaltimesheets.Select(t => t.Action).Sum(); 
            }
           
            var goalvalue = _ciplatformcontext.GoalMissions.Where(g => g.MissionId.ToString() == missionid).Select(g => g.GoalValue).SingleOrDefault();
            var valideaction = goalvalue - Totalgoalachieved;
            return valideaction;
        }
        public (List<SheetViewModel>, List<SheetViewModel>) getdatasheet(long userid)
        {
            var timesheets = _ciplatformcontext.Timesheets.Where(t=>t.UserId==userid).ToList();
            var goaltimesheets = _ciplatformcontext.Timesheets.Where(t => t.Action != 0 && t.UserId == userid).ToList();
            var goalmissions = _ciplatformcontext.GoalMissions.ToList();
            var missions = _ciplatformcontext.Missions.ToList();
            var viewModelListtime = new List<SheetViewModel>();
            var viewModelListgoal = new List<SheetViewModel>();
            foreach (var timesheet in timesheets)
            {   
                var mission = missions.FirstOrDefault(m => m.MissionId == timesheet.MissionId && m.MissionType=="time");
                if (mission!=null)
                {
                    long hours = (long)timesheet.Time.Value.Hours;
                    long minutes = (long)timesheet.Time.Value.Minutes;
                    var viewModel = new SheetViewModel
                    {   timesheetid=timesheet.TimesheetId,
                        missiontitle = mission != null ? mission.Title : null,
                        hour = hours,
                        minute = minutes,
                        DateVolunteered = timesheet.DateVolunteered,
                        Notes=timesheet.Notes,
                    };

                    // Add the view model to the list
                    viewModelListtime.Add(viewModel);
                }
                
            }
            foreach (var timesheet in goaltimesheets)
            {
               
                var mission = missions.FirstOrDefault(m => m.MissionId == timesheet.MissionId && m.MissionType == "goal");
                var goalvalue = _ciplatformcontext.GoalMissions.Where(g => g.MissionId == mission.MissionId).Select(g=>g.GoalValue).SingleOrDefault();
                var total_goal_achieve = _ciplatformcontext.Timesheets.Where(t => t.MissionId == mission.MissionId).GroupBy(t => t.MissionId).Select(g => g.Sum(t => t.Action));
                if (mission!=null)
                {
                    var viewModel1 = new SheetViewModel
                    {
                        missiontitle = mission != null ? mission.Title : null,
                        timesheetid = timesheet.TimesheetId,
                        DateVolunteered = timesheet.DateVolunteered,
                        Action = (int)timesheet.Action,
                        Notes = timesheet.Notes,
                        GoalValue = goalvalue,
                        Totalgoalachieved = total_goal_achieve.FirstOrDefault()
                    };

                    viewModelListgoal.Add(viewModel1);

                }
            }
            return (viewModelListtime, viewModelListgoal);
        }
    }
}
