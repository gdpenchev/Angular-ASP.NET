namespace CuriousReaders.Test.Validators.User;

using CuriousReadersData.Dto.User;
using CuriousReadersData.Validators.User;
using Xunit;

public class CreatePasswordModelValidatorTest
{
    private readonly ChangePasswordModelValidator changePasswordModelValidator = new ChangePasswordModelValidator();
    private readonly ChangePasswordModel changePasswordModel = new ChangePasswordModel();
    
    [Fact]
    public void ChangePasswordModelValidator_ShouldThrowError_IfEmailIsNull()
    {
        changePasswordModel.Email = null;

        var result = changePasswordModelValidator.Validate(changePasswordModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Email' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ChangePasswordModelValidator_ShouldThrowError_IfEmailIsEmpty()
    {
        changePasswordModel.Email = "";

        var result = changePasswordModelValidator.Validate(changePasswordModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Email' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ChangePasswordModelValidator_ShouldThrowError_IfEmailIsNotValid()
    {
        changePasswordModel.Email = "testMail";

        var result = changePasswordModelValidator.Validate(changePasswordModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Email' is not a valid email address.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ChangePasswordModelValidator_ShouldThrowError_IfNewPasswordIsNull()
    {
        changePasswordModel.Email = "testMail@mail.com";
        changePasswordModel.NewPassword = null;

        var result = changePasswordModelValidator.Validate(changePasswordModel);

        Assert.False(result.IsValid);
        Assert.Equal("'New Password' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ChangePasswordModelValidator_ShouldThrowError_IfNewPasswordIsEmpty()
    {
        changePasswordModel.Email = "testMail@mail.com";
        changePasswordModel.NewPassword = "";

        var result = changePasswordModelValidator.Validate(changePasswordModel);

        Assert.False(result.IsValid);
        Assert.Equal("'New Password' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ChangePasswordModelValidator_ShouldThrowError_IfNewPassworPatternNotMatched()
    {
        changePasswordModel.Email = "testMail@mail.com";
        changePasswordModel.NewPassword = "testpass";

        var result = changePasswordModelValidator.Validate(changePasswordModel);

        Assert.False(result.IsValid);
        Assert.Equal("'New Password' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ChangePasswordModelValidator_ShouldThrowError_IfNewPasswordLengthLessThan10()
    {
        changePasswordModel.Email = "testMail@mail.com";
        changePasswordModel.NewPassword = "Testpas1@";

        var result = changePasswordModelValidator.Validate(changePasswordModel);

        Assert.False(result.IsValid);
        Assert.Equal("'New Password' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ChangePasswordModelValidator_ShouldThrowError_IfNewPasswordLengthMoreThan65()
    {
        changePasswordModel.Email = "testMail@mail.com";
        changePasswordModel.NewPassword = "Testpass1@Testpass1@Testpass1@Testpass1@Testpass1@Testpass1@Testpass1@";

        var result = changePasswordModelValidator.Validate(changePasswordModel);

        Assert.False(result.IsValid);
        Assert.Equal("'New Password' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ChangePasswordModelValidator_ShouldThrowError_IfResetTokenIsNull()
    {
        changePasswordModel.Email = "testMail@mail.com";
        changePasswordModel.NewPassword = "TestPass1@";
        changePasswordModel.ResetToken = null;

        var result = changePasswordModelValidator.Validate(changePasswordModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Reset Token' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void ChangePasswordModelValidator_ShouldThrowError_IfResetTokenIsEmpty()
    {
        changePasswordModel.Email = "testMail@mail.com";
        changePasswordModel.NewPassword = "TestPass1@";
        changePasswordModel.ResetToken = "";

        var result = changePasswordModelValidator.Validate(changePasswordModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Reset Token' must not be empty.", result.Errors[0].ErrorMessage);
    }
}
