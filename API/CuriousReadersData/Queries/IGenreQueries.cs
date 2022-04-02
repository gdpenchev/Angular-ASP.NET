namespace CuriousReadersData.Queries;

using CuriousReadersData.Entities;

public interface IGenreQueries
{
    int GetAssignedBookGenresCount();

    IEnumerable<Genre> GetAllGenres();

    List<Genre> GetExistingGenres(IEnumerable<string> genres);

    List<string> GetNewGenres(IEnumerable<string> genres, List<Genre> existingGenres);
}
