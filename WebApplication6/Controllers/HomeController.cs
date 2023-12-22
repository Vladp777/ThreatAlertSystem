using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThreadAlert.Entities;
using ThreadAlert.Repositories;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Authorize(Roles = "Member")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DangerousObjectsRepository _dangerousObjectRepository;

        public HomeController(UserManager<AppUser> userManager, DangerousObjectsRepository dangerousObjectRepository)
        {
            _userManager = userManager;
            _dangerousObjectRepository = dangerousObjectRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.Users
                                .Include(u => u.Messages)
                                .ThenInclude(a => a.DangerousObject)
                                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            var messages = user.Messages.ToList();

            return View(messages);
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Settings()
        {
            var userId = _userManager.GetUserId(User);

            var user = await _userManager.Users
                                .Include(u => u.DangerousObjects)
                                .FirstAsync(u => u.Id == userId);
            
            var userDO = user.DangerousObjects;

            var allDO = await _dangerousObjectRepository.GetAll();

            var doflags = new List<DOFlag>();

            foreach (var dObj in allDO)
            {
                doflags.Add(new DOFlag 
                { 
                    DangerousObject = dObj,
                    IsPresent = userDO.Contains(dObj)
                });
            }

            return View(doflags);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(IFormCollection collection)
        {
            var userId = _userManager.GetUserId(User);

            var user = await _userManager.Users
                                .Include(u => u.DangerousObjects)
                                .FirstAsync(u => u.Id == userId);

            var newdo = new List<DangerousObject>();
            for (var i = 0; i < collection["item.DangerousObject.Id"].Count; i++)
            {
                var a = collection[$"[{i}].IsPresent"].ToString();
                if (a == null)
                {
                    continue;
                }
                var b = Int32.Parse(collection["item.DangerousObject.Id"][i]);
                if (a == "true")
                {
                    var dob = await _dangerousObjectRepository.Get(b);
                    if (dob != null)
                    {
                        newdo.Add(dob);
                    }
                }
            }

            user.DangerousObjects = newdo;

            await _userManager.UpdateAsync(user);

            var allDO = await _dangerousObjectRepository.GetAll();

            var doflags = new List<DOFlag>();

            foreach (var dObj in allDO)
            {
                doflags.Add(new DOFlag
                {
                    DangerousObject = dObj,
                    IsPresent = newdo.Contains(dObj)
                });
            }

            TempData["SuccessMessage"] = "Changes have been saved successfully.";

            return View(doflags);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}