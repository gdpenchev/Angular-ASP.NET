namespace CuriousReadersAPI.Controllers;


using CuriousReadersData.Dto.Books;
using CuriousReadersService.Services.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static CuriousReadersData.DataConstants;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService bookService;

    public BooksController(IBookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpPost]
    [Authorize(Roles = Librarian)]
    public async Task<ActionResult<CreateBookModel>> CreateBook([FromForm] CreateBookModel bookRequest)
    {
        var result = await bookService.CreateBook(bookRequest);

        if (result.restoreAvailable == true)
        {
            return Ok(new { restoreAvailable = result.restoreAvailable, bookId = result.book.Id });
        }
        else if (result.restoreAvailable == false)
        {
            return BadRequest("Book with this ISBN already exists.");
        }

        return Created("api/[controller]", bookRequest);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadBookModel>>> GetAllBooks(string? userEmail, int page = 1, int pageSize = 12, string? searchText = "")
    {
        var allBooks = await this.bookService.GetPaginatedBooks(userEmail, page, pageSize, searchText);

        return Ok(allBooks);
    }

    [HttpGet("Count")]
    public async Task<ActionResult<int>> GetBooksTotalCount(string? userEmail, string? searchText)
    {
        var allBooksTotalCount = await this.bookService.GetBooksTotalCount(userEmail, searchText);

        return Ok(allBooksTotalCount);
    }

    [HttpGet("{bookId}", Name = "GetBookById")]
    public ActionResult<ReadBookModel> GetBookById(int bookId)
    {
        var book = this.bookService.GetBookById(bookId);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPut("{bookId}")]
    [Authorize(Roles = Librarian)]
    public async Task<ActionResult<ReadBookModel>> UpdateBook(int bookId, [FromForm] UpdateBookModel bookRequest)
    {
        await bookService.UpdateBook(bookId, bookRequest);

        return Ok();
    }

    [HttpPatch("{bookId}")]
    [Authorize(Roles = Librarian)]
    public ActionResult UpdateBookPartially(int bookId, UpdateBookModel bookRequest)
    {
        bookService.UpdateBookPartially(bookId, bookRequest);

        return Ok();
    }
}
