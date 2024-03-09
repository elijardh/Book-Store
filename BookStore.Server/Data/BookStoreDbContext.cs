using System;
using BookStore.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Server.Data
{
	public class BookStoreDbContext : DbContext
	{
		public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
		{
		}

		public DbSet<BookModel> BookModels { get; set; }
	}
}

