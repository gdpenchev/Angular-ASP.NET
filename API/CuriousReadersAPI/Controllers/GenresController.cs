namespace CuriousReadersAPI.Controllers;

using CuriousReadersData.Dto.Genres;
using CuriousReadersService.Services.Genre;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService genreService;

    public GenresController(IGenreService genreService)
    {
        this.genreService = genreService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ReadGenreModel>> GetAllGenres()
    {
        var allGenres = genreService.GetAllGenres();
        return Ok(allGenres);
    }

    [HttpGet("Count")]
    public ActionResult<IEnumerable<ReadGenreModel>> GetAssignedBookGenresCount()
    {
        var genresTotalCount = genreService.GetAssignedBookGenresCount();
        return Ok(genresTotalCount);
    }
}
