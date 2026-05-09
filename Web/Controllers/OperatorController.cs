using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = "operator")]
    public class OperatorController : Controller
    {
        private readonly IOperatorService _operatorService;

        public OperatorController(IOperatorService operatorService)
        {
            _operatorService = operatorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var model = await _operatorService.GetDashboardData(userId);
            
            return View(model);

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
            //var serviceId = Guid.Parse("9d78a673-efa3-4af3-9828-55515d26e134"); // временно жестко задаем сервис для перенаправления, в будущем нужно будет выбирать из списка
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
