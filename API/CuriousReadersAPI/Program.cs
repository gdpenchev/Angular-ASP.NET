using CuriousReadersAPI.Infrastructure;
using CuriousReadersData;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CuriousReadersData.Profiles;
using CuriousReadersData.Entities;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs;
using CuriousReadersService.Profiles;

var builder = WebApplication.CreateBuilder(args);

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetService<IConfiguration>();

builder
    .Services
    .Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"));

builder
    .Services
    .AddCors(options => options.AddPolicy("allow origin", x =>
    {
        x.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    }));

ServicesSettings.BuildServices(builder);

var connectionString = builder
    .Configuration
    .GetConnectionString("DefaultConnection");


var config = new MapperConfiguration(config =>
{
    config.AddProfile(new BooksProfile());
    config.AddProfile(new AuthorsProfile());
    config.AddProfile(new CommentsProfile());
    config.AddProfile(new ReservationProfile());
    config.AddProfile(new NotificationProfile());
    config.AddProfile(new GenreProfile());
    config.AddProfile(new UserProfile());
});

var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder
    .Services
    .AddDbContext<LibraryDbContext>(options => options.UseSqlServer(connectionString));


builder
    .Services
    .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<LibraryDbContext>()
                .AddDefaultTokenProviders();

builder
    .Services
    .AddMvc();

builder.Services.AddAuthentication();

var azureBlobConnectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");

builder.Services.AddScoped(x => new BlobServiceClient(azureBlobConnectionString));


builder
    .Services
    .AddControllers();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
});


JWTSetting
    .ConfigureJWT(builder, configuration);

builder
    .Services
    .AddEndpointsApiExplorer();
builder
    .Services
    .AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder
    .Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection()
    .UseRouting()
    .UseAuthentication()
    .UseCors("allow origin")
    .UseAuthorization();

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();

});

SeedDatabase.MigrateDatabase(connectionString);

app.SeedDatabase();
app.Run();


