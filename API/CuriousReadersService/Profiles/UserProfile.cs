using AutoMapper;
using CuriousReadersData.Dto.User;
using CuriousReadersData.Entities;
using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersService.Profiles;

[ExcludeFromCodeCoverage]
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterModel, User>()
            .ForMember(x => x.RegistrationDate, opt => opt.MapFrom(x => DateTime.Now))
            .ForMember(x => x.IsActive, opt => opt.MapFrom(x => false))
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));

        CreateMap<User, UserModel>()
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FirstName + " " + x.LastName));
    }
}
