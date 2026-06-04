using Application.DTOs;
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
        private readonly ISettingsService _settingsService;

        public AdminController(IAdminService adminService, IWindowService windowService, IServiceService serviceService, ISettingsService settingsService)
        {
            _adminService = adminService;
            _windowService = windowService;
            _serviceService = serviceService;
            _settingsService = settingsService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _adminService.GetDashboardDataAsync();
            return View(model);
        }
        
        public async Task<IActionResult> ServiceList()
        {
            var services = await _serviceService.GetAllServicesAsync();
            return View(services);
        }

        [HttpGet]
        public async Task<IActionResult> AddService()
        {
            var services = await _serviceService.GetMainServicesAsync();
            return View(new CreateServiceViewModel
            {
                AvailableParents = services
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddService(CreateServiceViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                model.AvailableParents = await _serviceService.GetMainServicesAsync();
                return View(model); 
            }

            await _adminService.AddServiceAsync(model.Service);
            return RedirectToAction(nameof(ServiceList));
        }
        [HttpGet]
        public async Task<IActionResult> EditService(Guid id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(UpdateServiceDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                

            await _adminService.UpdateServiceAsync(new UpdateServiceDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IconName = model.IconName,
                Letter = model.Letter
            });

            return RedirectToAction(nameof(ServiceList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            await _adminService.DeleteServiceAsync(id);

            return RedirectToAction(nameof(ServiceList));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleServiceStatus(Guid id)
        {
            await _adminService.ToggleServiceStatus(id);
            return RedirectToAction(nameof(ServiceList));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleServiceFacets(Guid id)
        {
            await _adminService.ToggleServiceFacets(id);
            return RedirectToAction(nameof(ServiceList));
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

        [HttpGet]
        public async Task<IActionResult> EditWindow(Guid id)
        {
            var window = await _windowService.GetWindowById(id);
            var services = await _serviceService.GetAllServicesAsync();

            var model = new EditWindowViewModel
            {
                Window = new UpdateWindowDto
                {
                    Id = window.Id,
                    Number = window.Number,
                    ServiceId = window.ServiceId
                },

                Services = services.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWindow(EditWindowViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Services = (await _serviceService.GetAllServicesAsync()).ToList();
                return View(model);
            }

            await _windowService.UpdateWindowAsync(model.Window);

            return RedirectToAction(nameof(WindowsList));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWindow(Guid id)
        {
            await _windowService.DeleteWindowAsync(id);
            return RedirectToAction(nameof(WindowsList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QueueReset()
        {
            await _adminService.QueueResetAsync();

            TempData["Toast"] = "Очередь успешно сброшена";
            return RedirectToAction(nameof(Index));
            
        }

        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            var model = new StatisticsViewModel();
            model.Services = await _serviceService.GetAllServicesAsync();
            model.StartDate = DateTime.Today;
            model.EndDate = DateTime.Today;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Statistics(StatisticsViewModel model)
        {
            if (model.Today)
            {
                model.StartDate = DateTime.Today;
                model.EndDate = DateTime.Today;
            }
            
            if (model.Yesterday)
            {
                model.StartDate = DateTime.Today.AddDays(-1);
                model.EndDate = model.StartDate.AddDays(1);
            }

            model.Tickets = await _adminService.TicketStats(model.StartDate, model.EndDate, model.Status, model.ServiceId);
            model.Services = await _serviceService.GetAllServicesAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var settings = await _settingsService.GetSettingsAsync();
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSettings(Guid id, string value)
        {
            await _settingsService.UpdateSettingValueAsync(id, value);
            return RedirectToAction(nameof(Settings));
        }
    }
}