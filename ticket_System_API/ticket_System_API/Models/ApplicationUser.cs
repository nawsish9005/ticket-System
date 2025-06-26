using Microsoft.AspNetCore.Identity;

namespace ticket_System_API.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
    }
}
