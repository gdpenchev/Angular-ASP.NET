using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CuriousReadersData.Dto.User;

[ExcludeFromCodeCoverage]
public class RegisterModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string RepeatPassword { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public string? BuildingNumber { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? AdditionalInfo { get; set; }
}
