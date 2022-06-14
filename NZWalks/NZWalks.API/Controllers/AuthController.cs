using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;
        private readonly IMapper mapper;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
            this.mapper = mapper;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            //Validate the incoming request

            // Check if user is authenticated
            // Check username and password
            var user = await userRepository.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

            var userDTO = mapper.Map<Models.DTO.LoginRequest>(user);
            if (userDTO != null)
            {
                // Generated a Jwt Token
                var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);
            }

            return BadRequest("Username or Password is incorrect.");
        }
    }
}
