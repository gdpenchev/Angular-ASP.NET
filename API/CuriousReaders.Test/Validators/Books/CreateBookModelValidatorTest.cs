namespace CuriousReaders.Test.Validators.Books;

using CuriousReadersData.Dto.Books;
using CuriousReadersData.Validators.Books;
using System.Linq;
using Xunit;

public class CreateBookModelValidatorTest
{
    private readonly CreateBookModelValidator createBookModelValidator = new CreateBookModelValidator();
    private readonly CreateBookModel createBookModel = new CreateBookModel();

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfTitleIsNull()
    {
        createBookModel.Title = null;

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Title' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfTitleIsEmpty()
    {
        createBookModel.Title = "";

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Title' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfTitleLengthExceeds128Symbolls()
    {
        createBookModel.Title = "TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12TestTitle12";

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Title' must be 128 characters or fewer. You entered 132 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfISBNIsNull()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = null;

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'ISBN' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfISBNIsEmpty()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "";

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'ISBN' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfISBNNotMatchingTheRegex()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "912-123-1";

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'ISBN' is not in the correct format.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfAuthorsArrayIsNull()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = null;

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Authors' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfAuthorsArrayIsEmpty()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = Enumerable.Empty<string>();

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Authors' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfAuthorsArrayHasNullElements()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { null, "Author" };

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Authors' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfAuthorsArrayHasEmptyElements()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "", "Author" };

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Authors' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfGenresArrayIsNull()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        createBookModel.Genres = null;

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Genres' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfGenresArrayIsEmpty()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        createBookModel.Genres = Enumerable.Empty<string>();

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Genres' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfAuthorsArrayHasElementsWhichLengthExceeds128Symbols()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "New Author New Author New Author New Author New Author New Author New Author New Author New Author New Author New Author New Author New Author" };

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Authors' must be 128 characters or fewer. You entered 142 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfGenresArrayHasNullElements()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        createBookModel.Genres = new string[] { null, "New Genre" };

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Genres' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfGenresArrayHasEmptyElements()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        createBookModel.Genres = new string[] { "", "New Genre" };

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Genres' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfGenresArrayHasElementsWhichLengthExceeds128Symbols()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        createBookModel.Genres = new string[] { "New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre New Genre" };

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Genres' must be 128 characters or fewer. You entered 159 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfBookDescriptionLengthExceeds1028Symbols()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        createBookModel.Genres = new string[] { "New Comedy", "Adventure" };
        createBookModel.Description = $"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce venenatis magna ante. Sed euismod urna at consequat porta. Morbi ac porttitor massa. Proin ut lorem commodo, mattis nulla at, maximus ex. Vivamus euismod sapien eu felis convallis pretium. Nulla purus est, blandit et molestie in, mollis posuere neque. Sed sollicitudin at enim ac condimentum." +
            $"Suspendisse potenti. Donec quis scelerisque est. Integer at nibh condimentum, sagittis velit sit amet, scelerisque libero. Pellentesque eget pretium magna. Vestibulum ultrices sagittis lacus, at tempus neque convallis eu. Integer urna neque, interdum in imperdiet at, ultricies eget nulla. Mauris finibus velit id velit interdum dictum." +
            $"Suspendisse potenti. Donec quis scelerisque est. Integer at nibh condimentum, sagittis velit sit amet, scelerisque libero. Pellentesque eget pretium magna. Vestibulum ultrices sagittis lacus, at tempus neque convallis eu. Integer urna neque, interdum in imperdiet at, ultricies eget nulla. Mauris finibus velit id velit interdum dictum." +
            $"Suspendisse potenti. Donec quis scelerisque est. Integer at nibh condimentum, sagittis velit sit amet, scelerisque libero. Pellentesque eget pretium magna. Vestibulum ultrices sagittis lacus, at tempus neque convallis eu. Integer urna neque, interdum in imperdiet at, ultricies eget nulla. Mauris finibus velit id velit interdum dictum.";


        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("The length of 'Description' must be 1028 characters or fewer. You entered 1363 characters.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfQuantityIsZero()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        createBookModel.Genres = new string[] { "New Comedy", "Adventure" };
        createBookModel.Quantity = 0;

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Quantity' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfQuantityIsLessThanZero()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        createBookModel.Genres = new string[] { "New Comedy", "Adventure" };
        createBookModel.Quantity = -5;

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Quantity' must be greater than '0'.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateBookModelValidator_ShouldThrowError_IfImageIsNull()
    {
        createBookModel.Title = "Test title";
        createBookModel.ISBN = "978-9-3897-4501-6";
        createBookModel.Authors = new string[] { "Stephen King", "Ivan Vazov" };
        createBookModel.Genres = new string[] { "New Comedy", "Adventure" };
        createBookModel.Quantity = 15;
        createBookModel.Image = null;

        var result = createBookModelValidator.Validate(createBookModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Image' must not be empty.", result.Errors[0].ErrorMessage);
    }
}
