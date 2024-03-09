using System;
using BookStore.Server.Models;
using BookStore.Server.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Server.Services.BookServices
{
    public interface IBookServices
    {
        Task<(int, dynamic)> UploadBook(BookUploadRequest bookUploadRequest, string userID);
        Task<List<BookModel>> GetAllBooks(string? keyword);
        Task<(int, dynamic)> PurchaseBook(int id, string reference);
    }
}

