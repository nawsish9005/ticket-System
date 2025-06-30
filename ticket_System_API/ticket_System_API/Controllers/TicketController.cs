using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ticket_System_API.Data;
using ticket_System_API.Dtos;
using ticket_System_API.Models;

namespace CustomerSupportSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Create Ticket (User)
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateTicket([FromBody] TicketCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ticket = new ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = "Open"
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Ticket created successfully.", ticket.Id });
        }

        // Get User's Own Tickets (User)
        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var tickets = await _context.Tickets
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .Include(t => t.Responses)
                    .ThenInclude(r => r.Agent)
                .Select(t => new TicketResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    UserId = t.UserId,
                    UserName = t.User.FullName,
                    CreatedAt = t.CreatedAt,
                    Replies = t.Responses.Select(r => new TicketReplyDto
                    {
                        Id = r.Id,
                        TicketId = r.TicketId,
                        ResponseText = r.ResponseText,
                        AgentId = r.AgentId,
                        AgentName = r.Agent.FullName,
                        RespondedAt = r.RespondedAt
                    }).ToList()
                }).ToListAsync();

            return Ok(tickets);
        }

        // Get All Tickets (Agent)
        [HttpGet]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Responses)
                    .ThenInclude(r => r.Agent)
                .Select(t => new TicketResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    UserId = t.UserId,
                    UserName = t.User.FullName,
                    CreatedAt = t.CreatedAt,
                    Replies = t.Responses.Select(r => new TicketReplyDto
                    {
                        Id = r.Id,
                        TicketId = r.TicketId,
                        ResponseText = r.ResponseText,
                        AgentId = r.AgentId,
                        AgentName = r.Agent.FullName,
                        RespondedAt = r.RespondedAt
                    }).ToList()
                }).ToListAsync();

            return Ok(tickets);
        }

        // Update Ticket Status (Agent)
        [HttpPut("status/{id}")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> UpdateTicketStatus(int id, [FromBody] string status)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            ticket.Status = status;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Ticket status updated successfully." });
        }
    }
}
