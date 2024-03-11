using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Server.Models
{
	public class ApplicationUser : IdentityUser
	{

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Key]
        public List<BookModel> PurchasedBooks { get; set; } = new List<BookModel>();
    }
}

