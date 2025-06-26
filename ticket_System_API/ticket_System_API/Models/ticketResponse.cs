namespace ticket_System_API.Models
{
    public class ticketResponse
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string ResponseText { get; set; }
        public string AgentId { get; set; }
        public ApplicationUser Agent { get; set; }
        public DateTime RespondedAt { get; set; } = DateTime.UtcNow;
    }
}
