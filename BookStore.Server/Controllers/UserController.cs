using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Server.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.Server.Controllers
{
    [Route("api")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthenticationController> _logger;

        public UserController(IUserService userService, ILogger<AuthenticationController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("userlist")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FetchUsers()
        {
            var userList = await _userService.GetUsers();

            return Ok(userList);
        }
    }
}

