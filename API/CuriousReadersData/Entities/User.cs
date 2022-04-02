namespace CuriousReadersData.Entities;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    public bool IsActive { get; set; }

    public string PhoneNumber { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public string Street { get; set; }

    public string StreetNumber { get; set; }

    public string? BuildingNumber { get; set; }

    public string? ApartmentNumber { get; set; }

    public string? AdditionalInfo { get; set; }
}
