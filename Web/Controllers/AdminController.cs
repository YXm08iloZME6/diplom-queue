using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Queue.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IWindowService _windowService;

        public AdminController(IAdminService adminService, IWindowService windowService)
        {
            _adminService = adminService;
            _windowService = windowService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            var users = await _adminService.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> AddUserAsync()
        {
            var model = new AddUserViewModel
            {
                AvailableWindows = await _windowService.GetAllWindows()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserViewModel model, List<string> roleNames)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    if (errors.Any())
                    {
                        Console.WriteLine($"Key: {key}");
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"  Error: {error.ErrorMessage}");
                            Console.WriteLine($"  Exception: {error.Exception?.Message}");
                        }
                    }
                }

                model.AvailableWindows = await _windowService.GetAllWindows();
                return View(model);
            }

            var result = await _adminService.AddUser(model.UserData, roleNames);

            return RedirectToAction(nameof(UserList));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(Guid id)
        {
            var user = await _adminService.GetUserById(id);

            if (user == null)
                return NotFound();

            var dto = new EditUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                MiddleName = user.MiddleName,
                Email = user.Email,
                WindowId = user.WindowId,
                Roles = user.Roles
            };

            var model = new EditUserViewModel
            {
                UserData = dto,
                Roles = user.Roles.Select(r => r.ToLower()).ToList(),
                AvailableWindows = await _windowService.GetAllWindows()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableWindows = await _windowService.GetAllWindows();
                return View(model);
            }
                

            await _adminService.EditUser(model.UserData, model.Roles);

            return RedirectToAction(nameof(UserList));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            await _adminService.RemoveUser(id);
            return RedirectToAction(nameof(UserList));
        }
    }
}