using BookStore.Server.Data;
using BookStore.Server.Models;
using BookStore.Server.Models.RequestModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Server.Services.BookServices
{
    public class BookServices : IBookServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BookStoreDbContext _bookStoreDbContext;
        private readonly ILogger<BookServices> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookServices(UserManager<ApplicationUser> userManager, BookStoreDbContext bookStoreDbContext, ILogger<BookServices> logger, IWebHostEnvironment webHostEnvironment)
        {

            _userManager = userManager;
            _bookStoreDbContext = bookStoreDbContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;

            
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
            catch (Exception ex)
            {
                _logger.LogError(message: ex!.Message!);
                throw;
            }
        }

        async public Task<(int, dynamic)> GetBookContent(int id, string userName)
        {
            try
            {


                var _user = await _userManager.FindByNameAsync(userName);

                if (_user == null)
                {
                    return (0, "User not found");
                }

                var _book = _user.PurchasedBooks.FirstOrDefault(book => book.Id == id);

                if (_book == null)
                {   
                    return (0, "User has not purchased this book");
                }
                
                string projectRootPath = _webHostEnvironment.ContentRootPath;
                var filePath = Path.Combine(projectRootPath, _book.BookPath);

                if (!File.Exists(filePath))
                {
                    return (0, "File path not found");
                }
                return (1, File.ReadAllBytes(filePath));

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(int, dynamic)> PurchaseBook(int id, string username)
        {
            try
            {
                Console.WriteLine(id);
                var _book = await _bookStoreDbContext.BookModels.FindAsync(id);

                if (_book == null)
                {
                    return (0, "Book not found");

                }

                var _user = await _userManager.FindByNameAsync(username);

                if (_user == null)
                {
                    return (0, "User not found");
                }

                var _userPurchasedBooks = _bookStoreDbContext.PurchasedBooks.FirstOrDefault(pBook => pBook.UserId == _user.Id);


                if (_userPurchasedBooks == null)
                {
                    PurchasedBooks _purchasedBooks = new PurchasedBooks { Books = new List<BookModel> { _book }, UserId = _user.Id };

                    await _bookStoreDbContext.PurchasedBooks.AddAsync(_purchasedBooks);
                }
                else
                {
                    _userPurchasedBooks.Books.Add(_book);
                }


                await _bookStoreDbContext.SaveChangesAsync();

                return (1, _book);

            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex!.Message!);
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
            catch (Exception ex)
            {
                _logger.LogError(message: ex!.Message!);
                throw;
            }
        }
    }
}

