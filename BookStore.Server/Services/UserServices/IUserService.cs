using System;
using BookStore.Server.Models;

namespace BookStore.Server.Services.UserServices
{
	public interface IUserService
	{
		Task<List<ApplicationUser>> GetUsers();
	}
}

