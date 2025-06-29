namespace ticket_System_API.Dtos
{
    /// ✅ Purpose:Returned when fetching ticket(s), includes ticket details and all replies.
    public class TicketResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TicketReplyDto> Replies { get; set; }
    }
}
