namespace CuriousReadersData.Validators.User;

using CuriousReadersData.Dto.User;
using FluentValidation;
using static CuriousReadersData.DataConstants;
public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
{
    public ChangePasswordModelValidator()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(x => x.NewPassword).NotNull().NotEmpty().Matches(passwordRegex).MinimumLength(10).MaximumLength(65);
        RuleFor(x => x.ResetToken).NotNull().NotEmpty();
    }
}
