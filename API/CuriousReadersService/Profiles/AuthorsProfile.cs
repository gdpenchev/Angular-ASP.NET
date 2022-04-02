namespace CuriousReadersData.Profiles;

using AutoMapper;
using CuriousReadersData.Dto.Books;
using CuriousReadersData.Entities;
using CuriousReadersData.Dto.Authors;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class AuthorsProfile : Profile
{
    public AuthorsProfile()
    {
        CreateMap<CreateBookModel, CreateAuthorModel>();

        CreateMap<CreateAuthorModel, Author>();

        CreateMap<ReadAuthorModel, Author>();

        CreateMap<Author, ReadAuthorModel> ();
    }
}
