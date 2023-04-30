using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Repositories.Interface
{
    public interface IStoryRepository
    {
        public StoryViewModel getstories(string keyword, int pageindex = 1, int pageSize = 9);
        public StoryDatabaseViewModel addstorydetail(long user_id,string missionid);
        public StoryDetailViewModel getstorydetail(int story_id);
        public string storydatabase(long missionid, string title, string description, string status, string[] images, long userid, DateTime date);
        public string editstorydatabase(long missionid, string title, string description, string status, long userid, DateTime date);
        public List<Mission> missions(long userid);
        public string storymedia(long missionid, long user_id, string[] images, string video);
        public string editstorymedia(long missionid, long user_id, string[] images, string video);
        public void submit(long storyId);
        public string GetUsers_id(string url, int id, int storyid, string from_user);

        public List<User> getusersdata();
    }
}



























