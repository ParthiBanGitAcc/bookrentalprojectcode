using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookRentalService.Services;
using BookRentalService.Models;
using Azure.Core;

namespace BookRentalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BooksController> _logger; // Add a logger fiel

        public BooksController(BookService bookService, IConfiguration configuration, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _configuration = configuration;
            _logger = logger;
        }


        /// <summary>
        /// This method is used to search the books
        /// </summary>
        /// <param name="title"></param>
        /// <param name="genre"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string title, [FromQuery] string genre)
        {

            if (string.IsNullOrEmpty(title))
            {
                return BadRequest(new { message = _configuration["Errors:EnterTitle"] });
            }

            if (string.IsNullOrEmpty(genre))
            {
                return BadRequest(new { message = _configuration["Errors:EnterGenre"] });
            }

            try
            {
                var books = await _bookService.SearchBooks(title, genre);

                if (books.Count == 0)
                {
                    return BadRequest(new { message = _configuration["Errors:NoBooksFound"] });
                }
                else
                {
                    return Ok(books);
                }
            }
            catch (Exception ex)
            {

                // Log the exception
                _logger.LogError(ex, "Error while searching the book '{BookName}'", title);
                return StatusCode(500, "Internal server error");
            }
          
        }


        /// <summary>
        /// This method is used to rent book
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("rent")]
        public async Task<IActionResult> RentBook([FromBody] RentRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName))
            {
                return BadRequest(new { message = _configuration["Errors:MissingUserName"] });
            }

            if (string.IsNullOrEmpty(request.BookName))
            {
                return BadRequest(new { message = _configuration["Errors:MissingBookName"] });
            }

            var result = await _bookService.RentBook(request.UserName, request.BookName);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Rental);
        }


        /// <summary>
        /// This method is used to return book
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnRequest request)
        {

            if (string.IsNullOrEmpty(request.BookName))
            {
                return BadRequest(new { message = _configuration["Errors:MissingBookName"] });
            }

            var result = await _bookService.ReturnBook(request.BookName);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(new { message = _configuration["Errors:BookReturn"] });
        }

        /// <summary>
        /// This method is used to get the Rental History ByUser
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("rental-history/{userName}")]
        public async Task<IActionResult> GetRentalHistory(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest(new { message = _configuration["Errors:MissingUserName"] });
            }

            try
            {
                var rentalHistory = await _bookService.GetRentalHistoryByUser(userName);
                return Ok(rentalHistory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// This method is used to get the book statistics
        /// </summary>
        /// <returns></returns>

        [HttpGet("statistics")]
        public async Task<IActionResult> GetBookStatistics()
        {
            var statistics = await _bookService.GetBookStatistics();
            return Ok(statistics);
        }


    }

    public class RentRequest
    {
        public string UserName { get; set; }
        public string BookName { get; set; }
    }

    public class ReturnRequest
    {
        public string BookName { get; set; }
    }

    public class RentalHistoryDto
    {
        public string BookName { get; set; }
        public string RentalDate { get; set; }
        public string? ReturnDate { get; set; }
        public bool IsOverdue { get; set; }

        // Constructor to initialize the formatted dates
        public RentalHistoryDto(DateTime rentalDate, DateTime? returnDate, string bookName, bool isOverdue)
        {
            BookName = bookName;
            RentalDate = rentalDate.ToString("yyyy-MM-dd"); // Format RentalDate as yyyy-MM-dd
            ReturnDate = returnDate?.ToString("yyyy-MM-dd"); // Format ReturnDate as yyyy-MM-dd, or null if no return date
            IsOverdue = isOverdue;
        }

    }



    public class BookStatisticsResultDto
    {
        public BookStatisticsDto MostOverdue { get; set; }
        public BookStatisticsDto MostPopular { get; set; }
        public BookStatisticsDto LeastPopular { get; set; }
    }

    public class BookStatisticsDto
    {
        public string BookName { get; set; }
        public int RentalCount { get; set; }
        public bool IsOverdue { get; set; }
    }
}
