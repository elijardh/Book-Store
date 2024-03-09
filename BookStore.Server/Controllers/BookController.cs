using System.Security.Claims;
using BookStore.Server.Models.RequestModels;
using BookStore.Server.Services.BookServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.Server.Controllers
{
    [Route("api/books")]
    public class BookController : Controller
    {
        private readonly IBookServices _bookServices;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookServices bookServices, ILogger<BookController> logger)
        {
            _bookServices = bookServices;
            _logger = logger;
        }


       
        [HttpPost("upload")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadBook([FromForm] BookUploadRequest model)
        {
            try
            {
                var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

                if (userName == null || userName.Length! <= 0)
                {
                    return BadRequest(new { message = "User not found" });
                }

                var (status, value) = await _bookServices.UploadBook(model, userName);

                if (status == 0)
                {
                    return BadRequest(new { message = value, });

                }

                return Ok(value);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, new { messgae = ex.Message });
            }
        }

        [HttpGet("allbooks")]
        public async Task<IActionResult> GetAllBooks(string? keyword)
        {
            try
            {
                var response = await _bookServices.GetAllBooks(keyword);

                return Ok(new{
                    message = "Books returned successfully",
                    status = "Successful",
                    data = response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, new { messgae = ex.Message });
            }
        }

        [HttpGet("puchaseBook")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PurchaseBook(int id, string reference)
        {
            try
            {
                var (status, response) = await _bookServices.PurchaseBook(id, reference);

                if (status == 0)
                {
                    return BadRequest(new { message = "Purchase failed", status = "Unsuccesful", data = response });
                }

                return Ok(new
                {
                    message = "Books returned successfully",
                    status = "Successful",
                    data = response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, new { messgae = ex.Message });
            }
        }
    }
}

