namespace CuriousReadersAPI.Infrastructure;

using CuriousReadersData;
using CuriousReadersData.Entities;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class DataSeeder
{
    public async static void SeedDatabase(this WebApplication app)
    {
        var serviceApp = app.Services;
        using var scopedService = serviceApp.CreateScope();

        var service = scopedService.ServiceProvider;
        var userManager = service.GetRequiredService<UserManager<User>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        var dbContext = service.GetRequiredService<LibraryDbContext>();

        await SeedRoles(roleManager);
        await SeedReader(userManager);
        await SeedLibrarian(userManager);
        await SeedGenres(dbContext);
        await SeedAuthors(dbContext);
        await SeedStatuses(dbContext);
        await SeedExpiredBookReservation(dbContext, userManager);
    }

    private async static Task SeedExpiredBookReservation(LibraryDbContext dbContext, UserManager<User> userManager)
    {
        await SeedTestUser(userManager);

        await SeedExpiredBook(dbContext);

        var expiredBook = dbContext.Books.FirstOrDefault(b => b.ISBN == "121-2-1212-1212-9");

        var expiredUserId = userManager.Users.FirstOrDefault(u => u.Email == "baxelar862@procowork.com").Id;

        var reservations = new List<Reservation>()
        {
          new Reservation()
          {
              BookId = expiredBook.Id,
              RequestDate = DateTime.Now,
              ReturnDate = DateTime.Now.AddSeconds(10),
              UserId = expiredUserId,
              Status = dbContext.Statuses.FirstOrDefault(s => s.Name == Enumerators.ReservationStatus.Borrowed.ToString())
          }
        };


        if (!dbContext.Reservations.Any(r => r.UserId == expiredUserId))
        {
            expiredBook.Reservations = reservations;
            await dbContext.Reservations.AddRangeAsync(reservations);
            await dbContext.SaveChangesAsync();
        }

    }

    private static async Task SeedExpiredBook(LibraryDbContext dbContext)
    {
        var book = new Book
        {
            Title = "Expired Book",
            ISBN = "121-2-1212-1212-9",
            Quantity = 42,
            Image = "https://sdn2022pirin.blob.core.windows.net/book-covers/12279198_1171013439594594_6724005299696698505_n.png",
            Status = new Status() { Name = Enumerators.BookStatus.Enabled.ToString() },
            Description = "Describing in progress",
            Authors = new List<AuthorBook>()
            {
                new AuthorBook()
                {
                    Author = dbContext.Authors.FirstOrDefault(a => a.Name == "Pesho Peshev"),
                    Book = new Book(){Title = "Expired Book"},
                }
            },
            Genres = new List<BookGenre>()
            {
                new BookGenre()
                {
                     Book = new Book(){Title = "Expired Book"},
                     Genre = dbContext.Genres.FirstOrDefault(g => g.Name == "Art"),
                }
            },
        };

        if (!dbContext.Books.Any(b => b.ISBN == book.ISBN))
        {
            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedTestUser(UserManager<User> userManager)
    {
        var testUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Test",
            LastName = "Testov",
            Password = "@1234Reader",
            UserName = "baxelar862@procowork.com",
            Email = "baxelar862@procowork.com",
            PhoneNumber = "123456789",
            Country = "Test",
            Street = "Test",
            IsActive = true,
            RegistrationDate = DateTime.Now,
            City = "Test",
            StreetNumber = "1",
            BuildingNumber = "1",
            ApartmentNumber = "1",
            AdditionalInfo = "Some additional Info"
        };

        if (!userManager.Users.Contains(testUser))
        {
            string confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(testUser);
            await userManager.ConfirmEmailAsync(testUser, confirmEmailToken);
            var result = await userManager.CreateAsync(testUser, "@1234Reader");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(testUser, "Reader");
            }
        }

    }

    private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Librarian"))
            await roleManager.CreateAsync(new IdentityRole("Librarian"));

        if (!await roleManager.RoleExistsAsync("Reader"))
            await roleManager.CreateAsync(new IdentityRole("Reader"));
    }

    private static async Task SeedLibrarian(UserManager<User> userManager)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Admin",
            LastName = "Admin",
            Password = "@1234Library",
            UserName = "librarian@abv.bg",
            Email = "librarian@abv.bg",
            PhoneNumber = "123456789",
            Country = "Test",
            Street = "Test",
            IsActive = true,
            RegistrationDate = DateTime.Now,
            City = "Test",
            StreetNumber = "1",
            BuildingNumber = "1",
            ApartmentNumber = "1",
            AdditionalInfo = "Some additional Info"
        };

        if (!userManager.Users.Contains(user))
        {
            string confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, confirmEmailToken);
            var result = await userManager.CreateAsync(user, "@1234Library");

            if (result.Succeeded)
            {
                string[] roles = new string[2] { "Reader", "Librarian" };
                await userManager.AddToRolesAsync(user, roles);
            }

        }
    }

    private static async Task SeedReader(UserManager<User> userManager)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Reader",
            LastName = "Reader",
            Password = "@1234Reader",
            UserName = "reader@abv.bg",
            Email = "reader@abv.bg",
            PhoneNumber = "123456789",
            Country = "Test",
            Street = "Test",
            IsActive = true,
            RegistrationDate = DateTime.Now,
            City = "Test",
            StreetNumber = "1",
            BuildingNumber = "1",
            ApartmentNumber = "1",
            AdditionalInfo = "Some additional Info"
        };

        if (!userManager.Users.Contains(user))
        {
            string confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, confirmEmailToken);
            var result = await userManager.CreateAsync(user, "@1234Reader");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Reader");
            }
        }
    }

    private async static Task SeedGenres(LibraryDbContext dbContext)
    {
        if (dbContext.Genres.Any())
        {
            return;
        }

        await dbContext.Genres.AddRangeAsync(new[]
         {
                new Genre {Name = "Fantasy"},
                new Genre {Name = "Adventure"},
                new Genre {Name = "Romance"},
                new Genre {Name = "Contemporary"},
                new Genre {Name = "Dystopian"},
                new Genre {Name = "Mystery"},
                new Genre {Name = "Horror"},
                new Genre {Name = "Thriller"},
                new Genre {Name = "Paranormal"},
                new Genre {Name = "Historical fiction"},
                new Genre {Name = "Science Fiction"},
                new Genre {Name = "Children’s"},
                new Genre {Name = "Memoir"},
                new Genre {Name = "Cooking"},
                new Genre {Name = "Art"},
                new Genre {Name = "Self-help / Personal"},
                new Genre {Name = "Development"},
                new Genre {Name = "Motivational"},
                new Genre {Name = "Health"},
                new Genre {Name = "History"},
                new Genre {Name = "Travel"},
                new Genre {Name = "Guide / How-to"},
                new Genre {Name = "Humor"},
            });

        await dbContext.SaveChangesAsync();
    }

    private async static Task SeedAuthors(LibraryDbContext dbContext)
    {
        if (dbContext.Authors.Any())
        {
            return;
        }

        await dbContext.Authors.AddRangeAsync(new[]
         {
                new Author{Name = "Pesho Peshev"},
                new Author{Name = "Ivanov Ivanov"}
         });

        await dbContext.SaveChangesAsync();
    }

    private async static Task SeedStatuses(LibraryDbContext dbContext)
    {
        var bookStatuses = Enum.GetValues(typeof(Enumerators.BookStatus))
            .Cast<Enumerators.BookStatus>()
            .Select(x => new Status
            {
                Name = x.ToString()
            })
            .ToList();

        var reservationStatuses = Enum.GetValues(typeof(Enumerators.ReservationStatus))
            .Cast<Enumerators.ReservationStatus>()
            .Select(x => new Status
            {
                Name = x.ToString()
            })
            .ToList();

        var nonExistentBookStatuses = bookStatuses.Where(x => !dbContext.Statuses.Select(y => y.Name).Contains(x.Name))?.ToList() ?? new List<Status>();

        var nonExistentReservationStatuses = reservationStatuses.Where(x => !dbContext.Statuses.Select(y => y.Name).Contains(x.Name))?.ToList() ?? new List<Status>();


        if (dbContext.Statuses.Count() < bookStatuses.Count + reservationStatuses.Count)
        {
            await dbContext.Statuses.AddRangeAsync(nonExistentBookStatuses);
            await dbContext.Statuses.AddRangeAsync(nonExistentReservationStatuses);
            await dbContext.SaveChangesAsync();
        }
    }
}
