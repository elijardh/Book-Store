using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BookStore.Server.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage ="Username is required")]
		public String? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public String? Password { get; set; }
	}
}

