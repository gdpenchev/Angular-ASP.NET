namespace CuriousReaders.Test.Validators.User;

using CuriousReadersData.Dto.User;
using CuriousReadersData.Validators.User;
using Xunit;

public class RegisterUserModelValidatorTest
{
    private readonly RegisterUserModelValidator registerUserModelValidator = new RegisterUserModelValidator();
    private readonly RegisterModel registerUserModel = new RegisterModel();

    [Fact]
    public void RegisterUserModelValidator_ShouldThrowError_IfFirstNameIsNull()
    {
        registerUserModel.FirstName = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'First Name' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterUserModelValidator_ShouldThrowError_IfFirstNameIsEmpty()
    {
        registerUserModel.FirstName = "";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'First Name' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterUserModelValidator_ShouldThrowError_IfFirstNameExceeds128Symbols()
    {
        registerUserModel.FirstName = "TestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameee";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'First Name' must be 128 characters or fewer. You entered 130 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterUserModelValidator_ShouldThrowError_IfLastNameIsNull()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Last Name' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterUserModelValidator_ShouldThrowError_IfLastNameIsEmpty()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Last Name' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterUserModelValidator_ShouldThrowError_IfLastNameExceeds128Symbols()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "TestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameeeTestNameee";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Last Name' must be 128 characters or fewer. You entered 130 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfPasswordIsNull()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Password' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfPasswordIsEmpty()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Password' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfPasswordPatternNotMatched()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPass";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Password' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfPasswordLengthLessThan10()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "Testpas1@";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Password' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfPasswordLengthMoreThan65()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "Testpass1@Testpass1@Testpass1@Testpass1@Testpass1@Testpass1@Testpass1@";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Password' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfRepeatPasswordIsNull()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Repeat Password' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfRepeatPasswordIsEmpty()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "";


        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Repeat Password' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfRepeatPasswordPatternNotMatched()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPass";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Repeat Password' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfRepeatPasswordLengthLessThan10()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "Testpas1@";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Repeat Password' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfRepeatPasswordLengthMoreThan65()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "Testpass1@Testpass1@Testpass1@Testpass1@Testpass1@Testpass1@Testpass1@";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Repeat Password' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void registerUserModelValidator_ShouldThrowError_IfRepeatPasswordIsNotTheSameAsPassword()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@11";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Repeat Password' must be equal to 'TestPassword@1'.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfEmailIsNull()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Email' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfEmailIsEmpty()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Email' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfEmailIsNotValid()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Email' is not a valid email address.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfPhoneNumberrIsNull()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Phone Number' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfPhoneNumberIsEmpty()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Phone Number' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfPhoneNumberrIsNotValid()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0982";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Phone Number' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfCountryIsNull()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Country' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfCountryIsEmpty()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = "";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Country' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfCityIsNull()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = "Bulgaria";
        registerUserModel.City = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'City' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfCityIsEmpty()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = "Bulgaria";
        registerUserModel.City = "";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'City' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfStreetIsNull()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = "Bulgaria";
        registerUserModel.City = "Plovdiv";
        registerUserModel.Street = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Street' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfStreetIsEmpty()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = "Bulgaria";
        registerUserModel.City = "Plovdiv";
        registerUserModel.Street = "";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Street' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfStreetNumberIsNull()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = "Bulgaria";
        registerUserModel.City = "Plovdiv";
        registerUserModel.Street = "Test Street";
        registerUserModel.StreetNumber = null;

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Street Number' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterModelValidator_ShouldThrowError_IfStreetNumberIsEmpty()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = "Bulgaria";
        registerUserModel.City = "Plovdiv";
        registerUserModel.Street = "Test Street";
        registerUserModel.StreetNumber = "";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Street Number' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterUserModelValidator_ShouldThrowError_IfBuildingNumberExceeds128Symbols()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = "Bulgaria";
        registerUserModel.City = "Plovdiv";
        registerUserModel.Street = "Test Street";
        registerUserModel.StreetNumber = "12A";
        registerUserModel.BuildingNumber = "TestNumberTestNumberTestNumberTestNumberTestNumberTestNumberTestNumber";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Building Number' must be 65 characters or fewer. You entered 70 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterUserModelValidator_ShouldThrowError_IfApartmentNumberExceeds128Symbols()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = "Bulgaria";
        registerUserModel.City = "Plovdiv";
        registerUserModel.Street = "Test Street";
        registerUserModel.StreetNumber = "12A";
        registerUserModel.BuildingNumber = "13B";
        registerUserModel.ApartmentNumber = "TestNumberTestNumberTestNumberTestNumberTestNumberTestNumberTestNumber";

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Apartment Number' must be 65 characters or fewer. You entered 70 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void RegisterUserModelValidator_ShouldThrowError_IfAdditionalInfoExceeds128Symbols()
    {
        registerUserModel.FirstName = "John";
        registerUserModel.LastName = "Smith";
        registerUserModel.Password = "TestPassword@1";
        registerUserModel.RepeatPassword = "TestPassword@1";
        registerUserModel.Email = "testMail@mail.com";
        registerUserModel.PhoneNumber = "0885312000";
        registerUserModel.Country = "Bulgaria";
        registerUserModel.City = "Plovdiv";
        registerUserModel.Street = "Test Street";
        registerUserModel.StreetNumber = "12A";
        registerUserModel.BuildingNumber = "13B";
        registerUserModel.ApartmentNumber = "14C";
        registerUserModel.AdditionalInfo = $"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce venenatis magna ante. Sed euismod urna at consequat porta. Morbi ac porttitor massa. Proin ut lorem commodo, mattis nulla at, maximus ex. Vivamus euismod sapien eu felis convallis pretium. Nulla purus est, blandit et molestie in, mollis posuere neque. Sed sollicitudin at enim ac condimentum." +
            $"Suspendisse potenti. Donec quis scelerisque est. Integer at nibh condimentum, sagittis velit sit amet, scelerisque libero. Pellentesque eget pretium magna. Vestibulum ultrices sagittis lacus, at tempus neque convallis eu. Integer urna neque, interdum in imperdiet at, ultricies eget nulla. Mauris finibus velit id velit interdum dictum." +
            $"Suspendisse potenti. Donec quis scelerisque est. Integer at nibh condimentum, sagittis velit sit amet, scelerisque libero. Pellentesque eget pretium magna. Vestibulum ultrices sagittis lacus, at tempus neque convallis eu. Integer urna neque, interdum in imperdiet at, ultricies eget nulla. Mauris finibus velit id velit interdum dictum." +
            $"Suspendisse potenti. Donec quis scelerisque est. Integer at nibh condimentum, sagittis velit sit amet, scelerisque libero. Pellentesque eget pretium magna. Vestibulum ultrices sagittis lacus, at tempus neque convallis eu. Integer urna neque, interdum in imperdiet at, ultricies eget nulla. Mauris finibus velit id velit interdum dictum.";
            

        var result = registerUserModelValidator.Validate(registerUserModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Additional Info' must be 1028 characters or fewer. You entered 1363 characters.", result.Errors[0].ErrorMessage);
    }
}
