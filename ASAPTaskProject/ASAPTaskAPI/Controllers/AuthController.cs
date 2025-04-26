using ASAPTaskAPI.Application.Dto;
using ASAPTaskAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASAPTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAppService _userService;

        public AuthController(IUserAppService userService) => _userService = userService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var token =await  _userService.Authenticate(dto);
                return Ok(new { token });
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto dto)
        {
           await _userService.Register(dto);
            return Ok();
        }
    }

}
