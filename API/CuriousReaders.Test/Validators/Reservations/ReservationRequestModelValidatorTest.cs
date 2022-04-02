namespace CuriousReaders.Test.Validators.Reservations;

using CuriousReadersData.Validators.Reservations;
using CuriousReadersService.Request;
using Xunit;

public class ReservationRequestModelValidatorTest
{
    private readonly ReservationRequestModelValidator reservationRequestModelValidator = new ReservationRequestModelValidator();
    private readonly ReservationRequest reservationRequestModel = new ReservationRequest();

    [Fact]
    public void ReservationRequestModelValidator_ShouldThrowError_IfBookIdIsEmpty()
    {
        reservationRequestModel.BookId = 0;

        var result = reservationRequestModelValidator.Validate(reservationRequestModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Book Id' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ReservationRequestModelValidator_ShouldThrowError_IfUserEmailIsNull()
    {
        reservationRequestModel.BookId = 1;
        reservationRequestModel.UserEmail = null;

        var result = reservationRequestModelValidator.Validate(reservationRequestModel);

        Assert.False(result.IsValid);
        Assert.Equal("'User Email' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ReservationRequestModelValidator_ShouldThrowError_IfUserEmailIsEmpty()
    {
        reservationRequestModel.BookId = 1;
        reservationRequestModel.UserEmail = "";

        var result = reservationRequestModelValidator.Validate(reservationRequestModel);

        Assert.False(result.IsValid);
        Assert.Equal("'User Email' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ReservationRequestModelValidator_ShouldThrowError_IfUserEmailIsNotValid()
    {
        reservationRequestModel.BookId = 1;
        reservationRequestModel.UserEmail = "testMail";

        var result = reservationRequestModelValidator.Validate(reservationRequestModel);

        Assert.False(result.IsValid);
        Assert.Equal("'User Email' is not a valid email address.", result.Errors[0].ErrorMessage);
    }
}