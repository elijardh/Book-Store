using System;
using BookStore.Server.Models;
namespace BookStore.Server.Services.AuthServices
{
	public interface IAuthService
	{
		Task<(int, string)> Registration(RegistrationModel model, string role);
        Task<(int, string)> Login(LoginModel model);

    }
}

