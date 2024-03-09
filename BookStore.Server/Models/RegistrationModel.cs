using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Server.Models
{
	public class RegistrationModel
	{	
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage ="Email address is required")]
        public string? Email { get; set; }
    }
}

