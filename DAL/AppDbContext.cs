using System.Net;
using LumiaTemplate.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LumiaTemplate.DAL
{
	public class AppDbContext:IdentityDbContext<AppUser>
	{
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Position> Positions { get; set; }
    }
}
