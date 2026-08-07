using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpDesk.Mvc.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewBag.Priorities = GetPrioritySelectList();
            var ticket = new Ticket
            {
                Status = "Open",
                CreatedDate = DateTime.Now
            };
            return View(ticket);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            // Status must be hardcoded to Open as per requirement
            ticket.Status = "Open";
            if (ticket.CreatedDate == default)
            {
                ticket.CreatedDate = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                var success = await _ticketService.CreateTicketAsync(ticket);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Error creating ticket via API.");
            }

            ViewBag.Priorities = GetPrioritySelectList(ticket.Priority);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.Priorities = GetPrioritySelectList(ticket.Priority);
            ViewBag.Statuses = GetStatusSelectList(ticket.Status);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var success = await _ticketService.UpdateTicketAsync(id, ticket);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Error updating ticket via API.");
            }

            ViewBag.Priorities = GetPrioritySelectList(ticket.Priority);
            ViewBag.Statuses = GetStatusSelectList(ticket.Status);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Filter
        public async Task<IActionResult> Filter(string? status)
        {
            ViewBag.SelectedStatus = status ?? string.Empty;
            ViewBag.Statuses = GetStatusSelectList(status ?? string.Empty);

            List<Ticket> tickets;
            if (!string.IsNullOrWhiteSpace(status))
            {
                tickets = await _ticketService.GetTicketsByStatusAsync(status);
            }
            else
            {
                tickets = await _ticketService.GetAllTicketsAsync();
            }

            return View(tickets);
        }

        private static SelectList GetPrioritySelectList(string selectedValue = "")
        {
            var priorities = new List<string> { "Low", "Medium", "High" };
            return new SelectList(priorities, selectedValue);
        }

        private static SelectList GetStatusSelectList(string selectedValue = "")
        {
            var statuses = new List<string> { "Open", "In Progress", "Closed" };
            return new SelectList(statuses, selectedValue);
        }
    }
}
