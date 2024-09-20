using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> m_UserManager;
        private readonly TokenService m_tokenService;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            m_UserManager = userManager;
            this.m_tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await m_UserManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await m_UserManager.CheckPasswordAsync(user, loginDto.Password);
            if (result)
            {
                return CreateNewUserDto(user);
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await m_UserManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("Email", "Email is already used");
                return ValidationProblem();
            }


            if (await m_UserManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                ModelState.AddModelError("Username", "Username is already used");
                return ValidationProblem();
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username,
                Bio = ""
            };

            var result = await m_UserManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return CreateNewUserDto(user);
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await m_UserManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateNewUserDto(user);
        }

        private UserDto CreateNewUserDto(AppUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Image = null,
                Token = m_tokenService.CreateToken(user),
                Username = user.UserName,
            };
        }
    }
}