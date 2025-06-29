using ticket_System_API.Models;

namespace ticket_System_API.Dtos
{
/// ✅ Purpose:Used by Support Agents to send replies to tickets and also for displaying replies in the ticket view.
    public class TicketReplyDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string ResponseText { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public DateTime RespondedAt { get; set; }
    }
}
