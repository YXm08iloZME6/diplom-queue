using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Services;
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
        private readonly IServiceService _serviceService;

        public AdminController(IAdminService adminService, IWindowService windowService, IServiceService serviceService)
        {
            _adminService = adminService;
            _windowService = windowService;
            _serviceService = serviceService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ServiceListAsync()
        {
            var services = await _serviceService.GetMainServicesAsync();
            return View(services);
        }

        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddService(CreateServiceDto service)
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
                return View(service); 
            }

            await _serviceService.AddServiceAsync(service);
            return View(service);
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

        [HttpGet]
        public async Task<IActionResult> WindowsList()
        {
            var windows = await _windowService.GetAllWindows();
            return View(windows);
        }

        [HttpGet]
        public async Task<IActionResult> CreateWindow()
        {
            var model = new CreateWindowViewModel
            {
                Services = await _serviceService.GetMainServicesAsync(),
            };
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWindow(CreateWindowViewModel model)
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

                model.Services = await _serviceService.GetMainServicesAsync();
                return View(model);
            }

            await _windowService.CreateWindowAsync(model.Window);
            return RedirectToAction(nameof(WindowsList));
        }
    }
}