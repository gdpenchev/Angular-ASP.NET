namespace CuriousReadersService.Services.Author;

using AutoMapper;
using CuriousReadersData.Dto.Authors;
using CuriousReadersData.Entities;
using CuriousReadersData.Queries;

public class AuthorService : IAuthorService
{
    private readonly IAuthorQueries authorQuery;
    private readonly IMapper mapper;
    public AuthorService(IAuthorQueries authorQuery, IMapper mapper)
    {
        this.authorQuery = authorQuery;
        this.mapper = mapper;
    }

    public IEnumerable<ReadAuthorModel> GetAllAuthors()
    {
        var allAuthors = authorQuery.GetAllAuthors();

        return mapper.Map<IEnumerable<Author>, List<ReadAuthorModel>>(allAuthors);
    }
}
