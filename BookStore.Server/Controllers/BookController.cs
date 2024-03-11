using System.Security.Claims;
using BookStore.Server.Models.RequestModels;
using BookStore.Server.Services.BookServices;
using BookStore.Server.Services.PaymentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.Server.Controllers
{
    [Route("api/books")]
    public class BookController : Controller
    {
        private readonly IBookServices _bookServices;
        private readonly IPaymentServices _paymentServices;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookServices bookServices, ILogger<BookController> logger, IPaymentServices paymentServices)
        {
            _bookServices = bookServices;
            _logger = logger;
            _paymentServices = paymentServices;
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

        [HttpGet("getBookContent")]
        [Authorize()]
        public async Task<IActionResult> GetBookContent(int id)
        {
            try
            {
                var _userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

                if (_userName == null)
                {
                    return Unauthorized(new { message = "You are not authorized", status = "Unsuccesful"});
                }

                var (status, response) = await _bookServices.GetBookContent(id, _userName);

                if (status == 0)
                {
                    return BadRequest(new { message = response, status = "Unsuccesful" });
                }

                return File(response, "application/octet-stream");

            }
            catch (Exception ex)
            {
                {
                    _logger.LogError(ex.Message);

                    return StatusCode(StatusCodes.Status500InternalServerError, new { messgae = ex.Message });
                }
            }
        }
        [HttpGet("puchaseBook")]
        //[Authorize()]
        public async Task<IActionResult> PurchaseBook(int id, string reference)
        {
            try
            {
                //var (paymentStatus, paymentResponse) = await _paymentServices.ValidatePayment(reference);

                //if (paymentStatus == 0)
                //{
                //    return BadRequest(new { message = "Purchase failed, payment wasn't verified", status = "Unsuccesful", data = paymentResponse });
                //}

                Console.WriteLine($"Book id is {id}");
                var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

                if (userName == null)
                {
                    return Unauthorized(new { message = "Purchase failed, User not found", status = "Unsuccesful",});
                }

                var (status, response) = await _bookServices.PurchaseBook(id, userName);

                if (status == 0)
                {
                    return BadRequest(new { message = $"Purchase failed, {response}", status = "Unsuccesful", });
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

