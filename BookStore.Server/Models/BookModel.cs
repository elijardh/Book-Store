using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Server.Models
{
	public class BookModel
	{

        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }


        [Required]
        public required string BookPath { get; set; }

        [Required]
        public required string Author { get; set; }

        [Required]
        public required List<string> Genres { get; set; }

        [Required]
        public required string UploaderId { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}-\d{10}$", ErrorMessage = "Invalid ISBN format.")]
        public required string ISBN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublicationDate { get; set; }

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000.")]
        public decimal Price { get; set; }
    }
}

