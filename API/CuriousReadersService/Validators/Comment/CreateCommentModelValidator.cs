namespace CuriousReadersData.Validators.Comment;

using CuriousReadersData.Dto.Comments;
using FluentValidation;

public class CreateCommentModelValidator : AbstractValidator<CreateCommentModel>
{
    public CreateCommentModelValidator()
    {
        RuleFor(x => x.Content).NotNull().NotEmpty();
        RuleFor(x => x.BookId).NotEmpty().NotEmpty();
        RuleFor(x => x.UserName).NotEmpty().NotEmpty();
    }
}
