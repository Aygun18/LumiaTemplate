using LumiaTemplate.DAL;
using LumiaTemplate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LumiaTemplate
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<AppDbContext>(option =>
			{
				option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});
			builder.Services.AddIdentity<AppUser,IdentityRole>(option =>
			{
				option.Password.RequiredLength = 8;
				option.Password.RequireNonAlphanumeric = true;
				option.Password.RequireDigit = true;
				option.Password.RequireUppercase = true;
				option.Password.RequireLowercase = true;
				option.Password.RequiredUniqueChars = 1;

				option.User.RequireUniqueEmail = true;

				option.Lockout.AllowedForNewUsers= true;
				option.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(1);
				option.Lockout.MaxFailedAccessAttempts = 3;
			}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
			var app = builder.Build();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseRouting();
			app.UseStaticFiles();

			app.MapControllerRoute(name: "Default", pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");
			app.MapControllerRoute(name: "Default", pattern: "{controller=home}/{action=index}/{id?}");
			

			app.Run();
		}
	}
}