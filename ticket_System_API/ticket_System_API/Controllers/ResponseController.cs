using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ticket_System_API.Data;
using ticket_System_API.Dtos;
using ticket_System_API.Models;

namespace ticket_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResponseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Agent Reply to Ticket
        [HttpPost]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> AddResponse([FromBody] TicketReplyDto dto)
        {
            var ticket = await _context.Tickets.FindAsync(dto.TicketId);
            if (ticket == null)
                return NotFound();

            var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = new ticketResponse
            {
                TicketId = dto.TicketId,
                ResponseText = dto.ResponseText,
                AgentId = agentId,
                RespondedAt = DateTime.UtcNow
            };

            _context.TicketResponses.Add(response);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Response added successfully." });
        }
    }
}
