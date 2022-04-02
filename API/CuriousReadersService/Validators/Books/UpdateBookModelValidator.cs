﻿namespace CuriousReadersData.Validators.Books;

using FluentValidation;
using CuriousReadersData.Dto.Books;
using static CuriousReadersData.DataConstants;
public class UpdateBookModelValidator : AbstractValidator<UpdateBookModel>
{
    public UpdateBookModelValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(128);
        RuleFor(x => x.ISBN).NotNull().NotEmpty().Matches(isbnRegex);
        RuleFor(x => x.Authors).NotNull().NotEmpty();
        RuleForEach(x => x.Authors).NotNull().NotEmpty().MaximumLength(128);
        RuleFor(x => x.Genres).NotNull().NotEmpty();
        RuleForEach(x => x.Genres).NotNull().NotEmpty().MaximumLength(128);
        RuleFor(x => x.Description).MaximumLength(1028);
        RuleFor(x => x.Quantity).NotNull().NotEmpty().GreaterThan(0);
        RuleFor(x => x.Status).IsEnumName(typeof(Enumerators.BookStatus), caseSensitive: false);
        RuleFor(x => x.OldImageUrl).NotNull().NotEmpty();
    }
}
