namespace CuriousReadersService.Services.Genre;

using CuriousReadersData.Dto.Genres;

public interface IGenreService
{
    IEnumerable<ReadGenreModel> GetAllGenres();

    int GetAssignedBookGenresCount();
}
