using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using BookStore.Server.Data;
using BookStore.Server.Models;
using BookStore.Server.Models.RequestModels;
using BookStore.Server.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookStore.Server.Services.BookServices
{
    public class BookServices : IBookServices
    {
        private readonly BookStoreDbContext _bookStoreDbContext;
        private readonly ILogger<BookServices> _logger;

        public BookServices(BookStoreDbContext bookStoreDbContext, ILogger<BookServices> logger)
        {
            _bookStoreDbContext = bookStoreDbContext;
            _logger = logger;
        }

        public Task<List<BookModel>> GetAllBooks(string? keyword)
        {
            try
            {
                List<BookModel> _books = _bookStoreDbContext.BookModels.ToList();

                 
               
                if (keyword != null && keyword.Length >0)
                {
                    return Task.FromResult(_books.Where(book => book.Title.Contains(keyword)).ToList());
                }
                else
                {
                    return Task.FromResult(_books);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(int, dynamic?)> PurchaseBook(int id, string reference)
        {
            try
            {
                var _book = _bookStoreDbContext.BookModels.FirstOrDefault(book => book.Id == id);


                HttpClient httpClient = new()
                {
                    BaseAddress = new Uri("https://api.flutterwave.com/v3/transactions/"),
                };

                _logger.LogInformation($"{httpClient.BaseAddress.AbsolutePath}");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "FLWSECK_TEST-d28b00460564522d46cc948b1d06f71c-X");
                HttpResponseMessage response = await httpClient.GetAsync($"{id}/verify");

              

            

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    FlutterWaveResponse? result = JsonConvert.DeserializeObject<FlutterWaveResponse>(responseBody);
                    Console.WriteLine($"{result}  please work naaaa");

                    if (result == null)
                    {
                        return (0, "Unknown error");
                    }

                    if (result.Status != null && result.Status == "success")
                    {
                        return (1, result);
                    }
                    else
                    {
                        return (0, result);
                    }
                 
                    // Read and parse the response content
                    //Console.WriteLine(responseBody);
                    //return (1, result);
                }
                else
                {
                    // Handle unsuccessful response
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return (0, response.ReasonPhrase);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(int, dynamic)> UploadBook([FromForm] BookUploadRequest bookUploadRequest, string userID)
        {
            try
            {
                if (bookUploadRequest.BookDocument == null || bookUploadRequest.BookDocument.Length == 0)
                {
                    return (0, "Bad");
                }

                var fileName = Guid.NewGuid().ToString() + "_" + bookUploadRequest!.BookDocument.FileName;

                var filePath = Path.Combine("Books", fileName);

             

                BookModel _model = new()
                { Author = bookUploadRequest.Author, BookPath = filePath, Genres = bookUploadRequest.Genres, ISBN = bookUploadRequest.ISBN, Price = bookUploadRequest.Price, PublicationDate = bookUploadRequest.PublicationDate.ToUniversalTime(), Title = bookUploadRequest.Title, UploaderId = userID, };

                using FileStream stream = new FileStream(filePath, FileMode.Create);

                await bookUploadRequest.BookDocument.CopyToAsync(stream);


                _bookStoreDbContext.BookModels.Add(_model);
                _bookStoreDbContext.SaveChanges();

                return (0, _model);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

