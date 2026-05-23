using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Services;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "operator")]
    public class OperatorController : Controller
    {
        private readonly IOperatorService _operatorService;
        private readonly ISettingsService _settingsService;

        public OperatorController(IOperatorService operatorService, ISettingsService settingsService)
        {
            _operatorService = operatorService;
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var dto = await _operatorService.GetDashboardData(userId);
            var simpleMode = await _settingsService.GetSettingByNameAsync("Простой режим");

            if (simpleMode.Value == "true")
            {
                var svm = new OperatorDashboardViewModel
                {
                    dashboard = dto,
                    windowName = "0",
                    serviceName = "Общая очередь"
                };
                
                return View(svm);
            }
            
            var vm = new OperatorDashboardViewModel
            {
                dashboard = dto,
                windowName = dto.Window.Title,
                serviceName = dto.Window.ServiceName,
            };
            
            return View(vm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CallNext()
        {
            var userId = GetUserId();
            await _operatorService.CallNextTicket(userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recall()
        {
            var userId = GetUserId();
            await _operatorService.RecallTicket(userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartProcessing()
        {
            var userId = GetUserId();
            await _operatorService.StartProcessingTicket(userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete()
        {
            var userId = GetUserId();
            await _operatorService.CompleteTicket(userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel()
        {
            var userId = GetUserId();
            await _operatorService.CancelTicket(userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Redirect(Guid serviceId, string comment)
        {
            var userId = GetUserId();

            await _operatorService.RedirectTicket(userId, serviceId, comment);

            return RedirectToAction(nameof(Index));
        }


        private Guid GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(claim);
        }
    }
}
