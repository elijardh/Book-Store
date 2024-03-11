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
        public DbSet<PurchasedBooks> PurchasedBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PurchasedBooks>()
                .Property(e => e.Books)
                .HasColumnType("jsonb[]"); // Assuming the list contains JSONB objects
        }
    }
}