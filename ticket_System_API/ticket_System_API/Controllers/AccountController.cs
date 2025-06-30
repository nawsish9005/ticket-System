using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ticket_System_API.Dtos;
using ticket_System_API.Models;
using ticket_System_API.Service;

namespace ticket_System_API.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<ApplicationUser> userManager, TokenService tokenService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                return BadRequest("User already exists!");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Assign default role
            await _userManager.AddToRoleAsync(user, "User");

            return Ok(new { message = "User registered successfully." });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            return Ok(new
            {
                token,
                expiration = DateTime.UtcNow.AddHours(3),
                userName = user.UserName,
                roles
            });
        }

        // POST: api/role/create
        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Role name cannot be empty.");

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return BadRequest("Role already exists.");

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
                return Ok(new { message = "Role created successfully." });

            return BadRequest(result.Errors);
        }

        // POST: api/role/assign
        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound("User not found.");

            var roleExists = await _roleManager.RoleExistsAsync(dto.Role);
            if (!roleExists)
                return NotFound("Role not found.");

            var result = await _userManager.AddToRoleAsync(user, dto.Role);

            if (result.Succeeded)
                return Ok(new { message = $"Role '{dto.Role}' assigned to user '{dto.Email}' successfully." });

            return BadRequest(result.Errors);
        }
    }
}
