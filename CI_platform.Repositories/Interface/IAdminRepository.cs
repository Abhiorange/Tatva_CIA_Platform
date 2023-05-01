﻿using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_platform.Repositories.Interface
{
    public interface IAdminRepository
    {
        public UserAdminViewModel getuserdata(int pageindex, int pageSize, string SearchInputdata);
        public UserAdminViewModel getmissiondata(int pageindex, int pageSize, string SearchInputdata);
        public UserAdminViewModel getbannerdata(int pageindex, string SearchInputdata);
        public void addBanner(BannerAddViewModel model);
        public void editBanner(BannerAddViewModel model);
        public BannerAddViewModel getBanner(string bannerid);
        public MissionAddViewModel editmissondata(string missonid);
        public void Editmission(MissionAddViewModel model, List<int> selectedSkills);
        public UserAdminViewModel getcmspagedata(int pageindex, int pageSize, string SearchInputdata);
        public List<Country> getcountries();
        public List<City> getcities(string countryid);
        public List<MissionTheme> getthemes();
        public void deletemission(string missionid);
        public void Addmission(MissionAddViewModel model, List<int> selectedSkills);
        public SkillAddViewModel getskill(string skillid);

        public void Addskill(SkillAddViewModel model);
        public UserAdminViewModel getthemedata(int pageindex, int pageSize, string SearchInputdata);
        public void Addtheme(ThemeAddViewModel model);
        public bool deletetheme(string themeid);
        public UserAdminViewModel getskilldata(int pageindex, int pageSize, string SearchInputdata);
        public UserAdminViewModel getmissionapplicationdata(int pageindex, int pageSize, string SearchInputdata);
        public void approveapplication(string applicationid);
        public void declineapplication(string applicationid);
        public UserAdminViewModel getstorydata(int pageindex, int pageSize, string SearchInputdata);
        public void approvestory(string storyid);
        public void declinestory(string storyid);
        public MissionAddViewModel getmissionmodeldata();
        public void Adduser(UserAddViewModel model);
        public void updateuser(UserAddViewModel model);



        public UserAddViewModel edituserdata(string userid);

        public void editskilldatabase(SkillAddViewModel model);
        public bool deleteskill(string skillid);



    }
}
