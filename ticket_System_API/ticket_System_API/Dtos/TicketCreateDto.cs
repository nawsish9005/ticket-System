using ticket_System_API.Models;

namespace ticket_System_API.Dtos
{
   // ✅ Purpose: Used by Users when creating a new support ticket.
    public class TicketCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
