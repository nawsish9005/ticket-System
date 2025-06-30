using System.ComponentModel.DataAnnotations;

namespace ticket_System_API.Dtos
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
