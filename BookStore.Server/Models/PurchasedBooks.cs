using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Server.Models
{
	public class PurchasedBooks
	{
        public int Id { get; set; }
        [Required]
        public required string UserId { get; set; }
        [Required]
        public required List<BookModel> Books { get; set; }
    }
}

