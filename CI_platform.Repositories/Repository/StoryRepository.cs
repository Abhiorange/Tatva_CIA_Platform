﻿using CI_platform.Entities.DataModels;
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
    public class StoryRepository : IStoryRepository
    {
        private readonly CiPlatformContext _ciplatformcontext;

        public StoryRepository(CiPlatformContext ciplatformcontext)
        {
            _ciplatformcontext = ciplatformcontext;
        }

        public StoryViewModel getstories(string keyword, int pageindex = 1, int pageSize = 3)
        {
            var missionIds = _ciplatformcontext.Missions.Where(m => m.Theme.Title.Contains(keyword)).Select(m => m.MissionId);
            var stories = _ciplatformcontext.Stories.Include(s => s.Mission).Include(s => s.User).Where(s=>s.Status=="PUBLISHED" && (keyword== null || missionIds.Contains(s.MissionId) || s.Title.Contains(keyword))).ToList();
            var Missionthemes = _ciplatformcontext.Missions.Include(m => m.Theme).ToList();
            var storymedia = _ciplatformcontext.StoryMedia.ToList();
            var model = new StoryViewModel
            {
               StoryMedia=storymedia,
                Missions = Missionthemes
            };
            model.Stories = stories.ToPagedList(pageindex, 3);
            return model;
        }

        public StoryDatabaseViewModel addstorydetail(long user_id,string missionid)
        {  
            if(missionid==null)
            {
                return new StoryDatabaseViewModel();
            }
            else
            {
                var story = _ciplatformcontext.Stories.FirstOrDefault(u => u.UserId == user_id && u.MissionId == long.Parse(missionid));
                if (story != null)
                {
                    var storyMedia = _ciplatformcontext.StoryMedia.Where(u => u.StoryId == story.StoryId);
                    var images = storyMedia.Where(m => m.Type == "Image").Select(s => s.Path).ToList();
                    var video = storyMedia.SingleOrDefault(m => m.Type == "video");
                    var missionTitle = _ciplatformcontext.Missions.SingleOrDefault(m => m.MissionId == story.MissionId);
                    StoryDatabaseViewModel model = new StoryDatabaseViewModel()
                    {
                        Missiontitle = missionTitle.Title,
                        storyid=story.StoryId,
                        missionid = story.MissionId,
                        title = story.Title,
                        description = story.Description,
                        videourl = video.Path,
                        images = images,
                        PublishedAt=story.PublishedAt,                    
                    };
                    if(story.Status=="DRAFT")
                    {
                        model.aboutstatus = "D";
                        model.status = "DRAFT";
                    }
                    else if(story.Status== "PUBLISHED")
                    {
                        model.aboutstatus = "P";
                        model.status = "PUBLISHED";

                    }
                    else if(story.Status=="DECLINED")
                    {
                        model.aboutstatus = "P";
                        model.status = "DECLINED";

                    }
                    else if (story.Status == "PENDING")
                    {
                        model.aboutstatus = "P";
                        model.status = "PENDING";

                    }
                    return model;
                }
                var missionTitle1 = _ciplatformcontext.Missions.SingleOrDefault(m => m.MissionId == long.Parse(missionid));
                var story1 = _ciplatformcontext.Stories.FirstOrDefault(u => u.UserId == user_id && u.MissionId == long.Parse(missionid));
                if(story1==null)
                {
                    StoryDatabaseViewModel model1 = new StoryDatabaseViewModel()
                    {
                        Missiontitle = missionTitle1.Title,
                        missionid = long.Parse(missionid),
                        aboutstatus="N",
                    };
                    return model1;
                }
               
                
            }
            return new StoryDatabaseViewModel();
        }
        public void submit(long storyId)
        {
            var story = _ciplatformcontext.Stories.SingleOrDefault(m => m.StoryId == storyId);
            story.Status = "PENDING";
            _ciplatformcontext.Update(story);
            _ciplatformcontext.SaveChanges();
        }
        public StoryDetailViewModel getstorydetail(int story_id)
        {
            var stories = _ciplatformcontext.Stories.Include(s => s.Mission).Include(s => s.User).SingleOrDefault(s=>s.StoryId==story_id);
            var storyMedia = _ciplatformcontext.StoryMedia.Where(u => u.StoryId == story_id);
            var images = storyMedia.Where(m => m.Type == "Image").Select(s => s.Path).ToList();
            var video = storyMedia.SingleOrDefault(m => m.Type == "video");
            
            var model = new StoryDetailViewModel
            {
                Stories = stories,
                images = images,
                Views = stories.Views+1,
            };
            stories.Views = model.Views;
            _ciplatformcontext.Update(stories);
            _ciplatformcontext.SaveChanges();
            return model;
        }

        public string storydatabase(long missionid,string title,string  description, string status, string[] images,long userid,DateTime date)
        {
            var story = _ciplatformcontext.Stories.FirstOrDefault(u => u.UserId == userid  && u.MissionId == missionid);
            if(story==null)
            {
                var model = new Story
                {
                    MissionId = missionid,
                    Title = title,
                    Description = description,
                    Status = status,
                    UserId = userid,
                    PublishedAt=date
                };
                _ciplatformcontext.Add(model);
            }
            else
            {
                story.Title = title;
                story.Description = description;
                story.Status = "DRAFT";
                story.PublishedAt = date;
            }
           
            _ciplatformcontext.SaveChanges();
           
            return "success";
        }
        public string editstorydatabase(long missionid, string title, string description, string status,  long userid,DateTime date)
        {
            var story = _ciplatformcontext.Stories.FirstOrDefault(u => u.UserId == userid  && u.MissionId == missionid);
            story.Title = title;
            story.Description = description;
            story.Status = status;
            story.PublishedAt = date;
            _ciplatformcontext.Update(story);
            _ciplatformcontext.SaveChanges();

            return "success";
        }
        public string storymedia(long missionid,long user_id,string[] images,string video)
        {
            var story = _ciplatformcontext.Stories.FirstOrDefault(s => s.MissionId == missionid && s.UserId == user_id);
            var storyMediaToRemove = _ciplatformcontext.StoryMedia.Where(sm => sm.StoryId == story.StoryId);
            _ciplatformcontext.StoryMedia.RemoveRange(storyMediaToRemove);
            _ciplatformcontext.SaveChanges();
            foreach (var image in images)
            {
                var model1 = new StoryMedium
                {
                    StoryId = story.StoryId,
                    Type = "image",
                    Path = image

                };
                _ciplatformcontext.Add(model1);
            }
              
                var model2 = new StoryMedium
                {
                    StoryId = story.StoryId,
                    Type = "video",
                    Path = video
                };
            _ciplatformcontext.Add(model2);
            _ciplatformcontext.SaveChanges();
            
            return "sucess";
        }
        public string editstorymedia(long missionid, long user_id, string[] images, string video)
        {
            var story = _ciplatformcontext.Stories.FirstOrDefault(s => s.MissionId == missionid && s.UserId == user_id);
            // Remove all the StoryMedia entities with the given StoryId
            var storyMediaToRemove = _ciplatformcontext.StoryMedia.Where(sm => sm.StoryId == story.StoryId);
            _ciplatformcontext.StoryMedia.RemoveRange(storyMediaToRemove);
            _ciplatformcontext.SaveChanges();

            foreach (var image in images)
            {
                var model1 = new StoryMedium
                {
                    StoryId = story.StoryId,
                    Type = "image",
                    Path = image

                };
                _ciplatformcontext.Add(model1);
            }

            var model2 = new StoryMedium
            {
                StoryId = story.StoryId,
                Type = "video",
                Path = video
            };
            _ciplatformcontext.Add(model2);
            _ciplatformcontext.SaveChanges();
            return "sucess";
        }



        public List<Mission> missions(long userid)
        {
            var missionApplication =_ciplatformcontext.MissionApplications.Where(u => u.UserId == userid).Select(u => u.MissionId);
            return _ciplatformcontext.Missions.Where(u => missionApplication.Contains(u.MissionId)).OrderBy(m => m.Title).ToList();
        }
        public List<User> getusersdata()
        {
            var users = _ciplatformcontext.Users.ToList();
            return users;
        }
        public string GetUsers_id(string url, int id, int storyid, string from_user)
        {
            var user = _ciplatformcontext.Users.SingleOrDefault(m => m.UserId == id);
            var resetLink = url;

            var from = new MailAddress("orangetreeplant123@gmail.com", "Abhishek");

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
                Credentials = new NetworkCredential("orangetreeplant123@gmail.com", "jbadfitwuhphgwbw"),
                EnableSsl = true
            };
            smtpClient.Send(message);
            var story_invite = new StoryInvite
            {
                FromUserId = long.Parse(from_user),
                StoryId = storyid,
                ToUserId = id,
            };
            _ciplatformcontext.Add(story_invite);
            _ciplatformcontext.SaveChanges();
            return "sucess";
        }
    }
}

 
