namespace CuriousReadersService.Profiles;

using AutoMapper;
using CuriousReadersData.Entities;
using CuriousReadersService.Dto.Notifications;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

[ExcludeFromCodeCoverage]
public class NotificationProfile : Profile
{

    public NotificationProfile()
    {
        CreateMap<CreateNotificationModel, Notification>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.UserId, opt => opt.Ignore())
                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => DateTime.Now))
                .ForMember(x => x.IsRead, opt => opt.MapFrom(x => false));

        CreateMap<Notification, ReadNotificationModel>()
            .ForMember(x => x.BookTitle, opt => opt.MapFrom(x => x.Book.Title))
            .ForMember(x => x.Date, opt => opt.MapFrom(x => x.CreatedOn.ToString("d", CultureInfo.GetCultureInfo("es-ES"))))
            .ForMember(x => x.Time, opt => opt.MapFrom(x => x.CreatedOn.ToString("T", CultureInfo.GetCultureInfo("es-ES"))));

        CreateMap<Reservation, ReadLibrarianNotificationModel>()
            .ForMember(x => x.BookTitle, opt => opt.MapFrom(x => x.Book.Title))
            .ForMember(x => x.ReturnDate, opt => opt.MapFrom(x => x.ReturnDate))
            .ForMember(x => x.ReserveeUser, opt => opt.MapFrom(x => x.User.UserName))
            .ForMember(x => x.ReserveeNumber, opt => opt.MapFrom(x => x.User.PhoneNumber));
    }
    
}
