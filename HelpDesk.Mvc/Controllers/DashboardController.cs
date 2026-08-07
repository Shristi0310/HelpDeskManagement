using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Mvc.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ITicketService _ticketService;

        public DashboardController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            var model = new DashboardViewModel
            {
                TotalTickets = tickets.Count,
                OpenTickets = tickets.Count(t => string.Equals(t.Status, "Open", StringComparison.OrdinalIgnoreCase)),
                InProgressTickets = tickets.Count(t => string.Equals(t.Status, "In Progress", StringComparison.OrdinalIgnoreCase)),
                ClosedTickets = tickets.Count(t => string.Equals(t.Status, "Closed", StringComparison.OrdinalIgnoreCase))
            };
            return View(model);
        }
    }
}
