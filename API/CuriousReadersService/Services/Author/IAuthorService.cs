namespace CuriousReadersService.Services.Author;

using CuriousReadersData.Dto.Authors;

public interface IAuthorService
{
    IEnumerable<ReadAuthorModel> GetAllAuthors();
}
