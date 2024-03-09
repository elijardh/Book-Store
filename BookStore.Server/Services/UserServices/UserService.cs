using BookStore.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Server.Services.UserServices
{
	public class UserService : IUserService
	{
        private readonly UserManager<ApplicationUser> _userManager;
		public UserService(UserManager<ApplicationUser> userManager)
		{
            _userManager = userManager;
		}

        public async Task<List<ApplicationUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }
    }
}

