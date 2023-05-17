using CI_platform.Entities.DataModels;
using CI_platform.Entities.ViewModels;
using CI_platform.Repositories.Interface;
using CI_platform.Repositories.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CiPlatformContext>();
builder.Services.AddScoped<IMissionRepository, MissionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<ITimeSheetRepository, TimeSheetRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddCloudscribePagination();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
    AddCookie(option =>
    {
        option.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 1);
        option.LoginPath = "/Home/Index";
        option.AccessDeniedPath = "/Mission/platformlanding";
    });
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(60*1);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{                                                                           
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mission}/{action=platformlanding}/{id?}");

app.Run();
