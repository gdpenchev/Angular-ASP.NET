namespace CuriousReadersData.Validators.User;

using CuriousReadersData.Dto.User;
using FluentValidation;
using static CuriousReadersData.DataConstants;
public class RegisterUserModelValidator : AbstractValidator<RegisterModel>
{
    public RegisterUserModelValidator()
    {
        RuleFor(x => x.FirstName).NotNull().NotEmpty().MaximumLength(128);
        RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(128);
        RuleFor(x => x.Password).NotNull().NotEmpty().Matches(passwordRegex).MinimumLength(10).MaximumLength(65);
        RuleFor(x => x.RepeatPassword).NotNull().NotEmpty().Matches(passwordRegex).MinimumLength(10).MaximumLength(65).Equal(x => x.Password);
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotNull().NotEmpty().Matches(phoneNumberRegex);
        RuleFor(x => x.Country).NotNull().NotEmpty();
        RuleFor(x => x.City).NotNull().NotEmpty();
        RuleFor(x => x.Street).NotNull().NotEmpty();
        RuleFor(x => x.StreetNumber).NotNull().NotEmpty();
        RuleFor(x => x.BuildingNumber).MaximumLength(65);
        RuleFor(x => x.ApartmentNumber).MaximumLength(65);
        RuleFor(x => x.AdditionalInfo).MaximumLength(1028);
    }
}
