namespace ticket_System_API.Models
{
    public class ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Open"; // Open, In Progress, Closed
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ✅ Add this navigation property
        public List<ticketResponse> Responses { get; set; } = new List<ticketResponse>();
    }
}
