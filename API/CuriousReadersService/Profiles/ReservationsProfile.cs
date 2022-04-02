using AutoMapper;
using CuriousReadersData.Dto.Reservations;
using CuriousReadersData.Entities;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<Reservation, ReadReservationModel>()
            .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status.Name))
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.User.UserName));
    }
}