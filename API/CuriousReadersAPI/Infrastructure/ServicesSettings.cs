namespace CuriousReadersAPI.Infrastructure;

using CuriousReadersData.Commands;
using CuriousReadersData.Dto.Books;
using CuriousReadersData.Dto.Comments;
using CuriousReadersData.Dto.User;
using CuriousReadersData.Queries;
using CuriousReadersData.Validators.Books;
using CuriousReadersData.Validators.Comment;
using CuriousReadersData.Validators.Reservations;
using CuriousReadersData.Validators.User;
using CuriousReadersService.Request;
using CuriousReadersService.Services.Reservation;
using CuriousReadersService.Services.Author;
using CuriousReadersService.Services.Book;
using CuriousReadersService.Services.Comments;
using CuriousReadersService.Services.Genre;
using CuriousReadersService.Services.Image;
using CuriousReadersService.Services.Mail;
using CuriousReadersService.Services.Notifications;
using CuriousReadersService.Services.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class ServicesSettings
{
    public static void BuildServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CreateBookModelValidator>());
        builder.Services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RegisterUserModelValidator>());
        builder.Services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CreateCommentModelValidator>());
        builder.Services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ReservationRequestModelValidator>());
        builder.Services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UpdateBookModelValidator>());
        builder.Services.AddScoped<IBookQueries, BookQueries>();
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IBookCommands, BookCommands>();
        builder.Services.AddScoped<IAuthorQueries, AuthorQueries>();
        builder.Services.AddScoped<IAuthorService, AuthorService>();
        builder.Services.AddScoped<IGenreQueries, GenreQueries>();
        builder.Services.AddScoped<IGenreService, GenreService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICommentService, CommentService>();
        builder.Services.AddScoped<ICommentCommands, CommentCommands>();
        builder.Services.AddScoped<ICommentQueries, CommentQueries>();
        builder.Services.AddScoped<IUserQueries, UserQueries>();
        builder.Services.AddScoped<IReservationQueries, ReservationQueries>();
        builder.Services.AddScoped<IReservationService, ReservationService>();
        builder.Services.AddScoped<IReservationCommands, ReservationCommands>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<INotificationCommands, NotificationCommands>();
        builder.Services.AddScoped<INotificationQueries, NotificationQueries>();
        builder.Services.AddScoped<IMailService, MailService>();
        builder.Services.AddScoped<IValidator<CreateBookModel>, CreateBookModelValidator>();
        builder.Services.AddScoped<IValidator<UpdateBookModel>, UpdateBookModelValidator>();
        builder.Services.AddScoped<IValidator<RegisterModel>, RegisterUserModelValidator>();
        builder.Services.AddScoped<IValidator<ChangePasswordModel>, ChangePasswordModelValidator>();
        builder.Services.AddScoped<IValidator<CreateCommentModel>, CreateCommentModelValidator>();
        builder.Services.AddScoped<IValidator<ReservationRequest>, ReservationRequestModelValidator>();
        builder.Services.AddScoped<IImageService, ImageService>();
    }
}
