namespace CuriousReadersService.Services.Genre;

using AutoMapper;
using CuriousReadersData.Dto.Genres;
using CuriousReadersData.Queries;
using CuriousReadersData.Entities;
using System.Collections.Generic;

public class GenreService : IGenreService
{
    private readonly IGenreQueries genreQueries;
    private readonly IMapper mapper;

    public GenreService(IGenreQueries genreQueries, IMapper mapper)
    {
        this.genreQueries = genreQueries;
        this.mapper = mapper;
    }

    public IEnumerable<ReadGenreModel> GetAllGenres()
    {
        var allGenres = this.genreQueries.GetAllGenres();

        return mapper.Map<IEnumerable<Genre>, List<ReadGenreModel>>(allGenres);
    }

    public int GetAssignedBookGenresCount()
    {
        return genreQueries.GetAssignedBookGenresCount();
    }
}
