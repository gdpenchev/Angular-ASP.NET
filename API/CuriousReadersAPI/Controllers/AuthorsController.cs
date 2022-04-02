namespace CuriousReadersAPI.Controllers;

using CuriousReadersData.Dto.Authors;
using CuriousReadersService.Services.Author;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService authorService;

    public AuthorsController(IAuthorService authorService)
    {
        this.authorService = authorService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ReadAuthorModel>> GetAllAuthors()
    {
        var allAuthors = this.authorService.GetAllAuthors();
        return Ok(allAuthors);
    }
}
