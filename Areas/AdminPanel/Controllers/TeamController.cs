using LumiaTemplate.DAL;
using LumiaTemplate.Models;
using LumiaTemplate.Utilities.Extensions;
using LumiaTemplate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Core.Types;

namespace LumiaTemplate.Areas.AdminPanel.Controllers
{
		[Area("AdminPanel")]
	public class TeamController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;

		public TeamController(AppDbContext context,IWebHostEnvironment env)
        {
            _context=context;
			_env=env;
        }
		public async Task<IActionResult> Index(int take = 2,int page = 1)
		{
			List<Team> teams =await _context.Teams.Skip((page-1)*take).Take(take).Include(t=>t.Position).ToListAsync();
			ViewBag.TotalPage = (int)Math.Ceiling((double)_context.Teams.Count() / take);
			ViewBag.CurrentPage = page;
			return View(teams);
		}
		public IActionResult Create()
		{
			ViewBag.Positions=_context.Positions;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateTeamVM createTeamVM)
		{
			if (!ModelState.IsValid) return BadRequest();
			if (!createTeamVM.Photo.CheckFileType(createTeamVM.Photo.ContentType))
			{
				ModelState.AddModelError("Photo", "Faylin formati uygun deyil");
				ViewBag.Positions = _context.Positions;
				return View();
			}
			if (!createTeamVM.Photo.CheckFileSize(200))
			{
				ModelState.AddModelError("Photo", "Faylin hecmi boyukdur");
				ViewBag.Positions = _context.Positions;
				return View();
			}
			bool result= await _context.Positions.AnyAsync(p=>p.Id==createTeamVM.PositionId);
			if (!result)
			{
				ModelState.AddModelError("PositionId", "Bele id'li position yoxdur");
				ViewBag.Positions = _context.Positions;
				return View();
			}
			Team team = new Team()
			{
				Name= createTeamVM.Name,
				PositionId=createTeamVM.PositionId,
				Description=createTeamVM.Description,
				Image=await createTeamVM.Photo.CreateFileAsync(_env.WebRootPath,"assets/img/team")
			};
			team.Image = await createTeamVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team");
			await _context.AddAsync(team);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Update(int? id)
		{
            if (id == null || id < 1) return BadRequest();
            Team team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();
			UpdateTeamVM updateTeamVM = new UpdateTeamVM()
			{
				Name= team.Name,
				PositionId=team.PositionId,
				Description=team.Description,
				Image=team.Image,
			};
			ViewBag.Positions=_context.Positions;
            return View(updateTeamVM);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int? id,UpdateTeamVM updateTeamVM)
		{
            if (id == null || id < 1) return BadRequest();
            Team team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();
            bool result = await _context.Positions.AnyAsync(p => p.Id == updateTeamVM.PositionId);
            if (!result)
            {
                ModelState.AddModelError("PositionId", "Bele id'li position yoxdur");
                ViewBag.Positions = _context.Positions;
                return View();
            }
			if (updateTeamVM==null)
			{
                if (!updateTeamVM.Photo.CheckFileType(updateTeamVM.Photo.ContentType))
                {
                    ModelState.AddModelError("Photo", "Faylin formati uygun deyil");
                    ViewBag.Positions = _context.Positions;
					updateTeamVM.Image=team.Image;
                    return View(updateTeamVM);
                }
                if (!updateTeamVM.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Faylin hecmi boyukdur");
                    ViewBag.Positions = _context.Positions;
					updateTeamVM.Image= team.Image;
                    return View(updateTeamVM);
                }
				team.Image.DeleteFile(_env.WebRootPath, "assets/img/team");
				team.Image = await updateTeamVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team");
            }
			team.Name= updateTeamVM.Name;
			team.PositionId= updateTeamVM.PositionId;
			team.Description= updateTeamVM.Description;
			await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int? id)
		{
			if (id == null||id<1) return BadRequest();
			Team team =await _context.Teams.FirstOrDefaultAsync(t=>t.Id==id);
			if (team == null) return NotFound();
			team.Image.DeleteFile(_env.WebRootPath, "assets/img/team");
			_context.Teams.Remove(team);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");

        }

    }
}
