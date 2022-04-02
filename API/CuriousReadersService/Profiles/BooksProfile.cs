namespace CuriousReadersData.Profiles;

using AutoMapper;
using CuriousReadersData.Dto.Books;
using CuriousReadersData.Entities;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class BooksProfile : Profile
{
    public BooksProfile()
    {
        CreateMap<CreateBookModel, Book>()
            .ForMember(x => x.Image, opt => opt.Ignore())
            .ForMember(x => x.Status, opt => opt.Ignore())
            .ForMember(x => x.Genres, opt => opt.Ignore())
            .ForMember(x => x.Authors, opt => opt.Ignore())
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Comments, opt => opt.Ignore());

        CreateMap<UpdateBookModel, Book>()
           .ForMember(x => x.Image, opt => opt.Ignore())
           .ForMember(x => x.Status, opt => opt.Ignore())
           .ForMember(x => x.Genres, opt => opt.Ignore())
           .ForMember(x => x.Authors, opt => opt.Ignore())
           .ForMember(x => x.Id, opt => opt.Ignore())
           .ForMember(x => x.Comments, opt => opt.Ignore());

        CreateMap<Book, AuthorBook>()
            .ForMember(x => x.BookId, opts => opts.MapFrom(x => x.Id));

        CreateMap<Book, ReadBookModel>()
            .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status.Name))
            .ForMember(x => x.Genres, opt => opt.MapFrom(x => x.Genres.Select(g => g.Genre.Name).ToArray()))
            .ForMember(x => x.Authors, opt => opt.MapFrom(x => x.Authors.Select(a => a.Author.Name).ToArray()))
            .ForMember(x => x.ReserveeEmails, opt => opt.MapFrom(x => x.Reservations.Select(r => r.User.UserName).ToArray()));
    }
}
