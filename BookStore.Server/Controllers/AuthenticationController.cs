using BookStore.Server.Models;
using BookStore.Server.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.Server.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Payload");

                var (status, message) = await _authService.Login(model);

                if (status == 0)
                    return BadRequest(message);

                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }


        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Payload");

                var (status, message) = await _authService.Registration(model, UserRoles.Admin);

                if (status == 0)
                    return BadRequest(message);

                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
    }
}

