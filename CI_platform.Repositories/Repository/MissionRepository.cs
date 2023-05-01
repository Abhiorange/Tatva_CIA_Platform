using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CI_platform.Repositories.Repository
{
    public class MissionRepository: IMissionRepository
    {
        private readonly CiPlatformContext _ciplatformcontext;

        public MissionRepository(CiPlatformContext ciplatformcontext)

        {
            _ciplatformcontext = ciplatformcontext;
        }

        public  MisCouCity getmiscoucity(int pageindex, int pageSize,int id, string keyword, List<long> countryids, List<long> cityids, List<long> themeids, List<long> skillids,string user_id)
        {
            var missionQuery = _ciplatformcontext.Missions.Include(m => m.City).Include(m=>m.Country).Include(m=>m.MissionMedia).Include(m => m.Theme).Where(m=>m.Status==1).AsQueryable();
            var skillfilter = _ciplatformcontext.MissionSkills.Where(s => skillids.Contains(s.SkillId)).Select(s => s.MissionId);
            var missions = string.IsNullOrEmpty(keyword)
                ? missionQuery.Where(m => (m.MissionType == "time" || m.MissionType == "goal") && (m.Status==1))
                : missionQuery.Where(model => (model.Title.Contains(keyword) || model.Theme.Title.Contains(keyword) || model.City.Name.Contains(keyword)) && (model.Status==1)).AsQueryable();
            var missions1 = missions.Where(model => ((countryids.Contains(model.CountryId)) || countryids.Count() == 0) && ((cityids.Contains(model.CityId)) || cityids.Count() == 0) && ((skillfilter.Contains(model.MissionId)) || skillfilter.Count() == 0) && ((themeids.Contains(model.ThemeId)) || themeids.Count() == 0));
            var mission_skill = _ciplatformcontext.MissionSkills.Include(m => m.Mission).Include(m => m.Skill).ToList();
            var goal = _ciplatformcontext.GoalMissions.Include(g => g.Mission).ToList();
            var mission_rating = _ciplatformcontext.MissionRatings.Include(m => m.Mission).Include(m => m.User).ToList();
            var missionapplication = _ciplatformcontext.MissionApplications.ToList();
            var fav_mission = _ciplatformcontext.FavouriteMissions.Where(f=>f.UserId.ToString()==user_id).ToList();
            var Timesheets = _ciplatformcontext.Timesheets.ToList();
            var model = new MisCouCity 
            {
                MissionsSkill = mission_skill,
                GoalMissions=goal,
                totalrecord = missions1.Count(),
                Missionrating=mission_rating,
                favouriteMissions = fav_mission,
                MissionApplications=missionapplication,
                timesheets=Timesheets,
            };
            
            var sortmission = missions1.ToList();
            if (id == 1)
            {
                sortmission = sortmission.OrderByDescending(p => p.CreatedAt).ToList();
            }
            else if (id == 2)
            {
                sortmission = sortmission.OrderBy(p => p.CreatedAt).ToList();
            }
            else if (id == 3)
            {
                sortmission = sortmission.OrderBy(p => p.TotalSeats).ToList();
            }
            else if (id == 4)
            {
                sortmission = sortmission.OrderByDescending(p => p.TotalSeats).ToList();
            }
            else if (id == 5)
            {
                sortmission = sortmission.OrderByDescending(p => fav_mission.Any(f => f.MissionId == p.MissionId)).ToList();
            }

            model.Missions = sortmission.ToPagedList(pageindex, 9);
            return model;
        }
        public string updateandaddrate(int missionid, int rating, int userid)
        {
            var mission_rating = _ciplatformcontext.MissionRatings.Include(m => m.Mission).Include(m => m.User).ToList();
            var rate_update = mission_rating.SingleOrDefault(mr => mr.User.UserId == userid && mr.Mission.MissionId == missionid);

            
            if (rate_update != null)
            {
                var missionrating = new MissionRating
                {
                    MissionId = missionid,
                    UserId = rate_update.UserId,
                    Rating = rating,
                    MissionRatingId = rate_update.MissionRatingId
                };
                rate_update.Rating = rating;
                _ciplatformcontext.Update(rate_update);
          
                _ciplatformcontext.SaveChanges();

            }
            if (rate_update == null)
            {
              
                var missionrating = new MissionRating
                {
                    MissionId = missionid,
                    UserId = userid,
                    Rating = rating,
                    
                };

                _ciplatformcontext.Add(missionrating);
                _ciplatformcontext.SaveChanges();

            }
            return "successfull";
        }
        public string fav_mission(int missionid,int userid)
        {
           
            var user_check = _ciplatformcontext.FavouriteMissions
        .SingleOrDefault(u => u.UserId == userid && u.MissionId == missionid);

            if (user_check == null)
            {
                var favmission = new FavouriteMission
                {
                    UserId = userid,
                    MissionId = missionid
                };
                _ciplatformcontext.Add(favmission);
                _ciplatformcontext.SaveChanges();
                return "successfully added mission";
            }
            else
            {
                _ciplatformcontext.Remove(user_check);
                _ciplatformcontext.SaveChanges();
                return "already added favorite";
            }
        }
        public List<Country> GetCountries()
        {
            var country = _ciplatformcontext.Countries.ToList();
            return country;
        }

        public List<User> GetUsers()
        {
            var users = _ciplatformcontext.Users.ToList();
            return users;
        }
       
        public List<City> GetCities(int id)
        {
            var city = _ciplatformcontext.Cities.Where(m => m.CountryId == id).ToList();
            return city;
        }

        public string GetUsers_id(int id, string url,int missionid,int from_id)
        {   
            var user = _ciplatformcontext.Users.SingleOrDefault(m => m.UserId == id); //User who is recieving mail
            var resetLink = url;

            var from = new MailAddress("dummyblack92@gmail.com", "abhishek");

            var to = new MailAddress(user.Email);
            var subject = "Volunteer mission recommend";
            var body = $"Hi,<br /><br />Please click on the following to apply on mission:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true  
            };
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("dummyblack92@gmail.com", "bhilykvfemjbcceg"),
                EnableSsl = true
            };
            smtpClient.Send(message);
            var missioninvite = new MissionInvite
            {
                MissionId = missionid,
                FromUserId = from_id,
                ToUserId = id
            };
            _ciplatformcontext.Add(missioninvite);
            _ciplatformcontext.SaveChanges();
            return "email is send succesfully";
        }

        public List<MissionTheme> GetThemes()
        {
            var themes = _ciplatformcontext.MissionThemes.ToList();
            return themes;
        }

        public List<Skill> GetSkills()
        {
            var skills = _ciplatformcontext.Skills.ToList();
            return skills;
        }

        public VolunteerViewModel getvolunteermission(int id,int pageindex,int pagesize,string userid)
        {
            var missions = _ciplatformcontext.Missions
                 .Include(m => m.City)
                 .Include(m=>m.Theme)
                 .Include(m=>m.Country)
                 .FirstOrDefault(c => c.MissionId == id);
                                                       
            var goal = _ciplatformcontext.GoalMissions.FirstOrDefault(c => c.MissionId == id);
            var related_mission = _ciplatformcontext.Missions
                .Include(m => m.City)
                .Include(m => m.Theme)
                .Include(m=>m.Country).Where(rm => rm.MissionId != missions.MissionId && (rm.City.Name == missions.City.Name || rm.Theme.Title == missions.Theme.Title || rm.Country.Name == missions.Country.Name)).ToList();

            var mission_rating = _ciplatformcontext.MissionRatings.Include(m => m.Mission).Include(m => m.User).ToList();
            var comments = _ciplatformcontext.Comments.Include(c => c.Mission).Where(c => c.MissionId == missions.MissionId).OrderByDescending(c => c.CreatedAt).ToList();
            var missionapplication = _ciplatformcontext.MissionApplications.Include(m=>m.User).Where(m => m.MissionId == id).ToList();
            var recentvolunteer = _ciplatformcontext.MissionApplications.Include(m => m.User).Where(m => m.MissionId == id && m.ApprovalStatus== "APPROVE").Select(m => m.User).ToList();
            bool check_apply = _ciplatformcontext.MissionApplications.Any(m => m.MissionId == id && m.UserId.ToString() == userid && m.ApprovalStatus == "APPROVE");
            var missiondocument =_ciplatformcontext.MissionDocuments.Where(d => d.MissionId == id).ToList();
            bool checkClosedMission = _ciplatformcontext.Missions.Any(m => m.MissionId == id && m.RegistrationDeadline <= DateTime.Now);
            var timesheet_records = _ciplatformcontext.Timesheets.Where(t => t.MissionId == id).ToList();
            var goalvalue = _ciplatformcontext.GoalMissions.Where(g => g.MissionId == id).Select(g => g.GoalValue).SingleOrDefault(); 
            if (related_mission.Any(rm => rm.City.Name == missions.City.Name))
            {
                related_mission = related_mission.Where(rm => rm.City.Name == missions.City.Name).ToList();
            }
            else if (related_mission.Any(rm => rm.Country.Name == missions.Country.Name))
            {
                related_mission = related_mission.Where(rm => rm.Country.Name == missions.Country.Name).ToList();
            }
            else if (related_mission.Any(rm => rm.Theme.Title == missions.Theme.Title))
            {
                related_mission = related_mission.Where(rm => rm.Theme.Title == missions.Theme.Title).ToList();
            }
            else
            {
                related_mission = new List<Mission>();
            }
            var related_goal = _ciplatformcontext.GoalMissions.Include(g => g.Mission).ToList();

            var mission_skill = _ciplatformcontext.MissionSkills.Include(m => m.Skill).Where(c => c.MissionId == id).ToList();

            var fav_mission = _ciplatformcontext.FavouriteMissions.Where(m => m.MissionId == id).ToList();
            
            var model = new VolunteerViewModel
            {
                Missions = missions,
                GoalObjectiveText = null,
                GoalValue=goalvalue,
                timesheets=timesheet_records,
                MissionsSkill =mission_skill,
                Related_Mission=related_mission,
                Related_goal=related_goal,
                Missionrating=mission_rating,
                missionid=id,
                favouriteMissions=fav_mission,
                comment=comments,
               MissionApplications=missionapplication,
                MissionDocuments=missiondocument,
                checkApply=check_apply,
                checkClosed=checkClosedMission
               
            };
            model.recentvolunteers = recentvolunteer.ToPagedList(pageindex, 1);
            if (missions.MissionType == "goal")
            {
                model.GoalObjectiveText = goal.GoalObjectiveText;
                model.GoalValue = goal.GoalValue;
            }
           
            return model;
        }
        public MissionDocument GetByDocumentType(string documentType,int id)
        {
            return _ciplatformcontext.MissionDocuments.FirstOrDefault(d => d.DocumentType == documentType && d.MissionId==id);
        }

        public string AddComment(int missionid, string userid, string commentsDiscription)
        {
            var comment = new Comment
            {
                MissionId = missionid,
                UserId = long.Parse(userid),
                CommentDescription = commentsDiscription,

            };
            _ciplatformcontext.Add(comment);
            _ciplatformcontext.SaveChanges();

            return "suceessful";
        }

        public string apply_mission(int missionid,string userid)
        {
            var apply = new MissionApplication
            {
                MissionId = missionid,
                UserId = long.Parse(userid),
                ApprovalStatus = "PENDING",
                AppliedAt = DateTime.Now,
            };
       
            _ciplatformcontext.Add(apply);
            _ciplatformcontext.SaveChanges();
            return "sucess";
        }
    }
}
