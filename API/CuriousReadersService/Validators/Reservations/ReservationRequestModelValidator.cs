namespace CuriousReadersData.Validators.Reservations;

using CuriousReadersService.Request;
using FluentValidation;

public class ReservationRequestModelValidator : AbstractValidator<ReservationRequest>
{
    public ReservationRequestModelValidator()
    {
        RuleFor(x => x.BookId).NotNull().NotEmpty();
        RuleFor(x => x.UserEmail).NotNull().NotEmpty().EmailAddress();
    }
}
