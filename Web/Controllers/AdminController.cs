using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Queue.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(CreateUserDto model, List<string> roleNames)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _adminService.AddUser(model, roleNames);

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
                ServiceId = user.ServiceId,
                Roles = user.Roles
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _adminService.EditUser(model, model.Roles);

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