namespace CuriousReaders.Test.Validators.Comment;

using CuriousReadersData.Dto.Comments;
using CuriousReadersData.Validators.Comment;
using Xunit;

public class CreateCommentModelValidatorTest
{
    private readonly CreateCommentModelValidator createCommentModelValidator = new CreateCommentModelValidator();
    private readonly CreateCommentModel createCommentModel = new CreateCommentModel();

    [Fact]
    public void CreateCommentModelValidator_ShouldThrowError_IfContentIsNull()
    {
        createCommentModel.Content = null;

        var result = createCommentModelValidator.Validate(createCommentModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Content' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateCommentModelValidator_ShouldThrowError_IfContentIsEmpty()
    {
        createCommentModel.Content = "";

        var result = createCommentModelValidator.Validate(createCommentModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Content' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateCommentModelValidator_ShouldThrowError_IfBookIdIsEmpty()
    {
        createCommentModel.Content = "Some test content";
        createCommentModel.BookId = 0;

        var result = createCommentModelValidator.Validate(createCommentModel);

        Assert.False(result.IsValid);
        Assert.Equal("'Book Id' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateCommentModelValidator_ShouldThrowError_IfUserNameIsNull()
    {
        createCommentModel.Content = "Some test content";
        createCommentModel.BookId = 1;
        createCommentModel.UserName = null;

        var result = createCommentModelValidator.Validate(createCommentModel);

        Assert.False(result.IsValid);
        Assert.Equal("'User Name' must not be empty.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateCommentModelValidator_ShouldThrowError_IfUserNameIsEmpty()
    {
        createCommentModel.Content = "Some test content";
        createCommentModel.BookId = 1;
        createCommentModel.UserName = "";

        var result = createCommentModelValidator.Validate(createCommentModel);

        Assert.False(result.IsValid);
        Assert.Equal("'User Name' must not be empty.", result.Errors[0].ErrorMessage);
    }
}
