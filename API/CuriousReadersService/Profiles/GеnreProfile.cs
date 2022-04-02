namespace CuriousReadersService.Profiles;

using AutoMapper;
using CuriousReadersData.Dto.Genres;
using CuriousReadersData.Entities;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, ReadGenreModel>();
    }
}
