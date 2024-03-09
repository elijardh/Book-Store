using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Server.Services.AuthServices
{
    public class AuthService : IAuthService
	{

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IConfiguration configuration;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.userManager = userManager;
		}

        public async Task<(int, string)> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName!);

        

            if (user == null)
                return (0, "User not found");

            if (!await userManager.CheckPasswordAsync(user, model.Password!))
                return (0, "Incorrect Password");

            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var item in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            string token = GenerateToken(authClaims);

            return (1, token);
        }

        private string GenerateToken(List<Claim> authClaims)
        {
            var AuthSignInkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTKey:Secret"]!));

            var TokenExpiryTimeInHours = Convert.ToInt64(configuration["JWTKey:TokenExpiryTimeInHour"]);

            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = configuration["JWTKey:ValidIssuer"],
                Audience = configuration["JWTKey:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(40),
                SigningCredentials = new SigningCredentials(AuthSignInkey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(TokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<(int, string)> Registration(RegistrationModel model, string role)
        {
            var userExist = await userManager.FindByNameAsync(model.UserName!);
            Console.WriteLine("Works");
            if (userExist != null)
                return (0, "User already exist");

            ApplicationUser applicationUser = new()
            {
                UserName = model.UserName,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await userManager.CreateAsync(applicationUser, model.Password!);

            Console.WriteLine(createUserResult.Errors.ToString());
            if (!createUserResult.Succeeded)
                return (0, createUserResult.Errors.First().Description);

            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

            if (await roleManager.RoleExistsAsync(role))
                await userManager.AddToRoleAsync(applicationUser, role);


            return (1, "User created successfully");

        }
    }
}

