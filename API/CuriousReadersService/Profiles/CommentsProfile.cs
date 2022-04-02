namespace CuriousReadersData.Profiles
{
    using AutoMapper;
    using CuriousReadersData.Dto.Comments;
    using CuriousReadersData.Entities;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    [ExcludeFromCodeCoverage]
    public class CommentsProfile : Profile
    {
        public CommentsProfile()
        {
            CreateMap<CreateCommentModel, Comment>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Comment, ReadCommentModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Username))
                .ForMember(x => x.BookName, opt => opt.MapFrom(x => x.Book.Title))
                .ForMember(x => x.CreationDate, opt => opt.MapFrom(x => x.CreatedOn.ToString("d", CultureInfo.GetCultureInfo("es-ES"))))
                .ForMember(x => x.CreationTime, opt => opt.MapFrom(x => x.CreatedOn.ToString("T", CultureInfo.GetCultureInfo("es-ES"))));
        }
    }
}
