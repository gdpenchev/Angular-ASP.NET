namespace CuriousReaders.Test.Validators.Books;

using CuriousReadersData.Dto.Books;
using CuriousReadersData.Validators.Books;
using System.Linq;
using Xunit;

public class UpdateBookModelValidatorTest
{
    private readonly UpdateBookModelValidator updateBookModelValidator = new UpdateBookModelValidator();
    private readonly UpdateBookModel updateBookModel = new UpdateBookModel();

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfTitleIsNull()
    {
        updateBookModel.Title = null;

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Title' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfTitleIsEmpty()
    {
        updateBookModel.Title = "";

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Title' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfTitleLengthExceeds128Symbolls()
    {
        updateBookModel.Title = "TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12";

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Title' must be 128 characters or fewer. You entered 132 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfISBNIsNull()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = null;

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'ISBN' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfISBNIsEmpty()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "";

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'ISBN' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfISBNNotMatchingTheRegex()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "912-123-1";

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'ISBN' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfAuthorsArrayIsNull()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = null;

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Authors' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfAuthorsArrayIsEmpty()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = Enumerable.Empty<string>();

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Authors' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfAuthorsArrayHasNullElements()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { null, "Author" };

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Authors' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfAuthorsArrayHasEmptyElements()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "", "Author" };

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Authors' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfGenresArrayIsNull()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = null;

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Genres' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfGenresArrayIsEmpty()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = Enumerable.Empty<string>();

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Genres' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfAuthorsArrayHasElementsWhichLengthExceeds128Symbols()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "New Author New Author New Author New Author New Author New Author New Author New Author New Author New Author New Author New Author New Author" };

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Authors' must be 128 characters or fewer. You entered 142 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfGenresArrayHasNullElements()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = new string[] { null, "New Genre" };

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Genres' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfGenresArrayHasEmptyElements()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = new string[] { "", "New Genre" };

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Genres' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfGenresArrayHasElementsWhichLengthExceeds128Symbols()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = new string[] { "New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre" };

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Genres' must be 128 characters or fewer. You entered 159 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfBookDescriptionLengthExceeds1028Symbols()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = new string[] { "New Comedy", "Adventure" };
        updateBookModel.Description = $"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce venenatis magna ante. Sed euismod urna at consequat porta. Morbi ac porttitor massa. Proin ut lorem commodo, mattis nulla at, maximus ex. Vivamus euismod sapien eu felis convallis pretium. Nulla purus est, blandit et molestie in, mollis posuere neque. Sed sollicitudin at enim ac condimentum." +
            $"Suspendisse potenti. Donec quis scelerisque est. Integer at nibh condimentum, sagittis velit sit amet, scelerisque libero. Pellentesque eget pretium magna. Vestibulum ultrices sagittis lacus, at tempus neque convallis eu. Integer urna neque, interdum in imperdiet at, ultricies eget nulla. Mauris finibus velit id velit interdum dictum." +
            $"Suspendisse potenti. Donec quis scelerisque est. Integer at nibh condimentum, sagittis velit sit amet, scelerisque libero. Pellentesque eget pretium magna. Vestibulum ultrices sagittis lacus, at tempus neque convallis eu. Integer urna neque, interdum in imperdiet at, ultricies eget nulla. Mauris finibus velit id velit interdum dictum." +
            $"Suspendisse potenti. Donec quis scelerisque est. Integer at nibh condimentum, sagittis velit sit amet, scelerisque libero. Pellentesque eget pretium magna. Vestibulum ultrices sagittis lacus, at tempus neque convallis eu. Integer urna neque, interdum in imperdiet at, ultricies eget nulla. Mauris finibus velit id velit interdum dictum.";


        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Description' must be 1028 characters or fewer. You entered 1363 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfQuantityIsZero()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = new string[] { "New Comedy", "Adventure" };
        updateBookModel.Quantity = 0;

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Quantity' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfQuantityIsLessThanZero()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = new string[] { "New Comedy", "Adventure" };
        updateBookModel.Quantity = -5;

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Quantity' must be greater than '0'.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfImageUrlIsNull()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = new string[] { "New Comedy", "Adventure" };
        updateBookModel.Quantity = 15;
        updateBookModel.OldImageUrl = null;

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Old Image Url' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfImageUrlIsEmpty()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = new string[] { "New Comedy", "Adventure" };
        updateBookModel.Quantity = 15;
        updateBookModel.OldImageUrl = "";

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Old Image Url' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void UpdateBookModelValidator_ShouldThrowError_IfStatusIsNotFromTheValidStatuses()
    {
        updateBookModel.Title = "Test title";
        updateBookModel.ISBN = "978-9-3897-4501-6";
        updateBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        updateBookModel.Genres = new string[] { "New Comedy", "Adventure" };
        updateBookModel.Quantity = 15;
        updateBookModel.OldImageUrl = "someTestUrl";
        updateBookModel.Status = "NotSelected";

        var result = updateBookModelValidator.Validate(updateBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Status' has a range of values which does not include 'NotSelected'.", result.Errors[0].ErrorMessage);
    }
}
